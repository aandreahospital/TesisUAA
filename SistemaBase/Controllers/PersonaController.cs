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
    public class PersonaController : Controller
    {
        private readonly DbvinDbContext _context;

        public PersonaController(DbvinDbContext context)
        {
            _context = context;
        }

        // GET: Persona
    public async Task<IActionResult> Index()
    {
            return _context.Personas != null ?
              View(await _context.Personas.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Personas'  is null.");
    }









    public async Task<IActionResult>
    ResultTable()
    {
    ViewData["Show"] = true;
            return _context.Personas != null ?
              View("Index", await _context.Personas.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Personas'  is null.");
    }












        // GET: Persona/Details/5
        public async Task<IActionResult> Details(string CodPersona)
            {
            var persona = await _context.Personas
            .FindAsync(CodPersona);
            if (persona == null)
            {
            return NotFound();
            }

            return View(persona);
            }

            // GET: Persona/Create
            public IActionResult Create()
            {
            return View();
            }

            // POST: Persona/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Persona persona)
                {
            _context.Add(persona);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ResultTable");

                // return View(persona);
                }

                // GET: Persona/Edit/5
        public async Task<IActionResult> Edit(string CodPersona)
                    {

                    var persona = await _context.Personas.FindAsync(CodPersona);
                    if (persona == null)
                    {
                    return NotFound();
                    }
                    return View(persona);
                    }

                    // POST: Persona/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult>
                        Edit(string CodPersona,  Persona persona)
                        {

                        try
                        {
                        _context.Update(persona);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!PersonaExists(persona.CodPersona))
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

                        //return View(persona);
                        }

                        // GET: Persona/Delete/5
                        public async Task<IActionResult>
                            Delete(string CodPersona)
                            {

                            var persona = await _context.Personas
                            .FindAsync(CodPersona);
                            if (persona == null)
                            {
                            return NotFound();
                            }

                            return View(persona);
                            }

                            // POST: Persona/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult>
                                DeleteConfirmed(string CodPersona)
                                {
                                if (_context.Personas == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.Personas'  is null.");
                                }
                                var persona = await _context.Personas.FindAsync(CodPersona);
                                if (persona != null)
                                {
                                _context.Personas.Remove(persona);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool PersonaExists(string id)
                                {
                                return (_context.Personas?.Any(e => e.CodPersona == id)).GetValueOrDefault();
                                }
                                }
                                }
