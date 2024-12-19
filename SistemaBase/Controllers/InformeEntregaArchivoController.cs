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
    public class InformeEntregaArchivoController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public InformeEntregaArchivoController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMENTAR", "Index", "InformeEntregaArchivo" })]

        public IActionResult Index()
        {
            ViewBag.Oficina = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina");
            return View();
        }

        [HttpPost]
        public IActionResult GenerarPdf(Reportes parametros)
        {
            // Obtiene los datos filtrados según los parámetros del modelo
            var datosReporte = GetReporteData(parametros);

            // Lee la imagen y conviértela en una cadena de datos URI codificada en base64
            string imagePath = "wwwroot/assets/img/PJ/PoderJudicial.png"; // Ruta a la imagen en tu proyecto
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

            // Pasa la cadena de datos URI a la vista a través del modelo de vista
            datosReporte.ImageDataUri = imageDataUri;
            if (datosReporte.Salidas.Count() == 0)
            {
                return BadRequest("La vista HTML está vacía o nula.");
                // return Json(new { Success = false, ErrorMessage = "No tiene registros en ese rango de Fecha" });
            }
            // Renderiza la vista Razor a una cadena HTML
            string viewHtml = RenderViewToString("ReportePDF", datosReporte);

            if (string.IsNullOrWhiteSpace(viewHtml))
            {
                return BadRequest("La vista HTML está vacía o nula.");
            }

            // Crear un documento PDF utilizando iText7
            MemoryStream memoryStream = new MemoryStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(memoryStream));
            Document document = new Document(pdfDoc);

            // Agregar el contenido HTML convertido al documento PDF
            HtmlConverter.ConvertToPdf(viewHtml, pdfDoc, new ConverterProperties());

            document.Close();

            // Convertir el MemoryStream a un arreglo de bytes
            byte[] pdfBytes = memoryStream.ToArray();
            memoryStream.Close();

            // Configurar el encabezado Content-Disposition
            Response.Headers["Content-Disposition"] = "inline; filename=Reporte-de-Entrada.pdf";

            // Devolver el PDF como un archivo descargable con el nuevo nombre
            return File(pdfBytes, "application/pdf");
        }

        private string RenderViewToString(string viewName, object model)
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

                viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpPost]
        private Reportes GetReporteData(Reportes parametros)
        {
            var estadoRecibido = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Enviado a Archivo");
            var enviadoTriplicado = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Enviado Triplicado");
            var mesaEntrada = _dbContext.RmMesaEntrada.AsQueryable();
            var transacciones = _dbContext.RmTransacciones.AsQueryable();
            var boletaMarcas = _dbContext.RmBoletasMarcas.AsQueryable();
            var tipoSolicitud = _dbContext.RmTipoSolicituds.AsQueryable();
            var rmMovimientosDoc = _dbContext.RmMovimientosDocs.AsQueryable();
            
            if (parametros.Usuario!=null)
            {
               rmMovimientosDoc = rmMovimientosDoc.Where(e => e.CodUsuario == parametros.Usuario).AsQueryable();
            }

            if (parametros.CodigoOficina != null)
            {
                mesaEntrada = mesaEntrada.Where(e => e.CodOficinaRetiro == parametros.CodigoOficina).AsQueryable();
            }
            else
            {
                mesaEntrada = mesaEntrada.Where(e => e.CodOficinaRetiro != 1).AsQueryable();
            }

            var query = rmMovimientosDoc.Where(e => (e.EstadoEntrada == estadoRecibido.CodigoEstado || e.EstadoEntrada ==enviadoTriplicado.CodigoEstado) && e.FechaOperacion <= parametros.FechaHasta && e.FechaOperacion >= parametros.FechaDesde)
                                                    .AsQueryable()
                                                    .Join(mesaEntrada,
                                                        en => en.NroEntrada,
                                                        tipo => tipo.NumeroEntrada,
                                                        (en, tipo) => new MesaSalida
                                                        {
                                                            NumeroEntrada = en.NroEntrada,
                                                            FechaEntrada = tipo.FechaEntrada,
                                                            Titular = tipo.NomTitular,
                                                            TipoSolicitud = tipo.TipoSolicitud.ToString(),
                                                            UsuarioSalida = tipo.UsuarioSalida,
                                                            //NroBoleta = tipo.NroBoleta
                                                        })
                                                    .Distinct();

            var primer = query.ToList();

            var querySolicitud = query.AsQueryable().Join(tipoSolicitud,
                                                        en => en.TipoSolicitud,
                                                        tipo => tipo.TipoSolicitud.ToString(),
                                                        (en, tipo) => new MesaSalida
                                                        {
                                                            NumeroEntrada = en.NumeroEntrada,
                                                            FechaEntrada = en.FechaEntrada,
                                                            Titular = en.Titular,
                                                            TipoSolicitud = tipo.DescripSolicitud,
                                                            UsuarioSalida = en.UsuarioSalida,
                                                            //NroBoleta = tipo.NroBoleta
                                                        });


            var queryTrans = querySolicitud.AsQueryable().Join(transacciones,
                                             en => en.NumeroEntrada,
                                             tipo => tipo.NumeroEntrada,
                                             (en, tipo) => new MesaSalida
                                             {
                                                 NumeroEntrada = en.NumeroEntrada,
                                                 FechaEntrada = en.FechaEntrada,
                                                 Titular = en.Titular,
                                                 TipoSolicitud = en.TipoSolicitud,
                                                 UsuarioSalida = en.UsuarioSalida,
                                                 NroBoleta = tipo.NroBoleta,
                                                 FechaAlta = tipo.FechaAlta
                                             })
                                         .Distinct()
                                         .ToList(); // Ejecutamos la consulta para obtener los datos de la base de datos

            // Realizamos la conversión de tipo fuera de la expresión LINQ to Entities
            foreach (var item in queryTrans)
            {
                if (!string.IsNullOrEmpty(item.NroBoleta)) // Verifica si NroBoleta no es nulo ni vacío
                {
                    if (!decimal.TryParse(item.NroBoleta, out decimal nroBoletaDecimal))
                    {
                        // Si no se puede convertir a decimal, asumimos que ya es alfanumérico y no hacemos ninguna conversión
                        continue; // Pasamos al siguiente item
                    }

                    // Obtenemos la descripción de la boleta correspondiente al número de boleta
                    var descripcionBoleta = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.NroBoleta == Convert.ToInt32(nroBoletaDecimal))?.Descripcion;
                    if (descripcionBoleta != null)
                    {
                        item.NroBoleta = descripcionBoleta; // Reemplazamos el número de boleta con la descripción
                    }
                }
            }


            // Continuamos con la lógica de tu consulta
            var resultados = queryTrans.GroupBy(x => x.NumeroEntrada)
                                       .Select(g => g.OrderByDescending(e => e.FechaAlta).First())
                                       .ToList();
            // Asigna los valores de FechaActual y TotalIngresados desde el objeto parametros
            parametros.FechaActual = DateTime.Now;
            parametros.TotalIngresado = resultados.Count;

            Reportes tituloData = new();
            tituloData.Salidas = resultados;
            tituloData.FechaDesde = parametros.FechaDesde;
            tituloData.FechaHasta = parametros.FechaHasta;
            tituloData.FechaActual = DateTime.Now;
            tituloData.Usuario = parametros?.Usuario?? User.Identity.Name;
            tituloData.TotalIngresado = resultados.Count;

            return tituloData;
        }



    }
}
