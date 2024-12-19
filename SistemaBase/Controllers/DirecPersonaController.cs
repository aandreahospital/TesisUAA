using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
        using SistemaBase.Models;
using SistemaBase.ModelsCustom;

namespace SistemaBase.Controllers
{
    public class DirecPersonaController : Controller
    {
        private readonly DbvinDbContext _context;

        public DirecPersonaController(DbvinDbContext context)
        {
            _context = context;
        }

        // GET: DirecPersona
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSDIREC", "Index", "DirecPersona" })]

        public async Task<IActionResult> Index(int page = 1, string searchTerm = "")
        {
            var personas = (from p in _context.DirecPersonas
                            where string.IsNullOrEmpty(searchTerm) ||
                                  p.CodPersona.Contains(searchTerm) 
                            select new DirecPersona()
                            {
                                CodPersona = p.CodPersona,
                                CodDireccion = p.CodDireccion,
                                TipDireccion = p.TipDireccion,
                                Detalle = p.Detalle,
                                CodCiudad = (_context.Ciudades.FirstOrDefault(prof => prof.CodCiudad == p.CodCiudad).Descripcion) ?? "",
                                CodPais = (_context.Paises.FirstOrDefault(prof => prof.CodPais == p.CodPais).Descripcion) ?? "",
                                CodProvincia = (_context.AvDepartamentos.FirstOrDefault(prof => prof.CodDepartamento == p.CodProvincia).Descripcion) ?? ""
                            });
            var pageSize = 50;
            var totalCount = await personas.CountAsync();

            var data = await personas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            ViewBag.MaxPage = (totalCount / pageSize) + (totalCount % pageSize == 0 ? 0 : 1);
            ViewBag.Page = page;

            return View(data);
        }









        public async Task<IActionResult> ResultTable(int page = 1, string searchTerm = "")
        {
            ViewData["Show"] = true;
            var personas = (from p in _context.DirecPersonas
                            where string.IsNullOrEmpty(searchTerm) ||
                                  p.CodPersona.Contains(searchTerm)
                            select new DirecPersona()
                            {
                                CodPersona = p.CodPersona,
                                CodDireccion = p.CodDireccion,
                                TipDireccion = p.TipDireccion,
                                Detalle = p.Detalle,
                                CodCiudad = (_context.Ciudades.FirstOrDefault(prof => prof.CodCiudad == p.CodCiudad).Descripcion) ?? "",
                                CodPais = (_context.Paises.FirstOrDefault(prof => prof.CodPais == p.CodPais).Descripcion) ?? "",
                                CodProvincia = (_context.AvDepartamentos.FirstOrDefault(prof => prof.CodDepartamento == p.CodProvincia).Descripcion) ?? ""
                            });
            var pageSize = 50;
            var totalCount = await personas.CountAsync();

            var data = await personas
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            ViewBag.MaxPage = (totalCount / pageSize) + (totalCount % pageSize == 0 ? 0 : 1);
            ViewBag.Page = page;

            return View("Index",data);
        }












        // GET: DirecPersona/Details/5
        public async Task<IActionResult> Details(string CodPersona,string CodDireccion)
            {
            var direcPersona = await _context.DirecPersonas
            .FindAsync(CodPersona,CodDireccion);
            if (direcPersona == null)
            {
            return NotFound();
            }

            return View(direcPersona);
            }

            // GET: DirecPersona/Create
            public IActionResult Create()
            {
            return View();
            }

            // POST: DirecPersona/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( DirecPersona direcPersona)
                {
            var existingDireccion = _context.DirecPersonas.FirstOrDefault(x => x.CodPersona == direcPersona.CodPersona);
            if (existingDireccion != null)
            {
                return Json(new { success = false, message = "Direccion de Persona ya existe" });
            }
            else
            {
                _context.Add(direcPersona);
                await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");
            }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ResultTable");

                // return View(direcPersona);
                }

                // GET: DirecPersona/Edit/5
        public async Task<IActionResult> Edit(string CodPersona,string CodDireccion)
        {

                    var direcPersona = await _context.DirecPersonas.FindAsync(CodPersona,CodDireccion);
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "Descripcion", direcPersona?.CodPais ?? "");
            ViewData["CodProvincia"] = new SelectList(_context.Provincias, "CodProvincia", "Descripcion", direcPersona?.CodProvincia ?? "");
            ViewData["CodCiudad"] = new SelectList(_context.Ciudades, "CodCiudad", "Descripcion", direcPersona?.CodCiudad ?? "");
            ViewData["CodBarrio"] = new SelectList(_context.Barrios, "CodBarrio", "Descripcion", direcPersona?.CodBarrio ?? "");
            if (direcPersona == null)
                    {
                    return NotFound();
                    }
                    return View(direcPersona);
        }







                    // POST: DirecPersona/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult>
                        Edit(string CodPersona,string CodDireccion,  DirecPersona direcPersona)
                        {

                        try
                        {
                        _context.Update(direcPersona);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!DirecPersonaExists(direcPersona.CodPersona))
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

                        //return View(direcPersona);
                        }

                        // GET: DirecPersona/Delete/5
                        public async Task<IActionResult>
                            Delete(string CodPersona,string CodDireccion)
                            {

                            var direcPersona = await _context.DirecPersonas
                            .FindAsync(CodPersona,CodDireccion);
                            if (direcPersona == null)
                            {
                            return NotFound();
                            }

                            return View(direcPersona);
                            }

                            // POST: DirecPersona/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult>
                                DeleteConfirmed(string CodPersona,string CodDireccion)
                                {
                                if (_context.DirecPersonas == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.DirecPersonas'  is null.");
                                }
                                var direcPersona = await _context.DirecPersonas.FindAsync(CodPersona,CodDireccion);
                                if (direcPersona != null)
                                {
                                _context.DirecPersonas.Remove(direcPersona);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool DirecPersonaExists(string id)
                                {
                                return (_context.DirecPersonas?.Any(e => e.CodPersona == id)).GetValueOrDefault();
                                }
                                }
                                }
