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
    public class DatosLaboraleController : Controller
    {
        private readonly DbvinDbContext _context;

        public DatosLaboraleController(DbvinDbContext context)
        {
            _context = context;
        }

        // GET: DatosLaborale
    public async Task<IActionResult> Index()
    {
            return _context.DatosLaborales != null ?
              View(await _context.DatosLaborales.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.DatosLaborales'  is null.");
    }









    public async Task<IActionResult>
    ResultTable()
    {
    ViewData["Show"] = true;
            return _context.DatosLaborales != null ?
              View("Index", await _context.DatosLaborales.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.DatosLaborales'  is null.");
    }












        // GET: DatosLaborale/Details/5
        public async Task<IActionResult> Details(int IdDatosLaborales)
            {
            var datosLaborale = await _context.DatosLaborales
            .FindAsync(IdDatosLaborales);
            if (datosLaborale == null)
            {
            return NotFound();
            }

            return View(datosLaborale);
            }

            // GET: DatosLaborale/Create
            public IActionResult Create()
            {
            return View();
            }

            // POST: DatosLaborale/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( DatosLaborale datosLaborale)
                {
            _context.Add(datosLaborale);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ResultTable");

                // return View(datosLaborale);
                }

                // GET: DatosLaborale/Edit/5
        public async Task<IActionResult> Edit(int IdDatosLaborales)
                    {

                    var datosLaborale = await _context.DatosLaborales.FindAsync(IdDatosLaborales);
                    if (datosLaborale == null)
                    {
                    return NotFound();
                    }
                    return View(datosLaborale);
                    }

                    // POST: DatosLaborale/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult>
                        Edit(int IdDatosLaborales,  DatosLaborale datosLaborale)
                        {

                        try
                        {
                        _context.Update(datosLaborale);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!DatosLaboraleExists(datosLaborale.IdDatosLaborales))
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

                        //return View(datosLaborale);
                        }

                        // GET: DatosLaborale/Delete/5
                        public async Task<IActionResult>
                            Delete(int IdDatosLaborales)
                            {

                            var datosLaborale = await _context.DatosLaborales
                            .FindAsync(IdDatosLaborales);
                            if (datosLaborale == null)
                            {
                            return NotFound();
                            }

                            return View(datosLaborale);
                            }

                            // POST: DatosLaborale/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult>
                                DeleteConfirmed(int IdDatosLaborales)
                                {
                                if (_context.DatosLaborales == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.DatosLaborales'  is null.");
                                }
                                var datosLaborale = await _context.DatosLaborales.FindAsync(IdDatosLaborales);
                                if (datosLaborale != null)
                                {
                                _context.DatosLaborales.Remove(datosLaborale);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool DatosLaboraleExists(int id)
                                {
                                return (_context.DatosLaborales?.Any(e => e.IdDatosLaborales == id)).GetValueOrDefault();
                                }
                                }
                                }
