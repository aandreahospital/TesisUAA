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
    public class MsssmaDmNckeyColumnController : Controller
    {
        private readonly DbvinDbContext _context;
        public MsssmaDmNckeyColumnController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: MsssmaDmNckeyColumn
        public async Task<IActionResult> Index()
        {
            return _context.MsssmaDmNckeyColumns != null ?
              View(await _context.MsssmaDmNckeyColumns.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.MsssmaDmNckeyColumns'  is null.");
        }

        public async Task<IActionResult>
        ResultTable()
        {
            ViewData["Show"] = true;
            return _context.MsssmaDmNckeyColumns != null ?
              View("Index", await _context.MsssmaDmNckeyColumns.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.MsssmaDmNckeyColumns'  is null.");
        }

        // GET: MsssmaDmNckeyColumn/Details/5
        public async Task<IActionResult> Details(int ObjectId, string KeyName, int KeyColumnId, string KeyType)
        {
            var msssmaDmNckeyColumn = await _context.MsssmaDmNckeyColumns
            .FindAsync(ObjectId, KeyName, KeyColumnId, KeyType);
            if (msssmaDmNckeyColumn == null)
            {
                return NotFound();
            }
            return View(msssmaDmNckeyColumn);
        }
        // GET: MsssmaDmNckeyColumn/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: MsssmaDmNckeyColumn/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MsssmaDmNckeyColumn msssmaDmNckeyColumn)
        {
            _context.Add(msssmaDmNckeyColumn);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("ResultTable");
            // return View(msssmaDmNckeyColumn);
        }
        // GET: MsssmaDmNckeyColumn/Edit/5
        public async Task<IActionResult> Edit(int ObjectId, string KeyName, int KeyColumnId, string KeyType)
        {
            var msssmaDmNckeyColumn = await _context.MsssmaDmNckeyColumns.FindAsync(ObjectId, KeyName, KeyColumnId, KeyType);
            if (msssmaDmNckeyColumn == null)
            {
                return NotFound();
            }
            return View(msssmaDmNckeyColumn);
        }
        // POST: MsssmaDmNckeyColumn/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(int ObjectId, string KeyName, int KeyColumnId, string KeyType, MsssmaDmNckeyColumn msssmaDmNckeyColumn)
        {
            try
            {
                _context.Update(msssmaDmNckeyColumn);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MsssmaDmNckeyColumnExists(msssmaDmNckeyColumn.ObjectId))
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
            //return View(msssmaDmNckeyColumn);
        }
        // GET: MsssmaDmNckeyColumn/Delete/5
        public async Task<IActionResult>
            Delete(int ObjectId, string KeyName, int KeyColumnId, string KeyType)
        {
            var msssmaDmNckeyColumn = await _context.MsssmaDmNckeyColumns
            .FindAsync(ObjectId, KeyName, KeyColumnId, KeyType);
            if (msssmaDmNckeyColumn == null)
            {
                return NotFound();
            }
            return View(msssmaDmNckeyColumn);
        }
        // POST: MsssmaDmNckeyColumn/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(int ObjectId, string KeyName, int KeyColumnId, string KeyType)
        {
            if (_context.MsssmaDmNckeyColumns == null)
            {
                return Problem("Entity set 'DbvinDbContext.MsssmaDmNckeyColumns'  is null.");
            }
            var msssmaDmNckeyColumn = await _context.MsssmaDmNckeyColumns.FindAsync(ObjectId, KeyName, KeyColumnId, KeyType);
            if (msssmaDmNckeyColumn != null)
            {
                _context.MsssmaDmNckeyColumns.Remove(msssmaDmNckeyColumn);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction(nameof(Index));
        }
        private bool MsssmaDmNckeyColumnExists(int id)
        {
            return (_context.MsssmaDmNckeyColumns?.Any(e => e.ObjectId == id)).GetValueOrDefault();
        }
    }
}
