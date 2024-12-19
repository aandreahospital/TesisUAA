using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using static SistemaBase.ModelsCustom.Reportes;

namespace SistemaBase.Controllers
{
    public class EnvioCoordRegionalController : Controller
    {

        #region Instancia y Constructor

        //Instancia de la base de datos que solo se usara en este controlador
        private readonly DbvinDbContext _context;

        //Constructor del Controlador
        public EnvioCoordRegionalController(DbvinDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Index        
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMENVREG", "Index", "EnvioCoordRegional" })]

        public async Task <IActionResult> Index()
        {
            try
            {

                // Esta acción del controlador realiza una consulta a la base de datos
                // para obtener registros relacionados de múltiples tablas y proyectarlos
                // en una vista. Los registros se filtran por el estado "Recepcionado CoordRegional" Or DescripOfician != "Asuncion"
                // y se ordenan por fecha de alta en orden descendente.
                var queryFinal = (from rme in _context.RmMesaEntrada 
                                  join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                  join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                  join or in _context.RmOficinasRegistrales on rme.CodOficinaRetiro equals or.CodigoOficina
                                  where ee.DescripEstado == "Recepcionado Div Regional" && or.DescripOficina != "Asunción"
                                  orderby or.DescripOficina descending
                                  select new
                                  {
                                      NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                      DescSolicitud = ts.DescripSolicitud,
                                      FechaAlta = rme.FechaEntrada,
                                      Oficina = or.DescripOficina

                                  }).AsEnumerable()
                             .GroupBy(result => result.NroEntrada)
                             .Select(group => group.OrderByDescending(g => g.FechaAlta).First())
                             .Select(result => new Direccion()
                             {
                                 NroEntrada = result.NroEntrada,
                                 DescSolicitud = result.DescSolicitud,
                                 FechaAlta = result.FechaAlta,
                                 Oficina  = result.Oficina

                             });
                return View(queryFinal.ToList());

                //var direccionEntrada = (from rt in _context.RmTransacciones
                //                        join rme in _context.RmMesaEntrada on rt.NumeroEntrada equals rme.NumeroEntrada
                //                        join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                //                        join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                //                        join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                //                        where ee.DescripEstado == "Recepcionado Div Regional" && or.DescripOficina != "Asunción"
                //                        orderby or.DescripOficina descending
                //                        select new Direccion
                //                        {
                //                            NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                //                            DescSolicitud = ts.DescripSolicitud,
                //                            FechaAlta = rt.FechaAlta,
                //                            Oficina = or.DescripOficina

                //                        }).Distinct();

                //return View(await direccionEntrada.AsNoTracking().ToListAsync());


            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción inesperada aquí
                return View("Error");
            }
        }

        // Esta acción del controlador cambia el estado de las mesas de entrada seleccionadas
        // a "Recepcionado CoordRegional". Recorre la lista de números de entrada recibida como datos JSON,
        // busca cada mesa de entrada en la base de datos y actualiza su estado. Luego, registra
        // un nuevo movimiento relacionado con el cambio de estado.


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
                    var estadoEntrada = await _context.RmEstadosEntrada.FirstOrDefaultAsync(p => p.DescripEstado == "Enviado a Seccion Regional");

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

        #endregion

        [HttpPost]
        public async Task<IActionResult> GenerarPdf([FromBody] List<decimal> selectedNroEntradas)
        {
            try
            {
                decimal codOficina = 0;
                var tipoSolicitud = _context.RmTipoSolicituds.AsQueryable();
                var mesaEntrada = _context.RmMesaEntrada.AsQueryable();
                //Se utiliza codigo para Operaciones
                var transaccion = _context.RmTransacciones.AsQueryable();
                // Procesar la lista de números decimales y obtener los datos necesarios de la base de datos
                var listaProcesada = new List<ListadoJefes>();
                foreach (decimal numero in selectedNroEntradas)
                {
                    var queryTransaccion = _context.RmTransacciones
                         .Where(t => t.NumeroEntrada == numero)
                         .GroupBy(result => result.NumeroEntrada)
                         .Select(group => group.OrderByDescending(g => g.FechaAlta).First())
                         .ToList(); // Materializar los resultados aquí



                    var queryTipo = mesaEntrada.Where(m => m.NumeroEntrada == numero).AsQueryable().Join(tipoSolicitud,
                     en => en.TipoSolicitud,
                     tipo => tipo.TipoSolicitud,
                     (en, tipo) => new ListadoJefes
                     {
                         NumeroEntrada = en.NumeroEntrada,
                         TipoSolicitud = tipo.DescripSolicitud,
                         FechaEntrada = en.FechaEntrada,
                         NomTitular = en.NomTitular,
                         NomOperador = en.NroBoleta
                     }).ToList();

                    var segundo = queryTipo.ToList();

                    var queryFinal = queryTipo.AsQueryable().Join(queryTransaccion,
                       en => en.NumeroEntrada,
                       tipo => tipo.NumeroEntrada,
                       (en, tipo) => new ListadoJefes
                       {
                           NumeroEntrada = en.NumeroEntrada,
                           TipoSolicitud = en.TipoSolicitud,
                           FechaEntrada = en.FechaEntrada,
                           NomTitular = en.NomTitular,
                           NomOperador = tipo.NroBoleta
                       }).ToList();

                    var tercer = queryFinal.ToList();

                    // Realizamos la conversión de tipo fuera de la expresión LINQ to Entities
                    foreach (var item in queryFinal)
                    {
                        if (!string.IsNullOrEmpty(item.NomOperador)) // Verifica si NroBoleta no es nulo ni vacío
                        {
                            if (!decimal.TryParse(item.NomOperador, out decimal nroBoletaDecimal))
                            {
                                // Si no se puede convertir a decimal, asumimos que ya es alfanumérico y no hacemos ninguna conversión
                                continue; // Pasamos al siguiente item
                            }

                            // Obtenemos la descripción de la boleta correspondiente al número de boleta
                            var descripcionBoleta = _context.RmBoletasMarcas.FirstOrDefault(b => b.NroBoleta == Convert.ToInt32(nroBoletaDecimal))?.Descripcion;
                            if (descripcionBoleta != null)
                            {
                                item.NomOperador = descripcionBoleta; // Reemplazamos el número de boleta con la descripción
                            }
                        }
                    }


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
