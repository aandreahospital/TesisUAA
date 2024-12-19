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
    public class PlsqlProfilerDatumController : Controller
    {
        private readonly DbvinDbContext _context;
        public PlsqlProfilerDatumController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: PlsqlProfilerDatum
        public async Task<IActionResult> Index()
        {
            ViewData["Runid"] = new SelectList(_context.PlsqlProfilerUnits, "Runid", "Runid");
            var dbvinDbContext = _context.PlsqlProfilerData.Include(p => p.PlsqlProfilerUnit);
            return View(await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Runid"] = new SelectList(_context.PlsqlProfilerUnits, "Runid", "Runid");
            ViewData["Show"] = true;
            var dbvinDbContext = _context.PlsqlProfilerData.Include(p => p.PlsqlProfilerUnit);
            return View("Index", await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        // GET: PlsqlProfilerDatum/Details/5
        public async Task<IActionResult> Details(double Runid, double UnitNumber, double Line)
        {
            var plsqlProfilerDatum = await _context.PlsqlProfilerData
            .FindAsync(Runid, UnitNumber, Line);
            if (plsqlProfilerDatum == null)
            {
                return NotFound();
            }
            return View(plsqlProfilerDatum);
        }
        // GET: PlsqlProfilerDatum/Create
        public IActionResult Create()
        {
            ViewData["Runid"] = new SelectList(_context.PlsqlProfilerUnits, "Runid", "Runid");
            return View();
        }
        // POST: PlsqlProfilerDatum/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlsqlProfilerDatum plsqlProfilerDatum)
        {
            _context.Add(plsqlProfilerDatum);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            ViewData["Runid"] = new SelectList(_context.PlsqlProfilerUnits, "Runid", "Runid", plsqlProfilerDatum.Runid);
            return RedirectToAction("ResultTable");
            // return View(plsqlProfilerDatum);
        }
        // GET: PlsqlProfilerDatum/Edit/5
        public async Task<IActionResult> Edit(double Runid, double UnitNumber, double Line)
        {
            var plsqlProfilerDatum = await _context.PlsqlProfilerData.FindAsync(Runid, UnitNumber, Line);
            if (plsqlProfilerDatum == null)
            {
                return NotFound();
            }
            ViewData["Runid"] = new SelectList(_context.PlsqlProfilerUnits, "Runid", "Runid", plsqlProfilerDatum.Runid);
            return View(plsqlProfilerDatum);
        }
        // POST: PlsqlProfilerDatum/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(double Runid, double UnitNumber, double Line, PlsqlProfilerDatum plsqlProfilerDatum)
        {
            try
            {
                _context.Update(plsqlProfilerDatum);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlsqlProfilerDatumExists(plsqlProfilerDatum.Runid))
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
            ViewData["Runid"] = new SelectList(_context.PlsqlProfilerUnits, "Runid", "Runid", plsqlProfilerDatum.Runid);
            return RedirectToAction("ResultTable");
            //return View(plsqlProfilerDatum);
        }
        // GET: PlsqlProfilerDatum/Delete/5
        public async Task<IActionResult>
            Delete(double Runid, double UnitNumber, double Line)
        {
            var plsqlProfilerDatum = await _context.PlsqlProfilerData
            .FindAsync(Runid, UnitNumber, Line);
            if (plsqlProfilerDatum == null)
            {
                return NotFound();
            }
            return View(plsqlProfilerDatum);
        }
        // POST: PlsqlProfilerDatum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(double Runid, double UnitNumber, double Line)
        {
            if (_context.PlsqlProfilerData == null)
            {
                return Problem("Entity set 'DbvinDbContext.PlsqlProfilerData'  is null.");
            }
            var plsqlProfilerDatum = await _context.PlsqlProfilerData.FindAsync(Runid, UnitNumber, Line);
            if (plsqlProfilerDatum != null)
            {
                _context.PlsqlProfilerData.Remove(plsqlProfilerDatum);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool PlsqlProfilerDatumExists(double id)
        {
            return (_context.PlsqlProfilerData?.Any(e => e.Runid == id)).GetValueOrDefault();
        }
    }
}
