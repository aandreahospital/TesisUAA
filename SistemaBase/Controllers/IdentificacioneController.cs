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
    public class IdentificacioneController : BaseRyMController
    {
        private readonly DbvinDbContext _context;

        public IdentificacioneController(DbvinDbContext context, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _context = context;
        }

        
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSIDENTI", "Index", "Identificacionerio" })]

    public async Task<IActionResult> Index()
    {
            return _context.Identificaciones != null ?
              View(await _context.Identificaciones.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Identificaciones'  is null.");
    }









    public async Task<IActionResult>
    ResultTable()
    {
    ViewData["Show"] = true;
            return _context.Identificaciones != null ?
              View("Index", await _context.Identificaciones.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Identificaciones'  is null.");
    }












        // GET: Identificacione/Details/5
        public async Task<IActionResult> Details(string CodIdent)
            {
            var identificacione = await _context.Identificaciones
            .FindAsync(CodIdent);
            if (identificacione == null)
            {
            return NotFound();
            }

            return View(identificacione);
            }

            // GET: Identificacione/Create
            public IActionResult Create()
            {
            return View();
            }

            // POST: Identificacione/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSIDENTI", "Create", "Identificacionerio" })]

        public async Task<IActionResult> Create( Identificacione identificacione)
                {
            _context.Add(identificacione);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ResultTable");

                // return View(identificacione);
                }

                // GET: Identificacione/Edit/5
        public async Task<IActionResult> Edit(string CodIdent)
                    {

                    var identificacione = await _context.Identificaciones.FindAsync(CodIdent);
                    if (identificacione == null)
                    {
                    return NotFound();
                    }
                    return View(identificacione);
                    }

                    // POST: Identificacione/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSIDENTI", "Edit", "Identificacionerio" })]

        public async Task<IActionResult>
                        Edit(string CodIdent,  Identificacione identificacione)
                        {

                        try
                        {
                        _context.Update(identificacione);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!IdentificacioneExists(identificacione.CodIdent))
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

                        //return View(identificacione);
                        }

                        // GET: Identificacione/Delete/5
                        public async Task<IActionResult>
                            Delete(string CodIdent)
                            {

                            var identificacione = await _context.Identificaciones
                            .FindAsync(CodIdent);
                            if (identificacione == null)
                            {
                            return NotFound();
                            }

                            return View(identificacione);
                            }

                            // POST: Identificacione/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSIDENTI", "Delete", "Identificacionerio" })]

        public async Task<IActionResult>
                                DeleteConfirmed(string CodIdent)
                                {
                                if (_context.Identificaciones == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.Identificaciones'  is null.");
                                }
                                var identificacione = await _context.Identificaciones.FindAsync(CodIdent);
                                if (identificacione != null)
                                {
                                _context.Identificaciones.Remove(identificacione);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool IdentificacioneExists(string id)
                                {
                                return (_context.Identificaciones?.Any(e => e.CodIdent == id)).GetValueOrDefault();
                                }
                                }
                                }
