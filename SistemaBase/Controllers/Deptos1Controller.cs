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
    public class Deptos1Controller : Controller
    {
        private readonly DbvinDbContext _context;
        public Deptos1Controller(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: Deptos1
        public async Task<IActionResult> Index()
        {
            ViewData["IdOficina"] = new SelectList(_context.Oficinas, "IdOfi", "IdOfi");
            var dbvinDbContext = _context.Deptos1s.Include(d => d.IdOficinaNavigation);
            return View(await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["IdOficina"] = new SelectList(_context.Oficinas, "IdOfi", "IdOfi");
            ViewData["Show"] = true;
            var dbvinDbContext = _context.Deptos1s.Include(d => d.IdOficinaNavigation);
            return View("Index", await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        // GET: Deptos1/Details/5
        public async Task<IActionResult> Details(double IdDepto1)
        {
            var deptos1 = await _context.Deptos1s
            .FindAsync(IdDepto1);
            if (deptos1 == null)
            {
                return NotFound();
            }
            return View(deptos1);
        }
        // GET: Deptos1/Create
        public IActionResult Create()
        {
            ViewData["IdOficina"] = new SelectList(_context.Oficinas, "IdOfi", "IdOfi");
            return View();
        }
        // POST: Deptos1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Deptos1 deptos1)
        {
            _context.Add(deptos1);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            ViewData["IdOficina"] = new SelectList(_context.Oficinas, "IdOfi", "IdOfi", deptos1.IdOficina);
            return RedirectToAction("ResultTable");
            // return View(deptos1);
        }
        // GET: Deptos1/Edit/5
        public async Task<IActionResult> Edit(double IdDepto1)
        {
            var deptos1 = await _context.Deptos1s.FindAsync(IdDepto1);
            if (deptos1 == null)
            {
                return NotFound();
            }
            ViewData["IdOficina"] = new SelectList(_context.Oficinas, "IdOfi", "IdOfi", deptos1.IdOficina);
            return View(deptos1);
        }
        // POST: Deptos1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(double IdDepto1, Deptos1 deptos1)
        {
            try
            {
                _context.Update(deptos1);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Deptos1Exists(deptos1.IdDepto1))
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
            ViewData["IdOficina"] = new SelectList(_context.Oficinas, "IdOfi", "IdOfi", deptos1.IdOficina);
            return RedirectToAction("ResultTable");
            //return View(deptos1);
        }
        // GET: Deptos1/Delete/5
        public async Task<IActionResult>
            Delete(double IdDepto1)
        {
            var deptos1 = await _context.Deptos1s
            .FindAsync(IdDepto1);
            if (deptos1 == null)
            {
                return NotFound();
            }
            return View(deptos1);
        }
        // POST: Deptos1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(double IdDepto1)
        {
            if (_context.Deptos1s == null)
            {
                return Problem("Entity set 'DbvinDbContext.Deptos1s'  is null.");
            }
            var deptos1 = await _context.Deptos1s.FindAsync(IdDepto1);
            if (deptos1 != null)
            {
                _context.Deptos1s.Remove(deptos1);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool Deptos1Exists(double id)
        {
            return (_context.Deptos1s?.Any(e => e.IdDepto1 == id)).GetValueOrDefault();
        }
    }
}
