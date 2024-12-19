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
    public class FeriadoController : Controller
    {
        private readonly DbvinDbContext _context;
        public FeriadoController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: Feriado
        public async Task<IActionResult> Index()
        {
            return _context.Feriados != null ?
              View(await _context.Feriados.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Feriados'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.Feriados != null ?
              View("Index", await _context.Feriados.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Feriados'  is null.");
        }

        // GET: Feriado/Details/5
        public async Task<IActionResult> Details(DateTime Feriado1)
        {
            var feriado = await _context.Feriados
            .FindAsync(Feriado1);
            if (feriado == null)
            {
                return NotFound();
            }
            return View(feriado);
        }
        // GET: Feriado/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Feriado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feriado feriado)
        {
            _context.Add(feriado);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(feriado);
        }
        // GET: Feriado/Edit/5
        public async Task<IActionResult> Edit(DateTime Feriado1)
        {
            var feriado = await _context.Feriados.FindAsync(Feriado1);
            if (feriado == null)
            {
                return NotFound();
            }
            return View(feriado);
        }
        // POST: Feriado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(DateTime Feriado1, Feriado feriado)
        {
            try
            {
                _context.Update(feriado);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeriadoExists(feriado.Feriado1))
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
            //return View(feriado);
        }
        // GET: Feriado/Delete/5
        public async Task<IActionResult>
            Delete(DateTime Feriado1)
        {
            var feriado = await _context.Feriados
            .FindAsync(Feriado1);
            if (feriado == null)
            {
                return NotFound();
            }
            return View(feriado);
        }
        // POST: Feriado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(DateTime Feriado1)
        {
            if (_context.Feriados == null)
            {
                return Problem("Entity set 'DbvinDbContext.Feriados'  is null.");
            }
            var feriado = await _context.Feriados.FindAsync(Feriado1);
            if (feriado != null)
            {
                _context.Feriados.Remove(feriado);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool FeriadoExists(DateTime id)
        {
            return (_context.Feriados?.Any(e => e.Feriado1 == id)).GetValueOrDefault();
        }
    }
}
