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
    public class PropiedadesXPersonaController : Controller
    {
        private readonly DbvinDbContext _context;
        public PropiedadesXPersonaController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: PropiedadesXPersona
        public async Task<IActionResult> Index()
        {
            return _context.PropiedadesXPersonas != null ?
              View(await _context.PropiedadesXPersonas.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.PropiedadesXPersonas'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.PropiedadesXPersonas != null ?
              View("Index", await _context.PropiedadesXPersonas.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.PropiedadesXPersonas'  is null.");
        }

        // GET: PropiedadesXPersona/Details/5
        public async Task<IActionResult> Details(string CodPersona, string NumFinca, string Distrito)
        {
            var propiedadesXPersona = await _context.PropiedadesXPersonas
            .FindAsync(CodPersona, NumFinca, Distrito);
            if (propiedadesXPersona == null)
            {
                return NotFound();
            }
            return View(propiedadesXPersona);
        }
        // GET: PropiedadesXPersona/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: PropiedadesXPersona/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropiedadesXPersona propiedadesXPersona)
        {
            _context.Add(propiedadesXPersona);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(propiedadesXPersona);
        }
        // GET: PropiedadesXPersona/Edit/5
        public async Task<IActionResult> Edit(string CodPersona, string NumFinca, string Distrito)
        {
            var propiedadesXPersona = await _context.PropiedadesXPersonas.FindAsync(CodPersona, NumFinca, Distrito);
            if (propiedadesXPersona == null)
            {
                return NotFound();
            }
            return View(propiedadesXPersona);
        }
        // POST: PropiedadesXPersona/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodPersona, string NumFinca, string Distrito, PropiedadesXPersona propiedadesXPersona)
        {
            try
            {
                _context.Update(propiedadesXPersona);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropiedadesXPersonaExists(propiedadesXPersona.CodPersona))
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
            //return View(propiedadesXPersona);
        }
        // GET: PropiedadesXPersona/Delete/5
        public async Task<IActionResult>
            Delete(string CodPersona, string NumFinca, string Distrito)
        {
            var propiedadesXPersona = await _context.PropiedadesXPersonas
            .FindAsync(CodPersona, NumFinca, Distrito);
            if (propiedadesXPersona == null)
            {
                return NotFound();
            }
            return View(propiedadesXPersona);
        }
        // POST: PropiedadesXPersona/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodPersona, string NumFinca, string Distrito)
        {
            if (_context.PropiedadesXPersonas == null)
            {
                return Problem("Entity set 'DbvinDbContext.PropiedadesXPersonas'  is null.");
            }
            var propiedadesXPersona = await _context.PropiedadesXPersonas.FindAsync(CodPersona, NumFinca, Distrito);
            if (propiedadesXPersona != null)
            {
                _context.PropiedadesXPersonas.Remove(propiedadesXPersona);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool PropiedadesXPersonaExists(string id)
        {
            return (_context.PropiedadesXPersonas?.Any(e => e.CodPersona == id)).GetValueOrDefault();
        }
    }
}
