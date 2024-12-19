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
    public class EmpresaController : Controller
    {
        private readonly DbvinDbContext _context;
        public EmpresaController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: Empresa
        public async Task<IActionResult> Index()
        {
            return _context.Empresas != null ?
              View(await _context.Empresas.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Empresas'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.Empresas != null ?
              View("Index", await _context.Empresas.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Empresas'  is null.");
        }

        // GET: Empresa/Details/5
        public async Task<IActionResult> Details(string CodEmpresa)
        {
            var empresa = await _context.Empresas
            .FindAsync(CodEmpresa);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }
        // GET: Empresa/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Empresa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empresa empresa)
        {
            _context.Add(empresa);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(empresa);
        }
        // GET: Empresa/Edit/5
        public async Task<IActionResult> Edit(string CodEmpresa)
        {
            var empresa = await _context.Empresas.FindAsync(CodEmpresa);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }
        // POST: Empresa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodEmpresa, Empresa empresa)
        {
            try
            {
                _context.Update(empresa);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpresaExists(empresa.CodEmpresa))
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
            //return View(empresa);
        }
        // GET: Empresa/Delete/5
        public async Task<IActionResult>
            Delete(string CodEmpresa)
        {
            var empresa = await _context.Empresas
            .FindAsync(CodEmpresa);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }
        // POST: Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodEmpresa)
        {
            if (_context.Empresas == null)
            {
                return Problem("Entity set 'DbvinDbContext.Empresas'  is null.");
            }
            var empresa = await _context.Empresas.FindAsync(CodEmpresa);
            if (empresa != null)
            {
                _context.Empresas.Remove(empresa);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool EmpresaExists(string id)
        {
            return (_context.Empresas?.Any(e => e.CodEmpresa == id)).GetValueOrDefault();
        }
    }
}
