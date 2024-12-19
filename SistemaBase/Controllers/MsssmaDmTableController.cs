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
    public class MsssmaDmTableController : Controller
    {
        private readonly DbvinDbContext _context;
        public MsssmaDmTableController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: MsssmaDmTable
        public async Task<IActionResult> Index()
        {
            return _context.MsssmaDmTables != null ?
              View(await _context.MsssmaDmTables.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.MsssmaDmTables'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.MsssmaDmTables != null ?
              View("Index", await _context.MsssmaDmTables.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.MsssmaDmTables'  is null.");
        }

        // GET: MsssmaDmTable/Details/5
        public async Task<IActionResult> Details(int ObjectId)
        {
            var msssmaDmTable = await _context.MsssmaDmTables
            .FindAsync(ObjectId);
            if (msssmaDmTable == null)
            {
                return NotFound();
            }
            return View(msssmaDmTable);
        }
        // GET: MsssmaDmTable/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: MsssmaDmTable/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MsssmaDmTable msssmaDmTable)
        {
            _context.Add(msssmaDmTable);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(msssmaDmTable);
        }
        // GET: MsssmaDmTable/Edit/5
        public async Task<IActionResult> Edit(int ObjectId)
        {
            var msssmaDmTable = await _context.MsssmaDmTables.FindAsync(ObjectId);
            if (msssmaDmTable == null)
            {
                return NotFound();
            }
            return View(msssmaDmTable);
        }
        // POST: MsssmaDmTable/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(int ObjectId, MsssmaDmTable msssmaDmTable)
        {
            try
            {
                _context.Update(msssmaDmTable);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MsssmaDmTableExists(msssmaDmTable.ObjectId))
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
            //return View(msssmaDmTable);
        }
        // GET: MsssmaDmTable/Delete/5
        public async Task<IActionResult>
            Delete(int ObjectId)
        {
            var msssmaDmTable = await _context.MsssmaDmTables
            .FindAsync(ObjectId);
            if (msssmaDmTable == null)
            {
                return NotFound();
            }
            return View(msssmaDmTable);
        }
        // POST: MsssmaDmTable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(int ObjectId)
        {
            if (_context.MsssmaDmTables == null)
            {
                return Problem("Entity set 'DbvinDbContext.MsssmaDmTables'  is null.");
            }
            var msssmaDmTable = await _context.MsssmaDmTables.FindAsync(ObjectId);
            if (msssmaDmTable != null)
            {
                _context.MsssmaDmTables.Remove(msssmaDmTable);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool MsssmaDmTableExists(int id)
        {
            return (_context.MsssmaDmTables?.Any(e => e.ObjectId == id)).GetValueOrDefault();
        }
    }
}
