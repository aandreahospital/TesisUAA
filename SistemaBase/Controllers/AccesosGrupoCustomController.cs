using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Interface.Pdf;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
namespace SistemaBase.Controllers
{
    [Authorize]
    public class AccesosGrupoCustomController : BaseRyMController
    {
        private readonly DbvinDbContext _context;

        public AccesosGrupoCustomController(DbvinDbContext context, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _context = context;
        }
        // GET: AccesosGrupo
        public async Task<IActionResult> Index()
        {
            var accesosGruposJoin = (from ag in _context.AccesosGrupos
                                     join f in _context.Formas on ag.NomForma equals f.NomForma
                                     //where ag.NomForma == "RMUSUARI"
                                     select new AccesosGrupoCustom()
                                     {
                                         CodGrupo = ag.CodGrupo,
                                         CodModulo = ag.CodModulo,
                                         NomForma = ag.NomForma,
                                         Descripcion = f.Descripcion
                                     });

            return View(await accesosGruposJoin.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult>
        ResultTable()
        {
            //ViewData["CodGrupo"] = new SelectList(_context.GruposUsuarios, "CodGrupo", "CodGrupo");
            //ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo");
            ViewData["Show"] = true;
            var accesosGruposJoin = (from ag in _context.AccesosGrupos
                                     join f in _context.Formas on ag.NomForma equals f.NomForma
                                     select new AccesosGrupoCustom()
                                     {
                                         CodGrupo = ag.CodGrupo,
                                         CodModulo = ag.CodModulo,
                                         NomForma = ag.NomForma,
                                         Descripcion = f.Descripcion
                                     });
            return View("Index", await accesosGruposJoin.AsNoTracking().ToListAsync());
        }

        // GET: AccesosGrupo/Details/5
        public async Task<IActionResult> Details(string CodGrupo, string CodModulo, string NomForma)
        {
            var accesosGrupo = await _context.AccesosGrupos
            .FindAsync(CodGrupo, CodModulo, NomForma);
            if (accesosGrupo == null)
            {
                return NotFound();
            }
            return View(accesosGrupo);
        }
        // GET: AccesosGrupo/Create
        public IActionResult Create()
        {

            AccesosGrupoCreateCustom AccesosGrupoCreate = new AccesosGrupoCreateCustom();

            var grupoUsuario = (from gu in _context.GruposUsuarios
                                select new SelectListItem()
                                {
                                    Text = gu.CodGrupo + "-" + gu.Descripcion,
                                    Value = gu.CodGrupo
                                }).ToList();
            var modulos = (from m in _context.Modulos
                           select new SelectListItem()
                           {
                               Text = m.CodModulo + "-" + m.Descripcion,
                               Value = m.CodModulo
                           }).ToList();

            var formas = (from m in _context.Formas
                          select new SelectListItem()
                          {
                              Text = m.NomForma + "-" + m.Descripcion,
                              Value = m.NomForma
                          }).ToList();




            AccesosGrupoCreate.GruposUsuarios = grupoUsuario;
            AccesosGrupoCreate.Modulos = modulos;
            AccesosGrupoCreate.FormasCustom = formas;

            return View(AccesosGrupoCreate);
        }
        // POST: AccesosGrupo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccesosGrupo accesosGrupo)
        {
            _context.Add(accesosGrupo);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
        }
        // GET: AccesosGrupo/Edit/5
        public async Task<IActionResult> Edit(string CodGrupo, string CodModulo, string NomForma)
        {

            AccesosGrupoCreateCustom AccesosGrupoCreate = new AccesosGrupoCreateCustom();

            var accesoGrupoById = await _context.AccesosGrupos.FirstOrDefaultAsync(ag => ag.CodModulo == CodModulo && ag.NomForma == NomForma && ag.CodGrupo == CodGrupo);

            if(accesoGrupoById == null)
            {
                return NotFound();
            }

            var grupoUsuario = await (from gu in _context.GruposUsuarios
                                select new SelectListItem()
                                {
                                    Text = gu.CodGrupo + "-" + gu.Descripcion,
                                    Value = gu.CodGrupo
                                }).ToListAsync();
            
            var modulos = await (from m in _context.Modulos
                           select new SelectListItem()
                           {
                               Text = m.CodModulo + "-" + m.Descripcion,
                               Value = m.CodModulo
                           }).ToListAsync();

            var formas = await (from f in _context.Formas
                          select new SelectListItem()
                          {
                              Text = f.NomForma + "-" + f.Descripcion,
                              Value = f.NomForma
                          }).ToListAsync();


            AccesosGrupoCreate.CodGrupo = accesoGrupoById.CodGrupo;
            AccesosGrupoCreate.CodModulo = accesoGrupoById.CodModulo;
            AccesosGrupoCreate.NomForma = accesoGrupoById.NomForma;
            AccesosGrupoCreate.PuedeInsertar = accesoGrupoById.PuedeInsertar;
            AccesosGrupoCreate.PuedeBorrar = accesoGrupoById.PuedeBorrar;
            AccesosGrupoCreate.PuedeActualizar = accesoGrupoById.PuedeActualizar;
            AccesosGrupoCreate.PuedeConsultar = accesoGrupoById.PuedeConsultar;
            AccesosGrupoCreate.ItemMenu = accesoGrupoById.ItemMenu;
            AccesosGrupoCreate.Rowid = accesoGrupoById.Rowid;
            AccesosGrupoCreate.GruposUsuarios =  grupoUsuario;
            AccesosGrupoCreate.Modulos =  modulos;
            AccesosGrupoCreate.FormasCustom =  formas;


            return View(AccesosGrupoCreate);
        }
        // POST: AccesosGrupo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string CodGrupo, string CodModulo, string NomForma, AccesosGrupo accesosGrupo)
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
            ViewData["CodGrupo"] = new SelectList(_context.GruposUsuarios, "CodGrupo", "CodGrupo", accesosGrupo.CodGrupo);
            ViewData["CodModulo"] = new SelectList(_context.Modulos, "CodModulo", "CodModulo", accesosGrupo.CodModulo);
            return RedirectToAction("ResultTable");
            //return View(accesosGrupo);
        }
        // GET: AccesosGrupo/Delete/5
        public async Task<IActionResult>
            Delete(string CodGrupo, string CodModulo, string NomForma)
        {
            var accesosGrupo = await _context.AccesosGrupos
            .FindAsync(CodGrupo, CodModulo, NomForma);
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
            DeleteConfirmed(string CodGrupo, string CodModulo, string NomForma)
        {
            if (_context.AccesosGrupos == null)
            {
                return Problem("Entity set 'DbvinDbContext.AccesosGrupos'  is null.");
            }
            var accesosGrupo = await _context.AccesosGrupos.FindAsync(CodGrupo, CodModulo, NomForma);
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
