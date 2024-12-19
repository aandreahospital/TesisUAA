using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using System.Linq;
using static SistemaBase.ModelsCustom.Reportes;

namespace SistemaBase.Controllers
{
    public class InformeProduccionMensualController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public InformeProduccionMensualController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        /// <summary>
        //[TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMINMEN", "Index", "InformeProducciónMensual" })]
        /// </summary>
        /// <returns></returns>

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GenerarPdf(Reportes parametros)
        {
            // Obtiene los datos filtrados según los parámetros del modelo
            var datosReporte = GetReporteData(parametros);

            ViewBag.FechaActual = parametros.FechaActual;
            ViewBag.FechaDesde = parametros.FechaDesde;
            ViewBag.FechaHasta = parametros.FechaHasta;
            ViewBag.Usuario = parametros.Usuario;

            // Lee la imagen y conviértela en una cadena de datos URI codificada en base64
            string imagePath = "wwwroot/assets/img/PJ/PoderJudicial.png"; // Ruta a la imagen en tu proyecto
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

            // Pasa la cadena de datos URI a la vista a través del modelo de vista
            datosReporte.ImageDataUri = imageDataUri;

            string viewHtml= null;

            if (datosReporte.TipoUsuario == "MesaEntrada")
            {
                // Renderiza la vista Razor a una cadena HTML
                viewHtml = RenderViewToString("MesaEntradaPDF", datosReporte);
            }
            if (datosReporte.TipoUsuario == "Disenho")
            {
                // Renderiza la vista Razor a una cadena HTML
                viewHtml = RenderViewToString("DisenhoPDF", datosReporte);
            }
            if (datosReporte.TipoUsuario == "Registrador")
            {
                // Renderiza la vista Razor a una cadena HTML
                viewHtml = RenderViewToString("RegistradorPDF", datosReporte);
            }
            if (datosReporte.TipoUsuario == "JefaRegistral")
            {
                // Renderiza la vista Razor a una cadena HTML
                viewHtml = RenderViewToString("JefaRegistralPDF", datosReporte);
            }
            if (datosReporte.TipoUsuario == "Supervision")
            {
                // Renderiza la vista Razor a una cadena HTML
                viewHtml = RenderViewToString("SupervisionPDF", datosReporte);
            }
            if (datosReporte.TipoUsuario == "Regional")
            {
                // Renderiza la vista Razor a una cadena HTML
                viewHtml = RenderViewToString("RegionalPDF", datosReporte);
            }

            // Crear un documento PDF utilizando iText7
            MemoryStream memoryStream = new MemoryStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(memoryStream));
            Document document = new Document(pdfDoc);

            if (viewHtml != null)
            {
                // Agregar el contenido HTML convertido al documento PDF
                HtmlConverter.ConvertToPdf(viewHtml, pdfDoc, new ConverterProperties());
            }
            else
            {
                // Manejar el caso en que viewHtml es nulo
                return BadRequest("El contenido HTML es nulo");
            }

            document.Close();

            // Convertir el MemoryStream a un arreglo de bytes
            byte[] pdfBytes = memoryStream.ToArray();
            memoryStream.Close();

            // Generar el nombre del archivo PDF con el formato deseado
            string fileName = $"Informe-Mensual-Produccion.pdf";

            // Devolver el PDF como un archivo descargable con el nuevo nombre
            return File(pdfBytes, "application/pdf", fileName);
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
            var mesaEntrada = _dbContext.RmMesaEntrada.AsQueryable();

            var queryMovimiento = _dbContext.RmMovimientosDocs.Where(m=>m.CodUsuario==parametros.Usuario).AsQueryable();

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

            var usuario = _dbContext.Usuarios.FirstOrDefault(p => p.CodUsuario == parametros.Usuario);
            Reportes tituloData = new();
            tituloData.FechaActual = DateTime.Now;
            tituloData.Usuario = parametros.Usuario;
            tituloData.NombreUsuario = parametros.NombreUsuario;
            tituloData.FechaDesde = parametros.FechaDesde;
            tituloData.FechaHasta = parametros.FechaHasta;

            var grupoMesaEntrada = new List<string> { "EAS", "EASINS", "JEFONL", "JAP"
                , "JBR", "JCA", "JCD", "JCI", "JCO", "JCP", "JEN", "JFO", "JGA","" };

            if (grupoMesaEntrada.Contains(usuario.CodGrupo))
            {

                // Asignado
                var totalAsignado = queryMovimiento.Count(m => m.EstadoEntrada == 6);
                tituloData.TotalAsignado = totalAsignado;

                // Mesa entrada
                var totalMesaEntrada = queryMovimiento.Count(m => m.EstadoEntrada == 8);
                tituloData.TotalMesaEntrada = totalMesaEntrada;

                // Retirado/Aprobado, Retirado/Observado, Retirado/Nota negativa
                var totalRetirado = queryMovimiento.Count(m => m.EstadoEntrada == 13 || m.EstadoEntrada == 14 || m.EstadoEntrada == 15);
                tituloData.TotalRetirado = totalRetirado;

                // Recibido Mesa de Salida
                var totalRecibidoMesaSalida = queryMovimiento.Count(m => m.EstadoEntrada == 22);
                tituloData.TotalRecibidoMesaSalida = totalRecibidoMesaSalida;

                // Recepcion de Diseño
                var totalRecepcionDisenho = queryMovimiento.Count(m => m.EstadoEntrada == 31);
                tituloData.TotalRecepcionDisenho = totalRecepcionDisenho;

                // Enviado a Diseño
                var totalEnviadoDisenho = queryMovimiento.Count(m => m.EstadoEntrada == 42);
                tituloData.TotalEnviadoDisenho = totalEnviadoDisenho;

                // Enviado a Archivo
                var totalEnviadoArchivo = queryMovimiento.Count(m => m.EstadoEntrada == 43);
                tituloData.TotalEnviadoArchivo = totalEnviadoArchivo;

                // DesAsignado
                var totalDesAsignado = queryMovimiento.Count(m => m.EstadoEntrada == 45);
                tituloData.TotalReAsignado = totalDesAsignado;

                // Entrada Online
                var totalEntradaOnline = queryMovimiento.Count(m => m.EstadoEntrada == 46);
                tituloData.TotalEntradaOnline = totalEntradaOnline;

                tituloData.TipoUsuario = "MesaEntrada";

            }

            var grupoDisenho = new List<string> { "ASIGDI", "DISEN"};

            if (grupoDisenho.Contains(usuario.CodGrupo))
            {
                //Recepcionados por Jefe de Disenho
               var totalDisenho = queryMovimiento.Count(m=>m.EstadoEntrada==17);
                tituloData.TotalDisenho = totalDisenho;

                //Asignados por Jefe de Disenho
                var totalAsigDis = queryMovimiento.Count(m => m.EstadoEntrada == 18);
                tituloData.TotalAsigDis = totalAsigDis;

                //DeAsignados por Jefe de Disenho
                var totalDeAsigDis = queryMovimiento.Count(m => m.EstadoEntrada == 45);
                tituloData.TotalDeAsigDis = totalDeAsigDis;

                //Recepcionados por Disenhador
                var totalDisenhador = queryMovimiento.Count(m => m.EstadoEntrada == 33);
                tituloData.TotalDisenhador = totalDisenhador;

                //Finalizados por Disenhador
                var totalFinDise = queryMovimiento.Count(m => m.EstadoEntrada == 25);
                tituloData.TotalFinDise = totalFinDise;

                //Aprobados Jefe de Disenho
                var totalAproDis = queryMovimiento.Count(m => m.EstadoEntrada == 24);
                tituloData.TotalAproDis = totalAproDis;

                tituloData.TipoUsuario = "Disenho";
            }

            if (usuario.CodGrupo =="OPREG")
            {

                var observadoRegistrador = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Observado/Registrador");
                var aprobadoRegistrador = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Aprobado/Registrador");
                var notaNegativaRegistrador = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Nota Negativa/Registrador");


                var transaccion = _dbContext.RmTransacciones.Where(m => m.IdUsuario == parametros.Usuario).OrderByDescending(O => O.FechaAlta).AsQueryable();
                var tipo = _dbContext.RmTipoSolicituds.AsQueryable();
                var estado = _dbContext.RmEstadosEntrada.AsQueryable();

              


                var query = queryMovimiento.Where(e => e.CodUsuario == parametros.Usuario &&
              (e.EstadoEntrada == observadoRegistrador.CodigoEstado
              || e.EstadoEntrada == aprobadoRegistrador.CodigoEstado
              || e.EstadoEntrada == notaNegativaRegistrador.CodigoEstado)).AsQueryable().Join(mesaEntrada,
                  en => en.NroEntrada,
                  asi => asi.NumeroEntrada,
                  (en, asi) =>
                  new FinalizadoRegistrador
                  {
                      NumeroEntrada = asi.NumeroEntrada,
                      FechaEntrada = asi.FechaEntrada,
                      TipoSolicitud = asi.TipoSolicitud.ToString(),
                      Estado = en.EstadoEntrada.ToString(),
                      FechaFin = en.FechaOperacion
                  });

                var primero = query.ToList();

                var tran = query.AsQueryable().Join(transaccion,
                    en => en.FechaFin,
                    tra => tra.FechaAlta,
                    (en, tra) => new FinalizadoRegistrador
                    {
                        NumeroEntrada = en.NumeroEntrada,
                        FechaEntrada = en.FechaEntrada,
                        FechaFin = en.FechaFin,
                        TipoSolicitud = en.TipoSolicitud,
                        Estado = en.Estado
                    });

                var segundo = tran.ToList();

                var queryAsig = tran.AsQueryable().Join(_dbContext.RmAsignaciones,
                  en => en.NumeroEntrada,
                  tra => tra.NroEntrada,
                  (en, tra) => new FinalizadoRegistrador
                  {
                      NumeroEntrada = en.NumeroEntrada,
                      FechaEntrada = en.FechaEntrada,
                      FechaFin = en.FechaFin,
                      TipoSolicitud = en.TipoSolicitud,
                      FechaAsignacion = tra.FechaAsignada,
                      Estado = en.Estado
                  });


                var tercero = queryAsig.ToList();
                //var primero = query.ToList();
                var queryTipo = queryAsig.AsQueryable().Join(tipo,
                    asi => asi.TipoSolicitud,
                    ti => ti.TipoSolicitud.ToString(),
                    (asi, ti) => new FinalizadoRegistrador
                    {
                        NumeroEntrada = asi.NumeroEntrada,
                        FechaEntrada = asi.FechaEntrada,
                        FechaAsignacion = asi.FechaAsignacion,
                        TipoSolicitud = ti.DescripSolicitud,
                        FechaFin = asi.FechaFin,
                        Estado = asi.Estado
                    }
                    );
                var cuarto = queryTipo.ToList();

                var queryFinal = queryTipo.AsQueryable().Join(estado,
                   asi => asi.Estado,
                   ti => ti.CodigoEstado.ToString(),
                   (asi, ti) => new FinalizadoRegistrador
                   {
                       NumeroEntrada = asi.NumeroEntrada,
                       FechaEntrada = asi.FechaEntrada,
                       FechaAsignacion = asi.FechaAsignacion,
                       TipoSolicitud = asi.TipoSolicitud,
                       FechaFin = asi.FechaFin,
                       Estado = ti.DescripEstado
                   });

                var resultadosAgrupados = queryFinal
                 .GroupBy(x => new { x.FechaFin })
                 .Select(g => g.OrderByDescending(e => e.FechaFin).First())
                 .ToList();

                // Ordenar los resultados agrupados por Número de Entrada de menor a mayor
                var resultadosOrdenados = resultadosAgrupados
                    .OrderBy(o => o.NumeroEntrada)
                    .ToList();
                // Ordenar los resultados agrupados por Número de Entrada de menor a mayor
                // var resultadosOrdenados = resultadosAgrupados.OrderBy(o => o.NumeroEntrada).ToList();


                //Reportes tituloData = new();
                // tituloData.FinalizadoRegis = resultadosOrdenados;

                // Total Ingresado
                tituloData.TotalIngresado = queryMovimiento.Count(m => (m.EstadoEntrada == 2 || m.EstadoEntrada == 3 || m.EstadoEntrada == 4));
                tituloData.TipoUsuario = "Registrador";

                // Total Aprobado
                var totalAprobado = queryMovimiento.Count(m => m.EstadoEntrada == 4);
                tituloData.AprobRegistrador = totalAprobado;

                // Total Observado
                var totalObservado = queryMovimiento.Count(m => m.EstadoEntrada == 2);
                tituloData.ObsRegistrador = totalObservado;

                // Total Nota Negativa
                var totalNotaNegativa = queryMovimiento.Count(m => m.EstadoEntrada == 3);
                tituloData.NNRegistrador = totalNotaNegativa;

                #region TRABAJOS FINALIZADOS

                var queryReFinal = from movimiento in _dbContext.RmMovimientosDocs
                                 join mesa in mesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                                 //join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                                 join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                                 where movimiento.FechaOperacion <= parametros.FechaHasta &&
                                       movimiento.FechaOperacion >= parametros.FechaDesde
                                       && (movimiento.EstadoEntrada == 2 || movimiento.EstadoEntrada == 3 || movimiento.EstadoEntrada == 4)
                                       && movimiento.CodUsuario == parametros.Usuario

                                 select new
                                 {
                                     EstadoEntrada = movimiento.EstadoEntrada,
                                     CodOperacion = movimiento.CodOperacion,
                                     TipoSolicitud = mesa.TipoSolicitud,
                                     numeroEntrada = mesa.NumeroEntrada,
                                     CodigoOficina = mesa.CodigoOficina,
                                     DescripOficina = oficina.DescripOficina,
                                     ReIngreso = mesa.Reingreso,
                                 };

                var primerFinal = queryReFinal.ToList();

                var insFinal = primerFinal.Count(e => e.TipoSolicitud == 1 && e.ReIngreso == null);
                tituloData.InscripcionFinal = insFinal;
                var insFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 1);
                tituloData.InscripcionFinalRe = insFinalRe;

                var adjuFinal = primerFinal.Count(e => e.TipoSolicitud == 2 && e.ReIngreso == null);
                tituloData.AdjudicacionFinal = adjuFinal;
                var adjuFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 2);
                tituloData.AdjudicacionFinalRe = adjuFinalRe;

                var transFinal = primerFinal.Count(e => e.TipoSolicitud == 3 && e.ReIngreso == null);
                tituloData.TransferenciaFinal = transFinal;
                var transFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 3);
                tituloData.TransferenciaFinalRe = transFinalRe;

                var reinsriFinal = primerFinal.Count(e => e.TipoSolicitud == 4 && e.ReIngreso == null);
                tituloData.ReinscripcionFinal = reinsriFinal;
                var reinsriFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 4);
                tituloData.ReinscripcionFinalRe = reinsriFinalRe;

                var duFinal = primerFinal.Count(e => e.TipoSolicitud == 5 && e.ReIngreso == null);
                tituloData.DuplicadoFinal = duFinal;
                var duFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 5);
                tituloData.DuplicadoFinalRe = duFinalRe;

                var coFinal = primerFinal.Count(e => e.TipoSolicitud == 6 && e.ReIngreso == null);
                tituloData.CopiaFinal = coFinal;
                var coFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 6);
                tituloData.CopiaFinalRe = coFinalRe;

                var perFinal = primerFinal.Count(e => e.TipoSolicitud == 7 && e.ReIngreso == null);
                tituloData.PermutaFinal = perFinal;
                var perFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 7);
                tituloData.PermutaFinalRe = perFinalRe;

                var doFinal = primerFinal.Count(e => e.TipoSolicitud == 8 && e.ReIngreso == null);
                tituloData.DonacionFinal = doFinal;
                var doFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 8);
                tituloData.DonacionFinalRe = doFinalRe;

                var usuFinal = primerFinal.Count(e => e.TipoSolicitud == 9 && e.ReIngreso == null);
                tituloData.UsufructoFinal = usuFinal;
                var usuFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 9);
                tituloData.UsufructoFinalRe = usuFinalRe;

                var preFinal = primerFinal.Count(e => e.TipoSolicitud == 10 && e.ReIngreso == null);
                tituloData.PrendaFinal = preFinal;
                var preFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 10);
                tituloData.PrendaFinalRe = preFinalRe;

                var leFinal = primerFinal.Count(e => e.TipoSolicitud == 11 && e.ReIngreso == null);
                tituloData.LevantamientoFinal = leFinal;
                var leFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 11);
                tituloData.LevantamientoFinalRe = leFinalRe;

                var cerFinal = primerFinal.Count(e => e.ReIngreso == null && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22));
                tituloData.CertificadoFinal = cerFinal;
                var cerFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22));
                tituloData.CertificadoFinalRe = cerFinalRe;

                var infFinal = primerFinal.Count(e => e.TipoSolicitud == 13 && e.ReIngreso == null);
                tituloData.InformeFinal = infFinal;
                var infFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 13);
                tituloData.InformeFinalRe = infFinalRe;

                var ifFinal = primerFinal.Count(e => e.TipoSolicitud == 14 && e.ReIngreso == null);
                tituloData.InformeJudicialFinal = ifFinal;
                var ifFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 14);
                tituloData.InformeJudicialFinalRe = ifFinalRe;

                var liFinal = primerFinal.Count(e => e.TipoSolicitud == 15 && e.ReIngreso == null);
                tituloData.LitisFinal = liFinal;
                var liFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 15);
                tituloData.LitisFinalRe = liFinalRe;

                var aipFinal = primerFinal.Count(e => e.TipoSolicitud == 16 && e.ReIngreso == null);
                tituloData.AnotacionDeInscripcionPreventivaFinal = aipFinal;
                var aipFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 16);
                tituloData.AnotacionDeInscripcionPreventivaFinalRe = aipFinalRe;

                var emPFinal = primerFinal.Count(e => e.TipoSolicitud == 17 && e.ReIngreso == null);
                tituloData.EmbargoPreventivoFinal = emPFinal;
                var emPFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 17);
                tituloData.EmbargoPreventivoFinalRe = emPFinalRe;

                var epFinal = primerFinal.Count(e => e.TipoSolicitud == 18 && e.ReIngreso == null);
                tituloData.DacionPagoFinal = epFinal;
                var epFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 18);
                tituloData.DacionPagoFinalRe = epFinalRe;

                var emEFinal = primerFinal.Count(e => e.TipoSolicitud == 19 && e.ReIngreso == null);
                tituloData.EmbargoEjecutivoFinal = emEFinal;
                var emEFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 19);
                tituloData.EmbargoEjecutivoFinalRe = emEFinalRe;

                var canFinal = primerFinal.Count(e => e.TipoSolicitud == 23 && e.ReIngreso == null);
                tituloData.CancelacionFinal = canFinal;
                var canFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 23);
                tituloData.CancelacionFinalRe = canFinalRe;

                var CambDFinal = primerFinal.Count(e => e.TipoSolicitud == 25 && e.ReIngreso == null);
                tituloData.CambioDenominacionFinal = CambDFinal;
                var CambDFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 25);
                tituloData.CambioDenominacionFinalRe = CambDFinalRe;

                var recFinal = primerFinal.Count(e => e.TipoSolicitud == 26 && e.ReIngreso == null);
                tituloData.RectificacionFinal = recFinal;
                var recFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 26);
                tituloData.RectificacionFinalRe = recFinalRe;

                var cjFinal = primerFinal.Count(e => e.TipoSolicitud == 28 && e.ReIngreso == null);
                tituloData.CopiaJudicialFinal = cjFinal;
                var cjFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 28);
                tituloData.CopiaJudicialFinalRe = cjFinalRe;

                var fiFinal = primerFinal.Count(e => e.TipoSolicitud == 32 && e.ReIngreso == null);
                tituloData.FianzaFinal = fiFinal;
                var fiFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 32);
                tituloData.FianzaFinalRe = fiFinalRe;

                var picFinal = primerFinal.Count(e => e.TipoSolicitud == 34 && e.ReIngreso == null);
                tituloData.ProDeIn_y_ConFinal = picFinal;
                var picFinalRe = primerFinal.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 34);
                tituloData.ProDeIn_y_ConFinalRe = picFinalRe;

                var totalFinal = insFinal + adjuFinal + transFinal + reinsriFinal + duFinal + coFinal + perFinal + doFinal + usuFinal
                    + preFinal + leFinal + cerFinal + infFinal + ifFinal + liFinal + aipFinal + emPFinal + epFinal + emEFinal + canFinal + CambDFinal + recFinal
                    + cjFinal + fiFinal + picFinal;

                var totalFinalRe = insFinalRe + adjuFinalRe + transFinalRe + reinsriFinalRe + duFinalRe + coFinalRe + perFinalRe + doFinalRe + usuFinalRe
                    + preFinalRe + leFinalRe + cerFinalRe + infFinalRe + ifFinalRe + liFinalRe + aipFinalRe + emPFinalRe + epFinalRe + emEFinalRe + canFinalRe + CambDFinalRe + recFinalRe
                    + cjFinalRe + fiFinalRe + picFinalRe;

                var totalFinalEntrada = totalFinal + totalFinalRe;

                tituloData.TotalFinal = totalFinal;
                tituloData.TotalFinalRe = totalFinalRe;
                //tituloData.TotalFinalEntrada = totalFinalEntrada;

                #endregion


                return tituloData;
            }

            if (usuario.CodGrupo == "SUPGNL")
            {
                var estadoRecibido = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Aprobado/JefeRegistral/Firma");

                //Tipos Operaciones que emitan titulos
                var tipAprobadosFirma = new List<decimal> { 2, 5, 6, 15, 2, 19, 7, 8, 3, 25 };
                var tipoSolicitudFirma = _dbContext.RmTiposOperaciones
               .Where(to => tipAprobadosFirma.Contains(to.TipoOperacion))
               .AsQueryable();

                var transaccion = _dbContext.RmTransacciones.AsQueryable();              

                #region AprobadoFirma
                var queryF = queryMovimiento.Where(e => e.EstadoEntrada == estadoRecibido.CodigoEstado).AsQueryable().Join(mesaEntrada,
                 en => en.NroEntrada,
                 tipo => tipo.NumeroEntrada,
                 (en, tipo) =>
                 new ListadoJefes
                 {
                     NumeroEntrada = tipo.NumeroEntrada,
                     TipoSolicitud = tipo.TipoSolicitud.ToString(),
                     FechaEntrada = tipo.FechaEntrada,
                     NomTitular = tipo.CodigoOficina.ToString()
                 }).Distinct();

                var primer = queryF.ToList();

                var queryFirma = queryF.AsQueryable().Join(tipoSolicitudFirma,
                  en => en.TipoSolicitud,
                  tipo => tipo.TipoOperacion.ToString(),
                  (en, tipo) => new ListadoJefes
                  {
                      NumeroEntrada = en.NumeroEntrada,
                      TipoSolicitud = tipo.TipoOperacion.ToString(),
                      FechaEntrada = en.FechaEntrada,
                      FechaAlta = en.FechaAlta,
                      NomTitular = en.NomTitular,
                      NomOperador = en.NomOperador
                  });


                var adjudicacionFirma = queryFirma.Where(e=>e.TipoSolicitud=="2").ToList();
                tituloData.AdjudicacionFirma = adjudicacionFirma.Count();

                var inscripcionFirma = queryFirma.Where(e => e.TipoSolicitud == "6").ToList();
                tituloData.InscripcionFirma = inscripcionFirma.Count();

                var duplicadoFirma = queryFirma.Where(e => e.TipoSolicitud == "5").ToList();
                tituloData.DuplicadoFirma = duplicadoFirma.Count();

                var totalFirma = queryFirma.ToList();
                tituloData.TotalAprobadosFirma = totalFirma.Count;

                #endregion

                #region Para Mesa Salida 

                var estadoAprobado = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Aprobado/JefeRegistral/Firma");
                var estadoNotaNegativa = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Nota Negativa/JefeRegistral");
                var estadoObservado = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Observado/JefeRegistral");

                var solicitudSalida = _dbContext.RmTipoSolicituds.AsQueryable();


                var queryM = queryMovimiento.AsQueryable().Join(mesaEntrada,
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

                var tiposSolicitud = new List<string> { "6", "9", "10", "11", "12", "13", "14", "16", "17", "18", "19", "22", "23", "26", "27", "28", "33", "34" };
                var queryTransa = queryM.Where(e => e.Estado == estadoNotaNegativa.CodigoEstado
                || e.Estado == estadoObservado.CodigoEstado || 
                (e.Estado == estadoAprobado.CodigoEstado && 
                (tiposSolicitud.Contains(e.TipoSolicitud)))).AsQueryable().Join(transaccion,
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

                var querySalida = queryTransa.AsQueryable().Join(solicitudSalida,
                   en => en.TipoSolicitud,
                   tipo => tipo.TipoSolicitud.ToString(),
                   (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, 
                       TipoSolicitud = tipo.DescripSolicitud, 
                       FechaEntrada = en.FechaEntrada, 
                       NomTitular = en.NomTitular, 
                       NomOperador = en.NomOperador });

                var totalSalida = querySalida.ToList();
                tituloData.TotalMesaSalida = totalSalida.Count();


                #endregion

                #region PARA SUPERVISION
                var estadoSup = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Aprobado/JefeRegistral");
                var query = queryMovimiento
              .Where(e => e.EstadoEntrada == estadoSup.CodigoEstado)
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
                  });


                var queryTr = query.AsQueryable().Join(transaccion,
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
                   });

               
                var querySup = queryTr.AsQueryable().Join(tipoSolicitudFirma,
                   en => en.TipoSolicitud,
                   tipo => tipo.TipoOperacion.ToString(),
                   (en, tipo) => new ListadoJefes
                   {
                       NumeroEntrada = en.NumeroEntrada,
                       TipoSolicitud = tipo.DescripTipoOperacion,
                       FechaEntrada = en.FechaEntrada,
                       FechaAlta = en.FechaAlta,
                       NomTitular = en.NomTitular,
                       NomOperador = en.NomOperador
                   });

                var totalSup = querySup.Count();
                tituloData.TotalAprobadosSup = totalSup;

                #endregion

                tituloData.TipoUsuario = "JefaRegistral";
                return tituloData;
            }

            var grupoSup = new List<string> { "SUINS2", "SUINS3", "SUPDIR" };

            if (grupoSup.Contains(usuario.CodGrupo))
            {
                //Recepcionado por Jefe de Supervision
                var querySup = queryMovimiento.Where(e => e.EstadoEntrada == 35).AsQueryable().Join(mesaEntrada,
                 en => en.NroEntrada,
                 tipo => tipo.NumeroEntrada,
                 (en, tipo) =>
                 new ListadoJefes
                 {
                     NumeroEntrada = tipo.NumeroEntrada,
                     TipoSolicitud = tipo.TipoSolicitud.ToString(),
                     FechaEntrada = tipo.FechaEntrada,
                     NomTitular = tipo.CodigoOficina.ToString()
                 });

                var totalRecJefSup = querySup.Count();
                tituloData.TotalRecJefSup = totalRecJefSup;

                var inscripcionJS = querySup.Count(m=>m.TipoSolicitud=="1");



                //Asignados por Jefe de Supervision
                var queryAsigSup = queryMovimiento.Where(e => e.EstadoEntrada == 23).AsQueryable().Join(mesaEntrada,
                en => en.NroEntrada,
                tipo => tipo.NumeroEntrada,
                (en, tipo) =>
                new ListadoJefes
                {
                    NumeroEntrada = tipo.NumeroEntrada,
                    TipoSolicitud = tipo.TipoSolicitud.ToString(),
                    FechaEntrada = tipo.FechaEntrada,
                    NomTitular = tipo.CodigoOficina.ToString()
                });

                var totalAsigJefSup = queryAsigSup.Count();
                tituloData.TotalAsigJefSup = totalAsigJefSup;

                var inscripcionAsig = queryAsigSup.Count(m => m.TipoSolicitud == "1");


                //ReAsignado por Jefe de Supervision
                var queryDesAsigSup = queryMovimiento.Where(e => e.EstadoEntrada == 45).AsQueryable().Join(mesaEntrada,
               en => en.NroEntrada,
               tipo => tipo.NumeroEntrada,
               (en, tipo) =>
               new ListadoJefes
               {
                   NumeroEntrada = tipo.NumeroEntrada,
                   TipoSolicitud = tipo.TipoSolicitud.ToString(),
                   FechaEntrada = tipo.FechaEntrada,
                   NomTitular = tipo.CodigoOficina.ToString()
               });

                var totalDesAsigJefSup = queryDesAsigSup.Count();
                tituloData.TotalDesAsigJefSup = totalDesAsigJefSup;

                var inscripcionDesAsig = queryDesAsigSup.Count(m => m.TipoSolicitud == "1");

                //Recepcionado por Supervisor
                var queryRecSup = queryMovimiento.Where(e => e.EstadoEntrada == 36).AsQueryable().Join(mesaEntrada,
                  en => en.NroEntrada,
                  tipo => tipo.NumeroEntrada,
                  (en, tipo) =>
                  new ListadoJefes
                  {
                      NumeroEntrada = tipo.NumeroEntrada,
                      TipoSolicitud = tipo.TipoSolicitud.ToString(),
                      FechaEntrada = tipo.FechaEntrada,
                      NomTitular = tipo.CodigoOficina.ToString()
                  });

                var totalRecSup = queryRecSup.Count();
                tituloData.TotalRecSup = totalRecSup;

                var inscripcionRecSup = queryRecSup.Count(m => m.TipoSolicitud == "1");

                //Operacion de Supervisor
                        //aprobados por supervisor
                var queryAproSup = queryMovimiento.Where(e => e.EstadoEntrada == 20).AsQueryable().Join(mesaEntrada,
                 en => en.NroEntrada,
                 tipo => tipo.NumeroEntrada,
                 (en, tipo) =>
                 new ListadoJefes
                 {
                     NumeroEntrada = tipo.NumeroEntrada,
                     TipoSolicitud = tipo.TipoSolicitud.ToString(),
                     FechaEntrada = tipo.FechaEntrada,
                     NomTitular = tipo.CodigoOficina.ToString()
                 });

                var totalAproSup = queryAproSup.Count();
                tituloData.TotalAproSup = totalAproSup;

                var inscripcionAproSup = queryAproSup.Count(m => m.TipoSolicitud == "1");

                            //rechazados por supervisor
                var queryRechaSup = queryMovimiento.Where(e => e.EstadoEntrada == 9).AsQueryable().Join(mesaEntrada,
                 en => en.NroEntrada,
                 tipo => tipo.NumeroEntrada,
                 (en, tipo) =>
                 new ListadoJefes
                 {
                     NumeroEntrada = tipo.NumeroEntrada,
                     TipoSolicitud = tipo.TipoSolicitud.ToString(),
                     FechaEntrada = tipo.FechaEntrada,
                     NomTitular = tipo.CodigoOficina.ToString()
                 });

                var totalRechaSup = queryRechaSup.Count();
                tituloData.TotalRechaSup = totalRechaSup;

                var inscripcionRechaSup = queryRechaSup.Count(m => m.TipoSolicitud == "1");



                //Operacion Jefe de Supervision
                            //aprobados por jefe de supervision
                var queryAproJefSup = queryMovimiento.Where(e => e.EstadoEntrada == 20).AsQueryable().Join(mesaEntrada,
                en => en.NroEntrada,
                tipo => tipo.NumeroEntrada,
                (en, tipo) =>
                new ListadoJefes
                {
                    NumeroEntrada = tipo.NumeroEntrada,
                    TipoSolicitud = tipo.TipoSolicitud.ToString(),
                    FechaEntrada = tipo.FechaEntrada,
                    NomTitular = tipo.CodigoOficina.ToString()
                });

                var totalAproJefSup = queryAproJefSup.Count();
                tituloData.TotalAproJefSup = totalAproJefSup;

                var inscripcionAproJefSup = queryAproJefSup.Count(m => m.TipoSolicitud == "1");

                            //rechazados por jefe de supervision
                var queryRechaJefSup = queryMovimiento.Where(e => e.EstadoEntrada == 9).AsQueryable().Join(mesaEntrada,
                 en => en.NroEntrada,
                 tipo => tipo.NumeroEntrada,
                 (en, tipo) =>
                 new ListadoJefes
                 {
                     NumeroEntrada = tipo.NumeroEntrada,
                     TipoSolicitud = tipo.TipoSolicitud.ToString(),
                     FechaEntrada = tipo.FechaEntrada,
                     NomTitular = tipo.CodigoOficina.ToString()
                 });

                var totalRechaJefSup = queryRechaJefSup.Count();
                tituloData.TotalRechaJefSup = totalRechaJefSup;

                var inscripcionRechaJefSup = queryRechaJefSup.Count(m => m.TipoSolicitud == "1");


                tituloData.TipoUsuario = "Supervision";
            }

            var grupoRegional = new List<string> { "EASDR", "USUSP", "USUÑE", "USUEN", "USUPA", "USUCP","USUCO", "USUAP"
                , "USUMN", "USUPH", "USUCD", "USUBR", "USUPJC", "USUGA", "USUCA", "USUHO", "USUCI", "USUFO", "ADMIN", "JEFDIV"};

            if (grupoRegional.Contains(usuario.CodGrupo))
            {
                // Recepción de Entrada Div Regional
                var totalEntradaDivRegional = queryMovimiento.Count(m => m.EstadoEntrada == 26);
                tituloData.TotalRecepEntrada = totalEntradaDivRegional;

                // Recepcion de Diseño Div Regional
                var totalRecepcionDisenho = queryMovimiento.Count(m => m.EstadoEntrada == 44);
                tituloData.TotalRecepcionDisenho = totalRecepcionDisenho;

                // Remisión de Entradas a Diseño
                var totalEnviadoDisenho = queryMovimiento.Count(m => m.EstadoEntrada == 42);
                tituloData.TotalEnviadoDisenho = totalEnviadoDisenho;

                // Recepción de Documentos Firmados
                var totalRecepcionDocFirmados = queryMovimiento.Count(m => m.EstadoEntrada == 34);
                tituloData.TotalDocFirmadosReg = totalRecepcionDocFirmados;

                // Enviado a Seccion Regional
                var totalEnviadoSeccionRegional = queryMovimiento.Count(m => m.EstadoEntrada == 28);
                tituloData.TotalEnvSecRegional = totalEnviadoSeccionRegional;

                // Recibido Mesa Salida Seccion Regional
                var totalRecibidoMesaSalida = queryMovimiento.Count(m => m.EstadoEntrada == 38);
                tituloData.TotalRecibidoMesaSalida = totalRecibidoMesaSalida;

                // Enviado Triplicado
                var totalEnviadoTriplicado = queryMovimiento.Count(m => m.EstadoEntrada == 29);
                tituloData.TotalTriplicado = totalEnviadoTriplicado;

                tituloData.TipoUsuario = "Regional";
            }


            return tituloData;
        }
    }
}
