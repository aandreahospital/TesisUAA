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
    public class DepartamentoController : Controller
    {
        private readonly DbvinDbContext _context;
        public DepartamentoController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: Departamento
        public async Task<IActionResult> Index()
        {
            return _context.Departamentos != null ?
              View(await _context.Departamentos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Departamentos'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.Departamentos != null ?
              View("Index", await _context.Departamentos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Departamentos'  is null.");
        }

        // GET: Departamento/Details/5
        public async Task<IActionResult> Details(string CodEmpresa, string CodDepartamento)
        {
            var departamento = await _context.Departamentos
            .FindAsync(CodEmpresa, CodDepartamento);
            if (departamento == null)
            {
                return NotFound();
            }
            return View(departamento);
        }
        // GET: Departamento/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Departamento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Departamento departamento)
        {
            _context.Add(departamento);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(departamento);
        }
        // GET: Departamento/Edit/5
        public async Task<IActionResult> Edit(string CodEmpresa, string CodDepartamento)
        {
            var departamento = await _context.Departamentos.FindAsync(CodEmpresa, CodDepartamento);
            if (departamento == null)
            {
                return NotFound();
            }
            return View(departamento);
        }
        // POST: Departamento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodEmpresa, string CodDepartamento, Departamento departamento)
        {
            try
            {
                _context.Update(departamento);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartamentoExists(departamento.CodEmpresa))
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
            //return View(departamento);
        }
        // GET: Departamento/Delete/5
        public async Task<IActionResult>
            Delete(string CodEmpresa, string CodDepartamento)
        {
            var departamento = await _context.Departamentos
            .FindAsync(CodEmpresa, CodDepartamento);
            if (departamento == null)
            {
                return NotFound();
            }
            return View(departamento);
        }
        // POST: Departamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodEmpresa, string CodDepartamento)
        {
            if (_context.Departamentos == null)
            {
                return Problem("Entity set 'DbvinDbContext.Departamentos'  is null.");
            }
            var departamento = await _context.Departamentos.FindAsync(CodEmpresa, CodDepartamento);
            if (departamento != null)
            {
                _context.Departamentos.Remove(departamento);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool DepartamentoExists(string id)
        {
            return (_context.Departamentos?.Any(e => e.CodEmpresa == id)).GetValueOrDefault();
        }
    }
}
