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
    public class OficinaController : Controller
    {
        private readonly DbvinDbContext _context;
        public OficinaController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: Oficina
        public async Task<IActionResult> Index()
        {
            return _context.Oficinas != null ?
              View(await _context.Oficinas.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Oficinas'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.Oficinas != null ?
              View("Index", await _context.Oficinas.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Oficinas'  is null.");
        }

        // GET: Oficina/Details/5
        public async Task<IActionResult> Details(double IdOfi)
        {
            var oficina = await _context.Oficinas
            .FindAsync(IdOfi);
            if (oficina == null)
            {
                return NotFound();
            }
            return View(oficina);
        }
        // GET: Oficina/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Oficina/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Oficina oficina)
        {
            _context.Add(oficina);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(oficina);
        }
        // GET: Oficina/Edit/5
        public async Task<IActionResult> Edit(double IdOfi)
        {
            var oficina = await _context.Oficinas.FindAsync(IdOfi);
            if (oficina == null)
            {
                return NotFound();
            }
            return View(oficina);
        }
        // POST: Oficina/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(double IdOfi, Oficina oficina)
        {
            try
            {
                _context.Update(oficina);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OficinaExists(oficina.IdOfi))
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
            //return View(oficina);
        }
        // GET: Oficina/Delete/5
        public async Task<IActionResult>
            Delete(double IdOfi)
        {
            var oficina = await _context.Oficinas
            .FindAsync(IdOfi);
            if (oficina == null)
            {
                return NotFound();
            }
            return View(oficina);
        }
        // POST: Oficina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(double IdOfi)
        {
            if (_context.Oficinas == null)
            {
                return Problem("Entity set 'DbvinDbContext.Oficinas'  is null.");
            }
            var oficina = await _context.Oficinas.FindAsync(IdOfi);
            if (oficina != null)
            {
                _context.Oficinas.Remove(oficina);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool OficinaExists(double id)
        {
            return (_context.Oficinas?.Any(e => e.IdOfi == id)).GetValueOrDefault();
        }
    }
}
