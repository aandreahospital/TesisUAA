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
    public class ActividadesEconController : Controller
    {
        private readonly DbvinDbContext _context;
        public ActividadesEconController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: ActividadesEcon
        public async Task<IActionResult> Index()
        {
            return _context.ActividadesEcons != null ?
              View(await _context.ActividadesEcons.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.ActividadesEcons'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.ActividadesEcons != null ?
              View("Index", await _context.ActividadesEcons.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.ActividadesEcons'  is null.");
        }

        // GET: ActividadesEcon/Details/5
        public async Task<IActionResult> Details(string CodActividad)
        {
            var actividadesEcon = await _context.ActividadesEcons
            .FindAsync(CodActividad);
            if (actividadesEcon == null)
            {
                return NotFound();
            }
            return View(actividadesEcon);
        }
        // GET: ActividadesEcon/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: ActividadesEcon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActividadesEcon actividadesEcon)
        {
            _context.Add(actividadesEcon);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(actividadesEcon);
        }
        // GET: ActividadesEcon/Edit/5
        public async Task<IActionResult> Edit(string CodActividad)
        {
            var actividadesEcon = await _context.ActividadesEcons.FindAsync(CodActividad);
            if (actividadesEcon == null)
            {
                return NotFound();
            }
            return View(actividadesEcon);
        }
        // POST: ActividadesEcon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodActividad, ActividadesEcon actividadesEcon)
        {
            try
            {
                _context.Update(actividadesEcon);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActividadesEconExists(actividadesEcon.CodActividad))
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
            //return View(actividadesEcon);
        }
        // GET: ActividadesEcon/Delete/5
        public async Task<IActionResult>
            Delete(string CodActividad)
        {
            var actividadesEcon = await _context.ActividadesEcons
            .FindAsync(CodActividad);
            if (actividadesEcon == null)
            {
                return NotFound();
            }
            return View(actividadesEcon);
        }
        // POST: ActividadesEcon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodActividad)
        {
            if (_context.ActividadesEcons == null)
            {
                return Problem("Entity set 'DbvinDbContext.ActividadesEcons'  is null.");
            }
            var actividadesEcon = await _context.ActividadesEcons.FindAsync(CodActividad);
            if (actividadesEcon != null)
            {
                _context.ActividadesEcons.Remove(actividadesEcon);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool ActividadesEconExists(string id)
        {
            return (_context.ActividadesEcons?.Any(e => e.CodActividad == id)).GetValueOrDefault();
        }
    }
}
