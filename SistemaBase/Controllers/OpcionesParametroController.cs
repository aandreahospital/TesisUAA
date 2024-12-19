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
    public class OpcionesParametroController : Controller
    {
        private readonly DbvinDbContext _context;
        public OpcionesParametroController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: OpcionesParametro
        public async Task<IActionResult> Index()
        {
            return _context.OpcionesParametros != null ?
              View(await _context.OpcionesParametros.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.OpcionesParametros'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.OpcionesParametros != null ?
              View("Index", await _context.OpcionesParametros.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.OpcionesParametros'  is null.");
        }

        // GET: OpcionesParametro/Details/5
        public async Task<IActionResult> Details(string Tipo, string Parametro, string NomForma)
        {
            var opcionesParametro = await _context.OpcionesParametros
            .FindAsync(Tipo, Parametro, NomForma);
            if (opcionesParametro == null)
            {
                return NotFound();
            }
            return View(opcionesParametro);
        }
        // GET: OpcionesParametro/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: OpcionesParametro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OpcionesParametro opcionesParametro)
        {
            _context.Add(opcionesParametro);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(opcionesParametro);
        }
        // GET: OpcionesParametro/Edit/5
        public async Task<IActionResult> Edit(string Tipo, string Parametro, string NomForma)
        {
            var opcionesParametro = await _context.OpcionesParametros.FindAsync(Tipo, Parametro, NomForma);
            if (opcionesParametro == null)
            {
                return NotFound();
            }
            return View(opcionesParametro);
        }
        // POST: OpcionesParametro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string Tipo, string Parametro, string NomForma, OpcionesParametro opcionesParametro)
        {
            try
            {
                _context.Update(opcionesParametro);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpcionesParametroExists(opcionesParametro.Tipo))
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
            //return View(opcionesParametro);
        }
        // GET: OpcionesParametro/Delete/5
        public async Task<IActionResult>
            Delete(string Tipo, string Parametro, string NomForma)
        {
            var opcionesParametro = await _context.OpcionesParametros
            .FindAsync(Tipo, Parametro, NomForma);
            if (opcionesParametro == null)
            {
                return NotFound();
            }
            return View(opcionesParametro);
        }
        // POST: OpcionesParametro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string Tipo, string Parametro, string NomForma)
        {
            if (_context.OpcionesParametros == null)
            {
                return Problem("Entity set 'DbvinDbContext.OpcionesParametros'  is null.");
            }
            var opcionesParametro = await _context.OpcionesParametros.FindAsync(Tipo, Parametro, NomForma);
            if (opcionesParametro != null)
            {
                _context.OpcionesParametros.Remove(opcionesParametro);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool OpcionesParametroExists(string id)
        {
            return (_context.OpcionesParametros?.Any(e => e.Tipo == id)).GetValueOrDefault();
        }
    }
}
