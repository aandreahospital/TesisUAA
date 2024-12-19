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
    public class ProvinciaController : BaseRyMController
    {
        private readonly DbvinDbContext _context;

        public ProvinciaController(DbvinDbContext context, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _context = context;
        }

        
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSRPROVI", "Index", "Provincia" })]

    public async Task<IActionResult> Index()
    {
        ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais");
        var dbvinDbContext = _context.Provincias.Include(p => p.CodPaisNavigation);
        return View(await dbvinDbContext.AsNoTracking().ToListAsync());
    }









    public async Task<IActionResult>
    ResultTable()
    {
        ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais");
    ViewData["Show"] = true;
        var dbvinDbContext = _context.Provincias.Include(p => p.CodPaisNavigation);
        return View("Index",await dbvinDbContext.AsNoTracking().ToListAsync());
    }












        // GET: Provincia/Details/5
        public async Task<IActionResult> Details(string CodPais,string CodProvincia)
            {
            var provincia = await _context.Provincias
            .FindAsync(CodPais,CodProvincia);
            if (provincia == null)
            {
            return NotFound();
            }

            return View(provincia);
            }

            // GET: Provincia/Create
            public IActionResult Create()
            {
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais");
            return View();
            }

            // POST: Provincia/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSRPROVI", "Create", "Provincia" })]

        public async Task<IActionResult> Create( Provincia provincia)
                {
            _context.Add(provincia);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais", provincia.CodPais);
                return RedirectToAction("ResultTable");

                // return View(provincia);
                }

                // GET: Provincia/Edit/5
        public async Task<IActionResult> Edit(string CodPais,string CodProvincia)
                    {

                    var provincia = await _context.Provincias.FindAsync(CodPais,CodProvincia);
                    if (provincia == null)
                    {
                    return NotFound();
                    }
                    ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais", provincia.CodPais);
                    return View(provincia);
                    }

                    // POST: Provincia/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSRPROVI", "Edit", "Provincia" })]

        public async Task<IActionResult>
                        Edit(string CodPais,string CodProvincia,  Provincia provincia)
                        {

                        try
                        {
                        _context.Update(provincia);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!ProvinciaExists(provincia.CodPais))
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
                        ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais", provincia.CodPais);
                        return RedirectToAction("ResultTable");

                        //return View(provincia);
                        }

                        // GET: Provincia/Delete/5
                        public async Task<IActionResult>
                            Delete(string CodPais,string CodProvincia)
                            {

                            var provincia = await _context.Provincias
                            .FindAsync(CodPais,CodProvincia);
                            if (provincia == null)
                            {
                            return NotFound();
                            }

                            return View(provincia);
                            }

                            // POST: Provincia/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
         //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSRPROVI", "Delete", "Provincia" })]

                            public async Task<IActionResult>
                                DeleteConfirmed(string CodPais,string CodProvincia)
                                {
                                if (_context.Provincias == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.Provincias'  is null.");
                                }
                                var provincia = await _context.Provincias.FindAsync(CodPais,CodProvincia);
                                if (provincia != null)
                                {
                                _context.Provincias.Remove(provincia);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool ProvinciaExists(string id)
                                {
                                return (_context.Provincias?.Any(e => e.CodPais == id)).GetValueOrDefault();
                                }
                                }
                                }
