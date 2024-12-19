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
    public class TelefPersonaController : Controller
    {
        private readonly DbvinDbContext _context;
        public TelefPersonaController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: TelefPersona
       [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSTELEF", "Index", "TelefPersona" })]

        public async Task<IActionResult> Index(int page = 1, string searchTerm = "")
        {
            var personas = (from p in _context.TelefPersonas
                            where string.IsNullOrEmpty(searchTerm) ||
                                  p.CodPersona.Contains(searchTerm)
                            select new TelefPersona()
                            {
                                CodPersona = p.CodPersona,
                                CodigoArea = p.CodigoArea,
                                NumTelefono = p.NumTelefono,
                                TipTelefono = p.TipTelefono,
                                TelUbicacion = p.TelUbicacion,
                                Interno =p.Interno,
                                Nota =p.Nota,
                                PorDefecto = p.PorDefecto,
                                CodDireccion = (_context.DirecPersonas.FirstOrDefault(prof => prof.CodDireccion == p.CodDireccion).CodDireccion) ?? ""
                            });
            var pageSize = 50;
            var totalCount = await personas.CountAsync();

            var data = await personas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            ViewBag.MaxPage = (totalCount / pageSize) + (totalCount % pageSize == 0 ? 0 : 1);
            ViewBag.Page = page;

            return View(data);
        }

        public async Task<IActionResult> ResultTable(int page = 1, string searchTerm = "")
        {
            ViewData["CodPersona"] = new SelectList(_context.DirecPersonas, "CodPersona", "CodPersona");
            ViewData["Show"] = true;
            var personas = (from p in _context.TelefPersonas
                            where string.IsNullOrEmpty(searchTerm) ||
                                  p.CodPersona.Contains(searchTerm)
                            select new TelefPersona()
                            {
                                CodPersona = p.CodPersona,
                                CodigoArea = p.CodigoArea,
                                NumTelefono = p.NumTelefono,
                                TipTelefono = p.TipTelefono,
                                TelUbicacion = p.TelUbicacion,
                                Interno = p.Interno,
                                Nota = p.Nota,
                                PorDefecto = p.PorDefecto,
                                CodDireccion = (_context.DirecPersonas.FirstOrDefault(prof => prof.CodPersona == p.CodPersona).CodDireccion) ?? ""
                            });
            var pageSize = 50;
            var totalCount = await personas.CountAsync();

            var data = await personas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            ViewBag.MaxPage = (totalCount / pageSize) + (totalCount % pageSize == 0 ? 0 : 1);
            ViewBag.Page = page;

            return View("Index", data);
        }

        // GET: TelefPersona/Details/5
        public async Task<IActionResult> Details(string CodPersona, string CodigoArea, string NumTelefono)
        {
            var telefPersona = await _context.TelefPersonas
            .FindAsync(CodPersona, CodigoArea, NumTelefono);
            if (telefPersona == null)
            {
                return NotFound();
            }
            return View(telefPersona);
        }
        // GET: TelefPersona/Create
        public IActionResult Create()
        {
            ViewData["CodPersona"] = new SelectList(_context.DirecPersonas, "CodPersona", "CodPersona");
            return View();
        }
        // POST: TelefPersona/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TelefPersona telefPersona)
        {
            var existingTele = _context.TelefPersonas.FirstOrDefault(x => x.CodPersona == telefPersona.CodPersona && x.CodigoArea==telefPersona.CodigoArea && x.NumTelefono== telefPersona.NumTelefono);
            if (existingTele != null)
            {
                return Json(new { success = false, message = "Telefono de Persona ya existe" });
            }
            else
            {
                _context.Add(telefPersona);
                await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");
            }
            //return RedirectToAction(nameof(Index));
            ViewData["CodPersona"] = new SelectList(_context.DirecPersonas, "CodPersona", "CodPersona", telefPersona.CodPersona);
            return RedirectToAction("ResultTable");
            // return View(telefPersona);
        }
        // GET: TelefPersona/Edit/5
        public async Task<IActionResult> Edit(string CodPersona, string CodigoArea, string NumTelefono)
        {
            var telefPersona = await _context.TelefPersonas.FindAsync(CodPersona, CodigoArea, NumTelefono);
            if (telefPersona == null)
            {
                return NotFound();
            }
            ViewData["CodPersona"] = new SelectList(_context.DirecPersonas, "CodPersona", "CodPersona", telefPersona.CodPersona);
            return View(telefPersona);
        }
        // POST: TelefPersona/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodPersona, string CodigoArea, string NumTelefono, TelefPersona telefPersona)
        {
            try
            {
                _context.Update(telefPersona);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TelefPersonaExists(telefPersona.CodPersona))
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
            ViewData["CodPersona"] = new SelectList(_context.DirecPersonas, "CodPersona", "CodPersona", telefPersona.CodPersona);
            return RedirectToAction("ResultTable");
            //return View(telefPersona);
        }
        // GET: TelefPersona/Delete/5
        public async Task<IActionResult>
            Delete(string CodPersona, string CodigoArea, string NumTelefono)
        {
            var telefPersona = await _context.TelefPersonas
            .FindAsync(CodPersona, CodigoArea, NumTelefono);
            if (telefPersona == null)
            {
                return NotFound();
            }
            return View(telefPersona);
        }
        // POST: TelefPersona/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodPersona, string CodigoArea, string NumTelefono)
        {
            if (_context.TelefPersonas == null)
            {
                return Problem("Entity set 'DbvinDbContext.TelefPersonas'  is null.");
            }
            var telefPersona = await _context.TelefPersonas.FindAsync(CodPersona, CodigoArea, NumTelefono);
            if (telefPersona != null)
            {
                _context.TelefPersonas.Remove(telefPersona);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool TelefPersonaExists(string id)
        {
            return (_context.TelefPersonas?.Any(e => e.CodPersona == id)).GetValueOrDefault();
        }
    }
}
