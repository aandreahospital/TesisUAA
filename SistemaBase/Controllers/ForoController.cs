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
    public class ForoController : Controller
    {
        private readonly DbvinDbContext _context;

        public ForoController(DbvinDbContext context)
        {
            _context = context;
        }

        // GET: Foro
    public async Task<IActionResult> Index()
    {
        ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario");
        var dbvinDbContext = _context.Forodebates.Include(f => f.CodUsuarioNavigation);
        return View(await dbvinDbContext.AsNoTracking().ToListAsync());
    }









    public async Task<IActionResult>
    ResultTable()
    {
        ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario");
    ViewData["Show"] = true;
        var dbvinDbContext = _context.Forodebates.Include(f => f.CodUsuarioNavigation);
        return View("Index",await dbvinDbContext.AsNoTracking().ToListAsync());
    }












        // GET: Foro/Details/5
        public async Task<IActionResult> Details(int Idforodebate)
            {
            var forodebate = await _context.Forodebates
            .FindAsync(Idforodebate);
            if (forodebate == null)
            {
            return NotFound();
            }

            return View(forodebate);
            }

            // GET: Foro/Create
            public IActionResult Create()
            {
            ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario");
            return View();
            }

            // POST: Foro/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Forodebate forodebate)
                {
            _context.Add(forodebate);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario", forodebate.CodUsuario);
                return RedirectToAction("ResultTable");

                // return View(forodebate);
                }

                // GET: Foro/Edit/5
        public async Task<IActionResult> Edit(int Idforodebate)
                    {

                    var forodebate = await _context.Forodebates.FindAsync(Idforodebate);
                    if (forodebate == null)
                    {
                    return NotFound();
                    }
                    ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario", forodebate.CodUsuario);
                    return View(forodebate);
                    }

                    // POST: Foro/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult>
                        Edit(int Idforodebate,  Forodebate forodebate)
                        {

                        try
                        {
                        _context.Update(forodebate);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!ForodebateExists(forodebate.Idforodebate))
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
                        ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario", forodebate.CodUsuario);
                        return RedirectToAction("ResultTable");

                        //return View(forodebate);
                        }

                        // GET: Foro/Delete/5
                        public async Task<IActionResult>
                            Delete(int Idforodebate)
                            {

                            var forodebate = await _context.Forodebates
                            .FindAsync(Idforodebate);
                            if (forodebate == null)
                            {
                            return NotFound();
                            }

                            return View(forodebate);
                            }

                            // POST: Foro/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult>
                                DeleteConfirmed(int Idforodebate)
                                {
                                if (_context.Forodebates == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.Forodebates'  is null.");
                                }
                                var forodebate = await _context.Forodebates.FindAsync(Idforodebate);
                                if (forodebate != null)
                                {
                                _context.Forodebates.Remove(forodebate);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool ForodebateExists(int id)
                                {
                                return (_context.Forodebates?.Any(e => e.Idforodebate == id)).GetValueOrDefault();
                                }
                                }
                                }
