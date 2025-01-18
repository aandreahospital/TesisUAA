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
    public class DatoslaboraleController : Controller
    {
        private readonly DbvinDbContext _context;

        public DatoslaboraleController(DbvinDbContext context)
        {
            _context = context;
        }

        // GET: Datoslaborale
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSDALAB", "Index", "Datoslaborale" })]

        public async Task<IActionResult> Index()
    {
        ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario");
        var dbvinDbContext = _context.Datoslaborales.Include(d => d.CodUsuarioNavigation);
        return View(await dbvinDbContext.AsNoTracking().ToListAsync());
    }









    public async Task<IActionResult>
    ResultTable()
    {
        ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario");
    ViewData["Show"] = true;
        var dbvinDbContext = _context.Datoslaborales.Include(d => d.CodUsuarioNavigation);
        return View("Index",await dbvinDbContext.AsNoTracking().ToListAsync());
    }












        // GET: Datoslaborale/Details/5
        public async Task<IActionResult> Details(int Iddatoslaborales)
            {
            var datoslaborale = await _context.Datoslaborales
            .FindAsync(Iddatoslaborales);
            if (datoslaborale == null)
            {
            return NotFound();
            }

            return View(datoslaborale);
            }

            // GET: Datoslaborale/Create
            public IActionResult Create()
            {
            ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario");
            return View();
            }

            // POST: Datoslaborale/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Datoslaborale datoslaborale)
                {
            _context.Add(datoslaborale);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario", datoslaborale.CodUsuario);
                return RedirectToAction("ResultTable");

                // return View(datoslaborale);
                }

                // GET: Datoslaborale/Edit/5
        public async Task<IActionResult> Edit(int Iddatoslaborales)
                    {

                    var datoslaborale = await _context.Datoslaborales.FindAsync(Iddatoslaborales);
                    if (datoslaborale == null)
                    {
                    return NotFound();
                    }
                    ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario", datoslaborale.CodUsuario);
                    return View(datoslaborale);
                    }

                    // POST: Datoslaborale/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult>
                        Edit(int Iddatoslaborales,  Datoslaborale datoslaborale)
                        {

                        try
                        {
                        _context.Update(datoslaborale);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!DatoslaboraleExists(datoslaborale.Iddatoslaborales))
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
                        ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario", datoslaborale.CodUsuario);
                        return RedirectToAction("ResultTable");

                        //return View(datoslaborale);
                        }

                        // GET: Datoslaborale/Delete/5
                        public async Task<IActionResult>
                            Delete(int Iddatoslaborales)
                            {

                            var datoslaborale = await _context.Datoslaborales
                            .FindAsync(Iddatoslaborales);
                            if (datoslaborale == null)
                            {
                            return NotFound();
                            }

                            return View(datoslaborale);
                            }

                            // POST: Datoslaborale/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult>
                                DeleteConfirmed(int Iddatoslaborales)
                                {
                                if (_context.Datoslaborales == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.Datoslaborales'  is null.");
                                }
                                var datoslaborale = await _context.Datoslaborales.FindAsync(Iddatoslaborales);
                                if (datoslaborale != null)
                                {
                                _context.Datoslaborales.Remove(datoslaborale);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool DatoslaboraleExists(int id)
                                {
                                return (_context.Datoslaborales?.Any(e => e.Iddatoslaborales == id)).GetValueOrDefault();
                                }
                                }
                                }
