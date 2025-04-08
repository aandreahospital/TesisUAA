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
    public class AccesosGrupoController : Controller
    {
        private readonly UAADbContext _context;

        public AccesosGrupoController(UAADbContext context)
        {
            _context = context;
        }

        // GET: AccesosGrupo
    public async Task<IActionResult> Index()
    {
            return _context.AccesosGrupos != null ?
              View(await _context.AccesosGrupos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'UAADbContext.AccesosGrupos'  is null.");
    }









    public async Task<IActionResult>
    ResultTable()
    {
    ViewData["Show"] = true;
            return _context.AccesosGrupos != null ?
              View("Index", await _context.AccesosGrupos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'UAADbContext.AccesosGrupos'  is null.");
    }












        // GET: AccesosGrupo/Details/5
        public async Task<IActionResult> Details(string CodGrupo,string CodModulo,string NomForma)
            {
            var accesosGrupo = await _context.AccesosGrupos
            .FindAsync(CodGrupo,CodModulo,NomForma);
            if (accesosGrupo == null)
            {
            return NotFound();
            }

            return View(accesosGrupo);
            }

            // GET: AccesosGrupo/Create
            public IActionResult Create()
            {
            return View();
            }

            // POST: AccesosGrupo/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( AccesosGrupo accesosGrupo)
                {
            _context.Add(accesosGrupo);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ResultTable");

                // return View(accesosGrupo);
                }

                // GET: AccesosGrupo/Edit/5
        public async Task<IActionResult> Edit(string CodGrupo,string CodModulo,string NomForma)
                    {

                    var accesosGrupo = await _context.AccesosGrupos.FindAsync(CodGrupo,CodModulo,NomForma);
                    if (accesosGrupo == null)
                    {
                    return NotFound();
                    }
                    return View(accesosGrupo);
                    }

                    // POST: AccesosGrupo/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult>
                        Edit(string CodGrupo,string CodModulo,string NomForma,  AccesosGrupo accesosGrupo)
                        {

                        try
                        {
                        _context.Update(accesosGrupo);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!AccesosGrupoExists(accesosGrupo.CodGrupo))
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

                        //return View(accesosGrupo);
                        }

                        // GET: AccesosGrupo/Delete/5
                        public async Task<IActionResult>
                            Delete(string CodGrupo,string CodModulo,string NomForma)
                            {

                            var accesosGrupo = await _context.AccesosGrupos
                            .FindAsync(CodGrupo,CodModulo,NomForma);
                            if (accesosGrupo == null)
                            {
                            return NotFound();
                            }

                            return View(accesosGrupo);
                            }

                            // POST: AccesosGrupo/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult>
                                DeleteConfirmed(string CodGrupo,string CodModulo,string NomForma)
                                {
                                if (_context.AccesosGrupos == null)
                                {
                                return Problem("Entity set 'UAADbContext.AccesosGrupos'  is null.");
                                }
                                var accesosGrupo = await _context.AccesosGrupos.FindAsync(CodGrupo,CodModulo,NomForma);
                                if (accesosGrupo != null)
                                {
                                _context.AccesosGrupos.Remove(accesosGrupo);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool AccesosGrupoExists(string id)
                                {
                                return (_context.AccesosGrupos?.Any(e => e.CodGrupo == id)).GetValueOrDefault();
                                }
                                }
                                }
