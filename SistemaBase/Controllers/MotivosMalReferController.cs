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
    public class MotivosMalReferController : Controller
    {
        private readonly DbvinDbContext _context;
        public MotivosMalReferController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: MotivosMalRefer
        public async Task<IActionResult> Index()
        {
            return _context.MotivosMalRefers != null ?
              View(await _context.MotivosMalRefers.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.MotivosMalRefers'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.MotivosMalRefers != null ?
              View("Index", await _context.MotivosMalRefers.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.MotivosMalRefers'  is null.");
        }

        // GET: MotivosMalRefer/Details/5
        public async Task<IActionResult> Details(string CodMotivo)
        {
            var motivosMalRefer = await _context.MotivosMalRefers
            .FindAsync(CodMotivo);
            if (motivosMalRefer == null)
            {
                return NotFound();
            }
            return View(motivosMalRefer);
        }
        // GET: MotivosMalRefer/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: MotivosMalRefer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MotivosMalRefer motivosMalRefer)
        {
            _context.Add(motivosMalRefer);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(motivosMalRefer);
        }
        // GET: MotivosMalRefer/Edit/5
        public async Task<IActionResult> Edit(string CodMotivo)
        {
            var motivosMalRefer = await _context.MotivosMalRefers.FindAsync(CodMotivo);
            if (motivosMalRefer == null)
            {
                return NotFound();
            }
            return View(motivosMalRefer);
        }
        // POST: MotivosMalRefer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodMotivo, MotivosMalRefer motivosMalRefer)
        {
            try
            {
                _context.Update(motivosMalRefer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MotivosMalReferExists(motivosMalRefer.CodMotivo))
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
            //return View(motivosMalRefer);
        }
        // GET: MotivosMalRefer/Delete/5
        public async Task<IActionResult>
            Delete(string CodMotivo)
        {
            var motivosMalRefer = await _context.MotivosMalRefers
            .FindAsync(CodMotivo);
            if (motivosMalRefer == null)
            {
                return NotFound();
            }
            return View(motivosMalRefer);
        }
        // POST: MotivosMalRefer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(string CodMotivo)
        {
            if (_context.MotivosMalRefers == null)
            {
                return Problem("Entity set 'DbvinDbContext.MotivosMalRefers'  is null.");
            }
            var motivosMalRefer = await _context.MotivosMalRefers.FindAsync(CodMotivo);
            if (motivosMalRefer != null)
            {
                _context.MotivosMalRefers.Remove(motivosMalRefer);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool MotivosMalReferExists(string id)
        {
            return (_context.MotivosMalRefers?.Any(e => e.CodMotivo == id)).GetValueOrDefault();
        }
    }
}
