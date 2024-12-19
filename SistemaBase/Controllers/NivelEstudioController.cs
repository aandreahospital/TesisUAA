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
    public class NivelEstudioController : Controller
    {
        private readonly DbvinDbContext _context;
        public NivelEstudioController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: NivelEstudio
        public async Task<IActionResult> Index()
        {
            return _context.NivelEstudios != null ?
              View(await _context.NivelEstudios.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.NivelEstudios'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.NivelEstudios != null ?
              View("Index", await _context.NivelEstudios.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.NivelEstudios'  is null.");
        }

        // GET: NivelEstudio/Details/5
        public async Task<IActionResult> Details(string CodNivel)
        {
            var nivelEstudio = await _context.NivelEstudios
            .FindAsync(CodNivel);
            if (nivelEstudio == null)
            {
                return NotFound();
            }
            return View(nivelEstudio);
        }
        // GET: NivelEstudio/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: NivelEstudio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NivelEstudio nivelEstudio)
        {
            _context.Add(nivelEstudio);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(nivelEstudio);
        }
        // GET: NivelEstudio/Edit/5
        public async Task<IActionResult> Edit(string CodNivel)
        {
            var nivelEstudio = await _context.NivelEstudios.FindAsync(CodNivel);
            if (nivelEstudio == null)
            {
                return NotFound();
            }
            return View(nivelEstudio);
        }
        // POST: NivelEstudio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodNivel, NivelEstudio nivelEstudio)
        {
            try
            {
                _context.Update(nivelEstudio);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NivelEstudioExists(nivelEstudio.CodNivel))
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
            //return View(nivelEstudio);
        }
        // GET: NivelEstudio/Delete/5
        public async Task<IActionResult>
            Delete(string CodNivel)
        {
            var nivelEstudio = await _context.NivelEstudios
            .FindAsync(CodNivel);
            if (nivelEstudio == null)
            {
                return NotFound();
            }
            return View(nivelEstudio);
        }
        // POST: NivelEstudio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodNivel)
        {
            if (_context.NivelEstudios == null)
            {
                return Problem("Entity set 'DbvinDbContext.NivelEstudios'  is null.");
            }
            var nivelEstudio = await _context.NivelEstudios.FindAsync(CodNivel);
            if (nivelEstudio != null)
            {
                _context.NivelEstudios.Remove(nivelEstudio);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool NivelEstudioExists(string id)
        {
            return (_context.NivelEstudios?.Any(e => e.CodNivel == id)).GetValueOrDefault();
        }
    }
}
