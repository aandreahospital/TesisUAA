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
    public class PlsqlProfilerUnitController : Controller
    {
        private readonly DbvinDbContext _context;
        public PlsqlProfilerUnitController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: PlsqlProfilerUnit
        public async Task<IActionResult> Index()
        {
            ViewData["Runid"] = new SelectList(_context.PlsqlProfilerRuns, "Runid", "Runid");
            var dbvinDbContext = _context.PlsqlProfilerUnits.Include(p => p.Run);
            return View(await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Runid"] = new SelectList(_context.PlsqlProfilerRuns, "Runid", "Runid");
            ViewData["Show"] = true;
            var dbvinDbContext = _context.PlsqlProfilerUnits.Include(p => p.Run);
            return View("Index", await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        // GET: PlsqlProfilerUnit/Details/5
        public async Task<IActionResult> Details(double Runid, double UnitNumber)
        {
            var plsqlProfilerUnit = await _context.PlsqlProfilerUnits
            .FindAsync(Runid, UnitNumber);
            if (plsqlProfilerUnit == null)
            {
                return NotFound();
            }
            return View(plsqlProfilerUnit);
        }
        // GET: PlsqlProfilerUnit/Create
        public IActionResult Create()
        {
            ViewData["Runid"] = new SelectList(_context.PlsqlProfilerRuns, "Runid", "Runid");
            return View();
        }
        // POST: PlsqlProfilerUnit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlsqlProfilerUnit plsqlProfilerUnit)
        {
            _context.Add(plsqlProfilerUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            ViewData["Runid"] = new SelectList(_context.PlsqlProfilerRuns, "Runid", "Runid", plsqlProfilerUnit.Runid);
            return RedirectToAction("ResultTable");
            // return View(plsqlProfilerUnit);
        }
        // GET: PlsqlProfilerUnit/Edit/5
        public async Task<IActionResult> Edit(double Runid, double UnitNumber)
        {
            var plsqlProfilerUnit = await _context.PlsqlProfilerUnits.FindAsync(Runid, UnitNumber);
            if (plsqlProfilerUnit == null)
            {
                return NotFound();
            }
            ViewData["Runid"] = new SelectList(_context.PlsqlProfilerRuns, "Runid", "Runid", plsqlProfilerUnit.Runid);
            return View(plsqlProfilerUnit);
        }
        // POST: PlsqlProfilerUnit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(double Runid, double UnitNumber, PlsqlProfilerUnit plsqlProfilerUnit)
        {
            try
            {
                _context.Update(plsqlProfilerUnit);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlsqlProfilerUnitExists(plsqlProfilerUnit.Runid))
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
            ViewData["Runid"] = new SelectList(_context.PlsqlProfilerRuns, "Runid", "Runid", plsqlProfilerUnit.Runid);
            return RedirectToAction("ResultTable");
            //return View(plsqlProfilerUnit);
        }
        // GET: PlsqlProfilerUnit/Delete/5
        public async Task<IActionResult>
            Delete(double Runid, double UnitNumber)
        {
            var plsqlProfilerUnit = await _context.PlsqlProfilerUnits
            .FindAsync(Runid, UnitNumber);
            if (plsqlProfilerUnit == null)
            {
                return NotFound();
            }
            return View(plsqlProfilerUnit);
        }
        // POST: PlsqlProfilerUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(double Runid, double UnitNumber)
        {
            if (_context.PlsqlProfilerUnits == null)
            {
                return Problem("Entity set 'DbvinDbContext.PlsqlProfilerUnits'  is null.");
            }
            var plsqlProfilerUnit = await _context.PlsqlProfilerUnits.FindAsync(Runid, UnitNumber);
            if (plsqlProfilerUnit != null)
            {
                _context.PlsqlProfilerUnits.Remove(plsqlProfilerUnit);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool PlsqlProfilerUnitExists(double id)
        {
            return (_context.PlsqlProfilerUnits?.Any(e => e.Runid == id)).GetValueOrDefault();
        }
    }
}
