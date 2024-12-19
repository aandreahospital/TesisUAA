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
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace SistemaBase.Controllers
{
    public class InformeDiseñoAprobadoController : Controller
    {


        private readonly DbvinDbContext _dbContext;
        public InformeDiseñoAprobadoController(DbvinDbContext context)
        {
            _dbContext = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMREDIS", "Index", "InformeDiseñoAprobado" })]

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

            ViewBag.FechaActual = parametros.FechaActual;
            ViewBag.TotalIngresado = parametros.TotalIngresado;
            ViewBag.FechaDesde = parametros.FechaDesde;
            ViewBag.FechaHasta = parametros.FechaHasta;
            ViewBag.Usuario = parametros.Usuario;

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
        private Reportes GetReporteData(Reportes parametros)
        {
            var estadoRecibido = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Diseño Aprobado");
            var mesaEntrada = _dbContext.RmMesaEntrada.AsQueryable();
            var tipoSolicitud = _dbContext.RmTipoSolicituds.AsQueryable();

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


            var movimientos = _dbContext.RmMovimientosDocs
       .Where(e => e.FechaOperacion.HasValue &&
                   e.FechaOperacion.Value.Date <= parametros.FechaHasta.Value.Date &&
                   e.FechaOperacion.Value.Date >= parametros.FechaDesde.Value.Date &&
                   e.EstadoEntrada == estadoRecibido.CodigoEstado)
       .AsQueryable();


            var query = movimientos.Join(mesaEntrada,
               en => en.NroEntrada,
               tipo => tipo.NumeroEntrada,
               (en, tipo) => new EntradasCargas
               {
                   NumeroEntrada = tipo.NumeroEntrada,
                   FechaEntrada = tipo.FechaEntrada,
                   NombreTitular = tipo.NomTitular,
                   TipoSolicitud = tipo.TipoSolicitud.ToString(),
                   Usuario = tipo.UsuarioEntrada,
                   NroLiquidacion = tipo.CodigoOficina.ToString()
               }).Distinct();

            var queryTipo = query.AsQueryable().Join(tipoSolicitud,
                me => me.TipoSolicitud,
                ti => ti.TipoSolicitud.ToString(),
                (me, ti) => new EntradasCargas
                {
                    NumeroEntrada = me.NumeroEntrada,
                    FechaEntrada = me.FechaEntrada,
                    NombreTitular = me.NombreTitular,
                    TipoSolicitud = ti.DescripSolicitud,
                    Usuario = me.Usuario,
                    NroLiquidacion = me.NroLiquidacion
                }).Distinct();

            var queryFinal = queryTipo.AsQueryable().Join(_dbContext.RmOficinasRegistrales,
                me => me.NroLiquidacion,
                ti => ti.CodigoOficina.ToString(),
                (me, ti) => new EntradasCargas
                {
                    NumeroEntrada = me.NumeroEntrada,
                    FechaEntrada = me.FechaEntrada,
                    NombreTitular = me.NombreTitular,
                    TipoSolicitud = me.TipoSolicitud,
                    Usuario = me.Usuario,
                    NroLiquidacion = ti.DescripOficina
                }).Distinct();

            // Ordena la lista por FechaEntrada de forma descendente
            var resultados = queryFinal.OrderBy(e => e.NumeroEntrada).ToList();

            // Asigna los valores de FechaActual y TotalIngresados desde el objeto parametros
            parametros.FechaActual = DateTime.Now;
            parametros.TotalIngresado = resultados.Count;
            parametros.Usuario = User.Identity.Name;

            Reportes tituloData = new();
            tituloData.Entradas = resultados;
            tituloData.FechaDesde = parametros.FechaDesde;
            tituloData.FechaHasta = parametros.FechaHasta;
            tituloData.FechaActual = DateTime.Now;
            tituloData.Usuario = User.Identity.Name;
            tituloData.TotalIngresado = resultados.Count;
            tituloData.CodigoOficina = parametros.CodigoOficina;

            ViewBag.Oficina = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina");

            return tituloData;
        }


    }
}
