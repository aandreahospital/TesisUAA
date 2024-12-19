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
    public class CalendarioController : Controller
    {
        private readonly DbvinDbContext _context;
        public CalendarioController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: Calendario
        public async Task<IActionResult> Index()
        {
            ViewData["CodEmpresa"] = new SelectList(_context.Empresas, "CodEmpresa", "CodEmpresa");
            ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo");
            var dbvinDbContext = _context.Calendarios.Include(c => c.CodEmpresaNavigation).Include(c => c.CodModuloNavigation);
            return View(await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["CodEmpresa"] = new SelectList(_context.Empresas, "CodEmpresa", "CodEmpresa");
            ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo");
            ViewData["Show"] = true;
            var dbvinDbContext = _context.Calendarios.Include(c => c.CodEmpresaNavigation).Include(c => c.CodModuloNavigation);
            return View("Index", await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        // GET: Calendario/Details/5
        public async Task<IActionResult> Details(string CodEmpresa, string CodModulo)
        {
            var calendario = await _context.Calendarios
            .FindAsync(CodEmpresa, CodModulo);
            if (calendario == null)
            {
                return NotFound();
            }
            return View(calendario);
        }
        // GET: Calendario/Create
        public IActionResult Create()
        {
            ViewData["CodEmpresa"] = new SelectList(_context.Empresas, "CodEmpresa", "CodEmpresa");
            ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo");
            return View();
        }
        // POST: Calendario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Calendario calendario)
        {
            _context.Add(calendario);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            ViewData["CodEmpresa"] = new SelectList(_context.Empresas, "CodEmpresa", "CodEmpresa", calendario.CodEmpresa);
            ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo", calendario.CodModulo);
            return RedirectToAction("ResultTable");
            // return View(calendario);
        }
        // GET: Calendario/Edit/5
        public async Task<IActionResult> Edit(string CodEmpresa, string CodModulo)
        {
            var calendario = await _context.Calendarios.FindAsync(CodEmpresa, CodModulo);
            if (calendario == null)
            {
                return NotFound();
            }
            ViewData["CodEmpresa"] = new SelectList(_context.Empresas, "CodEmpresa", "CodEmpresa", calendario.CodEmpresa);
            ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo", calendario.CodModulo);
            return View(calendario);
        }
        // POST: Calendario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodEmpresa, string CodModulo, Calendario calendario)
        {
            try
            {
                _context.Update(calendario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalendarioExists(calendario.CodEmpresa))
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
            ViewData["CodEmpresa"] = new SelectList(_context.Empresas, "CodEmpresa", "CodEmpresa", calendario.CodEmpresa);
            ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo", calendario.CodModulo);
            return RedirectToAction("ResultTable");
            //return View(calendario);
        }
        // GET: Calendario/Delete/5
        public async Task<IActionResult>
            Delete(string CodEmpresa, string CodModulo)
        {
            var calendario = await _context.Calendarios
            .FindAsync(CodEmpresa, CodModulo);
            if (calendario == null)
            {
                return NotFound();
            }
            return View(calendario);
        }
        // POST: Calendario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodEmpresa, string CodModulo)
        {
            if (_context.Calendarios == null)
            {
                return Problem("Entity set 'DbvinDbContext.Calendarios'  is null.");
            }
            var calendario = await _context.Calendarios.FindAsync(CodEmpresa, CodModulo);
            if (calendario != null)
            {
                _context.Calendarios.Remove(calendario);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool CalendarioExists(string id)
        {
            return (_context.Calendarios?.Any(e => e.CodEmpresa == id)).GetValueOrDefault();
        }
    }
}
