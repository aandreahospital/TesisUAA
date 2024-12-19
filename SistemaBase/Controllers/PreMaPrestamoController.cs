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
    public class PreMaPrestamoController : Controller
    {
        private readonly DbvinDbContext _context;
        public PreMaPrestamoController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: PreMaPrestamo
        public async Task<IActionResult> Index()
        {
            return _context.PreMaPrestamos != null ?
              View(await _context.PreMaPrestamos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.PreMaPrestamos'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.PreMaPrestamos != null ?
              View("Index", await _context.PreMaPrestamos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.PreMaPrestamos'  is null.");
        }

        // GET: PreMaPrestamo/Details/5
        public async Task<IActionResult> Details(decimal PreCodigo)
        {
            var preMaPrestamo = await _context.PreMaPrestamos
            .FindAsync(PreCodigo);
            if (preMaPrestamo == null)
            {
                return NotFound();
            }
            return View(preMaPrestamo);
        }
        // GET: PreMaPrestamo/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: PreMaPrestamo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PreMaPrestamo preMaPrestamo)
        {
            _context.Add(preMaPrestamo);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(preMaPrestamo);
        }
        // GET: PreMaPrestamo/Edit/5
        public async Task<IActionResult> Edit(decimal PreCodigo)
        {
            var preMaPrestamo = await _context.PreMaPrestamos.FindAsync(PreCodigo);
            if (preMaPrestamo == null)
            {
                return NotFound();
            }
            return View(preMaPrestamo);
        }
        // POST: PreMaPrestamo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(decimal PreCodigo, PreMaPrestamo preMaPrestamo)
        {
            try
            {
                _context.Update(preMaPrestamo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreMaPrestamoExists(preMaPrestamo.PreCodigo))
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
            //return View(preMaPrestamo);
        }
        // GET: PreMaPrestamo/Delete/5
        public async Task<IActionResult>
            Delete(decimal PreCodigo)
        {
            var preMaPrestamo = await _context.PreMaPrestamos
            .FindAsync(PreCodigo);
            if (preMaPrestamo == null)
            {
                return NotFound();
            }
            return View(preMaPrestamo);
        }
        // POST: PreMaPrestamo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(decimal PreCodigo)
        {
            if (_context.PreMaPrestamos == null)
            {
                return Problem("Entity set 'DbvinDbContext.PreMaPrestamos'  is null.");
            }
            var preMaPrestamo = await _context.PreMaPrestamos.FindAsync(PreCodigo);
            if (preMaPrestamo != null)
            {
                _context.PreMaPrestamos.Remove(preMaPrestamo);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool PreMaPrestamoExists(decimal id)
        {
            return (_context.PreMaPrestamos?.Any(e => e.PreCodigo == id)).GetValueOrDefault();
        }
    }
}
