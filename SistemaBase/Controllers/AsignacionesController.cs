using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Html2pdf;
using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Scryber;
using Scryber.Components;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using static IronPdfEngine.Proto.AddHtmlHeaderFooterRequestStream.Types;
using static SistemaBase.ModelsCustom.Reportes;
using Document = iText.Layout.Document;

namespace SistemaBase.Controllers
{
    public class AsignacionesController : Controller
    {
        private readonly DbvinDbContext _context;

        public AsignacionesController(DbvinDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Obtine la vista
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        /// 
         //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMASIGOP", "Index" , "Asignaciones" })]

        public async Task<IActionResult> Index(int page = 0, string searchTerm = "")
        {
            return View();
        }


        /// <summary>
        /// devuelva la busqueda del datatable de forma personalizada
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CustomServerSideSearchAction([FromBody] DataTableAjaxPostModel model)
        {
            int filteredResultsCount;
            int totalResultsCount;
            var res = YourCustomSearchFunc(model, out filteredResultsCount, out totalResultsCount);

            var re = Json(new
            {
                draw = model.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res.ToList()
            });

            return re;
        }
        /// <summary>
        /// Realiza la busqueda con los filtros solicitados
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filteredResultsCount"></param>
        /// <param name="totalResultsCount"></param>
        /// <returns></returns>
        public IList<UsuarioAsignacion> YourCustomSearchFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.Search != null) ? model.Search.Value : null;
            var take = model.Length;
            var skip = model.Start;

            string sortBy = "";
            bool sortDir = true;

            var result = GetDataFromDbase(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            if (result == null)
            {
                return new List<UsuarioAsignacion>();
            }
            return result;
        }
        /// <summary>
        /// Realiza la busqueda en la base de datos con los filtros solicitados
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDir"></param>
        /// <param name="filteredResultsCount"></param>
        /// <param name="totalResultsCount"></param>
        /// <returns></returns>
        public List<UsuarioAsignacion> GetDataFromDbase(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "NroEntrada";
                sortDir = true;
            }
            IQueryable<Usuario> queryUsuario = _context.Usuarios.AsQueryable();
            IQueryable<Persona> queryPersona = _context.Personas.AsQueryable();
            IQueryable<GruposUsuario> queryGrupoUsuario = _context.GruposUsuarios.AsQueryable();
            //Para obtener el nombre del usuario
            var queryUsuarioPersona = queryUsuario.Where(t => t.Estado == "A" && (t.CodGrupo == "OPREG" || t.CodGrupo == "SUINS2" )).AsQueryable().Join(queryPersona,
                  user => user.CodPersona,
                  people => people.CodPersona,
                  (user, people) =>
                      new { NombreUsuario = people.Nombre, CodGrupoUsuario = user.CodGrupo, Id = user.CodUsuario, Estado = user.Estado });
            //queryGrupoUsuario = queryGrupoUsuario.Where(x => x.Descripcion.ToUpper().Contains("Registrador")).AsQueryable();
            //Para obtener la descripción del grupo de usuario
            var queryFinal = queryUsuarioPersona.AsQueryable().Join(queryGrupoUsuario,
                us => us.CodGrupoUsuario,
                gr => gr.CodGrupo,
                (us, gr) =>
                new UsuarioAsignacion { NombreUsuario = us.NombreUsuario, DescripcionGrupo = gr.Descripcion, Id = us.Id, Estado = us.Estado });


            var asigXUsuario = from r in _context.RmAsignaciones.Where(x => x.Desasignado == null).AsQueryable()
                               orderby r.IdUsuarioAsignado
                               group r by r.IdUsuarioAsignado into grp
                               select new { key = grp.Key, cnt = grp.Count() };

            if (!string.IsNullOrEmpty(searchBy))
            {
                queryFinal = queryFinal.AsQueryable().Where(item =>
                        item.NombreUsuario.Contains(searchBy) ||
                        item.DescripcionGrupo.Contains(searchBy) ||
                        item.Id.Contains(searchBy)
                    );
            }

            //asigna la cantidad de asignaciones por usuario
            queryFinal = queryFinal.AsQueryable().Select(x =>
            new UsuarioAsignacion { NombreUsuario = x.NombreUsuario, DescripcionGrupo = x.DescripcionGrupo, Id = x.Id, Estado = x.Estado, Cantidad = asigXUsuario.Where(t => t.key == x.Id).Select(x => x.cnt).FirstOrDefault() });


            var result = queryFinal
                           .Select(m => new UsuarioAsignacion
                           {
                               NombreUsuario = m.NombreUsuario,
                               DescripcionGrupo = m.DescripcionGrupo,
                               Cantidad = m.Cantidad,
                               Estado = m.Estado,
                               Id = m.Id
                           })
                           .Skip(skip)
                          .Take(take)
                          .ToList();

            filteredResultsCount = queryFinal.Count();
            totalResultsCount = queryFinal.Count();

            return result;
        }
        /// <summary>
        /// Obtiene el listado de asignacion con los filtros del datatable
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDir"></param>
        /// <param name="filteredResultsCount"></param>
        /// <param name="totalResultsCount"></param>
        /// <returns></returns>
        public List<MesaEntradaCustom> GetDataFromDbaseAsignacion(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "NroEntrada";
                sortDir = true;
            }
            IQueryable<RmAsignacione> queryAsignaciones = _context.RmAsignaciones.AsQueryable();
            IQueryable<RmMesaEntradum> queryMesaEntrada = _context.RmMesaEntrada
                .Where(m =>
                    (m.TipoSolicitud == 1 && (m.EstadoEntrada == 31 || m.EstadoEntrada == 44)) || 
                    (m.TipoSolicitud!=1 && m.EstadoEntrada == 8) ||
                    m.EstadoEntrada == 26 ||
                    m.EstadoEntrada == 0 ||
                    m.EstadoEntrada == 45
                )
                .AsQueryable()
                .OrderByDescending(m => m.FechaEntrada); 
            IQueryable<RmTipoSolicitud> queryTipoSolicitud = _context.RmTipoSolicituds.AsQueryable();
            IQueryable<RmReingreso> queryReingreso = _context.RmReingresos.AsQueryable();

            var queryFinal = (from me in queryMesaEntrada
                              join ts in queryTipoSolicitud on me.TipoSolicitud equals ts.TipoSolicitud
                              join reingreso in queryReingreso
                              on me.NumeroEntrada equals reingreso.NroEntrada into reingresos
                              from reingreso in reingresos.DefaultIfEmpty()
                              select new
                              {
                                  Descripcion = ts.DescripSolicitud,
                                  NumeroEntrada = me.NumeroEntrada,
                                  Fecha = reingreso != null ? reingreso.FechaReingreso : me.FechaEntrada
                              }).AsEnumerable()
                 .GroupBy(result => result.NumeroEntrada)
                 .Select(group => group.OrderByDescending(g => g.Fecha).First())
                 .Select(result => new MesaEntradaCustom
                 {
                     Descripcion = result.Descripcion,
                     NumeroEntrada = result.NumeroEntrada,
                     Fecha = result.Fecha
                 });


            //var queryFinal = from me in queryMesaEntrada
            //                 orderby me.FechaEntrada descending
            //                 select new MesaEntradaCustom
            //                 {
            //                     Descripcion = me.TipoSolicitud.ToString(),
            //                     NumeroEntrada = me.NumeroEntrada,
            //                     Fecha = me.FechaEntrada
            //                 };

            var primero = queryFinal.ToList();


            if (!string.IsNullOrEmpty(searchBy))
            {
                queryFinal = queryFinal.AsQueryable().Where(item =>
                        item.Descripcion.Contains(searchBy) ||
                        item.NumeroEntrada.ToString().Contains(searchBy) ||
                        item.Fecha.ToString().Contains(searchBy)
                    );
            }

            var result = queryFinal
                          .Skip(skip)
                          .Take(take)
                          .ToList();

            filteredResultsCount = queryFinal.Count();
            totalResultsCount = queryFinal.Count();

            return result;
        }
        /// <summary>
        /// Prepara los filtros para enviar la solicitud a la base de datos
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filteredResultsCount"></param>
        /// <param name="totalResultsCount"></param>
        /// <returns></returns>
        public IList<MesaEntradaCustom> SearchFuncMesaEntrada(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.Search != null) ? model.Search.Value : null;
            var take = model.Length;
            var skip = model.Start;

            string sortBy = "";
            bool sortDir = true;

            var result = GetDataFromDbaseAsignacion(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            if (result == null)
            {
                return new List<MesaEntradaCustom>();
            }
            return result;
        }

        /// <summary>
        /// Prepara el listado para mesa de entrada
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SearchMesaEntrada([FromBody] DataTableAjaxPostModel model)
        {
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchFuncMesaEntrada(model, out filteredResultsCount, out totalResultsCount);

            var re = Json(new
            {
                draw = model.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res.ToList()
            });

            return re;
        }
        public async Task<IActionResult> ResultTable()
        {
            ViewData["Show"] = true;
            return _context.RmAsignaciones != null ?
              View("Index", await _context.RmAsignaciones.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.RmAsignaciones'  is null.");
        }

        // GET: RmAsignaciones/Details/5
        public async Task<IActionResult> Details(decimal NroEntrada, decimal NroAsignacion)
        {

            var rmAsignacione = await _context.RmAsignaciones
            .FindAsync(NroEntrada, NroAsignacion);
            if (rmAsignacione == null)
            {
                return NotFound();
            }

            return View(rmAsignacione);
        }

        // Post: RmAsignaciones/SaveRmAsignaciones
        /// <summary>
        /// Guarda en RmAsignaciones
        /// </summary>
        /// <param name="userAsig"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveRmAsignaciones([FromBody] List<UsuarioAsignacion> userAsig)
        {
            IQueryable<RmAsignacione> queryAsignaciones = _context.RmAsignaciones.AsQueryable();
            IQueryable<RmMesaEntradum> queryMesaEntrada = _context.RmMesaEntrada.AsQueryable();
            var secuencia = queryAsignaciones.Max(x => x.NroAsignacion); //TODO: Asignar de otra forma la secuencia
            foreach (var item in userAsig)
            {
                var mesaEntrada = _context.RmMesaEntrada.FirstOrDefault(m => m.NumeroEntrada == item.NumeroEntrada);
                var ringreso = _context.RmReingresos.FirstOrDefault(m => m.NroEntrada == item.NumeroEntrada);
                if (mesaEntrada.TipoSolicitud == 1 && mesaEntrada.CodigoOficina==1 && (mesaEntrada.EstadoEntrada != 31 && mesaEntrada.EstadoEntrada !=45) && mesaEntrada.Reingreso!="S")
                {
                    return Json(new { Success = false, ErrorMessage = "Error, " + item.NumeroEntrada + " debe pasar por diseño" });
                }
                if (mesaEntrada.TipoSolicitud == 1 && mesaEntrada.CodigoOficina != 1 && (mesaEntrada.EstadoEntrada != 44 && mesaEntrada.EstadoEntrada != 45) && mesaEntrada.Reingreso != "S")
                {
                    return Json(new { Success = false, ErrorMessage = "Error, " + item.NumeroEntrada + " debe pasar por diseño" });
                }
                //if (mesaEntrada.CodigoOficina != 1 && mesaEntrada.EstadoEntrada != 26)
                //{
                //    return Json(new { Success = false, ErrorMessage = "Error, " + item.NumeroEntrada + " se debe recepcionar en Division Regional" });
                //}
            }

            foreach (var item in userAsig)
            {
                var estadoEntrada = _context.RmEstadosEntrada.FirstOrDefault(p => p.DescripEstado == "Asignado");
                var mesaEntrada = queryMesaEntrada.Where(x => x.NumeroEntrada == item.NumeroEntrada).FirstOrDefault();
                mesaEntrada.EstadoEntrada = estadoEntrada.CodigoEstado;
                _context.Update(mesaEntrada);

                RmAsignacione rmAsignacione = new RmAsignacione
                {
                    NroEntrada = item.NumeroEntrada.Value,
                    FechaAsignada = DateTime.Now,
                    IdUsuarioAsignado = item.Id,
                    IdUsuarioAlta = User.Identity.Name, // TODO:modificar cuando haya roles
                    NroAsignacion = secuencia + 1,
                    TipoAsignacion = "R" //TODO: verificar si está correcto que asigne.
                };
                _context.Add(rmAsignacione);

                RmMovimientosDoc movimientos = new()
                {
                    NroEntrada = item.NumeroEntrada,
                    CodUsuario = User.Identity.Name,
                    FechaOperacion = DateTime.Now,
                    CodOperacion = "6", //Parametro para asignado
                    NroMovimientoRef = item.NumeroEntrada.ToString(),
                    EstadoEntrada = mesaEntrada.EstadoEntrada
                };
                _context.AddAsync(movimientos);



                _context.SaveChanges();


            }


            return View("Index");

        }

        // POST: RmAsignaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMASIGOP", "Create", "Asignaciones" })]

        public async Task<IActionResult> Create(RmAsignacione rmAsignacione)
        {
            _context.Add(rmAsignacione);
            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
            ;
        }

        // GET: RmAsignaciones/Edit/5
        public async Task<IActionResult> Edit(decimal NroEntrada, decimal NroAsignacion)
        {

            var rmAsignacione = await _context.RmAsignaciones.FindAsync(NroEntrada, NroAsignacion);
            if (rmAsignacione == null)
            {
                return NotFound();
            }
            return View(rmAsignacione);
        }

        // POST: RmAsignaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMASIGOP", "Edit", "Asignaciones" })]

        public async Task<IActionResult>
            Edit(decimal NroEntrada, decimal NroAsignacion, RmAsignacione rmAsignacione)
        {

            try
            {
                _context.Update(rmAsignacione);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RmAsignacioneExists(rmAsignacione.NroEntrada))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("ResultTable");

        }

        // GET: RmAsignaciones/Delete/5
        public async Task<IActionResult>
            Delete(decimal NroEntrada, decimal NroAsignacion)
        {

            var rmAsignacione = await _context.RmAsignaciones
            .FindAsync(NroEntrada, NroAsignacion);
            if (rmAsignacione == null)
            {
                return NotFound();
            }

            return View(rmAsignacione);
        }

        // POST: RmAsignaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMASIGOP", "Delete", "Asignaciones" })]

        public async Task<IActionResult>
            DeleteConfirmed(decimal NroEntrada, decimal NroAsignacion)
        {
            if (_context.RmAsignaciones == null)
            {
                return Problem("Entity set 'DbvinDbContext.RmAsignaciones'  is null.");
            }
            var rmAsignacione = await _context.RmAsignaciones.FindAsync(NroEntrada, NroAsignacion);
            if (rmAsignacione != null)
            {
                _context.RmAsignaciones.Remove(rmAsignacione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ResultTable");
        }

        private bool RmAsignacioneExists(decimal id)
        {
            return (_context.RmAsignaciones?.Any(e => e.NroEntrada == id)).GetValueOrDefault();
        }


        [HttpPost]
        public async Task<IActionResult> GenerarPdf([FromBody] List<UsuarioAsignacion> userAsig)
        {
            try
            {
                // Ordenar la lista de UsuarioAsignacion de menor a mayor según alguna propiedad, por ejemplo, el ID
                var userAsigOrdenado = userAsig.OrderBy(u => u.NumeroEntrada).ToList();
                // Crear un modelo de datos que contiene la lista procesada
                Reportes tituloData = new Reportes
                {
                    FechaActual = DateTime.Now
                };
                // Procesar la lista de números decimales y obtener los datos necesarios de la base de datos
                var listaProcesada = new List<ListadoCarga>();
                foreach (var item in userAsigOrdenado)
                {
                    var query = _context.RmMesaEntrada.Where(m => m.NumeroEntrada == item.NumeroEntrada).Join(_context.RmTipoSolicituds,
                            en => en.TipoSolicitud,
                            tipo => tipo.TipoSolicitud,
                            (en, tipo) => new ListadoCarga
                            {
                                NumeroEntrada = en.NumeroEntrada,
                                FechaEntrada = en.FechaEntrada,
                                TipoSolicitud = tipo.DescripSolicitud
                            }).OrderBy(result => result.NumeroEntrada) // Ordenar los resultados por el número de entrada
                                .AsQueryable();
                    listaProcesada.AddRange(query);
                    tituloData.Usuario = item.Id;
                    tituloData.NombreUsuario = item.NombreUsuario;

                }

                tituloData.Listado = listaProcesada;
                tituloData.TotalIngresado = listaProcesada.Count;


                // Leer la imagen y convertirla en una cadena de datos URI codificada en base64
                string imagePath = "wwwroot/assets/img/PJ/PoderJudicial.png"; // Ruta a la imagen en tu proyecto
                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

                // Pasar la cadena de datos URI a la vista a través del modelo de vista
                tituloData.ImageDataUri = imageDataUri;

                // Renderizar la vista Razor a una cadena HTML
                string viewHtml = await RenderViewToStringAsync("ReportePDF", tituloData);

                if (string.IsNullOrWhiteSpace(viewHtml))
                {
                    return BadRequest("La vista HTML está vacía o nula.");
                }

                // Crear un documento PDF utilizando iText7
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfDocument pdfDoc = new PdfDocument(new PdfWriter(memoryStream));
                    Document document = new Document(pdfDoc);

                    // Agregar el contenido HTML convertido al documento PDF
                    HtmlConverter.ConvertToPdf(viewHtml, pdfDoc, new ConverterProperties());

                    document.Close();

                    // Convertir el MemoryStream a un arreglo de bytes
                    byte[] pdfBytes = memoryStream.ToArray();

                    // Configurar el encabezado Content-Disposition
                    Response.Headers["Content-Disposition"] = "inline; filename=Reporte-de-Asignacion.pdf";

                    // Devolver el PDF como un archivo descargable con el nuevo nombre
                    return File(pdfBytes, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que pueda ocurrir
                return BadRequest($"Error al generar el PDF: {ex.Message}");
            }
        }

        private async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var engine = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine)) as Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine;
                var viewResult = engine.FindView(ControllerContext, viewName, false);

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext); // Utilizar await para renderizar de manera asincrónica
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
