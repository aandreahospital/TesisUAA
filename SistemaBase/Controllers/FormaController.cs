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
    public class FormaController : Controller
    {
        private readonly UAADbContext _context;

        public FormaController(UAADbContext context)
        {
            _context = context;
        }

        // GET: Forma
    public async Task<IActionResult> Index()
    {
        ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo");
        var uAADbContext = _context.Formas.Include(f => f.CodModuloNavigation);
        return View(await uAADbContext.AsNoTracking().ToListAsync());
    }









    public async Task<IActionResult>
    ResultTable()
    {
        ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo");
    ViewData["Show"] = true;
        var uAADbContext = _context.Formas.Include(f => f.CodModuloNavigation);
        return View("Index",await uAADbContext.AsNoTracking().ToListAsync());
    }












        // GET: Forma/Details/5
        public async Task<IActionResult> Details(string CodModulo,string NomForma)
            {
            var forma = await _context.Formas
            .FindAsync(CodModulo,NomForma);
            if (forma == null)
            {
            return NotFound();
            }

            return View(forma);
            }

            // GET: Forma/Create
            public IActionResult Create()
            {
            ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo");
            return View();
            }

            // POST: Forma/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Forma forma)
                {
            _context.Add(forma);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo", forma.CodModulo);
                return RedirectToAction("ResultTable");

                // return View(forma);
                }

                // GET: Forma/Edit/5
        public async Task<IActionResult> Edit(string CodModulo,string NomForma)
                    {

                    var forma = await _context.Formas.FindAsync(CodModulo,NomForma);
                    if (forma == null)
                    {
                    return NotFound();
                    }
                    ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo", forma.CodModulo);
                    return View(forma);
                    }

                    // POST: Forma/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult>
                        Edit(string CodModulo,string NomForma,  Forma forma)
                        {

                        try
                        {
                        _context.Update(forma);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!FormaExists(forma.CodModulo))
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
                        ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo", forma.CodModulo);
                        return RedirectToAction("ResultTable");

                        //return View(forma);
                        }

                        // GET: Forma/Delete/5
                        public async Task<IActionResult>
                            Delete(string CodModulo,string NomForma)
                            {

                            var forma = await _context.Formas
                            .FindAsync(CodModulo,NomForma);
                            if (forma == null)
                            {
                            return NotFound();
                            }

                            return View(forma);
                            }

                            // POST: Forma/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult>
                                DeleteConfirmed(string CodModulo,string NomForma)
                                {
                                if (_context.Formas == null)
                                {
                                return Problem("Entity set 'UAADbContext.Formas'  is null.");
                                }
                                var forma = await _context.Formas.FindAsync(CodModulo,NomForma);
                                if (forma != null)
                                {
                                _context.Formas.Remove(forma);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool FormaExists(string id)
                                {
                                return (_context.Formas?.Any(e => e.CodModulo == id)).GetValueOrDefault();
                                }
                                }
                                }
