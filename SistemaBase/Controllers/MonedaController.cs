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
    public class MonedaController : Controller
    {
        private readonly DbvinDbContext _context;
        public MonedaController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: Moneda
        public async Task<IActionResult> Index()
        {
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais");
            var dbvinDbContext = _context.Monedas.Include(m => m.CodPaisNavigation);
            return View(await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais");
            ViewData["Show"] = true;
            var dbvinDbContext = _context.Monedas.Include(m => m.CodPaisNavigation);
            return View("Index", await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        // GET: Moneda/Details/5
        public async Task<IActionResult> Details(string CodMoneda)
        {
            var moneda = await _context.Monedas
            .FindAsync(CodMoneda);
            if (moneda == null)
            {
                return NotFound();
            }
            return View(moneda);
        }
        // GET: Moneda/Create
        public IActionResult Create()
        {
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais");
            return View();
        }
        // POST: Moneda/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Moneda moneda)
        {
            _context.Add(moneda);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais", moneda.CodPais);
            return RedirectToAction("ResultTable");
            // return View(moneda);
        }
        // GET: Moneda/Edit/5
        public async Task<IActionResult> Edit(string CodMoneda)
        {
            var moneda = await _context.Monedas.FindAsync(CodMoneda);
            if (moneda == null)
            {
                return NotFound();
            }
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais", moneda.CodPais);
            return View(moneda);
        }
        // POST: Moneda/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodMoneda, Moneda moneda)
        {
            try
            {
                _context.Update(moneda);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonedaExists(moneda.CodMoneda))
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
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais", moneda.CodPais);
            return RedirectToAction("ResultTable");
            //return View(moneda);
        }
        // GET: Moneda/Delete/5
        public async Task<IActionResult>
            Delete(string CodMoneda)
        {
            var moneda = await _context.Monedas
            .FindAsync(CodMoneda);
            if (moneda == null)
            {
                return NotFound();
            }
            return View(moneda);
        }
        // POST: Moneda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodMoneda)
        {
            if (_context.Monedas == null)
            {
                return Problem("Entity set 'DbvinDbContext.Monedas'  is null.");
            }
            var moneda = await _context.Monedas.FindAsync(CodMoneda);
            if (moneda != null)
            {
                _context.Monedas.Remove(moneda);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool MonedaExists(string id)
        {
            return (_context.Monedas?.Any(e => e.CodMoneda == id)).GetValueOrDefault();
        }
    }
}
