using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using static iText.IO.Image.Jpeg2000ImageData;
using static SistemaBase.ModelsCustom.Reportes;

namespace SistemaBase.Controllers
{
    public class EnvioDocArchivoController : Controller
    {
        private readonly DbvinDbContext _context;

        public EnvioDocArchivoController(DbvinDbContext context)
        {
            _context = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMENARCH", "Index", "EnvioDocArchivo" })]

        public async Task<IActionResult> Index(int page = 1, string searchTerm = "")
        {

            return View();
        }

        public async Task<IActionResult> ResultTable(int page = 1, string searchTerm = "")
        {

            return View();
        }

        public List<RecepcionEntradas> GetDataFromDbaseTransaccion(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "NroEntrada";
                sortDir = true;
            }
            List<string> descripcionesPermitidas = new List<string> {
                    "INSCRIPCION",
                    "DUPLICADO",
                    "REINSCRIPCION",
                    "ADJUDICACION",
                    "CAMBIO DE DENOMINACION",
                    "DACION EN PAGO",
                    "DONACION",
                    "PERMUTA",
                    "TRANSFERENCIA"
            };
            IQueryable<RmMesaEntradum> queyrMesaEntrada = _context.RmMesaEntrada.OrderByDescending(o=>o.FechaEntrada).Where(o=>o.EstadoEntrada==13 && o.CodOficinaRetiro==1).AsQueryable();
            IQueryable<RmTipoSolicitud> queryTipoSolicitud = _context.RmTipoSolicituds.Where(ts => descripcionesPermitidas.Contains(ts.DescripSolicitud));

            var resultado = queyrMesaEntrada.Join(queryTipoSolicitud, 
                en=>en.TipoSolicitud,
                tip=> tip.TipoSolicitud,
                (en,tip)=> new RecepcionEntradas { NroEntrada = en.NumeroEntrada, DescSolicitud= tip.DescripSolicitud, FechaEntrada = en.FechaEntrada });
          
            if (!string.IsNullOrEmpty(searchBy))
            {
                var searchTerm = searchBy;
                resultado = resultado.Where(item =>
                    item.NroEntrada.ToString().Contains(searchTerm) 
                );
            }

            var result = resultado
                           .Select(m => new RecepcionEntradas
                           {
                               NroEntrada = m.NroEntrada,
                               DescSolicitud = m.DescSolicitud,
                               FechaEntrada = m.FechaEntrada
                           })
                           .Skip(skip)
                          .Take(take)
                          .ToList();

            filteredResultsCount = resultado.Count();
            totalResultsCount = resultado.Count();

            return result;
        }

        public IList<RecepcionEntradas> SearchFuncTransacciones(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.Search != null) ? model.Search.Value : null;
            var take = model.Length;
            var skip = model.Start;

            string sortBy = "";
            bool sortDir = true;

            var result = GetDataFromDbaseTransaccion(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            if (result == null)
            {
                return new List<RecepcionEntradas>();
            }
            return result;
        }

        [HttpPost]
        public JsonResult SearchTransacciones([FromBody] DataTableAjaxPostModel model)
        {
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchFuncTransacciones(model, out filteredResultsCount, out totalResultsCount);

            var re = Json(new
            {
                draw = model.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res.ToList()
            });

            return re;
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado([FromBody] List<decimal> selectedNroEntradas)
        {

            foreach (decimal nroEntrada in selectedNroEntradas)
            {
                var mesaEntrada = await _context.RmMesaEntrada.FindAsync(nroEntrada);

                if (mesaEntrada == null)
                {
                    // Manejo si la mesaEntrada no se encuentra
                    return NotFound();
                }
                else
                {
                    var estadoEntrada = await _context.RmEstadosEntrada.FirstOrDefaultAsync(p => p.DescripEstado == "Enviado a Archivo");

                    mesaEntrada.EstadoEntrada = estadoEntrada.CodigoEstado;
                    _context.Update(mesaEntrada);
                    await _context.SaveChangesAsync();

                    RmMovimientosDoc movimientos = new()
                    {
                        NroEntrada = nroEntrada,
                        CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                        FechaOperacion = DateTime.Now,
                        CodOperacion = "09", //parametro para cambio de estado 
                        NroMovimientoRef = nroEntrada.ToString(),
                        EstadoEntrada = mesaEntrada.EstadoEntrada
                    };
                    await _context.AddAsync(movimientos);
                    await _context.SaveChangesAsync();


                }

            }
            return View("Index");
        }


        [HttpPost]
        public async Task<IActionResult> GenerarPdf([FromBody] List<decimal> selectedNroEntradas)
        {
            try
            {
                decimal codOficina = 0;
                var tipoSolicitud = _context.RmTipoSolicituds.AsQueryable();
                var mesaEntrada = _context.RmMesaEntrada.AsQueryable();
                //Se utiliza codigo para Operaciones
               // var transaccion = _context.RmTransacciones.AsQueryable();
                // Procesar la lista de números decimales y obtener los datos necesarios de la base de datos
                var listaProcesada = new List<ListadoJefes>();
                foreach (decimal numero in selectedNroEntradas)
                {
                    var queryTransaccion = _context.RmTransacciones
                          .Where(t => t.NumeroEntrada == numero)
                          .GroupBy(result => result.NumeroEntrada)
                          .Select(group => group.OrderByDescending(g => g.FechaAlta).First())
                          .ToList(); // Materializar los resultados aquí

                    var queryMesaEntrada = _context.RmMesaEntrada
                        .Where(e => e.NumeroEntrada == numero)
                        .Join(_context.RmTipoSolicituds,
                            en => en.TipoSolicitud,
                            tipo => tipo.TipoSolicitud,
                            (en, tipo) => new ListadoJefes
                            {
                                NumeroEntrada = en.NumeroEntrada,
                                FechaEntrada = en.FechaEntrada,
                                NomTitular = en.NomTitular,
                                TipoSolicitud = tipo.DescripSolicitud,
                            })
                        .ToList(); // Materializar los resultados aquí

                    var queryTrans = queryMesaEntrada
                        .Join(queryTransaccion,
                            en => en.NumeroEntrada,
                            tipo => tipo.NumeroEntrada,
                            (en, tipo) => new ListadoJefes
                            {
                                NumeroEntrada = en.NumeroEntrada,
                                FechaEntrada = en.FechaEntrada,
                                NomTitular = en.NomTitular,
                                TipoSolicitud = en.TipoSolicitud,
                                NomOperador = tipo.IdUsuario
                            })
                        .ToList(); // Materializar los resultados aquí

                    var queryUsu = queryTrans
                        .Join(_context.Usuarios,
                            en => en.NomOperador,
                            tipo => tipo.CodUsuario,
                            (en, tipo) => new ListadoJefes
                            {
                                NumeroEntrada = en.NumeroEntrada,
                                FechaEntrada = en.FechaEntrada,
                                NomTitular = en.NomTitular,
                                TipoSolicitud = en.TipoSolicitud,
                                NomOperador = tipo.CodPersona
                            })
                        .ToList(); // Materializar los resultados aquí

                    var queryFinal = queryUsu
                        .Join(_context.Personas,
                            en => en.NomOperador,
                            tipo => tipo.CodPersona,
                            (en, tipo) => new ListadoJefes
                            {
                                NumeroEntrada = en.NumeroEntrada,
                                FechaEntrada = en.FechaEntrada,
                                NomTitular = en.NomTitular,
                                TipoSolicitud = en.TipoSolicitud,
                                NomOperador = tipo.Nombre
                            })
                        .ToList(); // Materializar los resultados aquí

                    listaProcesada.AddRange(queryFinal);

                    var entrada = _context.RmMesaEntrada.FirstOrDefault(m => m.NumeroEntrada == numero);
                    var oficina = _context.RmOficinasRegistrales.FirstOrDefault(o => o.CodigoOficina == entrada.CodigoOficina);
                    codOficina = oficina?.CodigoOficina ?? 0;
                }
                ViewBag.Oficina = new SelectList(_context.RmOficinasRegistrales, "CodigoOficina", "DescripOficina");
                // Crear un modelo de datos que contiene la lista procesada
                Reportes tituloData = new Reportes
                {
                    ListadoParaJefes = listaProcesada,
                    FechaActual = DateTime.Now,
                    Usuario = User.Identity.Name,
                    TotalIngresado = listaProcesada.Count,
                    CodigoOficina = codOficina
                };

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
                    Response.Headers["Content-Disposition"] = "inline; filename=Reporte-de-Entrada.pdf";

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
