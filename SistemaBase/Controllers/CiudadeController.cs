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
    public class CiudadeController : Controller
    {
        private readonly DbvinDbContext _context;

        public CiudadeController(DbvinDbContext context)
        {
            _context = context;
        }

        
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSCIUDAD", "Index", "Ciudade" })]

    public async Task<IActionResult> Index()
    {
            return _context.Ciudades != null ?
              View(await _context.Ciudades.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Ciudades'  is null.");
    }









    public async Task<IActionResult>
    ResultTable()
    {
    ViewData["Show"] = true;
            return _context.Ciudades != null ?
              View("Index", await _context.Ciudades.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Ciudades'  is null.");
    }












        // GET: Ciudade/Details/5
        public async Task<IActionResult> Details(string CodCiudad)
            {
            var ciudade = await _context.Ciudades
            .FindAsync(CodCiudad);
            if (ciudade == null)
            {
            return NotFound();
            }

            return View(ciudade);
            }

            // GET: Ciudade/Create
            public IActionResult Create()
            {
            return View();
            }

            // POST: Ciudade/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSCIUDAD", "Create", "Ciudade" })]

        public async Task<IActionResult> Create( Ciudade ciudade)
                {
            var existingCiudad = _context.Ciudades.FirstOrDefault(c => c.CodCiudad== ciudade.CodCiudad);
            if (existingCiudad != null)
            {
                return Json(new { success = false, message = "Codigo de Ciudad ya existe" });
            }
            else
            {
                _context.Add(ciudade);
                await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");
            }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ResultTable");

                // return View(ciudade);
                }

                // GET: Ciudade/Edit/5
        public async Task<IActionResult> Edit(string CodCiudad)
                    {

                    var ciudade = await _context.Ciudades.FindAsync(CodCiudad);
                    if (ciudade == null)
                    {
                    return NotFound();
                    }
                    return View(ciudade);
                    }

                    // POST: Ciudade/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSCIUDAD", "Edit", "Ciudade" })]

        public async Task<IActionResult>
                        Edit(string CodCiudad,  Ciudade ciudade)
                        {

                        try
                        {
                        _context.Update(ciudade);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!CiudadeExists(ciudade.CodCiudad))
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

                        //return View(ciudade);
                        }

                        // GET: Ciudade/Delete/5
                        public async Task<IActionResult>
                            Delete(string CodCiudad)
                            {

                            var ciudade = await _context.Ciudades
                            .FindAsync(CodCiudad);
                            if (ciudade == null)
                            {
                            return NotFound();
                            }

                            return View(ciudade);
                            }

                            // POST: Ciudade/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSCIUDAD", "Delete", "Ciudade" })]


        public async Task<IActionResult>
                                DeleteConfirmed(string CodCiudad)
                                {
                                if (_context.Ciudades == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.Ciudades'  is null.");
                                }
                                var ciudade = await _context.Ciudades.FindAsync(CodCiudad);
                                if (ciudade != null)
                                {
                                _context.Ciudades.Remove(ciudade);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool CiudadeExists(string id)
                                {
                                return (_context.Ciudades?.Any(e => e.CodCiudad == id)).GetValueOrDefault();
                                }
                                }
                                }
