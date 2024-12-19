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
    public class BarrioController : Controller
    {
        private readonly DbvinDbContext _context;

        public BarrioController(DbvinDbContext context)
        {
            _context = context;
        }

        
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSBARRIO", "Index", "Barrio" })]

    public async Task<IActionResult> Index()
    {
            return _context.Barrios != null ?
              View(await _context.Barrios.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Barrios'  is null.");
    }









    public async Task<IActionResult>
    ResultTable()
    {
    ViewData["Show"] = true;
            return _context.Barrios != null ?
              View("Index", await _context.Barrios.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Barrios'  is null.");
    }












        // GET: Barrio/Details/5
        public async Task<IActionResult> Details(string CodPais,string CodProvincia,string CodCiudad,string CodBarrio)
            {
            var barrio = await _context.Barrios
            .FindAsync(CodPais,CodProvincia,CodCiudad,CodBarrio);
            if (barrio == null)
            {
            return NotFound();
            }

            return View(barrio);
            }

            // GET: Barrio/Create
            public IActionResult Create()
            {
            return View();
            }

            // POST: Barrio/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSBARRIO", "Create", "Barrio" })]

        public async Task<IActionResult> Create( Barrio barrio)
                {
            _context.Add(barrio);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ResultTable");

                // return View(barrio);
                }

                // GET: Barrio/Edit/5
        public async Task<IActionResult> Edit(string CodPais,string CodProvincia,string CodCiudad,string CodBarrio)
                    {

                    var barrio = await _context.Barrios.FindAsync(CodPais,CodProvincia,CodCiudad,CodBarrio);
                    if (barrio == null)
                    {
                    return NotFound();
                    }
                    return View(barrio);
                    }

                    // POST: Barrio/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSBARRIO", "Edit", "Barrio" })]

        public async Task<IActionResult>
                        Edit(string CodPais,string CodProvincia,string CodCiudad,string CodBarrio,  Barrio barrio)
                        {

                        try
                        {
                        _context.Update(barrio);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!BarrioExists(barrio.CodPais))
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

                        //return View(barrio);
                        }

                        // GET: Barrio/Delete/5
                        public async Task<IActionResult>
                            Delete(string CodPais,string CodProvincia,string CodCiudad,string CodBarrio)
                            {

                            var barrio = await _context.Barrios
                            .FindAsync(CodPais,CodProvincia,CodCiudad,CodBarrio);
                            if (barrio == null)
                            {
                            return NotFound();
                            }

                            return View(barrio);
                            }

                            // POST: Barrio/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSBARRIO", "Delete", "Barrio" })]

        public async Task<IActionResult>
                                DeleteConfirmed(string CodPais,string CodProvincia,string CodCiudad,string CodBarrio)
                                {
                                if (_context.Barrios == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.Barrios'  is null.");
                                }
                                var barrio = await _context.Barrios.FindAsync(CodPais,CodProvincia,CodCiudad,CodBarrio);
                                if (barrio != null)
                                {
                                _context.Barrios.Remove(barrio);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool BarrioExists(string id)
                                {
                                return (_context.Barrios?.Any(e => e.CodPais == id)).GetValueOrDefault();
                                }
                                }
                                }
