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
    public class OeClCoejecutorController : Controller
    {
        private readonly DbvinDbContext _context;
        public OeClCoejecutorController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: OeClCoejecutor
        public async Task<IActionResult> Index()
        {
            ViewData["PreCodigo"] = new SelectList(_context.PreMaPrestamos, "PreCodigo", "PreCodigo");
            var dbvinDbContext = _context.OeClCoejecutors.Include(o => o.PreCodigoNavigation);
            return View(await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["PreCodigo"] = new SelectList(_context.PreMaPrestamos, "PreCodigo", "PreCodigo");
            ViewData["Show"] = true;
            var dbvinDbContext = _context.OeClCoejecutors.Include(o => o.PreCodigoNavigation);
            return View("Index", await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        // GET: OeClCoejecutor/Details/5
        public async Task<IActionResult> Details(decimal CoeCodigo)
        {
            var oeClCoejecutor = await _context.OeClCoejecutors
            .FindAsync(CoeCodigo);
            if (oeClCoejecutor == null)
            {
                return NotFound();
            }
            return View(oeClCoejecutor);
        }
        // GET: OeClCoejecutor/Create
        public IActionResult Create()
        {
            ViewData["PreCodigo"] = new SelectList(_context.PreMaPrestamos, "PreCodigo", "PreCodigo");
            return View();
        }
        // POST: OeClCoejecutor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OeClCoejecutor oeClCoejecutor)
        {
            _context.Add(oeClCoejecutor);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            ViewData["PreCodigo"] = new SelectList(_context.PreMaPrestamos, "PreCodigo", "PreCodigo", oeClCoejecutor.PreCodigo);
            return RedirectToAction("ResultTable");
            // return View(oeClCoejecutor);
        }
        // GET: OeClCoejecutor/Edit/5
        public async Task<IActionResult> Edit(decimal CoeCodigo)
        {
            var oeClCoejecutor = await _context.OeClCoejecutors.FindAsync(CoeCodigo);
            if (oeClCoejecutor == null)
            {
                return NotFound();
            }
            ViewData["PreCodigo"] = new SelectList(_context.PreMaPrestamos, "PreCodigo", "PreCodigo", oeClCoejecutor.PreCodigo);
            return View(oeClCoejecutor);
        }
        // POST: OeClCoejecutor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(decimal CoeCodigo, OeClCoejecutor oeClCoejecutor)
        {
            try
            {
                _context.Update(oeClCoejecutor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OeClCoejecutorExists(oeClCoejecutor.CoeCodigo))
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
            ViewData["PreCodigo"] = new SelectList(_context.PreMaPrestamos, "PreCodigo", "PreCodigo", oeClCoejecutor.PreCodigo);
            return RedirectToAction("ResultTable");
            //return View(oeClCoejecutor);
        }
        // GET: OeClCoejecutor/Delete/5
        public async Task<IActionResult>
            Delete(decimal CoeCodigo)
        {
            var oeClCoejecutor = await _context.OeClCoejecutors
            .FindAsync(CoeCodigo);
            if (oeClCoejecutor == null)
            {
                return NotFound();
            }
            return View(oeClCoejecutor);
        }
        // POST: OeClCoejecutor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(decimal CoeCodigo)
        {
            if (_context.OeClCoejecutors == null)
            {
                return Problem("Entity set 'DbvinDbContext.OeClCoejecutors'  is null.");
            }
            var oeClCoejecutor = await _context.OeClCoejecutors.FindAsync(CoeCodigo);
            if (oeClCoejecutor != null)
            {
                _context.OeClCoejecutors.Remove(oeClCoejecutor);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool OeClCoejecutorExists(decimal id)
        {
            return (_context.OeClCoejecutors?.Any(e => e.CoeCodigo == id)).GetValueOrDefault();
        }
    }
}
