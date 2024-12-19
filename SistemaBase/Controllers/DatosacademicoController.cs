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
    public class DatosacademicoController : Controller
    {
        private readonly DbvinDbContext _context;

        public DatosacademicoController(DbvinDbContext context)
        {
            _context = context;
        }

        // GET: Datosacademico
    public async Task<IActionResult> Index()
    {
        ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario");
        var dbvinDbContext = _context.Datosacademicos.Include(d => d.CodUsuarioNavigation);
        return View(await dbvinDbContext.AsNoTracking().ToListAsync());
    }









    public async Task<IActionResult>
    ResultTable()
    {
        ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario");
    ViewData["Show"] = true;
        var dbvinDbContext = _context.Datosacademicos.Include(d => d.CodUsuarioNavigation);
        return View("Index",await dbvinDbContext.AsNoTracking().ToListAsync());
    }












        // GET: Datosacademico/Details/5
        public async Task<IActionResult> Details(int Iddatosacademicos)
            {
            var datosacademico = await _context.Datosacademicos
            .FindAsync(Iddatosacademicos);
            if (datosacademico == null)
            {
            return NotFound();
            }

            return View(datosacademico);
            }

            // GET: Datosacademico/Create
            public IActionResult Create()
            {
            ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario");
            return View();
            }

            // POST: Datosacademico/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Datosacademico datosacademico)
                {
            _context.Add(datosacademico);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario", datosacademico.CodUsuario);
                return RedirectToAction("ResultTable");

                // return View(datosacademico);
                }

                // GET: Datosacademico/Edit/5
        public async Task<IActionResult> Edit(int Iddatosacademicos)
                    {

                    var datosacademico = await _context.Datosacademicos.FindAsync(Iddatosacademicos);
                    if (datosacademico == null)
                    {
                    return NotFound();
                    }
                    ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario", datosacademico.CodUsuario);
                    return View(datosacademico);
                    }

                    // POST: Datosacademico/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult>
                        Edit(int Iddatosacademicos,  Datosacademico datosacademico)
                        {

                        try
                        {
                        _context.Update(datosacademico);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!DatosacademicoExists(datosacademico.Iddatosacademicos))
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
                        ViewData["CodUsuario"] = new SelectList(_context.Usuarios, "CodUsuario", "CodUsuario", datosacademico.CodUsuario);
                        return RedirectToAction("ResultTable");

                        //return View(datosacademico);
                        }

                        // GET: Datosacademico/Delete/5
                        public async Task<IActionResult>
                            Delete(int Iddatosacademicos)
                            {

                            var datosacademico = await _context.Datosacademicos
                            .FindAsync(Iddatosacademicos);
                            if (datosacademico == null)
                            {
                            return NotFound();
                            }

                            return View(datosacademico);
                            }

                            // POST: Datosacademico/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult>
                                DeleteConfirmed(int Iddatosacademicos)
                                {
                                if (_context.Datosacademicos == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.Datosacademicos'  is null.");
                                }
                                var datosacademico = await _context.Datosacademicos.FindAsync(Iddatosacademicos);
                                if (datosacademico != null)
                                {
                                _context.Datosacademicos.Remove(datosacademico);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool DatosacademicoExists(int id)
                                {
                                return (_context.Datosacademicos?.Any(e => e.Iddatosacademicos == id)).GetValueOrDefault();
                                }
                                }
                                }
