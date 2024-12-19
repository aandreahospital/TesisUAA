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
    public class FuncionarioController : Controller
    {
        private readonly DbvinDbContext _context;
        public FuncionarioController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: Funcionario
        public async Task<IActionResult> Index()
        {
            return _context.Funcionarios != null ?
              View(await _context.Funcionarios.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Funcionarios'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.Funcionarios != null ?
              View("Index", await _context.Funcionarios.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Funcionarios'  is null.");
        }

        // GET: Funcionario/Details/5
        public async Task<IActionResult> Details(string CodEmpresa, string CodFuncionario)
        {
            var funcionario = await _context.Funcionarios
            .FindAsync(CodEmpresa, CodFuncionario);
            if (funcionario == null)
            {
                return NotFound();
            }
            return View(funcionario);
        }
        // GET: Funcionario/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Funcionario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Funcionario funcionario)
        {
            _context.Add(funcionario);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(funcionario);
        }
        // GET: Funcionario/Edit/5
        public async Task<IActionResult> Edit(string CodEmpresa, string CodFuncionario)
        {
            var funcionario = await _context.Funcionarios.FindAsync(CodEmpresa, CodFuncionario);
            if (funcionario == null)
            {
                return NotFound();
            }
            return View(funcionario);
        }
        // POST: Funcionario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodEmpresa, string CodFuncionario, Funcionario funcionario)
        {
            try
            {
                _context.Update(funcionario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncionarioExists(funcionario.CodEmpresa))
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
            //return View(funcionario);
        }
        // GET: Funcionario/Delete/5
        public async Task<IActionResult>
            Delete(string CodEmpresa, string CodFuncionario)
        {
            var funcionario = await _context.Funcionarios
            .FindAsync(CodEmpresa, CodFuncionario);
            if (funcionario == null)
            {
                return NotFound();
            }
            return View(funcionario);
        }
        // POST: Funcionario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodEmpresa, string CodFuncionario)
        {
            if (_context.Funcionarios == null)
            {
                return Problem("Entity set 'DbvinDbContext.Funcionarios'  is null.");
            }
            var funcionario = await _context.Funcionarios.FindAsync(CodEmpresa, CodFuncionario);
            if (funcionario != null)
            {
                _context.Funcionarios.Remove(funcionario);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool FuncionarioExists(string id)
        {
            return (_context.Funcionarios?.Any(e => e.CodEmpresa == id)).GetValueOrDefault();
        }
    }
}
