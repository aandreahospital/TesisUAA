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
    public class AvZonaController : Controller
    {
        private readonly DbvinDbContext _context;
        public AvZonaController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: AvZona
        public async Task<IActionResult> Index()
        {
            return _context.AvZonas != null ?
              View(await _context.AvZonas.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.AvZonas'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.AvZonas != null ?
              View("Index", await _context.AvZonas.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.AvZonas'  is null.");
        }

        // GET: AvZona/Details/5
        public async Task<IActionResult> Details(string CodPais, string CodDepartamento, string CodZona)
        {
            var avZona = await _context.AvZonas
            .FindAsync(CodPais, CodDepartamento, CodZona);
            if (avZona == null)
            {
                return NotFound();
            }
            return View(avZona);
        }
        // GET: AvZona/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: AvZona/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AvZona avZona)
        {
            _context.Add(avZona);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(avZona);
        }
        // GET: AvZona/Edit/5
        public async Task<IActionResult> Edit(string CodPais, string CodDepartamento, string CodZona)
        {
            var avZona = await _context.AvZonas.FindAsync(CodPais, CodDepartamento, CodZona);
            if (avZona == null)
            {
                return NotFound();
            }
            return View(avZona);
        }
        // POST: AvZona/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodPais, string CodDepartamento, string CodZona, AvZona avZona)
        {
            try
            {
                _context.Update(avZona);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvZonaExists(avZona.CodPais))
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
            //return View(avZona);
        }
        // GET: AvZona/Delete/5
        public async Task<IActionResult>
            Delete(string CodPais, string CodDepartamento, string CodZona)
        {
            var avZona = await _context.AvZonas
            .FindAsync(CodPais, CodDepartamento, CodZona);
            if (avZona == null)
            {
                return NotFound();
            }
            return View(avZona);
        }
        // POST: AvZona/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodPais, string CodDepartamento, string CodZona)
        {
            if (_context.AvZonas == null)
            {
                return Problem("Entity set 'DbvinDbContext.AvZonas'  is null.");
            }
            var avZona = await _context.AvZonas.FindAsync(CodPais, CodDepartamento, CodZona);
            if (avZona != null)
            {
                _context.AvZonas.Remove(avZona);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool AvZonaExists(string id)
        {
            return (_context.AvZonas?.Any(e => e.CodPais == id)).GetValueOrDefault();
        }
    }
}
