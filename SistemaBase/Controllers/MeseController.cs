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
    public class MeseController : Controller
    {
        private readonly DbvinDbContext _context;

        public MeseController(DbvinDbContext context)
        {
            _context = context;
        }

        
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSMESES", "Index", "Mese" })]

        public async Task<IActionResult> Index()
        {
            var meses = (from m in _context.Meses

                        select new Mese()
                        {
                            Mes = (int)Math.Floor(m.Mes),
                            Descripcion =  m.Descripcion,
                            Abreviatura = m.Abreviatura, 
                            Rowid = m.Rowid

                        });

            return View(meses);



            //return _context.Meses != null ?
            //  View(await _context.Meses.AsNoTracking().ToListAsync()) :
            //  Problem("Entity set 'DbvinDbContext.Meses'  is null.");
        }









        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.Meses != null ?
              View("Index", await _context.Meses.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Meses'  is null.");
        }












        // GET: Mese/Details/5
        public async Task<IActionResult> Details(decimal Mes)
        {
            var mese = await _context.Meses
            .FindAsync(Mes);
            if (mese == null)
            {
                return NotFound();
            }

            return View(mese);
        }

        // GET: Mese/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mese/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSMESES", "Create", "Mese" })]

        public async Task<IActionResult> Create(Mese mese)
        {
            _context.Add(mese);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");

            
        }

        // GET: Mese/Edit/5
        public async Task<IActionResult> Edit(decimal Mes)
        {

            var mese = await _context.Meses.FindAsync(Mes);
            if (mese == null)
            {
                return NotFound();
            }
            return View(mese);
        }

        // POST: Mese/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSMESES", "Edit", "Mese" })]

        public async Task<IActionResult>
            Edit(decimal Mes, Mese mese)
        {

            try
            {
                _context.Update(mese);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeseExists(mese.Mes))
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

            //return View(mese);
        }

        // GET: Mese/Delete/5
        public async Task<IActionResult>
            Delete(decimal Mes)
        {

            var mese = await _context.Meses
            .FindAsync(Mes);
            if (mese == null)
            {
                return NotFound();
            }

            return View(mese);
        }

        // POST: Mese/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSMESES", "Delete", "Mese" })]

        public async Task<IActionResult>
            DeleteConfirmed(decimal Mes)
        {
            if (_context.Meses == null)
            {
                return Problem("Entity set 'DbvinDbContext.Meses'  is null.");
            }
            var mese = await _context.Meses.FindAsync(Mes);
            if (mese != null)
            {
                _context.Meses.Remove(mese);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");

            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }

        private bool MeseExists(decimal id)
        {
            return (_context.Meses?.Any(e => e.Mes == id)).GetValueOrDefault();
        }
    }
}
