using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using static iText.IO.Image.Jpeg2000ImageData;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static SistemaBase.ModelsCustom.Reportes;

namespace SistemaBase.Controllers
{
    public class DiseñoFinalizadoController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public DiseñoFinalizadoController(DbvinDbContext context)
        {
            _dbContext = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMDISAPR", "Index", "DiseñoFinalizado" })]
        public async Task<IActionResult> Index()
        {
            var queryFinal = (from ad in _dbContext.RmAsigDis
                              join me in _dbContext.RmMesaEntrada on ad.NroEntrada equals me.NumeroEntrada
                              join ts in _dbContext.RmTipoSolicituds on me.TipoSolicitud equals ts.TipoSolicitud
                              join ee in _dbContext.RmEstadosEntrada on me.EstadoEntrada equals ee.CodigoEstado
                              where ee.DescripEstado == "Diseño Finalizado" /*&& ad.IdUsuarioAsignado == User.Identity.Name*/
                              orderby ad.FechaAsignada descending
                              select new
                              {
                                  NroEntrada = Convert.ToInt32(ad.NroEntrada),
                                  TipoSolicitud = ts.DescripSolicitud,
                                  FechaIngreso = ad.FechaAsignada
                              }).AsEnumerable()
                            .GroupBy(result => result.NroEntrada)
                            .Select(group => group.OrderByDescending(g => g.FechaIngreso).First())
                            .Select(result => new Diseño()
                            {
                                NroEntrada = result.NroEntrada,
                                TipoSolicitud = result.TipoSolicitud,
                                FechaIngreso = result.FechaIngreso
                            });

            //var disenho = (from ad in _dbContext.RmAsigDis
            //               join me in _dbContext.RmMesaEntrada on ad.NroEntrada equals me.NumeroEntrada
            //               join ts in _dbContext.RmTipoSolicituds on me.TipoSolicitud equals ts.TipoSolicitud
            //               join ee in _dbContext.RmEstadosEntrada on me.EstadoEntrada equals ee.CodigoEstado
            //               where ee.DescripEstado== "Diseño Finalizado"
            //               orderby ad.FechaAsignada descending
            //               select new Diseño()
            //               {
            //                   NroEntrada = Convert.ToInt32(ad.NroEntrada),
            //                   TipoSolicitud = ts.DescripSolicitud,
            //                   FechaIngreso = ad.FechaAsignada
            //               });
            return View(queryFinal.ToList());
        }


        [HttpPost]
        public async Task<IActionResult> CambiarEstado([FromBody] List<decimal> selectedNroEntradas)
        {

            foreach (decimal nroEntrada in selectedNroEntradas)
            {
                var mesaEntrada = await _dbContext.RmMesaEntrada.FindAsync(nroEntrada);

                if (mesaEntrada == null)
                {
                    // Manejo si la mesaEntrada no se encuentra
                    return NotFound();
                }
                else
                {
                    var estadoEntrada = await _dbContext.RmEstadosEntrada.FirstOrDefaultAsync(p => p.DescripEstado == "Diseño Aprobado");
                    mesaEntrada.EstadoEntrada = estadoEntrada.CodigoEstado;
                    _dbContext.Update(mesaEntrada);
                    await _dbContext.SaveChangesAsync();

                    RmMovimientosDoc movimientos = new()
                    {
                        NroEntrada = nroEntrada,
                        CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                        FechaOperacion = DateTime.Now,
                        CodOperacion = "01", //Parametro para ingreso a mesa entrada ya que no hay de dise;o
                        NroMovimientoRef = nroEntrada.ToString(),
                        EstadoEntrada = mesaEntrada.EstadoEntrada
                    };
                    await _dbContext.AddAsync(movimientos);
                    await _dbContext.SaveChangesAsync();

                }
                //var asigDis = await _dbContext.RmAsigDis.FirstOrDefaultAsync(p => p.NroEntrada == nroEntrada);
                //if (asigDis == null)
                //{
                //    // Manejo si la asigDis no se encuentra
                //    return NotFound();
                //}
                //else
                //{
                //    asigDis.Recibido = "S";
                //    _dbContext.Update(asigDis);
                //    await _dbContext.SaveChangesAsync();
                //}
            }
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> GenerarPdf([FromBody] List<decimal> selectedNroEntradas)
        {
            try
            {
                // Procesar la lista de números decimales y obtener los datos necesarios de la base de datos
                var listaProcesada = new List<EntradasCargas>();
                foreach (decimal numero in selectedNroEntradas)
                {
                    var query = _dbContext.RmMesaEntrada
                        .Where(e => e.NumeroEntrada == numero)
                        .Join(_dbContext.RmTipoSolicituds,
                            en => en.TipoSolicitud,
                            tipo => tipo.TipoSolicitud,
                            (en, tipo) => new EntradasCargas
                            {
                                NumeroEntrada = en.NumeroEntrada,
                                FechaEntrada = en.FechaEntrada,
                                NombreTitular = en.NomTitular,
                                TipoSolicitud = tipo.DescripSolicitud,
                                Usuario = en.UsuarioEntrada,
                                NroLiquidacion = en.NumeroLiquidacionLetras
                            });
                    listaProcesada.AddRange(query);
                }

                // Crear un modelo de datos que contiene la lista procesada
                Reportes tituloData = new Reportes
                {
                    Entradas = listaProcesada,
                    FechaActual = DateTime.Now,
                    Usuario = User.Identity.Name,
                    TotalIngresado = listaProcesada.Count
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
