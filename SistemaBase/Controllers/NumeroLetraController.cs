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
    public class NumeroLetraController : Controller
    {
        private readonly DbvinDbContext _context;
        public NumeroLetraController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: NumeroLetra
        public async Task<IActionResult> Index()
        {
            return _context.NumeroLetras != null ?
              View(await _context.NumeroLetras.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.NumeroLetras'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.NumeroLetras != null ?
              View("Index", await _context.NumeroLetras.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.NumeroLetras'  is null.");
        }

        // GET: NumeroLetra/Details/5
        public async Task<IActionResult> Details(decimal Numero)
        {
            var numeroLetra = await _context.NumeroLetras
            .FindAsync(Numero);
            if (numeroLetra == null)
            {
                return NotFound();
            }
            return View(numeroLetra);
        }
        // GET: NumeroLetra/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: NumeroLetra/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NumeroLetra numeroLetra)
        {
            _context.Add(numeroLetra);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(numeroLetra);
        }
        // GET: NumeroLetra/Edit/5
        public async Task<IActionResult> Edit(decimal Numero)
        {
            var numeroLetra = await _context.NumeroLetras.FindAsync(Numero);
            if (numeroLetra == null)
            {
                return NotFound();
            }
            return View(numeroLetra);
        }
        // POST: NumeroLetra/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(decimal Numero, NumeroLetra numeroLetra)
        {
            try
            {
                _context.Update(numeroLetra);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NumeroLetraExists(numeroLetra.Numero))
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
            //return View(numeroLetra);
        }
        // GET: NumeroLetra/Delete/5
        public async Task<IActionResult>
            Delete(decimal Numero)
        {
            var numeroLetra = await _context.NumeroLetras
            .FindAsync(Numero);
            if (numeroLetra == null)
            {
                return NotFound();
            }
            return View(numeroLetra);
        }
        // POST: NumeroLetra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(decimal Numero)
        {
            if (_context.NumeroLetras == null)
            {
                return Problem("Entity set 'DbvinDbContext.NumeroLetras'  is null.");
            }
            var numeroLetra = await _context.NumeroLetras.FindAsync(Numero);
            if (numeroLetra != null)
            {
                _context.NumeroLetras.Remove(numeroLetra);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool NumeroLetraExists(decimal id)
        {
            return (_context.NumeroLetras?.Any(e => e.Numero == id)).GetValueOrDefault();
        }
    }
}
