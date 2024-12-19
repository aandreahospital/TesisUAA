using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using static SistemaBase.ModelsCustom.Reportes;

namespace SistemaBase.Controllers
{
    public class InformeOficinaRegionalController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public InformeOficinaRegionalController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMIDR", "Index", "InformeOficinaRegional" })]

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
            if (datosReporte.ListadoParaJefes.Count() == 0)
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
            //var tipoSoli = _dbContext.RmTiposOperaciones.Where(t => t.DescripTipoOperacion == "CERTIFICADO DE CONDICION DE DOMINIO" || t.DescripTipoOperacion == "INFORME").FirstOrDefault();
            //var query = _dbContext.RmMesaEntrada.Where(m=>m.TipoSolicitud== tipoSoli.TipoSolicitud).AsQueryable();

            var estadoRegional = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Enviado a Seccion Regional");

            //var inscripcion = _dbContext.RmTipoSolicituds.FirstOrDefault(c => c.DescripSolicitud == "INSCRIPCION");
            //var reinscripcion = _dbContext.RmTipoSolicituds.FirstOrDefault(c => c.DescripSolicitud == "REINSCRIPCION");

            var tipoSolicitud = _dbContext.RmTipoSolicituds.AsQueryable();

            var mesaEntrada = _dbContext.RmMesaEntrada.AsQueryable();
            if (parametros.CodigoOficina != null)
            {
                mesaEntrada = mesaEntrada.Where(e => e.CodOficinaRetiro == parametros.CodigoOficina).AsQueryable();
            }
            else
            {
                mesaEntrada = mesaEntrada.Where(e => e.CodOficinaRetiro != 1).AsQueryable();
            }
            //Se utiliza codigo para Operaciones
            var transaccion = _dbContext.RmTransacciones.AsQueryable();
            if (parametros.Usuario != null)
            {
                transaccion = transaccion.Where(t => t.IdUsuario == parametros.Usuario).AsQueryable();
            }
            //var query = _dbContext.RmMovimientosDocs.Where(e => e.FechaOperacion <= parametros.FechaHasta && e.FechaOperacion >= parametros.FechaDesde && e.EstadoEntrada == estadoRegional.CodigoEstado).AsQueryable().Join(mesaEntrada,
            //    en => en.NroEntrada,
            //    tipo => tipo.NumeroEntrada,
            //    (en, tipo) =>
            //    new ListadoJefes
            //    {
            //        NumeroEntrada = tipo.NumeroEntrada,
            //        TipoSolicitud = tipo.TipoSolicitud.ToString(),
            //        FechaEntrada = tipo.FechaEntrada,
            //        NomTitular = tipo.NomTitular,
            //        NomOperador = tipo.EstadoEntrada.ToString()
            //    }).Distinct();

            var querytiposolicitud = _dbContext.RmMovimientosDocs
            .Where(e => e.EstadoEntrada == estadoRegional.CodigoEstado && e.FechaOperacion <= parametros.FechaHasta && e.FechaOperacion >= parametros.FechaDesde)
            .AsQueryable()
            .Join(mesaEntrada,
            en => en.NroEntrada,
            tipo => tipo.NumeroEntrada,
            (en, tipo) =>
                new ListadoJefes
            {
                NumeroEntrada = tipo.NumeroEntrada,
                TipoSolicitud = tipo.TipoSolicitud.ToString(),
                FechaEntrada = tipo.FechaEntrada,
                NomTitular = tipo.NomTitular,
                NomOperador = tipo.NroBoleta
            })
            .OrderBy(l => l.NumeroEntrada)  // Ordenar por NumeroEntrada antes de Distinct
            .Distinct();

            var primer = querytiposolicitud.ToList();


            var querySolicitud = querytiposolicitud.AsQueryable().Join(tipoSolicitud,
                                                      en => en.TipoSolicitud,
                                                      tipo => tipo.TipoSolicitud.ToString(),
                                                      (en, tipo) => new ListadoJefes
                                                      {
                                                          NumeroEntrada = en.NumeroEntrada,
                                                          TipoSolicitud = tipo.DescripSolicitud,
                                                          FechaEntrada = en.FechaEntrada,
                                                          NomTitular = en.NomTitular,
                                                          NomOperador = en.NomOperador
                                                      });

            var queryTransa = querySolicitud.AsQueryable().Join(transaccion,
               en => en.NumeroEntrada,
               tipo => tipo.NumeroEntrada,
               (en, tipo) => new ListadoJefes
               {
                   NumeroEntrada = en.NumeroEntrada,
                   TipoSolicitud = en.TipoSolicitud,
                   FechaEntrada = en.FechaEntrada,
                   NomTitular = en.NomTitular,
                   NomOperador = tipo.NroBoleta,
                   FechaAlta = tipo.FechaAlta
               }).Distinct().ToList();

            var prueba = queryTransa.ToList();




            // Realizamos la conversión de tipo fuera de la expresión LINQ to Entities
            foreach (var item in queryTransa)
            {
                if (!string.IsNullOrEmpty(item.NomOperador)) // Verifica si NroBoleta no es nulo ni vacío
                {
                    if (!decimal.TryParse(item.NomOperador, out decimal nroBoletaDecimal))
                    {
                        // Si no se puede convertir a decimal, asumimos que ya es alfanumérico y no hacemos ninguna conversión
                        continue; // Pasamos al siguiente item
                    }

                    // Obtenemos la descripción de la boleta correspondiente al número de boleta
                    var descripcionBoleta = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.NroBoleta == Convert.ToInt32(nroBoletaDecimal))?.Descripcion;
                    if (descripcionBoleta != null)
                    {
                        item.NomOperador = descripcionBoleta; // Reemplazamos el número de boleta con la descripción
                    }
                }
            }


            var resultado = queryTransa.GroupBy(x => x.NumeroEntrada)
                                        .Select(g => g.OrderByDescending(e => e.FechaAlta).First())
                                        .ToList();

            // Asigna los valores de FechaActual y TotalIngresados desde el objeto parametros
            parametros.FechaActual = DateTime.Now;
            parametros.TotalIngresado = resultado.Count;

            //var oficina = _dbContext.RmOficinasRegistrales.FirstOrDefault(o=>o.CodigoOficina== parametros.CodigoOficina);

            Reportes tituloData = new();
            tituloData.ListadoParaJefes = resultado;
            tituloData.FechaDesde = parametros.FechaDesde;
            tituloData.FechaHasta = parametros.FechaHasta;
            tituloData.FechaActual = DateTime.Now;
            tituloData.Usuario = User.Identity.Name;
            tituloData.TotalIngresado = resultado.Count;
            tituloData.CodigoOficina = parametros.CodigoOficina;

            ViewBag.Oficina = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina");

            return tituloData;
        }

    }
}
