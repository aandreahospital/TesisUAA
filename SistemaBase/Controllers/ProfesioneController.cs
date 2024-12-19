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
    public class ProfesioneController : Controller
    {
        private readonly DbvinDbContext _context;

        public ProfesioneController(DbvinDbContext context)
        {
            _context = context;
        }

        
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSPROFES", "Index", "Profesione" })]

        public async Task<IActionResult> Index()
        {
            return _context.Profesiones != null ?
              View(await _context.Profesiones.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Profesiones'  is null.");
        }









        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.Profesiones != null ?
              View("Index", await _context.Profesiones.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Profesiones'  is null.");
        }












        // GET: Profesione/Details/5
        public async Task<IActionResult> Details(string CodProfesion)
        {
            var profesione = await _context.Profesiones
            .FindAsync(CodProfesion);
            if (profesione == null)
            {
                return NotFound();
            }

            return View(profesione);
        }

        // GET: Profesione/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profesione/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSPROFES", "Create", "Profesione" })]

        public async Task<IActionResult> Create(Profesione profesione)
        {
            var existingProfe = _context.Profesiones.FirstOrDefault(p => p.CodProfesion == profesione.CodProfesion);
            if (existingProfe != null)
            {
                return Json(new { success = false, message = "Codigo de Profesion ya existe" });
            }
            else
            {
                _context.Add(profesione);
                await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");
            }
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");

            // return View(profesione);
        }

        // GET: Profesione/Edit/5
        public async Task<IActionResult> Edit(string CodProfesion)
        {

            var profesione = await _context.Profesiones.FindAsync(CodProfesion);
            if (profesione == null)
            {
                return NotFound();
            }
            return View(profesione);
        }

        // POST: Profesione/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSPROFES", "Edit", "Profesione" })]

        public async Task<IActionResult>
            Edit(string CodProfesion, Profesione profesione)
        {

            try
            {
                _context.Update(profesione);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfesioneExists(profesione.CodProfesion))
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

            //return View(profesione);
        }

        // GET: Profesione/Delete/5
        public async Task<IActionResult>
            Delete(string CodProfesion)
        {

            var profesione = await _context.Profesiones
            .FindAsync(CodProfesion);
            if (profesione == null)
            {
                return NotFound();
            }

            return View(profesione);
        }

        // POST: Profesione/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSPROFES", "Delete", "Profesione" })]

        public async Task<IActionResult>
            DeleteConfirmed(string CodProfesion)
        {
            if (_context.Profesiones == null)
            {
                return Problem("Entity set 'DbvinDbContext.Profesiones'  is null.");
            }
            var profesione = await _context.Profesiones.FindAsync(CodProfesion);
            if (profesione != null)
            {
                _context.Profesiones.Remove(profesione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");

            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }

        private bool ProfesioneExists(string id)
        {
            return (_context.Profesiones?.Any(e => e.CodProfesion == id)).GetValueOrDefault();
        }
    }
}
