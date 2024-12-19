using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Interface.Pdf;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
namespace SistemaBase.Controllers
{
    public class AccesosGrupoController : BaseRyMController
    {
        private readonly DbvinDbContext _context;

        public AccesosGrupoController(DbvinDbContext context, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _context = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSACCGRU", "Index", "AccesosGrupo" })]

        public async Task<IActionResult> Index(int page = 1, string searchTerm = "")
        {
            var accesosGruposJoin = (from ag in _context.AccesosGrupos
                                     join f in _context.Formas on ag.NomForma equals f.NomForma
                                     where string.IsNullOrEmpty(searchTerm) ||
                                           ag.NomForma.Contains(searchTerm) ||
                                           f.Descripcion.Contains(searchTerm) ||
                                           ag.CodGrupo.Contains(searchTerm) // Agrega condiciones de búsqueda
                                     select new AccesosGrupoCustom()
                                     {
                                         CodGrupo = ag.CodGrupo,
                                         CodModulo = ag.CodModulo,
                                         NomForma = ag.NomForma,
                                         Descripcion = f.Descripcion
                                     });

            var pageSize = 50;
            var totalCount = await accesosGruposJoin.CountAsync();

            var data = await accesosGruposJoin
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            ViewBag.MaxPage = (totalCount / pageSize) + (totalCount % pageSize == 0 ? 0 : 1);
            ViewBag.Page = page;

            return View(data);
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
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSACCGRU", "Create", "AccesosGrupo" })]

        public async Task<IActionResult> Create(AccesosGrupo accesosGrupo)
        {
            var existingAcceso = await _context.AccesosGrupos.FirstOrDefaultAsync(a=>a.CodGrupo== accesosGrupo.CodGrupo && a.CodModulo== accesosGrupo.CodModulo&& a.NomForma == accesosGrupo.NomForma);
            if (existingAcceso!=null)
            {
                return Json(new { success = false, message = "Acceso de Grupo ya existe" });
            }
            else
            {
                _context.Add(accesosGrupo);
                await _context.SaveChangesAsync();
                return RedirectToAction("ResultTable");
            }
            
        }
        // GET: AccesosGrupo/Edit/5
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSACCGRU", "Edit", "AccesosGrupo" })]

        public async Task<IActionResult> Edit(string CodGrupo, string CodModulo, string NomForma)
        {

            AccesosGrupoCreateCustom AccesosGrupoCreate = new AccesosGrupoCreateCustom();

            var accesoGrupoById = await _context.AccesosGrupos.FirstOrDefaultAsync(ag => ag.CodModulo == CodModulo && ag.NomForma == NomForma && ag.CodGrupo == CodGrupo);

            if (accesoGrupoById == null)
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
            AccesosGrupoCreate.GruposUsuarios = grupoUsuario;
            AccesosGrupoCreate.Modulos = modulos;
            AccesosGrupoCreate.FormasCustom = formas;


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
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSACCGRU", "Delete", "AccesosGrupo" })]

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
