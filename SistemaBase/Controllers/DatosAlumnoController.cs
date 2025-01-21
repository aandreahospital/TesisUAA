using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using System.Security.Claims;

namespace SistemaBase.Controllers
{
    public class DatosAlumnoController : Controller
    {

        private readonly DbvinDbContext _context;

        public DatosAlumnoController(DbvinDbContext context)
        {
            _context = context;
        }

        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "SCDATOS", "Index", "DatosAlumno" })]

        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.FirstOrDefault(x=>x.CodUsuario == User.Identity.Name);
           /// var alumno = ObtenerDatosAlumno(usuarios.CodPersona);
            var alumno = _context.Personas.Where(a => a.CodPersona == usuarios.CodPersona).FirstOrDefault();
            if (alumno == null)
            {
                return NotFound("Datos del alumno no encontrados");
            }

            var experiencia = ObtenerExperienciaLaboral();
            var educacion = ObtenerEducacion();
            DatosAlumnoCustom datosAlumno = new()
            {
                //NumeroEntrada = idMesaEntrada,
                CodPersona = usuarios.CodPersona,
                Nombre = alumno.Nombre,
                Email = alumno?.Email??"",
                SitioWeb = alumno?.Sitioweb??"",
                Sexo = alumno?.Sexo??"",
                DireccionParticular = alumno?.Direccionparticular??"",
                FecNacimiento = alumno?.FecNacimiento,
                ExperienciaLaboral = experiencia,
                Educacion = educacion
            };
           
            return View(datosAlumno);
        }



        private List<Datoslaborale> ObtenerExperienciaLaboral()
        {
            var laboral = _context.Datoslaborales.Where(x => x.CodUsuario == User.Identity.Name).ToList();

            var listaLaborales = laboral.Select(laboral => new Datoslaborale
            {
                Lugartrabajo = laboral?.Lugartrabajo ?? "",
                Cargo = laboral?.Cargo??"",
                Direccionlaboral = laboral?.Direccionlaboral ?? "",
                Antiguedad = laboral?.Antiguedad ?? ""
            }).ToList();

            return listaLaborales;
        }

        private List<Datosacademico> ObtenerEducacion()
        {
            var academico = _context.Datosacademicos.Where(x => x.CodUsuario == User.Identity.Name).ToList();
            // Simulando datos; reemplaza con datos reales
            var ListaAcademico = academico.Select(academico => new Datosacademico
            {
                CentroEducativo = _context.Centroestudios
                .Where(ce => ce.Idcentroestudio == academico.Idcentroestudio)
                .Select(ce => ce.Descripcion)
                .FirstOrDefault() ?? "",
                Carrera = _context.Carreras
                .Where(c => c.Idcarrera == academico.Idcarrera)
                .Select(c => c.Descripcion)
                .FirstOrDefault() ?? "",
                Fechainicio = academico?.Fechainicio??DateTime.MinValue,
                Fechafin = academico?.Fechafin??DateTime.MinValue,
                Estado = academico?.Estado?? ""
            }).ToList();

            return ListaAcademico;


        }

        [HttpPost]
        public async Task<IActionResult> GuardarDatos(DatosAlumnoCustom datosAlumno)
        {
            try
            {

                var datosPersonales = _context.Personas.FirstOrDefault(m => m.CodPersona == datosAlumno.CodPersona);
                if (datosPersonales!= null)
                {
                    datosPersonales.Nombre = datosAlumno.Nombre;
                    datosPersonales.Email  = datosAlumno?.Email;
                    datosPersonales.Sexo = datosAlumno?.Sexo;
                    datosPersonales.FecNacimiento = datosAlumno?.FecNacimiento;
                    datosPersonales.Sitioweb = datosAlumno?.SitioWeb;
                    datosPersonales.Direccionparticular = datosAlumno?.DireccionParticular;
                    _context.Update(datosPersonales);
                    _context.SaveChanges();
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExist(datosAlumno.CodPersona))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }
      

        [HttpPost]
        public async Task<IActionResult> GuardarLaboral(Datoslaborale datosLaboral)
        {
            try
            {
                var datosLaborales = _context.Datoslaborales.FirstOrDefault(m => m.CodUsuario == datosLaboral.CodUsuario);
                var laborales = _context.Datoslaborales.Max(m => m.Iddatoslaborales) + 1;
                Datoslaborale addLaboral = new()
                {
                    Iddatoslaborales = laborales,
                    CodUsuario = datosLaboral.CodUsuario,
                    Lugartrabajo = datosLaboral?.Lugartrabajo,
                    Cargo = datosLaboral?.Cargo,
                    Antiguedad = datosLaboral?.Antiguedad,
                    Direccionlaboral = datosLaboral?.Direccionlaboral,
                    Herramientas = datosLaboral?.Herramientas
                    
                };
                 _context.Add(addLaboral);
                 _context.SaveChanges();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExist(datosLaboral.CodUsuario))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GuardarEducacion(Datosacademico datosAcademico)
        {
            try
            {
              
                var academicos = _context.Datosacademicos.Max(m => m.Iddatosacademicos) + 1;
                Datosacademico addAcademico = new()
                {
                    CodUsuario = datosAcademico.CodUsuario,
                    Iddatosacademicos = academicos,
                    Idcentroestudio = datosAcademico.Idcentroestudio,
                    Idcarrera = datosAcademico.Idcarrera,
                    Fechainicio = datosAcademico?.Fechainicio,
                    Fechafin = datosAcademico?.Fechafin,
                    Estado = datosAcademico?.Estado
                };
                _context.Add(addAcademico);
                _context.SaveChanges();

            }
            catch (DbUpdateConcurrencyException)
            {
                    return NotFound();
               
            }
            return RedirectToAction("Index");
        }


        private bool UsuarioExist(string id)
        {
            return (_context.Usuarios?.Any(e => e.CodUsuario == id)).GetValueOrDefault();
        }

        public ActionResult AbrirLaboral()
        {
            
            return View("AddLaboral");
            //return View("AddTitular", new PersonaTitular());

        }

        public ActionResult AbrirAcademico()
        {
            ViewBag.CentoEstudio = new SelectList(_context.Centroestudios, "Idcentroestudio", "Descripcion");
            ViewBag.Carrera = new SelectList(_context.Carreras, "Idcarrera", "Descripcion");

            return View("AddAcademico");

        }
    }
}
