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
    public class CarreraController : Controller
    {
        private readonly DbvinDbContext _context;

        public CarreraController(DbvinDbContext context)
        {
            _context = context;
        }

        // GET: Carrera
    public async Task<IActionResult> Index()
    {
            return _context.Carreras != null ?
              View(await _context.Carreras.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Carreras'  is null.");
    }









    public async Task<IActionResult>
    ResultTable()
    {
    ViewData["Show"] = true;
            return _context.Carreras != null ?
              View("Index", await _context.Carreras.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Carreras'  is null.");
    }












        // GET: Carrera/Details/5
        public async Task<IActionResult> Details(int Idcarrera)
            {
            var carrera = await _context.Carreras
            .FindAsync(Idcarrera);
            if (carrera == null)
            {
            return NotFound();
            }

            return View(carrera);
            }

            // GET: Carrera/Create
            public IActionResult Create()
            {
            return View();
            }

            // POST: Carrera/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Carrera carrera)
                {
            _context.Add(carrera);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ResultTable");

                // return View(carrera);
                }

                // GET: Carrera/Edit/5
        public async Task<IActionResult> Edit(int Idcarrera)
                    {

                    var carrera = await _context.Carreras.FindAsync(Idcarrera);
                    if (carrera == null)
                    {
                    return NotFound();
                    }
                    return View(carrera);
                    }

                    // POST: Carrera/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult>
                        Edit(int Idcarrera,  Carrera carrera)
                        {

                        try
                        {
                        _context.Update(carrera);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!CarreraExists(carrera.Idcarrera))
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

                        //return View(carrera);
                        }

                        // GET: Carrera/Delete/5
                        public async Task<IActionResult>
                            Delete(int Idcarrera)
                            {

                            var carrera = await _context.Carreras
                            .FindAsync(Idcarrera);
                            if (carrera == null)
                            {
                            return NotFound();
                            }

                            return View(carrera);
                            }

                            // POST: Carrera/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult>
                                DeleteConfirmed(int Idcarrera)
                                {
                                if (_context.Carreras == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.Carreras'  is null.");
                                }
                                var carrera = await _context.Carreras.FindAsync(Idcarrera);
                                if (carrera != null)
                                {
                                _context.Carreras.Remove(carrera);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool CarreraExists(int id)
                                {
                                return (_context.Carreras?.Any(e => e.Idcarrera == id)).GetValueOrDefault();
                                }
                                }
                                }
