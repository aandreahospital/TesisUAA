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
    public class DptoController : Controller
    {
        private readonly DbvinDbContext _context;
        public DptoController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: Dpto
        public async Task<IActionResult> Index()
        {
            return _context.Dptos != null ?
              View(await _context.Dptos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Dptos'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.Dptos != null ?
              View("Index", await _context.Dptos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Dptos'  is null.");
        }

        // GET: Dpto/Details/5
        public async Task<IActionResult> Details(double CodDpto)
        {
            var dpto = await _context.Dptos
            .FindAsync(CodDpto);
            if (dpto == null)
            {
                return NotFound();
            }
            return View(dpto);
        }
        // GET: Dpto/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Dpto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dpto dpto)
        {
            _context.Add(dpto);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(dpto);
        }
        // GET: Dpto/Edit/5
        public async Task<IActionResult> Edit(double CodDpto)
        {
            var dpto = await _context.Dptos.FindAsync(CodDpto);
            if (dpto == null)
            {
                return NotFound();
            }
            return View(dpto);
        }
        // POST: Dpto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(double CodDpto, Dpto dpto)
        {
            try
            {
                _context.Update(dpto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DptoExists(dpto.CodDpto))
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
            //return View(dpto);
        }
        // GET: Dpto/Delete/5
        public async Task<IActionResult>
            Delete(double CodDpto)
        {
            var dpto = await _context.Dptos
            .FindAsync(CodDpto);
            if (dpto == null)
            {
                return NotFound();
            }
            return View(dpto);
        }
        // POST: Dpto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(double CodDpto)
        {
            if (_context.Dptos == null)
            {
                return Problem("Entity set 'DbvinDbContext.Dptos'  is null.");
            }
            var dpto = await _context.Dptos.FindAsync(CodDpto);
            if (dpto != null)
            {
                _context.Dptos.Remove(dpto);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool DptoExists(double id)
        {
            return (_context.Dptos?.Any(e => e.CodDpto == id)).GetValueOrDefault();
        }
    }
}
