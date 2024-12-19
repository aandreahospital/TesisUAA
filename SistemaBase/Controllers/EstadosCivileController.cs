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
    public class EstadosCivileController : BaseRyMController
    {
        private readonly DbvinDbContext _context;

        public EstadosCivileController(DbvinDbContext context, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _context = context;
        }

        
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSESCIVI", "Index", "EstadosCivile" })]

    public async Task<IActionResult> Index()
    {
            return _context.EstadosCiviles != null ?
              View(await _context.EstadosCiviles.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.EstadosCiviles'  is null.");
    }









    public async Task<IActionResult>
    ResultTable()
    {
    ViewData["Show"] = true;
            return _context.EstadosCiviles != null ?
              View("Index", await _context.EstadosCiviles.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.EstadosCiviles'  is null.");
    }












        // GET: EstadosCivile/Details/5
        public async Task<IActionResult> Details(string CodEstadoCivil)
            {
            var estadosCivile = await _context.EstadosCiviles
            .FindAsync(CodEstadoCivil);
            if (estadosCivile == null)
            {
            return NotFound();
            }

            return View(estadosCivile);
            }

            // GET: EstadosCivile/Create
            public IActionResult Create()
            {
            return View();
            }

            // POST: EstadosCivile/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSESCIVI", "Create", "EstadosCivile" })]


        public async Task<IActionResult> Create( EstadosCivile estadosCivile)
                {
            _context.Add(estadosCivile);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ResultTable");

                // return View(estadosCivile);
                }

                // GET: EstadosCivile/Edit/5
        public async Task<IActionResult> Edit(string CodEstadoCivil)
                    {

                    var estadosCivile = await _context.EstadosCiviles.FindAsync(CodEstadoCivil);
                    if (estadosCivile == null)
                    {
                    return NotFound();
                    }
                    return View(estadosCivile);
                    }

                    // POST: EstadosCivile/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSESCIVI", "Edit", "EstadosCivile" })]

        public async Task<IActionResult>
                        Edit(string CodEstadoCivil,  EstadosCivile estadosCivile)
                        {

                        try
                        {
                        _context.Update(estadosCivile);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!EstadosCivileExists(estadosCivile.CodEstadoCivil))
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

                        //return View(estadosCivile);
                        }

                        // GET: EstadosCivile/Delete/5
                        public async Task<IActionResult>
                            Delete(string CodEstadoCivil)
                            {

                            var estadosCivile = await _context.EstadosCiviles
                            .FindAsync(CodEstadoCivil);
                            if (estadosCivile == null)
                            {
                            return NotFound();
                            }

                            return View(estadosCivile);
                            }

                            // POST: EstadosCivile/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSESCIVI", "Delete", "EstadosCivile" })]

        public async Task<IActionResult>
                                DeleteConfirmed(string CodEstadoCivil)
                                {
                                if (_context.EstadosCiviles == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.EstadosCiviles'  is null.");
                                }
                                var estadosCivile = await _context.EstadosCiviles.FindAsync(CodEstadoCivil);
                                if (estadosCivile != null)
                                {
                                _context.EstadosCiviles.Remove(estadosCivile);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool EstadosCivileExists(string id)
                                {
                                return (_context.EstadosCiviles?.Any(e => e.CodEstadoCivil == id)).GetValueOrDefault();
                                }
                                }
                                }
