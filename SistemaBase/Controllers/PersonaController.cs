using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;

namespace SistemaBase.Controllers
{
    [Authorize]
    public class PersonaController : BaseRyMController
    {
        private readonly DbvinDbContext _context;

        public PersonaController(DbvinDbContext context, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _context = context;
        }

        // GET: Persona
        public async Task<IActionResult> Index(int page = 1, string searchTerm = "")
        {
            const int PageSize = 50;

            IQueryable<Persona> query = _context.Personas.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(item =>
                    item.CodPersona.Contains(searchTerm) ||
                    item.CodPerFisica.Contains(searchTerm) ||
                    item.CodPerJuridica.Contains(searchTerm) ||
                    item.EsFisica.Contains(searchTerm) ||
                    item.Nombre.Contains(searchTerm) ||
                    item.NombFantasia.Contains(searchTerm) ||
                    item.Sexo.Contains(searchTerm) ||
                    item.Profesion.Contains(searchTerm) ||
                    item.Conyugue.Contains(searchTerm) ||
                    item.CodSector.Contains(searchTerm) ||
                    item.DirecElectronica.Contains(searchTerm) ||
                    item.EsMalDeudor.Contains(searchTerm) ||
                    item.NivelEstudios.Contains(searchTerm) ||
                    item.TotalIngresos.ToString().Contains(searchTerm) ||  // Convert decimal to string
                    item.CodPais.Contains(searchTerm) ||
                    item.AltaPor.Contains(searchTerm) ||
                    item.TipoSociedad.Contains(searchTerm) ||
                    item.Lucrativa.Contains(searchTerm) ||
                    item.Estatal.Contains(searchTerm) ||
                    item.PaginaWeb.Contains(searchTerm) ||
                    item.CodEstadoCivil.Contains(searchTerm) ||
                    item.NroRegistroProf.Contains(searchTerm) ||
                    item.NroRegistroSenacsa.Contains(searchTerm) ||
                    item.EsFuncionarioSenacsa.Contains(searchTerm) ||
                    item.EsVacunador.Contains(searchTerm) ||
                    item.EsFiscalizador.Contains(searchTerm) ||
                    item.EsVeterinario.Contains(searchTerm) ||
                    item.EsPropietario.Contains(searchTerm) ||
                    item.CodIdent.Contains(searchTerm) ||
                    item.CodPropietarioOld.Contains(searchTerm) ||
                    item.EsCoordinador.Contains(searchTerm)
                );
            }

            var count = await query.CountAsync();
            var data = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            ViewBag.MaxPage = (count / PageSize) + (count % PageSize == 0 ? 0 : 1); // Calculate the max number of pages
            ViewBag.Page = page;

            return View(data);
        }









        public async Task<IActionResult>
    ResultTable()
        {
            ViewData["Show"] = true;
            return _context.Personas != null ?
              View("Index", await _context.Personas.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Personas'  is null.");
        }












        // GET: Persona/Details/5
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

        // GET: Persona/Create
        public IActionResult Create()
        {
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "Descripcion");
            //ViewData["CodSector"] = new SelectList(_context.Sectores, "CodSector", "Descripcion");
            //ViewData["TipoSociedad"] = new SelectList(_context.TiposSociedads, "TipoSociedad", "Descripcion");
            //ViewData["CodEstadoCivil"] = new SelectList(_context.EstadosCiviles, "CodEstadoCivil", "Descripcion");

            return View();
        }

        // POST: Persona/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Persona persona)
        {
            _context.Add(persona);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");

            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");

            // return View(persona);
        }

        // GET: Persona/Edit/5
        public async Task<IActionResult> Edit(string CodPersona)
        {

            var persona = await _context.Personas.FindAsync(CodPersona);
            if (persona == null)
            {
                return NotFound();
            }
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "Descripcion", persona.CodPais);
            //ViewData["CodSector"] = new SelectList(_context.Sectores, "CodSector", "Descripcion", persona.CodSector);
            //ViewData["TipoSociedad"] = new SelectList(_context.TiposSociedads, "TipoSociedad", "Descripcion", persona.TipoSociedad);
            ///ViewData["CodEstadoCivil"] = new SelectList(_context.EstadosCiviles, "CodEstadoCivil", "Descripcion", persona.CodEstadoCivil);

            return View(persona);
        }

        // POST: Persona/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodPersona, Persona persona)
        {

            try
            {
                _context.Update(persona);
                await _context.SaveChangesAsync();
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

            // return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");

            //return View(persona);
        }

        // GET: Persona/Delete/5
        public async Task<IActionResult>
            Delete(string CodPersona)
        {

            var persona = await _context.Personas
            .FindAsync(CodPersona);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Persona/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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

            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(string id)
        {
            return (_context.Personas?.Any(e => e.CodPersona == id)).GetValueOrDefault();
        }
    }
}
