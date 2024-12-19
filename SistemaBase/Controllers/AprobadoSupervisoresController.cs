using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using System.Linq;
using static SistemaBase.ModelsCustom.Reportes;

namespace SistemaBase.Controllers
{
    public class AprobadoSupervisoresController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public AprobadoSupervisoresController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMRTRASU", "Index", "AprobadoSupervisores" })]

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
            //var tipoSoli = _dbContext.RmTiposOperaciones.FirstOrDefault(t => t.DescripTipoOperacion != "CERTIFICADO DE CONDICION DE DOMINIO" || t.DescripTipoOperacion != "INFORME");
            //var query = _dbContext.RmMesaEntrada.Where(m=>m.TipoSolicitud== tipoSoli.TipoSolicitud).AsQueryable();

            var estadoRecibido = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Aprobado/Supervisor");
            var tipoSolicitud = _dbContext.RmTiposOperaciones.AsQueryable();
            var mesaEntrada = _dbContext.RmMesaEntrada.AsQueryable();
            if (parametros.Oficina != null)
            {
                if (parametros.Oficina == "C")
                {
                    mesaEntrada = mesaEntrada.Where(m => m.CodigoOficina == 1).AsQueryable();
                }
                else
                {
                    mesaEntrada = mesaEntrada.Where(m => m.CodigoOficina != 1).AsQueryable();
                }

            }
            var transaccion = _dbContext.RmTransacciones.AsQueryable();
            if (parametros.Usuario != null)
            {
                transaccion = transaccion.Where(t => t.UsuarioSup == parametros.Usuario).AsQueryable();
            }

            var query = _dbContext.RmMovimientosDocs
                .Where(e => e.FechaOperacion <= parametros.FechaHasta && e.FechaOperacion >= parametros.FechaDesde && e.EstadoEntrada == estadoRecibido.CodigoEstado)
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
                            NomTitular = tipo.CodigoOficina.ToString()
                            //UsuarioSup=en.CodUsuario
                        })
                .OrderBy(l => l.NumeroEntrada)  // Ordenar por NumeroEntrada antes de Distinct
                .Distinct();

            var primer = query.ToList();

            var queryTransa = query.AsQueryable()
                .Join(transaccion,
                    en => en.NumeroEntrada,
                    tipo => tipo.NumeroEntrada,
                    (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, TipoSolicitud = tipo.TipoOperacion, FechaEntrada = en.FechaEntrada, FechaAlta = tipo.FechaAlta, NomTitular = en.NomTitular, UsuarioSup = tipo.UsuarioSup, NomOperador = tipo.IdUsuario })
                .Distinct();

            var tercer = queryTransa.ToList();

            var queryTipo = queryTransa.AsQueryable()
                .Join(tipoSolicitud,
                    en => en.TipoSolicitud,
                    tipo => tipo.TipoOperacion.ToString(),
                    (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, TipoSolicitud = tipo.DescripTipoOperacion, FechaEntrada = en.FechaEntrada, FechaAlta = en.FechaAlta, NomTitular = en.NomTitular, UsuarioSup = en.UsuarioSup, NomOperador = en.NomOperador })
                .Distinct();

            var segundo = queryTipo.ToList();

            var queryUsu = queryTipo.AsQueryable()
                .Join(_dbContext.Usuarios,
                    en => en.NomOperador,
                    tipo => tipo.CodUsuario,
                    (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, TipoSolicitud = en.TipoSolicitud, FechaEntrada = en.FechaEntrada, FechaAlta = en.FechaAlta, NomTitular = en.NomTitular, UsuarioSup = en.UsuarioSup, NomOperador = tipo.CodPersona })
                .Distinct();

            var cuarto = queryUsu.ToList();

            var queryTitu = queryUsu.OrderByDescending(u => u.FechaEntrada).AsQueryable()
                .Join(_dbContext.Personas,
                    en => en.NomOperador,
                    tipo => tipo.CodPersona,
                    (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, TipoSolicitud = en.TipoSolicitud, FechaEntrada = en.FechaEntrada, FechaAlta = en.FechaAlta, NomTitular = en.NomTitular, UsuarioSup = en.UsuarioSup, NomOperador = tipo.Nombre })
                .Distinct();

            var queryFinal = queryTitu.AsQueryable()
                .Join(_dbContext.RmOficinasRegistrales,
                    en => en.NomTitular,
                    tipo => tipo.CodigoOficina.ToString(),
                    (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, TipoSolicitud = en.TipoSolicitud, FechaEntrada = en.FechaEntrada, FechaAlta = en.FechaAlta, NomTitular = tipo.DescripOficina, UsuarioSup = en.UsuarioSup, NomOperador = en.NomOperador })
                .Distinct();


            // Agrega más filtros según los otros parámetros del modelo
            if (parametros.NumeroEntradaDesde > 0)
            {
                queryFinal = queryFinal.Where(e => e.NumeroEntrada >= parametros.NumeroEntradaDesde).AsQueryable();
            }
            if (parametros.NumeroEntradaHasta > 0)
            {
                queryFinal = queryFinal.Where(e => e.NumeroEntrada <= parametros.NumeroEntradaHasta).AsQueryable();
            }

            // Ordena la lista por el Número de Entrada de forma ascendente
            var queryFinalOrdered = queryFinal.OrderBy(e => e.NumeroEntrada);

            // Agrupa por número de entrada y selecciona el elemento con la fecha más alta en cada grupo
            var resultados = queryFinalOrdered.GroupBy(x => x.NumeroEntrada)
                                               .Select(g => g.OrderByDescending(e => e.FechaAlta).First())
                                               .ToList();

            // Asigna los valores de FechaActual y TotalIngresados desde el objeto parametros
            parametros.FechaActual = DateTime.Now;
            parametros.TotalIngresado = resultados.Count;

            Reportes tituloData = new();
            tituloData.ListadoParaJefes = resultados;
            tituloData.FechaDesde = parametros.FechaDesde;
            tituloData.FechaHasta = parametros.FechaHasta;
            tituloData.FechaActual = DateTime.Now;
            tituloData.Usuario = User.Identity.Name;
            tituloData.TotalIngresado = resultados.Count;
            tituloData.UsuarioSup = cuarto.FirstOrDefault()?.UsuarioSup; // Asigna UsuarioSup aquí


            return tituloData;
        }



    }
}
