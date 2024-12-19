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
    public class AvFinalidadeController : Controller
    {
        private readonly DbvinDbContext _context;
        public AvFinalidadeController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: AvFinalidade
        public async Task<IActionResult> Index()
        {
            return _context.AvFinalidades != null ?
              View(await _context.AvFinalidades.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.AvFinalidades'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.AvFinalidades != null ?
              View("Index", await _context.AvFinalidades.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.AvFinalidades'  is null.");
        }

        // GET: AvFinalidade/Details/5
        public async Task<IActionResult> Details(string CodFinalidad)
        {
            var avFinalidade = await _context.AvFinalidades
            .FindAsync(CodFinalidad);
            if (avFinalidade == null)
            {
                return NotFound();
            }
            return View(avFinalidade);
        }
        // GET: AvFinalidade/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: AvFinalidade/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AvFinalidade avFinalidade)
        {
            _context.Add(avFinalidade);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(avFinalidade);
        }
        // GET: AvFinalidade/Edit/5
        public async Task<IActionResult> Edit(string CodFinalidad)
        {
            var avFinalidade = await _context.AvFinalidades.FindAsync(CodFinalidad);
            if (avFinalidade == null)
            {
                return NotFound();
            }
            return View(avFinalidade);
        }
        // POST: AvFinalidade/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodFinalidad, AvFinalidade avFinalidade)
        {
            try
            {
                _context.Update(avFinalidade);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvFinalidadeExists(avFinalidade.CodFinalidad))
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
            //return View(avFinalidade);
        }
        // GET: AvFinalidade/Delete/5
        public async Task<IActionResult>
            Delete(string CodFinalidad)
        {
            var avFinalidade = await _context.AvFinalidades
            .FindAsync(CodFinalidad);
            if (avFinalidade == null)
            {
                return NotFound();
            }
            return View(avFinalidade);
        }
        // POST: AvFinalidade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodFinalidad)
        {
            if (_context.AvFinalidades == null)
            {
                return Problem("Entity set 'DbvinDbContext.AvFinalidades'  is null.");
            }
            var avFinalidade = await _context.AvFinalidades.FindAsync(CodFinalidad);
            if (avFinalidade != null)
            {
                _context.AvFinalidades.Remove(avFinalidade);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool AvFinalidadeExists(string id)
        {
            return (_context.AvFinalidades?.Any(e => e.CodFinalidad == id)).GetValueOrDefault();
        }
    }
}
