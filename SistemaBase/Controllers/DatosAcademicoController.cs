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
    public class DatosAcademicoController : Controller
    {
        private readonly DbvinDbContext _context;

        public DatosAcademicoController(DbvinDbContext context)
        {
            _context = context;
        }

        // GET: DatosAcademico
    public async Task<IActionResult> Index()
    {
            return _context.DatosAcademicos != null ?
              View(await _context.DatosAcademicos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.DatosAcademicos'  is null.");
    }









    public async Task<IActionResult>
    ResultTable()
    {
    ViewData["Show"] = true;
            return _context.DatosAcademicos != null ?
              View("Index", await _context.DatosAcademicos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.DatosAcademicos'  is null.");
    }












        // GET: DatosAcademico/Details/5
        public async Task<IActionResult> Details(int IdDatosAcademicos)
            {
            var datosAcademico = await _context.DatosAcademicos
            .FindAsync(IdDatosAcademicos);
            if (datosAcademico == null)
            {
            return NotFound();
            }

            return View(datosAcademico);
            }

            // GET: DatosAcademico/Create
            public IActionResult Create()
            {
            return View();
            }

            // POST: DatosAcademico/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( DatosAcademico datosAcademico)
                {
            _context.Add(datosAcademico);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ResultTable");

                // return View(datosAcademico);
                }

                // GET: DatosAcademico/Edit/5
        public async Task<IActionResult> Edit(int IdDatosAcademicos)
                    {

                    var datosAcademico = await _context.DatosAcademicos.FindAsync(IdDatosAcademicos);
                    if (datosAcademico == null)
                    {
                    return NotFound();
                    }
                    return View(datosAcademico);
                    }

                    // POST: DatosAcademico/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult>
                        Edit(int IdDatosAcademicos,  DatosAcademico datosAcademico)
                        {

                        try
                        {
                        _context.Update(datosAcademico);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!DatosAcademicoExists(datosAcademico.IdDatosAcademicos))
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

                        //return View(datosAcademico);
                        }

                        // GET: DatosAcademico/Delete/5
                        public async Task<IActionResult>
                            Delete(int IdDatosAcademicos)
                            {

                            var datosAcademico = await _context.DatosAcademicos
                            .FindAsync(IdDatosAcademicos);
                            if (datosAcademico == null)
                            {
                            return NotFound();
                            }

                            return View(datosAcademico);
                            }

                            // POST: DatosAcademico/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult>
                                DeleteConfirmed(int IdDatosAcademicos)
                                {
                                if (_context.DatosAcademicos == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.DatosAcademicos'  is null.");
                                }
                                var datosAcademico = await _context.DatosAcademicos.FindAsync(IdDatosAcademicos);
                                if (datosAcademico != null)
                                {
                                _context.DatosAcademicos.Remove(datosAcademico);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool DatosAcademicoExists(int id)
                                {
                                return (_context.DatosAcademicos?.Any(e => e.IdDatosAcademicos == id)).GetValueOrDefault();
                                }
                                }
                                }
