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
    public class PaiController : Controller
    {
        private readonly DbvinDbContext _context;
        public PaiController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: Pai
        public async Task<IActionResult> Index()
        {
            return _context.Pais != null ?
              View(await _context.Pais.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Pais'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.Pais != null ?
              View("Index", await _context.Pais.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Pais'  is null.");
        }

        // GET: Pai/Details/5
        public async Task<IActionResult> Details(string CodPais)
        {
            var pai = await _context.Pais
            .FindAsync(CodPais);
            if (pai == null)
            {
                return NotFound();
            }
            return View(pai);
        }
        // GET: Pai/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Pai/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pai pai)
        {
            _context.Add(pai);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(pai);
        }
        // GET: Pai/Edit/5
        public async Task<IActionResult> Edit(string CodPais)
        {
            var pai = await _context.Pais.FindAsync(CodPais);
            if (pai == null)
            {
                return NotFound();
            }
            return View(pai);
        }
        // POST: Pai/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodPais, Pai pai)
        {
            try
            {
                _context.Update(pai);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaiExists(pai.CodPais))
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
            //return View(pai);
        }
        // GET: Pai/Delete/5
        public async Task<IActionResult>
            Delete(string CodPais)
        {
            var pai = await _context.Pais
            .FindAsync(CodPais);
            if (pai == null)
            {
                return NotFound();
            }
            return View(pai);
        }
        // POST: Pai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodPais)
        {
            if (_context.Pais == null)
            {
                return Problem("Entity set 'DbvinDbContext.Pais'  is null.");
            }
            var pai = await _context.Pais.FindAsync(CodPais);
            if (pai != null)
            {
                _context.Pais.Remove(pai);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool PaiExists(string id)
        {
            return (_context.Pais?.Any(e => e.CodPais == id)).GetValueOrDefault();
        }
    }
}
