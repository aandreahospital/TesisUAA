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
    public class InformeDireccionSalidaController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public InformeDireccionSalidaController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMREDIR", "Index", "InformeDireccionSalida" })]

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

            var estadoRecibido = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Salida Direccion");

            var mesaEntrada = _dbContext.RmMesaEntrada.AsQueryable();
            if (parametros.Oficina != null)
            {
                if (parametros.Oficina == "C")
                {
                    mesaEntrada = mesaEntrada.Where(m => m.CodigoOficina == 1).AsQueryable();
                }
                else if(parametros.Oficina == "I")
                {
                    mesaEntrada = mesaEntrada.Where(m => m.CodigoOficina != 1).AsQueryable();
                }

            }
            var transaccion = _dbContext.RmTransacciones.AsQueryable();
           

            var queryMovimiento = _dbContext.RmMovimientosDocs.AsQueryable();

            if (parametros.Usuario != null)
            {
                queryMovimiento = queryMovimiento.Where(t => t.CodUsuario == parametros.Usuario).AsQueryable();
            }

            var query = queryMovimiento
                .Where(e => e.EstadoEntrada == estadoRecibido.CodigoEstado && e.FechaOperacion >= parametros.FechaDesde && e.FechaOperacion <= parametros.FechaHasta)
                .AsQueryable()
                .Join(mesaEntrada,
                    en => en.NroEntrada,
                    tipo => tipo.NumeroEntrada,
                    (en, tipo) => new ListadoJefes
                    {
                        NumeroEntrada = tipo.NumeroEntrada,
                        TipoSolicitud = tipo.TipoSolicitud.ToString(),
                        FechaEntrada = tipo.FechaEntrada,
                        NomTitular = tipo.CodigoOficina.ToString()
                    })
                .Distinct();

            var primer = query.ToList();

            var queryTransa = query.AsQueryable().Join(transaccion,
               en => en.NumeroEntrada,
               tipo => tipo.NumeroEntrada,
               (en, tipo) => new ListadoJefes
               {
                   NumeroEntrada = en.NumeroEntrada,
                   TipoSolicitud = tipo.TipoOperacion,
                   FechaEntrada = en.FechaEntrada,
                   NomTitular = en.NomTitular,
                   NomOperador = tipo.IdUsuario,
                   FechaAlta = tipo.FechaAlta
               }).Distinct();

            var tercer = queryTransa.ToList();

            var queryTipoOpe = queryTransa.AsQueryable().Join(_dbContext.RmTiposOperaciones,
              en => en.TipoSolicitud,
              tipo => tipo.TipoOperacion.ToString(),
              (en, tipo) => new ListadoJefes
              {
                  NumeroEntrada = en.NumeroEntrada,
                  TipoSolicitud = tipo.DescripTipoOperacion,
                  FechaEntrada = en.FechaEntrada,
                  NomTitular = en.NomTitular,
                  NomOperador = en.NomOperador,
                  FechaAlta = en.FechaAlta
              }).Distinct();

            var operacion = queryTransa.ToList();



            var queryUsu = queryTipoOpe.AsQueryable().Join(_dbContext.Usuarios,
              en => en.NomOperador,
              tipo => tipo.CodUsuario,
              (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, TipoSolicitud = en.TipoSolicitud, FechaEntrada = en.FechaEntrada, FechaAlta = en.FechaAlta, NomTitular = en.NomTitular, NomOperador = tipo.CodPersona }).Distinct();

            var cuarto = queryUsu.ToList();

            var queryTitu = queryUsu.AsQueryable().Join(_dbContext.Personas,
            en => en.NomOperador,
            tipo => tipo.CodPersona,
            (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, TipoSolicitud = en.TipoSolicitud, FechaEntrada = en.FechaEntrada, FechaAlta = en.FechaAlta, NomTitular = en.NomTitular, NomOperador = tipo.Nombre }).Distinct();


            var queryFinal = queryTitu.AsQueryable().Join(_dbContext.RmOficinasRegistrales,
          en => en.NomTitular,
          tipo => tipo.CodigoOficina.ToString(),
          (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, TipoSolicitud = en.TipoSolicitud, FechaEntrada = en.FechaEntrada, FechaAlta = en.FechaAlta, NomTitular = tipo.DescripOficina, NomOperador = en.NomOperador }).Distinct();


            // Agrega más filtros según los otros parámetros del modelo
            //if (parametros.NumeroEntradaDesde > 0)
            //{
            //    queryFinal = queryFinal.Where(e => e.NumeroEntrada >= parametros.NumeroEntradaDesde).AsQueryable();
            //}
            //if (parametros.NumeroEntradaHasta > 0)
            //{
            //    queryFinal = queryFinal.Where(e => e.NumeroEntrada <= parametros.NumeroEntradaHasta).AsQueryable();
            //}

            // Ordena la lista por el Número de Entrada de forma ascendente
            queryFinal = queryFinal.OrderBy(e => e.NumeroEntrada);

            // Agrupa por número de entrada y selecciona el elemento con la fecha más alta en cada grupo
            var resultados = queryFinal.GroupBy(x => x.NumeroEntrada)
                                               .Select(g => g.OrderByDescending(e => e.FechaAlta).First())
                                               .ToList();

            //var resultados = queryFinal.OrderBy(o => o.NumeroEntrada).GroupBy(x => x.NumeroEntrada).Select(g => g.First()).ToList();
            //var resultados = queryFinal.ToList();

            //// Ordena la lista por la FechaEntrada de forma ascendente
            //queryFinal = queryFinal.OrderBy(e => e.FechaEntrada);

            //// Elimina duplicados y selecciona el primer elemento de cada grupo
            //var resultados = queryFinal.GroupBy(x => x.NumeroEntrada).Select(g => g.First()).ToList();

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

            return tituloData;
        }
    }
}
