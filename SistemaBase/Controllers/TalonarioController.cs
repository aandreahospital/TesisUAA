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
    public class TalonarioController : Controller
    {
        private readonly DbvinDbContext _context;
        public TalonarioController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: Talonario
        public async Task<IActionResult> Index()
        {
            return _context.Talonarios != null ?
              View(await _context.Talonarios.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Talonarios'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.Talonarios != null ?
              View("Index", await _context.Talonarios.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Talonarios'  is null.");
        }

        // GET: Talonario/Details/5
        public async Task<IActionResult> Details(string CodEmpresa, string TipTalonario, decimal NroTalonario)
        {
            var talonario = await _context.Talonarios
            .FindAsync(CodEmpresa, TipTalonario, NroTalonario);
            if (talonario == null)
            {
                return NotFound();
            }
            return View(talonario);
        }
        // GET: Talonario/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Talonario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Talonario talonario)
        {
            _context.Add(talonario);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(talonario);
        }
        // GET: Talonario/Edit/5
        public async Task<IActionResult> Edit(string CodEmpresa, string TipTalonario, decimal NroTalonario)
        {
            var talonario = await _context.Talonarios.FindAsync(CodEmpresa, TipTalonario, NroTalonario);
            if (talonario == null)
            {
                return NotFound();
            }
            return View(talonario);
        }
        // POST: Talonario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodEmpresa, string TipTalonario, decimal NroTalonario, Talonario talonario)
        {
            try
            {
                _context.Update(talonario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TalonarioExists(talonario.CodEmpresa))
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
            //return View(talonario);
        }
        // GET: Talonario/Delete/5
        public async Task<IActionResult>
            Delete(string CodEmpresa, string TipTalonario, decimal NroTalonario)
        {
            var talonario = await _context.Talonarios
            .FindAsync(CodEmpresa, TipTalonario, NroTalonario);
            if (talonario == null)
            {
                return NotFound();
            }
            return View(talonario);
        }
        // POST: Talonario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodEmpresa, string TipTalonario, decimal NroTalonario)
        {
            if (_context.Talonarios == null)
            {
                return Problem("Entity set 'DbvinDbContext.Talonarios'  is null.");
            }
            var talonario = await _context.Talonarios.FindAsync(CodEmpresa, TipTalonario, NroTalonario);
            if (talonario != null)
            {
                _context.Talonarios.Remove(talonario);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool TalonarioExists(string id)
        {
            return (_context.Talonarios?.Any(e => e.CodEmpresa == id)).GetValueOrDefault();
        }
    }
}
