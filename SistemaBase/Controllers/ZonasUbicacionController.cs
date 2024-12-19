using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
namespace SistemaBase.Controllers
{
    [Authorize]
    public class ZonasUbicacionController : Controller
    {
        private readonly DbvinDbContext _context;
        public ZonasUbicacionController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: ZonasUbicacion
        public async Task<IActionResult> Index()
        {
            ViewData["CodPais"] = new SelectList(_context.Barrios, "CodPais", "CodPais");
            ViewData["CodPais"] = new SelectList(_context.Provincias, "CodPais", "CodPais");
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais");
            ViewData["CodZona"] = new SelectList(_context.ZonasGeograficas, "CodZona", "CodZona");
            var dbvinDbContext = _context.ZonasUbicacions.Include(z => z.Cod).Include(z => z.CodP).Include(z => z.CodPaisNavigation).Include(z => z.CodZonaNavigation);
            return View(await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["CodPais"] = new SelectList(_context.Barrios, "CodPais", "CodPais");
            ViewData["CodPais"] = new SelectList(_context.Provincias, "CodPais", "CodPais");
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais");
            ViewData["CodZona"] = new SelectList(_context.ZonasGeograficas, "CodZona", "CodZona");
            ViewData["Show"] = true;
            var dbvinDbContext = _context.ZonasUbicacions.Include(z => z.Cod).Include(z => z.CodP).Include(z => z.CodPaisNavigation).Include(z => z.CodZonaNavigation);
            return View("Index", await dbvinDbContext.AsNoTracking().ToListAsync());
        }

        // GET: ZonasUbicacion/Details/5
        public async Task<IActionResult> Details(string CodZona, string CodPais, string CodProvincia, string CodCiudad, string CodBarrio)
        {
            var zonasUbicacion = await _context.ZonasUbicacions
            .FindAsync(CodZona, CodPais, CodProvincia, CodCiudad, CodBarrio);
            if (zonasUbicacion == null)
            {
                return NotFound();
            }
            return View(zonasUbicacion);
        }
        // GET: ZonasUbicacion/Create
        public IActionResult Create()
        {
            ViewData["CodPais"] = new SelectList(_context.Barrios, "CodPais", "CodPais");
            ViewData["CodPais"] = new SelectList(_context.Provincias, "CodPais", "CodPais");
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais");
            ViewData["CodZona"] = new SelectList(_context.ZonasGeograficas, "CodZona", "CodZona");
            return View();
        }
        // POST: ZonasUbicacion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ZonasUbicacion zonasUbicacion)
        {
            _context.Add(zonasUbicacion);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            ViewData["CodPais"] = new SelectList(_context.Barrios, "CodPais", "CodPais", zonasUbicacion.CodPais);
            ViewData["CodPais"] = new SelectList(_context.Provincias, "CodPais", "CodPais", zonasUbicacion.CodPais);
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais", zonasUbicacion.CodPais);
            ViewData["CodZona"] = new SelectList(_context.ZonasGeograficas, "CodZona", "CodZona", zonasUbicacion.CodZona);
            return RedirectToAction("ResultTable");
            // return View(zonasUbicacion);
        }
        // GET: ZonasUbicacion/Edit/5
        public async Task<IActionResult> Edit(string CodZona, string CodPais, string CodProvincia, string CodCiudad, string CodBarrio)
        {
            var zonasUbicacion = await _context.ZonasUbicacions.FindAsync(CodZona, CodPais, CodProvincia, CodCiudad, CodBarrio);
            if (zonasUbicacion == null)
            {
                return NotFound();
            }
            ViewData["CodPais"] = new SelectList(_context.Barrios, "CodPais", "CodPais", zonasUbicacion.CodPais);
            ViewData["CodPais"] = new SelectList(_context.Provincias, "CodPais", "CodPais", zonasUbicacion.CodPais);
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais", zonasUbicacion.CodPais);
            ViewData["CodZona"] = new SelectList(_context.ZonasGeograficas, "CodZona", "CodZona", zonasUbicacion.CodZona);
            return View(zonasUbicacion);
        }
        // POST: ZonasUbicacion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodZona, string CodPais, string CodProvincia, string CodCiudad, string CodBarrio, ZonasUbicacion zonasUbicacion)
        {
            try
            {
                _context.Update(zonasUbicacion);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZonasUbicacionExists(zonasUbicacion.CodZona))
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
            ViewData["CodPais"] = new SelectList(_context.Barrios, "CodPais", "CodPais", zonasUbicacion.CodPais);
            ViewData["CodPais"] = new SelectList(_context.Provincias, "CodPais", "CodPais", zonasUbicacion.CodPais);
            ViewData["CodPais"] = new SelectList(_context.Paises, "CodPais", "CodPais", zonasUbicacion.CodPais);
            ViewData["CodZona"] = new SelectList(_context.ZonasGeograficas, "CodZona", "CodZona", zonasUbicacion.CodZona);
            return RedirectToAction("ResultTable");
            //return View(zonasUbicacion);
        }
        // GET: ZonasUbicacion/Delete/5
        public async Task<IActionResult>
            Delete(string CodZona, string CodPais, string CodProvincia, string CodCiudad, string CodBarrio)
        {
            var zonasUbicacion = await _context.ZonasUbicacions
            .FindAsync(CodZona, CodPais, CodProvincia, CodCiudad, CodBarrio);
            if (zonasUbicacion == null)
            {
                return NotFound();
            }
            return View(zonasUbicacion);
        }
        // POST: ZonasUbicacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodZona, string CodPais, string CodProvincia, string CodCiudad, string CodBarrio)
        {
            if (_context.ZonasUbicacions == null)
            {
                return Problem("Entity set 'DbvinDbContext.ZonasUbicacions'  is null.");
            }
            var zonasUbicacion = await _context.ZonasUbicacions.FindAsync(CodZona, CodPais, CodProvincia, CodCiudad, CodBarrio);
            if (zonasUbicacion != null)
            {
                _context.ZonasUbicacions.Remove(zonasUbicacion);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool ZonasUbicacionExists(string id)
        {
            return (_context.ZonasUbicacions?.Any(e => e.CodZona == id)).GetValueOrDefault();
        }
    }
}
