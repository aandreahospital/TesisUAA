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
    public class LugarController : Controller
    {
        private readonly DbvinDbContext _context;
        public LugarController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: Lugar
        public async Task<IActionResult> Index()
        {
            return _context.Lugars != null ?
              View(await _context.Lugars.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Lugars'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.Lugars != null ?
              View("Index", await _context.Lugars.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.Lugars'  is null.");
        }

        // GET: Lugar/Details/5
        public async Task<IActionResult> Details(double IdLugar)
        {
            var lugar = await _context.Lugars
            .FindAsync(IdLugar);
            if (lugar == null)
            {
                return NotFound();
            }
            return View(lugar);
        }
        // GET: Lugar/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Lugar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lugar lugar)
        {
            _context.Add(lugar);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(lugar);
        }
        // GET: Lugar/Edit/5
        public async Task<IActionResult> Edit(double IdLugar)
        {
            var lugar = await _context.Lugars.FindAsync(IdLugar);
            if (lugar == null)
            {
                return NotFound();
            }
            return View(lugar);
        }
        // POST: Lugar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(double IdLugar, Lugar lugar)
        {
            try
            {
                _context.Update(lugar);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LugarExists(lugar.IdLugar))
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
            //return View(lugar);
        }
        // GET: Lugar/Delete/5
        public async Task<IActionResult>
            Delete(double IdLugar)
        {
            var lugar = await _context.Lugars
            .FindAsync(IdLugar);
            if (lugar == null)
            {
                return NotFound();
            }
            return View(lugar);
        }
        // POST: Lugar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(double IdLugar)
        {
            if (_context.Lugars == null)
            {
                return Problem("Entity set 'DbvinDbContext.Lugars'  is null.");
            }
            var lugar = await _context.Lugars.FindAsync(IdLugar);
            if (lugar != null)
            {
                _context.Lugars.Remove(lugar);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool LugarExists(double id)
        {
            return (_context.Lugars?.Any(e => e.IdLugar == id)).GetValueOrDefault();
        }
    }
}
