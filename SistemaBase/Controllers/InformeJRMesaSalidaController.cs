﻿using Humanizer;
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
    public class InformeJRMesaSalidaController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public InformeJRMesaSalidaController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMIJRAMS", "Index", "InformeJRMesaSalida" })]

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

            var estadoAprobado= _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Aprobado/JefeRegistral/Firma");
            var estadoNotaNegativa = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Nota Negativa/JefeRegistral");
            var estadoObservado = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Observado/JefeRegistral");

          

            var tipoSolicitud = _dbContext.RmTipoSolicituds.AsQueryable();
            
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
            //Se utiliza codigo para Operaciones
            var transaccion = _dbContext.RmTransacciones.AsQueryable();
            if (parametros.Usuario != null)
            {
                transaccion = transaccion.Where(t => t.IdUsuario == parametros.Usuario).AsQueryable();
            }

            var queryMovimiento = _dbContext.RmMovimientosDocs.AsQueryable();

            if (parametros.FechaDesde != null && parametros.FechaHasta != null)
            {
                // Extraer solo fechas
                var soloFechaDesde = parametros.FechaDesde.Value.Date;
                var soloFechaHasta = parametros.FechaHasta.Value.Date;

                // Extraer solo horas
                var soloHoraDesde = parametros.FechaDesde.Value.TimeOfDay;
                var soloHoraHasta = parametros.FechaHasta.Value.TimeOfDay;

                // Comparar solo fechas
                queryMovimiento = queryMovimiento
                    .Where(e => e.FechaOperacion.HasValue && e.FechaOperacion.Value.Date >= soloFechaDesde && e.FechaOperacion.Value.Date <= soloFechaHasta)
                    .AsQueryable();

                var solfecha = queryMovimiento.ToList();
                // Comparar solo horas
                queryMovimiento = queryMovimiento
                    .Where(e => e.FechaOperacion.HasValue &&
                                e.FechaOperacion.Value.TimeOfDay >= soloHoraDesde &&
                                e.FechaOperacion.Value.TimeOfDay <= soloHoraHasta)
                    .AsQueryable();
                var horas = queryMovimiento.ToList();
            }
            if (parametros.NombreUsuario != null)
            {
                queryMovimiento = queryMovimiento.Where(m => m.CodUsuario == parametros.NombreUsuario).AsQueryable();
            }
            var query = queryMovimiento.AsQueryable().Join(mesaEntrada,
                en => en.NroEntrada,
                tipo => tipo.NumeroEntrada,
                (en, tipo) =>
                new ListadoJefes
                {
                    NumeroEntrada = tipo.NumeroEntrada,
                    TipoSolicitud = tipo.TipoSolicitud.ToString(),
                    FechaEntrada = tipo.FechaEntrada,
                    NomTitular = tipo.CodigoOficina.ToString(),
                    Estado = en.EstadoEntrada
                });

            var primer = query.ToList();
            var tiposSolicitud = new List<string> { "6", "9", "10", "11", "12", "13", "14", "16", "17", "18", "19", "22", "23", "26", "27", "28", "33", "34" };
            var queryTransa = query.Where(e=>e.Estado == estadoNotaNegativa.CodigoEstado
            || e.Estado == estadoObservado.CodigoEstado || (e.Estado == estadoAprobado.CodigoEstado&&(tiposSolicitud.Contains(e.TipoSolicitud)))).AsQueryable().Join(transaccion,
               en => en.NumeroEntrada,
               tipo => tipo.NumeroEntrada,
               (en, tipo) => new ListadoJefes
               {
                   NumeroEntrada = en.NumeroEntrada,
                   TipoSolicitud = en.TipoSolicitud,
                   FechaEntrada = en.FechaEntrada,
                   NomTitular = en.NomTitular,
                   NomOperador = tipo.IdUsuario
               });

            var tercer = queryTransa.ToList();

            var queryTipo = queryTransa.AsQueryable().Join(tipoSolicitud,
               en => en.TipoSolicitud,
               tipo => tipo.TipoSolicitud.ToString(),
               (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, TipoSolicitud = tipo.DescripSolicitud, FechaEntrada = en.FechaEntrada, NomTitular = en.NomTitular, NomOperador = en.NomOperador });

            var segundo = queryTipo.ToList();


            var queryUsu = queryTipo.AsQueryable().Join(_dbContext.Usuarios,
              en => en.NomOperador,
              tipo => tipo.CodUsuario,
              (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, TipoSolicitud = en.TipoSolicitud, FechaEntrada = en.FechaEntrada, NomTitular = en.NomTitular, NomOperador = tipo.CodPersona });

            var cuarto = queryUsu.ToList();

            var queryTitu = queryUsu.OrderByDescending(u => u.FechaEntrada).AsQueryable().Join(_dbContext.Personas,
            en => en.NomOperador,
            tipo => tipo.CodPersona,
            (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, TipoSolicitud = en.TipoSolicitud, FechaEntrada = en.FechaEntrada, NomTitular = en.NomTitular, NomOperador = tipo.Nombre });

            var queryFinal = queryTitu.AsQueryable().Join(_dbContext.RmOficinasRegistrales,
            en => en.NomTitular,
            tipo => tipo.CodigoOficina.ToString(),
            (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, TipoSolicitud = en.TipoSolicitud, FechaEntrada = en.FechaEntrada, NomTitular = tipo.DescripOficina, NomOperador = en.NomOperador });


            // Ordena la lista por el Número de Entrada de forma ascendente
            queryFinal = queryFinal.OrderBy(e => e.NumeroEntrada);

            // Elimina duplicados y selecciona el primer elemento de cada grupo
            var resultados = queryFinal.GroupBy(x => x.NumeroEntrada).Select(g => g.First()).ToList();

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