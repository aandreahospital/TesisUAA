using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Ini;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using static SistemaBase.ModelsCustom.Reportes;
using System.Data;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Html2pdf;
using iText.Commons.Actions.Contexts;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scryber.Data;

namespace SistemaBase.Controllers
{
    public class InformeEntradaNroFechaController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public InformeEntradaNroFechaController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        // GET: InformeEntradaNroFecha
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMRINROE", "Index", "InformeEntradaNroFecha" })]

        public ActionResult Index()
        {
            ViewBag.Oficina = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina");
            return View();
        }

        [HttpPost]
        public IActionResult GenerarPdf(Reportes parametros)
        {
          
            // return Json(new { Success = false, ErrorMessage = "Debe ingresar la Fecha Hasta." });
            // Obtiene los datos filtrados según los parámetros del modelo
            var datosReporte = GetReporteData(parametros);

            ViewBag.FechaActual = parametros.FechaActual;
            ViewBag.TotalIngresado = parametros.TotalIngresado;
            ViewBag.FechaDesde = parametros.FechaDesde;
            ViewBag.FechaHasta = parametros.FechaHasta;
            ViewBag.Usuario = parametros.Usuario;
            ViewBag.CodigoOficina = parametros.CodigoOficina;

            // Lee la imagen y conviértela en una cadena de datos URI codificada en base64
            string imagePath = "wwwroot/assets/img/PJ/PoderJudicial.png"; // Ruta a la imagen en tu proyecto
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

            // Pasa la cadena de datos URI a la vista a través del modelo de vista
            datosReporte.ImageDataUri = imageDataUri;
            if (datosReporte.Entradas.Count() == 0)
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
        public Reportes GetReporteData(Reportes parametros)
        {
            IQueryable<RmMesaEntradum> queryMesaEntrada = _dbContext.RmMesaEntrada.Where(e => e.FechaEntrada <= parametros.FechaHasta && e.FechaEntrada >= parametros.FechaDesde).AsQueryable();

            if (parametros.CodigoOficina.HasValue)
            {
                // Filtra los registros basados en parametros.CodigoOficina
                queryMesaEntrada = queryMesaEntrada.Where(m => m.CodigoOficina == parametros.CodigoOficina.Value);
            }
            else
            {
                // Filtra los registros donde CodigoOficina no sea igual a 1 si parametros.CodigoOficina es nulo
                queryMesaEntrada = queryMesaEntrada.Where(e => e.CodigoOficina != 1);
            }

            //if (parametros.CodigoOficina != null)
            //{
            //    queryMesaEntrada = queryMesaEntrada.Where(m => m.CodigoOficina == parametros.CodigoOficina);
            //}
            IQueryable<RmOficinasRegistrale> queryOficina = _dbContext.RmOficinasRegistrales.AsQueryable();
            IQueryable<RmTipoSolicitud> queryTipoSolicitud = _dbContext.RmTipoSolicituds.AsQueryable();
            IQueryable<RmReingreso> queryReingreso = _dbContext.RmReingresos.AsQueryable();
            IQueryable<RmMovimientosDoc> queryMovimientos = _dbContext.RmMovimientosDocs.Where(e => e.FechaOperacion <= parametros.FechaHasta && e.FechaOperacion >= parametros.FechaDesde && (e.EstadoEntrada == 8 || e.EstadoEntrada == 0)).AsQueryable().Distinct();

            var primer = queryMovimientos.ToList();

            var queryOficinas = queryMesaEntrada.OrderByDescending(u => u.FechaEntrada).AsQueryable().Join(queryOficina,
            en => en.CodigoOficina,
            tipo => tipo.CodigoOficina,
            (en, tipo) => new EntradasCargas
            {
                NumeroEntrada = en.NumeroEntrada,
                NombreTitular = en.NomTitular,
                Reingreso = en.Reingreso,
                TipoSolicitud = en.TipoSolicitud.ToString(),
                UsuarioEntrada = en.UsuarioEntrada,
                NroLiquidacion = en.NumeroLiquidacionLetras,
                FechaEntrada = en.FechaEntrada,
                Oficina = tipo.DescripOficina

            }).Distinct();

           // var queryEntrada = queryOficinas.AsQueryable().Join(queryMovimientos,
           //en => en.NumeroEntrada,
           //tipo => tipo.NroEntrada,
           //(en, tipo) => new EntradasCargas
           //{
           //    NumeroEntrada = en.NumeroEntrada,
           //    NombreTitular = en.NombreTitular,
           //    Reingreso = en.Reingreso,
           //    TipoSolicitud = en.TipoSolicitud,
           //    UsuarioEntrada = en.UsuarioEntrada,
           //    NroLiquidacion = en.NroLiquidacion,
           //    FechaEntrada = en.FechaEntrada,
           //    Oficina = en.Oficina

           //}).Distinct();

            var prime = queryOficinas.ToList();

            var query = queryOficinas.AsQueryable().Join(queryTipoSolicitud,
            en => en.TipoSolicitud,
            tipo => tipo.TipoSolicitud.ToString(),
            (en, tipo) => new EntradasCargas
            {
                NumeroEntrada = en.NumeroEntrada,
                NombreTitular = en.NombreTitular,
                Reingreso = en.Reingreso,
                TipoSolicitud = tipo.DescripSolicitud,
                UsuarioEntrada = en.UsuarioEntrada,
                NroLiquidacion = en.NroLiquidacion,
                FechaEntrada = en.FechaEntrada,
                Oficina = en.Oficina
            }).Distinct();

            var primero = query.ToList();

            // Aplicar el ordenamiento por FechaEntrada de forma descendente
            query = query.OrderBy(e => e.FechaEntrada).AsQueryable();

            // Agrega más filtros según los otros parámetros del modelo
            if (parametros.NumeroEntradaDesde > 0)
            {
                query = query.Where(e => e.NumeroEntrada >= parametros.NumeroEntradaDesde).AsQueryable();
            }
            if (parametros.NumeroEntradaHasta > 0)
            {
                query = query.Where(e => e.NumeroEntrada <= parametros.NumeroEntradaHasta).AsQueryable();
            }

            // Asigna los valores de FechaActual y TotalIngresados desde el objeto parametros
            parametros.FechaActual = DateTime.Now;
            parametros.TotalIngresado = primero.Count();
            parametros.Usuario = User.Identity.Name;




            Reportes tituloData = new()
            {
                Entradas = query.ToList(),
                FechaDesde = parametros.FechaDesde,
                FechaHasta = parametros.FechaHasta,
                FechaActual = DateTime.Now,
                Usuario = User.Identity.Name,
                TotalIngresado = parametros.TotalIngresado,
                //CodigoOficina = parametros.CodigoOficina
            };

            ViewBag.Oficina = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina");

            return tituloData;
        }

    }
}



