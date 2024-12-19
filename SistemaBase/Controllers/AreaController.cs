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
    public class AreaController : Controller
    {
        private readonly DbvinDbContext _context;
        public AreaController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: Area
        public async Task<IActionResult> Index()
        {
            return _context.Areas != null ?
              View(await _context.Areas.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Areas'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.Areas != null ?
              View("Index", await _context.Areas.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Areas'  is null.");
        }

        // GET: Area/Details/5
        public async Task<IActionResult> Details(string CodEmpresa, string CodArea)
        {
            var area = await _context.Areas
            .FindAsync(CodEmpresa, CodArea);
            if (area == null)
            {
                return NotFound();
            }
            return View(area);
        }
        // GET: Area/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Area/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Area area)
        {
            _context.Add(area);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(area);
        }
        // GET: Area/Edit/5
        public async Task<IActionResult> Edit(string CodEmpresa, string CodArea)
        {
            var area = await _context.Areas.FindAsync(CodEmpresa, CodArea);
            if (area == null)
            {
                return NotFound();
            }
            return View(area);
        }
        // POST: Area/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodEmpresa, string CodArea, Area area)
        {
            try
            {
                _context.Update(area);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AreaExists(area.CodEmpresa))
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
            //return View(area);
        }
        // GET: Area/Delete/5
        public async Task<IActionResult>
            Delete(string CodEmpresa, string CodArea)
        {
            var area = await _context.Areas
            .FindAsync(CodEmpresa, CodArea);
            if (area == null)
            {
                return NotFound();
            }
            return View(area);
        }
        // POST: Area/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodEmpresa, string CodArea)
        {
            if (_context.Areas == null)
            {
                return Problem("Entity set 'DbvinDbContext.Areas'  is null.");
            }
            var area = await _context.Areas.FindAsync(CodEmpresa, CodArea);
            if (area != null)
            {
                _context.Areas.Remove(area);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool AreaExists(string id)
        {
            return (_context.Areas?.Any(e => e.CodEmpresa == id)).GetValueOrDefault();
        }
    }
}
