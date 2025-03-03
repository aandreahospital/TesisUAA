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
    public class ModuloController : Controller
    {
        private readonly DbvinDbContext _context;

        public ModuloController(DbvinDbContext context)
        {
            _context = context;
        }

        // GET: Modulo
    public async Task<IActionResult> Index()
    {
            return _context.Modulos != null ?
              View(await _context.Modulos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Modulos'  is null.");
    }









    public async Task<IActionResult>
    ResultTable()
    {
    ViewData["Show"] = true;
            return _context.Modulos != null ?
              View("Index", await _context.Modulos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Modulos'  is null.");
    }












        // GET: Modulo/Details/5
        public async Task<IActionResult> Details(string CodModulo)
            {
            var modulo = await _context.Modulos
            .FindAsync(CodModulo);
            if (modulo == null)
            {
            return NotFound();
            }

            return View(modulo);
            }

            // GET: Modulo/Create
            public IActionResult Create()
            {
            return View();
            }

            // POST: Modulo/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Modulo modulo)
                {
            _context.Add(modulo);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ResultTable");

                // return View(modulo);
                }

                // GET: Modulo/Edit/5
        public async Task<IActionResult> Edit(string CodModulo)
                    {

                    var modulo = await _context.Modulos.FindAsync(CodModulo);
                    if (modulo == null)
                    {
                    return NotFound();
                    }
                    return View(modulo);
                    }

                    // POST: Modulo/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult>
                        Edit(string CodModulo,  Modulo modulo)
                        {

                        try
                        {
                        _context.Update(modulo);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!ModuloExists(modulo.CodModulo))
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

                        //return View(modulo);
                        }

                        // GET: Modulo/Delete/5
                        public async Task<IActionResult>
                            Delete(string CodModulo)
                            {

                            var modulo = await _context.Modulos
                            .FindAsync(CodModulo);
                            if (modulo == null)
                            {
                            return NotFound();
                            }

                            return View(modulo);
                            }

                            // POST: Modulo/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult>
                                DeleteConfirmed(string CodModulo)
                                {
                                if (_context.Modulos == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.Modulos'  is null.");
                                }
                                var modulo = await _context.Modulos.FindAsync(CodModulo);
                                if (modulo != null)
                                {
                                _context.Modulos.Remove(modulo);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool ModuloExists(string id)
                                {
                                return (_context.Modulos?.Any(e => e.CodModulo == id)).GetValueOrDefault();
                                }
                                }
                                }
