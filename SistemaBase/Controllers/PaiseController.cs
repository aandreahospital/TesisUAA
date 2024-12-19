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
    public class PaiseController : BaseRyMController
    {
        private readonly DbvinDbContext _context;

        public PaiseController(DbvinDbContext context, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _context = context;
        }

        
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSPAISES", "Index", "Paise" })]

    public async Task<IActionResult> Index()
    {
            return _context.Paises != null ?
              View(await _context.Paises.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Paises'  is null.");
    }









    public async Task<IActionResult>
    ResultTable()
    {
    ViewData["Show"] = true;
            return _context.Paises != null ?
              View("Index", await _context.Paises.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Paises'  is null.");
    }












        // GET: Paise/Details/5
        public async Task<IActionResult> Details(string CodPais)
            {
            var paise = await _context.Paises
            .FindAsync(CodPais);
            if (paise == null)
            {
            return NotFound();
            }

            return View(paise);
            }

            // GET: Paise/Create
            public IActionResult Create()
            {
            return View();
            }

            // POST: Paise/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSPAISES", "Create", "Paise" })]

        public async Task<IActionResult> Create( Paise paise)
                {
            var existingPais = _context.Paises.FirstOrDefault(p=>p.CodPais== paise.CodPais);
            if (existingPais != null)
            {
                return Json(new { success = false, message = "Codigo de Pais ya existe" });
            }
            else
            {
                _context.Add(paise);
                await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");
            }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ResultTable");

                // return View(paise);
                }

                // GET: Paise/Edit/5
        public async Task<IActionResult> Edit(string CodPais)
                    {

                    var paise = await _context.Paises.FindAsync(CodPais);
                    if (paise == null)
                    {
                    return NotFound();
                    }
                    return View(paise);
                    }

                    // POST: Paise/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSPAISES", "Edit", "Paise" })]

        public async Task<IActionResult>
                        Edit(string CodPais,  Paise paise)
                        {

                        try
                        {
                        _context.Update(paise);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!PaiseExists(paise.CodPais))
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

                        //return View(paise);
                        }

                        // GET: Paise/Delete/5
                        public async Task<IActionResult>
                            Delete(string CodPais)
                            {

                            var paise = await _context.Paises
                            .FindAsync(CodPais);
                            if (paise == null)
                            {
                            return NotFound();
                            }

                            return View(paise);
                            }

                            // POST: Paise/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSPAISES", "Delete", "Paise" })]

        public async Task<IActionResult>
                                DeleteConfirmed(string CodPais)
                                {
                                if (_context.Paises == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.Paises'  is null.");
                                }
                                var paise = await _context.Paises.FindAsync(CodPais);
                                if (paise != null)
                                {
                                _context.Paises.Remove(paise);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool PaiseExists(string id)
                                {
                                return (_context.Paises?.Any(e => e.CodPais == id)).GetValueOrDefault();
                                }
                                }
                                }
