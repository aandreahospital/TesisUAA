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
    public class AvDepartamentoController : Controller
    {
        private readonly DbvinDbContext _context;
        public AvDepartamentoController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: AvDepartamento
        public async Task<IActionResult> Index()
        {
            return _context.AvDepartamentos != null ?
              View(await _context.AvDepartamentos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.AvDepartamentos'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.AvDepartamentos != null ?
              View("Index", await _context.AvDepartamentos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.AvDepartamentos'  is null.");
        }

        // GET: AvDepartamento/Details/5
        public async Task<IActionResult> Details(string CodDepartamento)
        {
            var avDepartamento = await _context.AvDepartamentos
            .FindAsync(CodDepartamento);
            if (avDepartamento == null)
            {
                return NotFound();
            }
            return View(avDepartamento);
        }
        // GET: AvDepartamento/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: AvDepartamento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AvDepartamento avDepartamento)
        {
            var existingDepartamento = _context.AvDepartamentos.FirstOrDefault(d=>d.CodDepartamento== avDepartamento.CodDepartamento);
            if (existingDepartamento != null)
            {
                return Json(new { success = false, message = "Codigo de Departamento ya existe" });
            }
            else
            {
                _context.Add(avDepartamento);
                await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");
            }
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(avDepartamento);
        }
        // GET: AvDepartamento/Edit/5
        public async Task<IActionResult> Edit(string CodDepartamento)
        {
            var avDepartamento = await _context.AvDepartamentos.FindAsync(CodDepartamento);
            if (avDepartamento == null)
            {
                return NotFound();
            }
            return View(avDepartamento);
        }
        // POST: AvDepartamento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodDepartamento, AvDepartamento avDepartamento)
        {
            try
            {
                _context.Update(avDepartamento);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvDepartamentoExists(avDepartamento.CodDepartamento))
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
            //return View(avDepartamento);
        }
        // GET: AvDepartamento/Delete/5
        public async Task<IActionResult>
            Delete(string CodDepartamento)
        {
            var avDepartamento = await _context.AvDepartamentos
            .FindAsync(CodDepartamento);
            if (avDepartamento == null)
            {
                return NotFound();
            }
            return View(avDepartamento);
        }
        // POST: AvDepartamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodDepartamento)
        {
            if (_context.AvDepartamentos == null)
            {
                return Problem("Entity set 'DbvinDbContext.AvDepartamentos'  is null.");
            }
            var avDepartamento = await _context.AvDepartamentos.FindAsync(CodDepartamento);
            if (avDepartamento != null)
            {
                _context.AvDepartamentos.Remove(avDepartamento);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool AvDepartamentoExists(string id)
        {
            return (_context.AvDepartamentos?.Any(e => e.CodDepartamento == id)).GetValueOrDefault();
        }
    }
}
