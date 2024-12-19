//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web.Mvc;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using SistemaBase.Models;
//namespace SistemaBase.Controllers
//{

//    public class AvTipoEstablecimientoController : Controller
//    {
//        private readonly DbvinDbContext _context;
//        public AvTipoEstablecimientoController(DbvinDbContext context)
//        {
//            _context = context;
//        }
//        // GET: AvTipoEstablecimiento
//        public async Task<IActionResult> Index()
//        {
//            return _context.AvTipoEstablecimientos != null ?
//              View(await _context.AvTipoEstablecimientos.Take(100).AsNoTracking().ToListAsync()) :
//              Problem("Entity set 'DbvinDbContext.AvTipoEstablecimientos'  is null.");
//        }

//        public async Task<IActionResult>
//        ResultTable()
//        {
//            ViewData["Show"] = true;
//            return _context.AvTipoEstablecimientos != null ?
//              View("Index", await _context.AvTipoEstablecimientos.AsNoTracking().ToListAsync()) :
//              Problem("Entity set 'DbvinDbContext.AvTipoEstablecimientos'  is null.");
//        }

//        // GET: AvTipoEstablecimiento/Details/5
//        public async Task<IActionResult> Details(string CodTipoEstab)
//        {
//            var avTipoEstablecimiento = await _context.AvTipoEstablecimientos
//            .FindAsync(CodTipoEstab);
//            if (avTipoEstablecimiento == null)
//            {
//                return NotFound();
//            }
//            return View(avTipoEstablecimiento);
//        }
//        // GET: AvTipoEstablecimiento/Create
//        public IActionResult Create()
//        {
//            return View();
//        }
//        // POST: AvTipoEstablecimiento/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        //[ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(AvTipoEstablecimiento avTipoEstablecimiento)
//        {
//            _context.Add(avTipoEstablecimiento);
//            await _context.SaveChangesAsync();
//            return RedirectToAction("ResultTable");
//            //return RedirectToAction(nameof(Index));
//            return RedirectToAction("ResultTable");
//            // return View(avTipoEstablecimiento);
//        }
//        // GET: AvTipoEstablecimiento/Edit/5
//        public async Task<IActionResult> Edit(string CodTipoEstab)
//        {
//            var avTipoEstablecimiento = await _context.AvTipoEstablecimientos.FindAsync(CodTipoEstab);
//            if (avTipoEstablecimiento == null)
//            {
//                return NotFound();
//            }
//            return View(avTipoEstablecimiento);
//        }
//        // POST: AvTipoEstablecimiento/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        public async Task<IActionResult>
//            Edit(string CodTipoEstab, AvTipoEstablecimiento avTipoEstablecimiento)
//        {
//            try
//            {
//                _context.Update(avTipoEstablecimiento);
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!AvTipoEstablecimientoExists(avTipoEstablecimiento.CodTipoEstab))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//            return RedirectToAction("ResultTable");
//            // return RedirectToAction(nameof(Index));
//            return RedirectToAction("ResultTable");
//            //return View(avTipoEstablecimiento);
//        }
//        // GET: AvTipoEstablecimiento/Delete/5
//        public async Task<IActionResult>
//            Delete(string CodTipoEstab)
//        {
//            var avTipoEstablecimiento = await _context.AvTipoEstablecimientos
//            .FindAsync(CodTipoEstab);
//            if (avTipoEstablecimiento == null)
//            {
//                return NotFound();
//            }
//            return View(avTipoEstablecimiento);
//        }
//        // POST: AvTipoEstablecimiento/Delete/5
//        //[HttpPost, ActionName("Delete")]
//        //[ValidateAntiForgeryToken]
//        public async Task<IActionResult>
//            DeleteConfirmed(string CodTipoEstab)
//        {
//            if (_context.AvTipoEstablecimientos == null)
//            {
//                return Problem("Entity set 'DbvinDbContext.AvTipoEstablecimientos'  is null.");
//            }
//            var avTipoEstablecimiento = await _context.AvTipoEstablecimientos.FindAsync(CodTipoEstab);
//            if (avTipoEstablecimiento != null)
//            {
//                _context.AvTipoEstablecimientos.Remove(avTipoEstablecimiento);
//            }
//            await _context.SaveChangesAsync();
//            return RedirectToAction("ResultTable");
//            //return RedirectToAction(nameof(Index));
//            //return RedirectToAction(nameof(Index));
//        }
//        private bool AvTipoEstablecimientoExists(string id)
//        {
//            return (_context.AvTipoEstablecimientos?.Any(e => e.CodTipoEstab == id)).GetValueOrDefault();
//        }
//    }
//}
