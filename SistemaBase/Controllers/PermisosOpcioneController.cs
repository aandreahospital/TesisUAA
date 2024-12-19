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
    public class PermisosOpcioneController : Controller
    {
        private readonly DbvinDbContext _context;
        public PermisosOpcioneController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: PermisosOpcione
        public async Task<IActionResult> Index()
        {
            return _context.PermisosOpciones != null ?
              View(await _context.PermisosOpciones.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.PermisosOpciones'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.PermisosOpciones != null ?
              View("Index", await _context.PermisosOpciones.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.PermisosOpciones'  is null.");
        }

        // GET: PermisosOpcione/Details/5
        public async Task<IActionResult> Details(string CodEmpresa, string CodUsuario, string CodModulo, string Parametro, string NomForma)
        {
            var permisosOpcione = await _context.PermisosOpciones
            .FindAsync(CodEmpresa, CodUsuario, CodModulo, Parametro, NomForma);
            if (permisosOpcione == null)
            {
                return NotFound();
            }
            return View(permisosOpcione);
        }
        // GET: PermisosOpcione/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: PermisosOpcione/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PermisosOpcione permisosOpcione)
        {
            _context.Add(permisosOpcione);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(permisosOpcione);
        }
        // GET: PermisosOpcione/Edit/5
        public async Task<IActionResult> Edit(string CodEmpresa, string CodUsuario, string CodModulo, string Parametro, string NomForma)
        {
            var permisosOpcione = await _context.PermisosOpciones.FindAsync(CodEmpresa, CodUsuario, CodModulo, Parametro, NomForma);
            if (permisosOpcione == null)
            {
                return NotFound();
            }
            return View(permisosOpcione);
        }
        // POST: PermisosOpcione/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodEmpresa, string CodUsuario, string CodModulo, string Parametro, string NomForma, PermisosOpcione permisosOpcione)
        {
            try
            {
                _context.Update(permisosOpcione);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermisosOpcioneExists(permisosOpcione.CodEmpresa))
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
            //return View(permisosOpcione);
        }
        // GET: PermisosOpcione/Delete/5
        public async Task<IActionResult>
            Delete(string CodEmpresa, string CodUsuario, string CodModulo, string Parametro, string NomForma)
        {
            var permisosOpcione = await _context.PermisosOpciones
            .FindAsync(CodEmpresa, CodUsuario, CodModulo, Parametro, NomForma);
            if (permisosOpcione == null)
            {
                return NotFound();
            }
            return View(permisosOpcione);
        }
        // POST: PermisosOpcione/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodEmpresa, string CodUsuario, string CodModulo, string Parametro, string NomForma)
        {
            if (_context.PermisosOpciones == null)
            {
                return Problem("Entity set 'DbvinDbContext.PermisosOpciones'  is null.");
            }
            var permisosOpcione = await _context.PermisosOpciones.FindAsync(CodEmpresa, CodUsuario, CodModulo, Parametro, NomForma);
            if (permisosOpcione != null)
            {
                _context.PermisosOpciones.Remove(permisosOpcione);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool PermisosOpcioneExists(string id)
        {
            return (_context.PermisosOpciones?.Any(e => e.CodEmpresa == id)).GetValueOrDefault();
        }
    }
}
