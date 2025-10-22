using iText.Kernel.Pdf.Canvas.Wmf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using System.Security.Claims;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace SistemaBase.Controllers
{
    public class DatosAlumnoController : Controller
    {

        private readonly Models.UAADbContext _context;

        public DatosAlumnoController(Models.UAADbContext context)
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
                return NotFound("Datos del graduado no encontrados");
            }
            // Buscar imagen del usuario
            string fileName = $"{usuarios.CodPersona}.png";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", fileName);

            string fotoPerfil;
            if (System.IO.File.Exists(filePath))
            {
                // Si existe la imagen, usarla
                fotoPerfil = $"~/assets/img/{fileName}";
            }
            else
            {
                // Si no existe, usar una imagen vacía o por defecto
                fotoPerfil = "~/assets/img/user.png";
            }
            var experiencia = ObtenerExperienciaLaboral();
            var educacion = ObtenerEducacion();
            DatosAlumnoCustom datosAlumno = new()
            {
                //NumeroEntrada = idMesaEntrada,
                CodPersona = usuarios.CodPersona,
                Nombre = alumno.Nombre,
                Email = alumno?.Email??"",
                SitioWeb = alumno?.SitioWeb??"",
                Sexo = alumno?.Sexo??"",
                DireccionParticular = alumno?.DireccionParticular??"",
                FecNacimiento = alumno?.FecNacimiento,
                ExperienciaLaboral = experiencia,
                Educacion = educacion,
                FotoPerfil = fotoPerfil


            };
           
            return View(datosAlumno);
        }


        public async Task<IActionResult> ResultTable()
        {
            var usuarios = _context.Usuarios.FirstOrDefault(x => x.CodUsuario == User.Identity.Name);
            /// var alumno = ObtenerDatosAlumno(usuarios.CodPersona);
            var alumno = _context.Personas.Where(a => a.CodPersona == usuarios.CodPersona).FirstOrDefault();
            if (alumno == null)
            {
                return NotFound("Datos del graduado no encontrados");
            }

            var experiencia = ObtenerExperienciaLaboral();
            var educacion = ObtenerEducacion();
            DatosAlumnoCustom datosAlumno = new()
            {
                //NumeroEntrada = idMesaEntrada,
                CodPersona = usuarios.CodPersona,
                Nombre = alumno.Nombre,
                Email = alumno?.Email ?? "",
                SitioWeb = alumno?.SitioWeb ?? "",
                Sexo = alumno?.Sexo ?? "",
                DireccionParticular = alumno?.DireccionParticular ?? "",
                FecNacimiento = alumno?.FecNacimiento,
                ExperienciaLaboral = experiencia,
                Educacion = educacion
            };

            return View(datosAlumno);
        }
        private List<DatosLaborale> ObtenerExperienciaLaboral()
        {
            var laboral = _context.DatosLaborales.OrderByDescending(x=>x.IdDatosLaborales).Where(x => x.CodUsuario == User.Identity.Name).ToList();

            var listaLaborales = laboral.Select(laboral => new DatosLaborale
            {
                IdDatosLaborales = laboral.IdDatosLaborales,
                LugarTrabajo = laboral?.LugarTrabajo ?? "",
                Cargo = laboral?.Cargo??"",
                Antiguedad = laboral?.Antiguedad ?? 0,
                MateriaTrabajo =laboral?.MateriaTrabajo ?? "",
                UniversidadTrabajo = laboral?.UniversidadTrabajo??"",
                Sector = laboral?.Sector ?? "",
                Estado = laboral?.Estado ??"",
                Herramientas = laboral?.Herramientas ??""

            }).ToList();

            return listaLaborales;
        }

        private List<DatosAcademicoCustom> ObtenerEducacion()
        {
            var academico = _context.DatosAcademicos.OrderByDescending(X=>X.IdDatosAcademicos).Where(x => x.CodUsuario == User.Identity.Name).ToList();
            // Simulando datos; reemplaza con datos reales
            var ListaAcademico = academico.Select(academico => new DatosAcademicoCustom
            {
                IdDatosAcademicos = academico.IdDatosAcademicos,
                CentroEstudio = _context.CentroEstudios
                .Where(ce => ce.IdCentroEstudio == academico.IdCentroEstudio)
                .Select(ce => ce.Descripcion)
                .FirstOrDefault() ?? "",
                Carrera = _context.Carreras
                .Where(c => c.IdCarrera == academico.IdCarrera)
                .Select(c => c.Descripcion)
                .FirstOrDefault() ?? "",
                AnhoInicio = academico?.AnhoInicio??"",
                AnhoFin = academico?.AnhoFin ?? "",
                Estado = academico?.Estado?? ""
            }).ToList();

            return ListaAcademico;


        }

        [HttpPost]
        public async Task<IActionResult> GuardarDatos(DatosAlumnoCustom datosAlumno, IFormFile? FotoPerfil)
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
                    datosPersonales.SitioWeb = datosAlumno?.SitioWeb;
                    datosPersonales.DireccionParticular = datosAlumno?.DireccionParticular;
                    _context.Update(datosPersonales);
                    _context.SaveChanges();
                }
                if (FotoPerfil != null && FotoPerfil.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img");
                    string fileName = $"{datosAlumno.CodPersona}.png"; 
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    // Crear carpeta si no existe
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    // Guardar la imagen (reemplazando si ya existe)
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await FotoPerfil.CopyToAsync(stream);
                    }
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
        public async Task<IActionResult> GuardarLaboral(DatosLaborale datosLaboral)
        {
            try
            {
                var datosLaborales = _context.DatosLaborales.FirstOrDefault(m => m.CodUsuario == datosLaboral.CodUsuario);
               
                DatosLaborale addLaboral = new()
                {
                    CodUsuario = datosLaboral.CodUsuario,
                    LugarTrabajo = datosLaboral?.LugarTrabajo,
                    Cargo = datosLaboral?.Cargo,
                    Antiguedad = datosLaboral?.Antiguedad,
                    Estado = datosLaboral?.Estado,
                    MateriaTrabajo= datosLaboral?.MateriaTrabajo ?? "NO APLICA",
                    UniversidadTrabajo = datosLaboral?.UniversidadTrabajo ?? "NO APLICA",
                    Herramientas = datosLaboral?.Herramientas,
                    Sector = datosLaboral?.Sector
                    
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
        public async Task<IActionResult> GuardarEducacion(DatosAcademico datosAcademico)
        {
            try
            {
                DatosAcademico addAcademico = new()
                {
                    CodUsuario = datosAcademico.CodUsuario,
                    IdCentroEstudio = datosAcademico.IdCentroEstudio,
                    IdCarrera = datosAcademico.IdCarrera,
                    AnhoInicio = datosAcademico?.AnhoInicio,
                    AnhoFin = datosAcademico?.AnhoFin,
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
            ViewBag.CentoEstudio = new SelectList(_context.CentroEstudios, "IdCentroEstudio", "Descripcion");
            ViewBag.Carrera = new SelectList(_context.Carreras, "IdCarrera", "Descripcion");

            return View("AddAcademico");

        }


        //PARA ABRIR LA PANTALLA DE EDICION

        [HttpGet]
        public IActionResult EditLaboral(int id)
        {
            var experiencia = _context.DatosLaborales.Find(id);
            if (experiencia == null)
            {
                return NotFound();
            }
            else
            {
                DatosLaborale laboral = new()
                {
                    IdDatosLaborales = experiencia.IdDatosLaborales,
                    CodUsuario = experiencia?.CodUsuario??"",
                    LugarTrabajo = experiencia?.LugarTrabajo ?? "",
                    Cargo = experiencia?.Cargo ?? "",
                    Antiguedad = experiencia?.Antiguedad ?? 0,
                    Estado = experiencia?.Estado ?? "",
                    MateriaTrabajo = experiencia?.MateriaTrabajo ?? "NO APLICA",
                    UniversidadTrabajo = experiencia?.UniversidadTrabajo ?? "NO APLICA",
                    Herramientas = experiencia?.Herramientas ?? "",
                    Sector = experiencia?.Sector ?? ""

                };

                return View("EditLaboral", laboral);
            }
        }

        //PARA GUARAR LA EDICION

        [HttpPost]
        public async Task<IActionResult> GuardarEditLaboral(DatosLaborale datosLaboral)
        {
            try
            {
                var datosLaborales = _context.DatosLaborales.FirstOrDefault(m => m.IdDatosLaborales == datosLaboral.IdDatosLaborales);
                if (datosLaborales != null) {
                    datosLaborales.CodUsuario = datosLaboral.CodUsuario;
                    datosLaborales.LugarTrabajo = datosLaboral?.LugarTrabajo;
                    datosLaborales.Cargo = datosLaboral?.Cargo;
                    datosLaborales.Antiguedad = datosLaboral?.Antiguedad;
                    datosLaborales.Estado = datosLaboral?.Estado;
                    datosLaborales.MateriaTrabajo = datosLaboral?.MateriaTrabajo ?? "NO APLICA";
                    datosLaborales.UniversidadTrabajo = datosLaboral?.UniversidadTrabajo ?? "NO APLICA";
                    datosLaborales.Herramientas = datosLaboral?.Herramientas;
                    datosLaborales.Sector = datosLaboral?.Sector;
                    _context.Update(datosLaborales);
                    _context.SaveChanges();
                }
                
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
        public IActionResult Edit(int id, DatosLaborale model)
        {
            if (ModelState.IsValid)
            {
                _context.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteLaboral(int id)
        {
            var experiencia = _context.DatosLaborales.Find(id);
            if (experiencia == null)
            {
                return NotFound();
            }
            _context.DatosLaborales.Remove(experiencia);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //PARA ABRIR LA PANTALLA DE EDICION

        [HttpGet]
        public IActionResult EditAcademico(int id)
        {
            var academico = _context.DatosAcademicos.Find(id);
            if (academico == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.CentoEstudio = new SelectList(_context.CentroEstudios, "IdCentroEstudio", "Descripcion", academico.IdCentroEstudio);
                ViewBag.Carrera = new SelectList(_context.Carreras, "IdCarrera", "Descripcion", academico.IdCarrera);
                DatosAcademicoCustom EditAcademico = new()
                {
                    IdCarrera = academico.IdCarrera,
                    IdCentroEstudio = academico.IdCentroEstudio,
                    IdDatosAcademicos = academico.IdDatosAcademicos,
                    AnhoInicio = academico?.AnhoInicio ?? "",
                    AnhoFin = academico?.AnhoFin ?? "",
                    Estado = academico?.Estado ?? ""
                };
               
                return View("EditAcademico", EditAcademico);
            }
        }

        //PARA GUARAR LA EDICION
        [HttpPost]
        public async Task<IActionResult> GuardarEditEducacion(DatosAcademico datosAcademico)
        {
            try
            {
                var academico = _context.DatosAcademicos.Find(datosAcademico.IdDatosAcademicos);
                if (academico != null)
                {
                    academico.CodUsuario = datosAcademico.CodUsuario;
                    academico.IdCentroEstudio = datosAcademico.IdCentroEstudio;
                    academico.IdCarrera = datosAcademico.IdCarrera;
                    academico.AnhoInicio = datosAcademico?.AnhoInicio;
                    academico.AnhoFin = datosAcademico?.AnhoFin;
                    academico.Estado = datosAcademico?.Estado;

                    _context.Update(academico);
                    _context.SaveChanges();
                }
                
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();

            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult DeleteAcademico(int id)
        {
            var academico = _context.DatosAcademicos.Find(id);
            if (academico == null)
            {
                return NotFound();
            }
            _context.DatosAcademicos.Remove(academico);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }




    }
}
