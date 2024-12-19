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
    public class AreasUsuarioController : Controller
    {
        private readonly DbvinDbContext _context;
        public AreasUsuarioController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: AreasUsuario
        public async Task<IActionResult> Index()
        {
            return _context.AreasUsuarios != null ?
              View(await _context.AreasUsuarios.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.AreasUsuarios'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.AreasUsuarios != null ?
              View("Index", await _context.AreasUsuarios.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.AreasUsuarios'  is null.");
        }

        // GET: AreasUsuario/Details/5
        public async Task<IActionResult> Details(string CodEmpresa, string CodArea, string CodUsuario)
        {
            var areasUsuario = await _context.AreasUsuarios
            .FindAsync(CodEmpresa, CodArea, CodUsuario);
            if (areasUsuario == null)
            {
                return NotFound();
            }
            return View(areasUsuario);
        }
        // GET: AreasUsuario/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: AreasUsuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AreasUsuario areasUsuario)
        {
            _context.Add(areasUsuario);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(areasUsuario);
        }
        // GET: AreasUsuario/Edit/5
        public async Task<IActionResult> Edit(string CodEmpresa, string CodArea, string CodUsuario)
        {
            var areasUsuario = await _context.AreasUsuarios.FindAsync(CodEmpresa, CodArea, CodUsuario);
            if (areasUsuario == null)
            {
                return NotFound();
            }
            return View(areasUsuario);
        }
        // POST: AreasUsuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodEmpresa, string CodArea, string CodUsuario, AreasUsuario areasUsuario)
        {
            try
            {
                _context.Update(areasUsuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AreasUsuarioExists(areasUsuario.CodEmpresa))
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
            //return View(areasUsuario);
        }
        // GET: AreasUsuario/Delete/5
        public async Task<IActionResult>
            Delete(string CodEmpresa, string CodArea, string CodUsuario)
        {
            var areasUsuario = await _context.AreasUsuarios
            .FindAsync(CodEmpresa, CodArea, CodUsuario);
            if (areasUsuario == null)
            {
                return NotFound();
            }
            return View(areasUsuario);
        }
        // POST: AreasUsuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodEmpresa, string CodArea, string CodUsuario)
        {
            if (_context.AreasUsuarios == null)
            {
                return Problem("Entity set 'DbvinDbContext.AreasUsuarios'  is null.");
            }
            var areasUsuario = await _context.AreasUsuarios.FindAsync(CodEmpresa, CodArea, CodUsuario);
            if (areasUsuario != null)
            {
                _context.AreasUsuarios.Remove(areasUsuario);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool AreasUsuarioExists(string id)
        {
            return (_context.AreasUsuarios?.Any(e => e.CodEmpresa == id)).GetValueOrDefault();
        }
    }
}
