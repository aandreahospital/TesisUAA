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
    public class UsuarioController : Controller
    {
        private readonly Models.UAADbContext _context;

        public UsuarioController(Models.UAADbContext context)
        {
            _context = context;
        }

        // GET: Usuario
    public async Task<IActionResult> Index()
    {
        ViewData["CodGrupo"] = new SelectList(_context.GruposUsuarios, "CodGrupo", "CodGrupo");
        ViewData["CodPersona"] = new SelectList(_context.Personas, "CodPersona", "CodPersona");
        var dbvinDbContext = _context.Usuarios.Include(u => u.CodGrupoNavigation).Include(u => u.CodPersonaNavigation);
        return View(await dbvinDbContext.AsNoTracking().ToListAsync());
    }









    public async Task<IActionResult>
    ResultTable()
    {
        ViewData["CodGrupo"] = new SelectList(_context.GruposUsuarios, "CodGrupo", "CodGrupo");
        ViewData["CodPersona"] = new SelectList(_context.Personas, "CodPersona", "CodPersona");
    ViewData["Show"] = true;
        var dbvinDbContext = _context.Usuarios.Include(u => u.CodGrupoNavigation).Include(u => u.CodPersonaNavigation);
        return View("Index",await dbvinDbContext.AsNoTracking().ToListAsync());
    }












        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(string CodUsuario)
            {
            var usuario = await _context.Usuarios
            .FindAsync(CodUsuario);
            if (usuario == null)
            {
            return NotFound();
            }

            return View(usuario);
            }

            // GET: Usuario/Create
            public IActionResult Create()
            {
            ViewData["CodGrupo"] = new SelectList(_context.GruposUsuarios, "CodGrupo", "CodGrupo");
            ViewData["CodPersona"] = new SelectList(_context.Personas, "CodPersona", "CodPersona");
            return View();
            }

            // POST: Usuario/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Usuario usuario)
                {
            _context.Add(usuario);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                ViewData["CodGrupo"] = new SelectList(_context.GruposUsuarios, "CodGrupo", "CodGrupo", usuario.CodGrupo);
                ViewData["CodPersona"] = new SelectList(_context.Personas, "CodPersona", "CodPersona", usuario.CodPersona);
                return RedirectToAction("ResultTable");

                // return View(usuario);
                }

                // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(string CodUsuario)
                    {

                    var usuario = await _context.Usuarios.FindAsync(CodUsuario);
                    if (usuario == null)
                    {
                    return NotFound();
                    }
                    ViewData["CodGrupo"] = new SelectList(_context.GruposUsuarios, "CodGrupo", "CodGrupo", usuario.CodGrupo);
                    ViewData["CodPersona"] = new SelectList(_context.Personas, "CodPersona", "CodPersona", usuario.CodPersona);
                    return View(usuario);
                    }

                    // POST: Usuario/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult>
                        Edit(string CodUsuario,  Usuario usuario)
                        {

                        try
                        {
                        _context.Update(usuario);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!UsuarioExists(usuario.CodUsuario))
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
                        ViewData["CodGrupo"] = new SelectList(_context.GruposUsuarios, "CodGrupo", "CodGrupo", usuario.CodGrupo);
                        ViewData["CodPersona"] = new SelectList(_context.Personas, "CodPersona", "CodPersona", usuario.CodPersona);
                        return RedirectToAction("ResultTable");

                        //return View(usuario);
                        }

                        // GET: Usuario/Delete/5
                        public async Task<IActionResult>
                            Delete(string CodUsuario)
                            {

                            var usuario = await _context.Usuarios
                            .FindAsync(CodUsuario);
                            if (usuario == null)
                            {
                            return NotFound();
                            }

                            return View(usuario);
                            }

                            // POST: Usuario/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult>
                                DeleteConfirmed(string CodUsuario)
                                {
                                if (_context.Usuarios == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.Usuarios'  is null.");
                                }
                                var usuario = await _context.Usuarios.FindAsync(CodUsuario);
                                if (usuario != null)
                                {
                                _context.Usuarios.Remove(usuario);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool UsuarioExists(string id)
                                {
                                return (_context.Usuarios?.Any(e => e.CodUsuario == id)).GetValueOrDefault();
                                }
                                }
                                }
