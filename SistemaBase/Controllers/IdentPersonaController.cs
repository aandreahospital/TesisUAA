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
    public class IdentPersonaController : Controller
    {
        private readonly DbvinDbContext _context;
        public IdentPersonaController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: IdentPersona
        public async Task<IActionResult> Index()
        {
            ViewData["CodIdent"] = new SelectList(_context.Identificaciones, "CodIdent", "CodIdent");
            var dbvinDbContext = _context.IdentPersonas.Include(i => i.CodIdentNavigation);
            return View(await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["CodIdent"] = new SelectList(_context.Identificaciones, "CodIdent", "CodIdent");
            ViewData["Show"] = true;
            var dbvinDbContext = _context.IdentPersonas.Include(i => i.CodIdentNavigation);
            return View("Index", await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        // GET: IdentPersona/Details/5
        public async Task<IActionResult> Details(string CodPersona, string CodIdent)
        {
            var identPersona = await _context.IdentPersonas
            .FindAsync(CodPersona, CodIdent);
            if (identPersona == null)
            {
                return NotFound();
            }
            return View(identPersona);
        }
        // GET: IdentPersona/Create
        public IActionResult Create()
        {
            ViewData["CodIdent"] = new SelectList(_context.Identificaciones, "CodIdent", "CodIdent");
            return View();
        }
        // POST: IdentPersona/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentPersona identPersona)
        {
            _context.Add(identPersona);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            ViewData["CodIdent"] = new SelectList(_context.Identificaciones, "CodIdent", "CodIdent", identPersona.CodIdent);
            return RedirectToAction("ResultTable");
            // return View(identPersona);
        }
        // GET: IdentPersona/Edit/5
        public async Task<IActionResult> Edit(string CodPersona, string CodIdent)
        {
            var identPersona = await _context.IdentPersonas.FindAsync(CodPersona, CodIdent);
            if (identPersona == null)
            {
                return NotFound();
            }
            ViewData["CodIdent"] = new SelectList(_context.Identificaciones, "CodIdent", "CodIdent", identPersona.CodIdent);
            return View(identPersona);
        }
        // POST: IdentPersona/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodPersona, string CodIdent, IdentPersona identPersona)
        {
            try
            {
                _context.Update(identPersona);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdentPersonaExists(identPersona.CodPersona))
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
            ViewData["CodIdent"] = new SelectList(_context.Identificaciones, "CodIdent", "CodIdent", identPersona.CodIdent);
            return RedirectToAction("ResultTable");
            //return View(identPersona);
        }
        // GET: IdentPersona/Delete/5
        public async Task<IActionResult>
            Delete(string CodPersona, string CodIdent)
        {
            var identPersona = await _context.IdentPersonas
            .FindAsync(CodPersona, CodIdent);
            if (identPersona == null)
            {
                return NotFound();
            }
            return View(identPersona);
        }
        // POST: IdentPersona/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodPersona, string CodIdent)
        {
            if (_context.IdentPersonas == null)
            {
                return Problem("Entity set 'DbvinDbContext.IdentPersonas'  is null.");
            }
            var identPersona = await _context.IdentPersonas.FindAsync(CodPersona, CodIdent);
            if (identPersona != null)
            {
                _context.IdentPersonas.Remove(identPersona);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool IdentPersonaExists(string id)
        {
            return (_context.IdentPersonas?.Any(e => e.CodPersona == id)).GetValueOrDefault();
        }
    }
}
