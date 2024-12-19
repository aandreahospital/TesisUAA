using iText.StyledXmlParser.Jsoup.Select;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;

namespace SistemaBase.Controllers
{
    public class PersonaCustomController : Controller
    {

        private readonly DbvinDbContext _context;

        public PersonaCustomController(DbvinDbContext context)
        {
            _context = context;
        }
        
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSACTPER", "Index", "PersonaCustom" })]

        public async Task<IActionResult> Index(int page = 1, string searchTerm = "")
        {
            var personas = (from p in _context.Personas
                            where string.IsNullOrEmpty(searchTerm) ||
                                  p.CodPersona.Contains(searchTerm) ||
                                  p.Nombre.Contains(searchTerm)
                            select new PersonaCustom()
                            {
                                CodPersona = p.CodPersona,
                                CodPerFisica = p.CodPerFisica,
                                CodPerJuridica = p.CodPerJuridica,
                                EsFisica = p.EsFisica,
                                Nombre = p.Nombre,
                                NombFantasia = p.NombFantasia,
                                Sexo = p.Sexo == "M" ? "Masculino" : (p.Sexo == "F" ? "Femenino" : ""), // Asignar el texto correspondiente al sexo
                                Profesion = (_context.Profesiones.FirstOrDefault(prof => prof.CodProfesion == p.Profesion).Descripcion) ?? "", // Buscar la descripción de la profesión en la tabla de profesiones
                                Conyugue = p.Conyugue,
                                CodSector = p.CodSector,
                                DirecElectronica = p.DirecElectronica,
                                EsMalDeudor = p.EsMalDeudor,
                                NivelEstudios = p.NivelEstudios,
                                TotalIngresos = p.TotalIngresos,
                                CodPais = p.CodPais,
                                AltaPor = p.AltaPor,
                                TipoSociedad = p.TipoSociedad,
                                Lucrativa = p.Lucrativa,
                                Estatal = p.Estatal,
                                PaginaWeb = p.PaginaWeb,
                                CodEstadoCivil = (_context.EstadosCiviles.FirstOrDefault(prof => prof.CodEstadoCivil == p.CodEstadoCivil).Descripcion) ?? "",
                                NroRegistroProf = p.NroRegistroProf,
                                NroRegistroSenacsa = p.NroRegistroSenacsa,
                                EsFuncionarioSenacsa = p.EsFuncionarioSenacsa,
                                EsVacunador = p.EsVacunador,
                                EsFiscalizador = p.EsFiscalizador,
                                EsVeterinario = p.EsVeterinario,
                                EsPropietario = p.EsPropietario,
                                CodIdent = p.CodIdent,
                                CodPropietarioOld = p.CodPropietarioOld,
                                EsCoordinador = p.EsCoordinador
                            });
            var pageSize = 50;
            var totalCount = await personas.CountAsync();

            var data = await personas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            ViewBag.MaxPage = (totalCount / pageSize) + (totalCount % pageSize == 0 ? 0 : 1);
            ViewBag.Page = page;

            return View(data);

        }
        public async Task<IActionResult>ResultTable(int page = 1, string searchTerm = "")
        {

            ViewData["Show"] = true;
            var personas = (from p in _context.Personas
                            where string.IsNullOrEmpty(searchTerm) ||
                                  p.CodPersona.Contains(searchTerm) ||
                                  p.Nombre.Contains(searchTerm)
                            select new PersonaCustom()
                            {
                                CodPersona = p.CodPersona,
                                CodPerFisica = p.CodPerFisica,
                                CodPerJuridica = p.CodPerJuridica,
                                EsFisica = p.EsFisica,
                                Nombre = p.Nombre,
                                NombFantasia = p.NombFantasia,
                                Sexo = p.Sexo,
                                Profesion = p.Profesion,
                                Conyugue = p.Conyugue,
                                CodSector = p.CodSector,
                                DirecElectronica = p.DirecElectronica,
                                EsMalDeudor = p.EsMalDeudor,
                                NivelEstudios = p.NivelEstudios,
                                TotalIngresos = p.TotalIngresos,
                                CodPais = p.CodPais,
                                AltaPor = p.AltaPor,
                                TipoSociedad = p.TipoSociedad,
                                Lucrativa = p.Lucrativa,
                                Estatal = p.Estatal,
                                PaginaWeb = p.PaginaWeb,
                                CodEstadoCivil = p.CodEstadoCivil,
                                NroRegistroProf = p.NroRegistroProf,
                                NroRegistroSenacsa = p.NroRegistroSenacsa,
                                EsFuncionarioSenacsa = p.EsFuncionarioSenacsa,
                                EsVacunador = p.EsVacunador,
                                EsFiscalizador = p.EsFiscalizador,
                                EsVeterinario = p.EsVeterinario,
                                EsPropietario = p.EsPropietario,
                                CodIdent = p.CodIdent,
                                CodPropietarioOld = p.CodPropietarioOld,
                                EsCoordinador = p.EsCoordinador
                            });
            var pageSize = 50;
            var totalCount = await personas.CountAsync();

            var data = await personas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            ViewBag.MaxPage = (totalCount / pageSize) + (totalCount % pageSize == 0 ? 0 : 1);
            ViewBag.Page = page;

            //return View(data);
            return View("Index", data);
        }


        public async Task<IActionResult> Details(string CodPersona)
        {
            var persona = await _context.Personas
            .FindAsync(CodPersona);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }


        public IActionResult Create()
        {

            PersonaCreateCustom personaCreate = new PersonaCreateCustom();



            return View(personaCreate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSACTPER", "Create", "PersonaCustom" })]

        public async Task<IActionResult> Create(Persona persona)
        {
            var existingPersona = _context.Personas.FirstOrDefault(x => x.CodPersona== persona.CodPersona);
            if (existingPersona != null)
            {
                return Json(new { success = false, message = "Persona ya existe" });
            }
            else
            {
                _context.Add(persona);
                await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");
            }
        }

        public async Task<IActionResult> Edit(string CodPersona)
        {

            PersonaCreateCustom personaCreate = new PersonaCreateCustom();

            var personaById = await _context.Personas.FirstOrDefaultAsync(p => p.CodPersona == CodPersona);

            if (personaById == null)
            {
                return NotFound();
            }

            personaCreate.CodPersona = personaById.CodPersona;
            personaCreate.CodPerFisica = personaById.CodPerFisica;
            personaCreate.CodPerJuridica = personaById.CodPerJuridica;
            personaCreate.EsFisica = personaById.EsFisica;
            personaCreate.Nombre = personaById.Nombre;
            personaCreate.NombFantasia = personaById.NombFantasia;
            personaCreate.Sexo = personaById.Sexo;
            personaCreate.Profesion = personaById.Profesion;
            personaCreate.Conyugue = personaById.Conyugue;
            personaCreate.CodSector = personaById.CodSector;
            personaCreate.DirecElectronica = personaById.DirecElectronica;
            personaCreate.EsMalDeudor = personaById.EsMalDeudor;
            personaCreate.NivelEstudios = personaById.NivelEstudios;
            personaCreate.TotalIngresos = personaById.TotalIngresos;
            personaCreate.CodPais = personaById.CodPais;
            personaCreate.AltaPor = personaById.AltaPor;
            personaCreate.TipoSociedad = personaById.TipoSociedad;
            personaCreate.Lucrativa = personaById.Lucrativa;
            personaCreate.Estatal = personaById.Estatal;
            personaCreate.PaginaWeb = personaById.PaginaWeb;
            personaCreate.CodEstadoCivil = personaById.CodEstadoCivil;
            personaCreate.NroRegistroProf = personaById.NroRegistroProf;
            personaCreate.NroRegistroSenacsa = personaById.NroRegistroSenacsa;
            personaCreate.EsFuncionarioSenacsa = personaById.EsFuncionarioSenacsa;
            personaCreate.EsVacunador = personaById.EsVacunador;
            personaCreate.EsFiscalizador = personaById.EsFiscalizador;
            personaCreate.EsVeterinario = personaById.EsVeterinario;
            personaCreate.EsPropietario = personaById.EsPropietario;
            personaCreate.CodIdent = personaById.CodIdent;
            personaCreate.CodPropietarioOld = personaById.CodPropietarioOld;
            personaCreate.EsCoordinador = personaById.EsCoordinador;


            return View(personaCreate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSACTPER", "Edit", "PersonaCustom" })]

        public async Task<IActionResult> Edit(string CodPersona, Persona persona)
        {
            try
            {
                var personaById = await _context.Personas.FirstOrDefaultAsync(m => m.CodPersona== persona.CodPersona);
                if (personaById != null)
                {
                    personaById.CodPersona = persona.CodPersona;
                    personaById.CodPerFisica = persona.CodPerFisica;
                    personaById.CodPerJuridica = persona.CodPerJuridica;
                    personaById.EsFisica = persona.EsFisica;
                    personaById.Nombre = persona.Nombre;
                    personaById.NombFantasia = persona.NombFantasia;
                    personaById.Sexo = persona.Sexo;
                    personaById.Profesion = persona.Profesion;
                    personaById.Conyugue = persona.Conyugue;
                    personaById.CodSector = persona.CodSector;
                    personaById.DirecElectronica = persona.DirecElectronica;
                    personaById.EsMalDeudor = persona.EsMalDeudor;
                    personaById.NivelEstudios = persona.NivelEstudios;
                    personaById.TotalIngresos = persona.TotalIngresos;
                    personaById.CodPais = persona.CodPais;
                    personaById.AltaPor = persona.AltaPor;
                    personaById.TipoSociedad = persona.TipoSociedad;
                    personaById.Lucrativa = persona.Lucrativa;
                    personaById.Estatal = persona.Estatal;
                    personaById.PaginaWeb = persona.PaginaWeb;
                    personaById.CodEstadoCivil = persona.CodEstadoCivil;
                    personaById.NroRegistroProf = persona.NroRegistroProf;
                    personaById.NroRegistroSenacsa = persona.NroRegistroSenacsa;
                    personaById.EsFuncionarioSenacsa = persona.EsFuncionarioSenacsa;
                    personaById.EsVacunador = persona.EsVacunador;
                    personaById.EsFiscalizador = persona.EsFiscalizador;
                    personaById.EsVeterinario = persona.EsVeterinario;
                    personaById.EsPropietario = persona.EsPropietario;
                    personaById.CodIdent = persona.CodIdent;
                    personaById.CodPropietarioOld = persona.CodPropietarioOld;
                    personaById.EsCoordinador = persona.EsCoordinador;

                    _context.Update(personaById);
                    await _context.SaveChangesAsync();
                }



            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonaExists(persona.CodPersona))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("ResultTable");
        }



        public async Task<IActionResult>
            Delete(string CodPersona)
        {
            var persona = await _context.Personas
            .FindAsync(CodPersona);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona );
        }
        // POST: AccesosGrupo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSACTPER", "Delete", "PersonaCustom" })]

        public async Task<IActionResult>
            DeleteConfirmed(string CodPersona)
        {
            if (_context.Personas == null)
            {
                return Problem("Entity set 'DbvinDbContext.Personas'  is null.");
            }
            var persona = await _context.Personas.FindAsync(CodPersona);
            if (persona != null)
            {
                _context.Personas.Remove(persona);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("ResultTable");

        }

        private bool PersonaExists(string CodPersona)
        {
            return (_context.Personas?.Any(e => e.CodPersona == CodPersona)).GetValueOrDefault();
        }

    }
}
