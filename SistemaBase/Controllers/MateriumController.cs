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
    public class MateriumController : Controller
    {
        private readonly DbvinDbContext _context;
        public MateriumController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: Materium
        public async Task<IActionResult> Index()
        {
            return _context.Materia != null ?
              View(await _context.Materia.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Materia'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.Materia != null ?
              View("Index", await _context.Materia.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Materia'  is null.");
        }

        // GET: Materium/Details/5
        public async Task<IActionResult> Details(double IdMateria)
        {
            var materium = await _context.Materia
            .FindAsync(IdMateria);
            if (materium == null)
            {
                return NotFound();
            }
            return View(materium);
        }
        // GET: Materium/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Materium/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Materium materium)
        {
            _context.Add(materium);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(materium);
        }
        // GET: Materium/Edit/5
        public async Task<IActionResult> Edit(double IdMateria)
        {
            var materium = await _context.Materia.FindAsync(IdMateria);
            if (materium == null)
            {
                return NotFound();
            }
            return View(materium);
        }
        // POST: Materium/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(double IdMateria, Materium materium)
        {
            try
            {
                _context.Update(materium);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MateriumExists(materium.IdMateria))
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
            //return View(materium);
        }
        // GET: Materium/Delete/5
        public async Task<IActionResult>
            Delete(double IdMateria)
        {
            var materium = await _context.Materia
            .FindAsync(IdMateria);
            if (materium == null)
            {
                return NotFound();
            }
            return View(materium);
        }
        // POST: Materium/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(double IdMateria)
        {
            if (_context.Materia == null)
            {
                return Problem("Entity set 'DbvinDbContext.Materia'  is null.");
            }
            var materium = await _context.Materia.FindAsync(IdMateria);
            if (materium != null)
            {
                _context.Materia.Remove(materium);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool MateriumExists(double id)
        {
            return (_context.Materia?.Any(e => e.IdMateria == id)).GetValueOrDefault();
        }
    }
}
