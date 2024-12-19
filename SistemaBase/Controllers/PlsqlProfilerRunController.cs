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
    public class PlsqlProfilerRunController : Controller
    {
        private readonly DbvinDbContext _context;
        public PlsqlProfilerRunController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: PlsqlProfilerRun
        public async Task<IActionResult> Index()
        {
            return _context.PlsqlProfilerRuns != null ?
              View(await _context.PlsqlProfilerRuns.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.PlsqlProfilerRuns'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.PlsqlProfilerRuns != null ?
              View("Index", await _context.PlsqlProfilerRuns.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.PlsqlProfilerRuns'  is null.");
        }

        // GET: PlsqlProfilerRun/Details/5
        public async Task<IActionResult> Details(double Runid)
        {
            var plsqlProfilerRun = await _context.PlsqlProfilerRuns
            .FindAsync(Runid);
            if (plsqlProfilerRun == null)
            {
                return NotFound();
            }
            return View(plsqlProfilerRun);
        }
        // GET: PlsqlProfilerRun/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: PlsqlProfilerRun/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlsqlProfilerRun plsqlProfilerRun)
        {
            _context.Add(plsqlProfilerRun);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(plsqlProfilerRun);
        }
        // GET: PlsqlProfilerRun/Edit/5
        public async Task<IActionResult> Edit(double Runid)
        {
            var plsqlProfilerRun = await _context.PlsqlProfilerRuns.FindAsync(Runid);
            if (plsqlProfilerRun == null)
            {
                return NotFound();
            }
            return View(plsqlProfilerRun);
        }
        // POST: PlsqlProfilerRun/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(double Runid, PlsqlProfilerRun plsqlProfilerRun)
        {
            try
            {
                _context.Update(plsqlProfilerRun);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlsqlProfilerRunExists(plsqlProfilerRun.Runid))
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
            //return View(plsqlProfilerRun);
        }
        // GET: PlsqlProfilerRun/Delete/5
        public async Task<IActionResult>
            Delete(double Runid)
        {
            var plsqlProfilerRun = await _context.PlsqlProfilerRuns
            .FindAsync(Runid);
            if (plsqlProfilerRun == null)
            {
                return NotFound();
            }
            return View(plsqlProfilerRun);
        }
        // POST: PlsqlProfilerRun/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(double Runid)
        {
            if (_context.PlsqlProfilerRuns == null)
            {
                return Problem("Entity set 'DbvinDbContext.PlsqlProfilerRuns'  is null.");
            }
            var plsqlProfilerRun = await _context.PlsqlProfilerRuns.FindAsync(Runid);
            if (plsqlProfilerRun != null)
            {
                _context.PlsqlProfilerRuns.Remove(plsqlProfilerRun);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool PlsqlProfilerRunExists(double id)
        {
            return (_context.PlsqlProfilerRuns?.Any(e => e.Runid == id)).GetValueOrDefault();
        }
    }
}
