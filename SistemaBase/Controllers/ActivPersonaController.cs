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
    public class ActivPersonaController : Controller
    {
        private readonly DbvinDbContext _context;
        public ActivPersonaController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: ActivPersona
        public async Task<IActionResult> Index()
        {
            ViewData["CodActividad"] = new SelectList(_context.ActividadesEcons, "CodActividad", "CodActividad");
            var dbvinDbContext = _context.ActivPersonas.Include(a => a.CodActividadNavigation);
            return View(await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["CodActividad"] = new SelectList(_context.ActividadesEcons, "CodActividad", "CodActividad");
            ViewData["Show"] = true;
            var dbvinDbContext = _context.ActivPersonas.Include(a => a.CodActividadNavigation);
            return View("Index", await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        // GET: ActivPersona/Details/5
        public async Task<IActionResult> Details(string CodActividad, string CodPersona)
        {
            var activPersona = await _context.ActivPersonas
            .FindAsync(CodActividad, CodPersona);
            if (activPersona == null)
            {
                return NotFound();
            }
            return View(activPersona);
        }
        // GET: ActivPersona/Create
        public IActionResult Create()
        {
            ViewData["CodActividad"] = new SelectList(_context.ActividadesEcons, "CodActividad", "CodActividad");
            return View();
        }
        // POST: ActivPersona/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivPersona activPersona)
        {
            _context.Add(activPersona);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            ViewData["CodActividad"] = new SelectList(_context.ActividadesEcons, "CodActividad", "CodActividad", activPersona.CodActividad);
            return RedirectToAction("ResultTable");
            // return View(activPersona);
        }
        // GET: ActivPersona/Edit/5
        public async Task<IActionResult> Edit(string CodActividad, string CodPersona)
        {
            var activPersona = await _context.ActivPersonas.FindAsync(CodActividad, CodPersona);
            if (activPersona == null)
            {
                return NotFound();
            }
            ViewData["CodActividad"] = new SelectList(_context.ActividadesEcons, "CodActividad", "CodActividad", activPersona.CodActividad);
            return View(activPersona);
        }
        // POST: ActivPersona/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodActividad, string CodPersona, ActivPersona activPersona)
        {
            try
            {
                _context.Update(activPersona);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivPersonaExists(activPersona.CodActividad))
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
            ViewData["CodActividad"] = new SelectList(_context.ActividadesEcons, "CodActividad", "CodActividad", activPersona.CodActividad);
            return RedirectToAction("ResultTable");
            //return View(activPersona);
        }
        // GET: ActivPersona/Delete/5
        public async Task<IActionResult>
            Delete(string CodActividad, string CodPersona)
        {
            var activPersona = await _context.ActivPersonas
            .FindAsync(CodActividad, CodPersona);
            if (activPersona == null)
            {
                return NotFound();
            }
            return View(activPersona);
        }
        // POST: ActivPersona/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodActividad, string CodPersona)
        {
            if (_context.ActivPersonas == null)
            {
                return Problem("Entity set 'DbvinDbContext.ActivPersonas'  is null.");
            }
            var activPersona = await _context.ActivPersonas.FindAsync(CodActividad, CodPersona);
            if (activPersona != null)
            {
                _context.ActivPersonas.Remove(activPersona);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool ActivPersonaExists(string id)
        {
            return (_context.ActivPersonas?.Any(e => e.CodActividad == id)).GetValueOrDefault();
        }
    }
}
