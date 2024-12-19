using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
namespace SistemaBase.Controllers
{
    public class ParametrosGeneraleController : BaseRyMController
    {
        private readonly DbvinDbContext _context;
        public ParametrosGeneraleController(DbvinDbContext context, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _context = context;
        }
        // GET: ParametrosGenerale
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSPARAME", "Index", "ParametrosGenerale" })]

        public async Task<IActionResult> Index()
        {
            ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo");
            var dbvinDbContext = _context.ParametrosGenerales.Include(p => p.CodModuloNavigation);
            return View(await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo");
            ViewData["Show"] = true;
            var dbvinDbContext = _context.ParametrosGenerales.Include(p => p.CodModuloNavigation);
            return View("Index", await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        // GET: ParametrosGenerale/Details/5
        public async Task<IActionResult> Details(string Parametro, string CodModulo)
        {
            var parametrosGenerale = await _context.ParametrosGenerales
            .FindAsync(Parametro, CodModulo);
            if (parametrosGenerale == null)
            {
                return NotFound();
            }
            return View(parametrosGenerale);
        }
        // GET: ParametrosGenerale/Create
        public IActionResult Create()
        {
            ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo");
            return View();
        }
        // POST: ParametrosGenerale/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSPARAME", "Create", "ParametrosGenerale" })]

        public async Task<IActionResult> Create(ParametrosGenerale parametrosGenerale)
        {
            var existingParametro = _context.ParametrosGenerales.FirstOrDefault(p => p.Parametro == parametrosGenerale.Parametro && p.CodModulo== parametrosGenerale.CodModulo);
            if (existingParametro != null)
            {
                return Json(new { success = false, message = "Acceso de Grupo ya existe" });
            }
            else
            {
                _context.Add(parametrosGenerale);
                await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");
            }
            //return RedirectToAction(nameof(Index));
            ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo", parametrosGenerale.CodModulo);
            return RedirectToAction("ResultTable");
            // return View(parametrosGenerale);
        }
        // GET: ParametrosGenerale/Edit/5
        public async Task<IActionResult> Edit(string Parametro, string CodModulo)
        {
            var parametrosGenerale = await _context.ParametrosGenerales.FindAsync(Parametro, CodModulo);
            if (parametrosGenerale == null)
            {
                return NotFound();
            }
            ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo", parametrosGenerale.CodModulo);
            return View(parametrosGenerale);
        }
        // POST: ParametrosGenerale/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSPARAME", "Edit", "ParametrosGenerale" })]

        public async Task<IActionResult>
            Edit(string Parametro, string CodModulo, ParametrosGenerale parametrosGenerale)
        {
            try
            {
                _context.Update(parametrosGenerale);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParametrosGeneraleExists(parametrosGenerale.Parametro))
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
            ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo", parametrosGenerale.CodModulo);
            return RedirectToAction("ResultTable");
            //return View(parametrosGenerale);
        }
        // GET: ParametrosGenerale/Delete/5
        public async Task<IActionResult>
            Delete(string Parametro, string CodModulo)
        {
            var parametrosGenerale = await _context.ParametrosGenerales
            .FindAsync(Parametro, CodModulo);
            if (parametrosGenerale == null)
            {
                return NotFound();
            }
            return View(parametrosGenerale);
        }
        // POST: ParametrosGenerale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSPARAME", "Delete", "ParametrosGenerale" })]

        public async Task<IActionResult>
            DeleteConfirmed(string Parametro, string CodModulo)
        {
            if (_context.ParametrosGenerales == null)
            {
                return Problem("Entity set 'DbvinDbContext.ParametrosGenerales'  is null.");
            }
            var parametrosGenerale = await _context.ParametrosGenerales.FindAsync(Parametro, CodModulo);
            if (parametrosGenerale != null)
            {
                _context.ParametrosGenerales.Remove(parametrosGenerale);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool ParametrosGeneraleExists(string id)
        {
            return (_context.ParametrosGenerales?.Any(e => e.Parametro == id)).GetValueOrDefault();
        }
    }
}
