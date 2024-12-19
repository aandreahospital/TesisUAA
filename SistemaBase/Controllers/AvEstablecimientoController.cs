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
    public class AvEstablecimientoController : BaseRyMController
    {
        private readonly DbvinDbContext _context;

        public AvEstablecimientoController(DbvinDbContext context, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _context = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "AVESTABL", "Index", "AvEstablecimiento" })]
        public async Task<IActionResult> Index(int page = 1, string searchTerm = "")
        {
            const int PageSize = 50;

            IQueryable<AvEstablecimiento> query = _context.AvEstablecimientos.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(item =>
                    item.CodEstable.Contains(searchTerm) ||
                    item.CodPropietario.Contains(searchTerm) ||
                    item.Descripcion.Contains(searchTerm) ||
                    item.Telefono.Contains(searchTerm) ||
                    item.Domicilio.Contains(searchTerm) ||
                    item.CodDepartamento.Contains(searchTerm) ||
                    item.CodZona.Contains(searchTerm) ||
                    item.CodRegional.Contains(searchTerm) ||
                    item.CodDistrito.Contains(searchTerm) ||
                    item.CodLocalidad.Contains(searchTerm) ||
                    item.CodTipoEstab.Contains(searchTerm) ||
                    item.CodFinalidad.Contains(searchTerm) ||
                    item.PistaAterrisaje.Contains(searchTerm) ||
                    item.Banio.Contains(searchTerm) ||
                    item.Brete.Contains(searchTerm) ||
                    item.Embarcadero.Contains(searchTerm) ||
                    item.Cepo.Contains(searchTerm) ||
                    item.Galpon.Contains(searchTerm) ||
                    item.Corral.Contains(searchTerm) ||
                    item.FrecuenciaRadio.Contains(searchTerm) ||
                    item.NroPotero.ToString().Contains(searchTerm) ||  // Convert decimal to string
                    item.PasturaNatural.ToString().Contains(searchTerm) ||  // Convert decimal to string
                    item.PasturaCultivada.ToString().Contains(searchTerm) ||  // Convert decimal to string
                    item.PasturaMonte.ToString().Contains(searchTerm) ||  // Convert decimal to string
                    item.PasturaOtro.ToString().Contains(searchTerm) ||  // Convert decimal to string
                    item.TotalHectarea.ToString().Contains(searchTerm) ||  // Convert decimal to string
                    item.LinderoNorte.Contains(searchTerm) ||
                    item.LinderoSur.Contains(searchTerm) ||
                    item.LinderoEste.Contains(searchTerm) ||
                    item.LinderoOeste.Contains(searchTerm) ||
                    item.GpsV.Contains(searchTerm) ||
                    item.GpsS.Contains(searchTerm) ||
                    item.GpsW.Contains(searchTerm) ||
                    item.GpsH.Contains(searchTerm) ||
                    item.GpsSc.Contains(searchTerm) ||
                    item.CodEstableOld.Contains(searchTerm) ||
                    item.UsuarioUltMod.Contains(searchTerm) ||
                    item.CodUsuarioAlta.Contains(searchTerm) ||
                    item.CodEstablePj.Contains(searchTerm)
                );
            }

            var count = await query.CountAsync();
            var data = await query.Skip((page-1) * PageSize).Take(PageSize).ToListAsync();

            ViewBag.MaxPage = (count / PageSize) + (count % PageSize == 0 ? 0 : 1); // Calculate the max number of pages
            ViewBag.Page = page;

            return View(data);
        }









        public async Task<IActionResult>
    ResultTable()
    {
    ViewData["Show"] = true;
            return _context.AvEstablecimientos != null ?
              View("Index", await _context.AvEstablecimientos.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.AvEstablecimientos'  is null.");
    }












        // GET: AvEstablecimiento/Details/5
        public async Task<IActionResult> Details(string CodEstable,string CodEstablePj)
            {
            var avEstablecimiento = await _context.AvEstablecimientos
            .FindAsync(CodEstable,CodEstablePj);
            if (avEstablecimiento == null)
            {
            return NotFound();
            }

            return View(avEstablecimiento);
            }

            // GET: AvEstablecimiento/Create
            public IActionResult Create()
            {
            return View();
            }

            // POST: AvEstablecimiento/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( AvEstablecimiento avEstablecimiento)
                {
            _context.Add(avEstablecimiento);
            await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");

                //return RedirectToAction(nameof(Index));
                return RedirectToAction("ResultTable");

                // return View(avEstablecimiento);
                }

                // GET: AvEstablecimiento/Edit/5
        public async Task<IActionResult> Edit(string CodEstable,string CodEstablePj)
                    {

                    var avEstablecimiento = await _context.AvEstablecimientos.FindAsync(CodEstable,CodEstablePj);
                    if (avEstablecimiento == null)
                    {
                    return NotFound();
                    }
                    return View(avEstablecimiento);
                    }

                    // POST: AvEstablecimiento/Edit/5
                    // To protect from overposting attacks, enable the specific properties you want to bind to.
                    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult>
                        Edit(string CodEstable,string CodEstablePj,  AvEstablecimiento avEstablecimiento)
                        {

                        try
                        {
                        _context.Update(avEstablecimiento);
                        await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        if (!AvEstablecimientoExists(avEstablecimiento.CodEstable))
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

                        //return View(avEstablecimiento);
                        }

                        // GET: AvEstablecimiento/Delete/5
                        public async Task<IActionResult>
                            Delete(string CodEstable,string CodEstablePj)
                            {

                            var avEstablecimiento = await _context.AvEstablecimientos
                            .FindAsync(CodEstable,CodEstablePj);
                            if (avEstablecimiento == null)
                            {
                            return NotFound();
                            }

                            return View(avEstablecimiento);
                            }

                            // POST: AvEstablecimiento/Delete/5
                            [HttpPost, ActionName("Delete")]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult>
                                DeleteConfirmed(string CodEstable,string CodEstablePj)
                                {
                                if (_context.AvEstablecimientos == null)
                                {
                                return Problem("Entity set 'DbvinDbContext.AvEstablecimientos'  is null.");
                                }
                                var avEstablecimiento = await _context.AvEstablecimientos.FindAsync(CodEstable,CodEstablePj);
                                if (avEstablecimiento != null)
                                {
                                _context.AvEstablecimientos.Remove(avEstablecimiento);
                                }

                                await _context.SaveChangesAsync();
                                return RedirectToAction("ResultTable");

                                //return RedirectToAction(nameof(Index));
                                //return RedirectToAction(nameof(Index));
                                }

                                private bool AvEstablecimientoExists(string id)
                                {
                                return (_context.AvEstablecimientos?.Any(e => e.CodEstable == id)).GetValueOrDefault();
                                }
                                }
                                }
