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
    public class CentroEstudioController : Controller
    {
        private readonly Models.UAADbContext _context;

        public CentroEstudioController(Models.UAADbContext context)
        {
            _context = context;
        }

        // GET: CentroEstudio
    public async Task<IActionResult> Index()
    {
            return _context.CentroEstudios != null ?
              View(await _context.CentroEstudios.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.CentroEstudios'  is null.");
    }









    public async Task<IActionResult>
    ResultTable()
    {
    ViewData["Show"] = true;
            return _context.CentroEstudios != null ?
              View("Index", await _context.CentroEstudios.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.CentroEstudios'  is null.");
    }












        // GET: CentroEstudio/Details/5
        public async Task<IActionResult> Details(int IdCentroEstudio)
            {
            var centroEstudio = await _context.CentroEstudios
            .FindAsync(IdCentroEstudio);
            if (centroEstudio == null)
            {
            return NotFound();
            }

            return View(centroEstudio);
            }

            // GET: CentroEstudio/Create
            public IActionResult Create()
            {
            return View();
            }

            // POST: CentroEstudio/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( CentroEstudio centroEstudio)
                {
            _context.Add(centroEstudio);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ResultTable");

                // return View(centroEstudio);
                }

                // GET: CentroEstudio/Edit/5
        public async Task<IActionResult> Edit(int IdCentroEstudio)
                    {

                    var centroEstudio = await _context.CentroEstudios.FindAsync(IdCentroEstudio);
                    if (centroEstudio == null)
                    {
                    return NotFound();
                    }
                    return View(centroEstudio);
                    }

                    // POST: CentroEstudio/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult>
                        Edit(int IdCentroEstudio,  CentroEstudio centroEstudio)
                        {

                        try
                        {
                        _context.Update(centroEstudio);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!CentroEstudioExists(centroEstudio.IdCentroEstudio))
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

                        //return View(centroEstudio);
                        }

                        // GET: CentroEstudio/Delete/5
                        public async Task<IActionResult>
                            Delete(int IdCentroEstudio)
                            {

                            var centroEstudio = await _context.CentroEstudios
                            .FindAsync(IdCentroEstudio);
                            if (centroEstudio == null)
                            {
                            return NotFound();
                            }

                            return View(centroEstudio);
                            }

                            // POST: CentroEstudio/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult>
                                DeleteConfirmed(int IdCentroEstudio)
                                {
                                if (_context.CentroEstudios == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.CentroEstudios'  is null.");
                                }
                                var centroEstudio = await _context.CentroEstudios.FindAsync(IdCentroEstudio);
                                if (centroEstudio != null)
                                {
                                _context.CentroEstudios.Remove(centroEstudio);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool CentroEstudioExists(int id)
                                {
                                return (_context.CentroEstudios?.Any(e => e.IdCentroEstudio == id)).GetValueOrDefault();
                                }
                                }
                                }
