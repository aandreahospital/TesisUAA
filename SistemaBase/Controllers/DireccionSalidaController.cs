using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using static SistemaBase.ModelsCustom.Reportes;

namespace SistemaBase.Controllers
{
    public class DireccionSalidaController : Controller
    {

        private readonly DbvinDbContext _context;

        public DireccionSalidaController(DbvinDbContext context)
        {
            _context = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMDIRSA", "Index", "DireccionSalida" })]

        public async Task<IActionResult> Index()
        {
            var direccionSalida = (from rme in _context.RmMesaEntrada
                                    join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                    join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                   join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                   where ee.DescripEstado == "Recibido Direccion"
                                    //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" 
                                    //    || ts.DescripSolicitud != "INFORME"
                                    //    || ts.DescripSolicitud != "PRENDA" 
                                    //    || ts.DescripSolicitud != "COPIA" 
                                    //    || ts.DescripSolicitud != "COPIA JUDICIAL"
                                    //    || ts.DescripSolicitud != "EMBARGO EJECUTIVO" 
                                    //    || ts.DescripSolicitud != "EMBARGO PREVENTIVO"
                                    //    || ts.DescripSolicitud != "RECTIFICACION" 
                                    //    || ts.DescripSolicitud != "USUFRUCTO" 
                                    //    || ts.DescripSolicitud != "FIANZA"
                                    //    || ts.DescripSolicitud != "LEVANTAMIENTO" 
                                    //    || ts.DescripSolicitud != "LITIS" 
                                    //    || ts.DescripSolicitud != "INFORME JUDICIAL"
                                    //    || ts.DescripSolicitud != "CANCELACION" 
                                    //    || ts.DescripSolicitud != "ANOTACION DE INSCRIPCION PREVENTIVA"
                                    //    || ts.DescripSolicitud != "PROHIBICIÓN DE INNOVAR Y CONTRATAR")
                                  
                                   orderby rme.FechaEntrada descending
                                    select new Direccion
                                    {
                                        NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                        DescSolicitud = ts.DescripSolicitud,
                                        FechaAlta = rme.FechaEntrada,
                                        Oficina = or.DescripOficina

                                    });

            return View(await direccionSalida.AsNoTracking().ToListAsync());
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
                    var estadoEntrada = await _context.RmEstadosEntrada.FirstOrDefaultAsync(p => p.DescripEstado == "Salida Direccion");

                    mesaEntrada.EstadoEntrada = estadoEntrada.CodigoEstado;
                    _context.Update(mesaEntrada);
                    await _context.SaveChangesAsync();

                    RmMovimientosDoc movimientos = new()
                    {
                        NroEntrada = nroEntrada,
                        CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                        FechaOperacion = DateTime.Now,
                        CodOperacion = "38", //parametro para cambio de estado 
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
                                FechaEntrada = tipo.FechaAlta,
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


                }

                // Crear un modelo de datos que contiene la lista procesada
                Reportes tituloData = new Reportes
                {
                    ListadoParaJefes = listaProcesada,
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


        public async Task<IActionResult> AddRechazar(decimal nEntrada)
        {
            try
            {
                var mesaentrada = await _context.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                if (mesaentrada == null)
                {
                    return NotFound();
                }
                else
                {
                    var tipoSolicitud = _context.RmTipoSolicituds.FirstOrDefault(t=>t.TipoSolicitud==mesaentrada.TipoSolicitud);
                    Direccion direccion = new()
                    {
                        DescSolicitud = tipoSolicitud?.DescripSolicitud,
                        NroEntrada =Convert.ToInt32(nEntrada) 
                    };
                    return View("Rechazar", direccion);
                }

            }
            catch (Exception ex)
            {
                // Manejar el error de generación de PDF de alguna manera
                return BadRequest("Error al cargar la pagina " + ex.Message);
            }

        }


        [HttpPost]
        public async Task<IActionResult> Rechazar(int id, string nuevoEstado, string comentario)
        {
            try
            {
                var mesaEntrada = await _context.RmMesaEntrada.FirstOrDefaultAsync(t => t.NumeroEntrada == id);

                if (mesaEntrada != null)
                {
                    var transaccion = await _context.RmTransacciones.FirstOrDefaultAsync(t => t.NumeroEntrada == id);

                    var estadoEntrada = await _context.RmEstadosEntrada.FirstOrDefaultAsync(e => e.DescripEstado == nuevoEstado);
                    mesaEntrada.EstadoEntrada = estadoEntrada.CodigoEstado;
                    _context.Update(mesaEntrada);
                    RmMovimientosDoc movimientos = new()
                    {
                        NroEntrada = id,
                        CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                        FechaOperacion = DateTime.Now,
                        CodOperacion = "09", //Parametro para cambio de estado 
                        NroMovimientoRef = id.ToString(),
                        EstadoEntrada = mesaEntrada.EstadoEntrada
                    };
                    await _context.AddAsync(movimientos);
                    await _context.SaveChangesAsync();
                    if (transaccion == null)
                    {
                        // Manejo si la transacción no se encuentra
                        return NotFound();
                    }
                    else
                    {
                        transaccion.EstadoTransaccion = estadoEntrada.CodigoEstado.ToString();
                        transaccion.UsuarioSup = User.Identity.Name;
                        if (comentario == "")
                        {
                            transaccion.ObservacionSup = transaccion.ObservacionSup;
                        }
                        else
                        {
                            transaccion.ObservacionSup = comentario;
                        }
                        _context.Update(transaccion);
                        await _context.SaveChangesAsync();
                    }
                    return View("Index");
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return BadRequest("Error al cargar la pagina " + ex.Message);

            }

        }
    }
}
