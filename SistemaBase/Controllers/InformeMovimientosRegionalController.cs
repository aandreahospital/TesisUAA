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
using System.Diagnostics.Metrics;
using static iText.IO.Image.Jpeg2000ImageData;


namespace SistemaBase.Controllers
{
    public class InformeMovimientosRegionalController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public InformeMovimientosRegionalController(DbvinDbContext context)
        {
            _dbContext = context;
        }


        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        //[TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMIMPF", "Index", "InformeMovimientosPorFecha" })]

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

            Reportes tituloData = new()
            {

            };

            var queryMesaEntrada = _dbContext.RmMesaEntrada.AsQueryable();

            //if (parametros.CodigoOficina == null)
            //{
            //    queryMesaEntrada = _dbContext.RmMesaEntrada.Where(m => m.CodigoOficina != 1).AsQueryable();
            //}
            //else
            //{
            //    queryMesaEntrada = _dbContext.RmMesaEntrada.Where(m => m.CodigoOficina == parametros.CodigoOficina).AsQueryable();
            //}



            var query = from mesa in queryMesaEntrada
                        join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                        where mesa.FechaEntrada <= parametros.FechaHasta &&
                              mesa.FechaEntrada >= parametros.FechaDesde

                        select new
                        {
                            //EstadoEntrada = md.EstadoEntrada,
                            TipoSolicitud = mesa.TipoSolicitud,
                            numeroEntrada = mesa.NumeroEntrada,
                            DescripOficina = oficina.DescripOficina,
                            ReIngreso = mesa.Reingreso,
                            UsuarioEntrada = mesa.UsuarioEntrada
                        };


            var primer = query.ToList();


            #region TRABAJOS INGRESADOS
            // Contar la cantidad de INSCRIPCION
            var ins = primer.Count(e => e.TipoSolicitud == 1);

            // Contar la cantidad de INSCRIPCiON REINGRESO
            var insre = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 1);

            //Contar la cantidad de INSCRIPCION ONLINE 
            var oi = primer.Count(e => e.TipoSolicitud == 1 && e.UsuarioEntrada == "USERONLINE");


            // Contar la cantidad de ADJUDICACION
            var adju = primer.Count(e => e.TipoSolicitud == 2 && e.ReIngreso == null);

            // Contar la cantidad de ADJUDICACION REINGRESO
            var adjuRe = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 2);

            // Contar la cantidad de TRANSFERENCIA
            var trans = primer.Count(e => e.TipoSolicitud == 3 && e.ReIngreso == null);

            // Contar la cantidad de TRANSFERENCIA REINGRESO
            var transre = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 3);

            // Contar la cantidad de REINSCRIPCION 
            var reinsri = primer.Count(e => e.TipoSolicitud == 4 && e.ReIngreso == null);

            // Contar la cantidad de REINSCRIPCION REINGRESO
            var reinsriRe = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 4);

            // Contar la cantidad de DUPLICADO
            var Du = primer.Count(e => e.TipoSolicitud == 5 && e.ReIngreso == null);
            //Contar la cantidad de DUPLICADO REINGRESO
            var DuRe = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 5);
            //Contar la cantidad de DUPLICADO ONLINE
            var odu = primer.Count(e => e.TipoSolicitud == 5 && e.UsuarioEntrada == "USERONLINE");

            // Contar la cantidad de COPIA
            var Co = primer.Count(e => e.TipoSolicitud == 6 && e.ReIngreso == null);
            //Contar la cantidad de COPIA REINGRESO
            var CoRe = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 6);

            // Contar la cantidad de PERMUTA
            var Per = primer.Count(e => e.TipoSolicitud == 7 && e.ReIngreso == null);
            // Contar la cantidad de PERMUTA REINGRESO
            var PerRe = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 7);

            // Contar la cantidad de DONACION
            var Do = primer.Count(e => e.TipoSolicitud == 8 && e.ReIngreso == null);
            //Contar la cantidad de DONACION REINGRESO
            var DoRe = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 8);

            // Contar la cantidad de USUFRUCTO
            var Usuf = primer.Count(e => e.TipoSolicitud == 9 && e.ReIngreso == null);
            //Contar la cantidad de USUFRUCTO REINGRESO
            var UsufR = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 9);

            // Contar la cantidad de PRENDA
            var Pre = primer.Count(e => e.TipoSolicitud == 10 && e.ReIngreso == null);
            //Contar la cantidad de PRENDA REINGRESO
            var PreR = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 10);

            // Contar la cantidad de LEVANTAMIENTO
            var Lev = primer.Count(e => e.TipoSolicitud == 11 && e.ReIngreso == null);
            //Contar la cantidad de LEVANTAMIENTO REINGRESO
            var LevR = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 11);

            // Contar la cantidad de CERTIFICADO
            var Cer = primer.Count(e => e.TipoSolicitud == 12 && e.ReIngreso == null);
            //Contar la cantidad de CERTIFICADO REINGRESO
            var CerE = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 12);
            //Contar l cantidad de  CERTIFICADO ONLINE  
            var ocer = primer.Count(e => e.TipoSolicitud == 22 && e.UsuarioEntrada == "USERONLINE");

            // Contar la cantidad de CERTIFICADO DE CONDOMINIO DE DOMINIO
            var CerCD = primer.Count(e => e.TipoSolicitud == 22 && e.ReIngreso == null);
            //Contar la cantidad  de CANCELACION REINGRESO
            var CerCDR = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 22);

            CerCD = CerCD + Cer;

            CerCDR = CerE + CerCDR;

            // Contar la cantidad de INFORME
            var In = primer.Count(e => e.TipoSolicitud == 13 && e.ReIngreso == null);
            //Contar la cantidad de IFORME REINGRESO
            var InE = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 13);
            //Contar la cantidad de INFORME
            var oinf = primer.Count(e => e.TipoSolicitud == 13 && e.UsuarioEntrada == "USERONLINE");


            // Contar la cantidad de INFORME JUDICIAL
            var InJ = primer.Count(e => e.TipoSolicitud == 14 && e.ReIngreso == null);
            //Contar la cantidad de INFORME JUDICIAL REINGRESO
            var InJE = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 14);

            // Contar la cantidad de LITIS
            var Li = primer.Count(e => e.TipoSolicitud == 15 && e.ReIngreso == null);
            //Contar la cantidad de LITIS REINGRESO
            var LiRe = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 15);

            // Contar la cantidad de ANOTACION DE INSCRIPCION PREVENTIVA
            var AIP = primer.Count(e => e.TipoSolicitud == 16 && e.ReIngreso == null);
            //Contar la cantidad de ANOTACION DE INSCRIPCION PREVENTIVA REINGRESO
            var AIPR = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 16);

            // Contar la cantidad de EMBARGO PREVENTIVA
            var EP = primer.Count(e => e.TipoSolicitud == 17 && e.ReIngreso == null);
            //Contar la cantidad de ANOTACION DE INSCRIPCION PREVENTIVA REINGRESO
            var EPR = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 17);

            // Contar la cantidad de DACION PAGO
            var Dp = primer.Count(e => e.TipoSolicitud == 18 && e.ReIngreso == null);
            //Contar la cantidad de DACION PAGO REINGRESO
            var DpR = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 18);


            // Contar la cantidad de EMBARGO EJECUTIVO
            var Emb = primer.Count(e => e.TipoSolicitud == 19 && e.ReIngreso == null);
            // Contar la cantidad de EMBARGO EJECUTIVO REINGRESO
            var EmbRe = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 19);



            // Contar la cantidad de CANCELACION
            var Can = primer.Count(e => e.TipoSolicitud == 23 && e.ReIngreso == null);
            //Contar la cantidad  de CANCELACION REINGRESO
            var CanR = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 23);

            // Contar la cantidad de CAMBIO DENOMINACION
            var CamD = primer.Count(e => e.TipoSolicitud == 25 && e.ReIngreso == null);
            //Contar la cantidad  de CAMBIO DENOMINACION REINGRESO
            var CamDR = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 25);

            // Contar la cantidad de RECTIFICACION
            var Rec = primer.Count(e => e.TipoSolicitud == 26 && e.ReIngreso == null);
            // Contar la cantidad de CAMBIO DENOMINACION REINGRESO
            var RecR = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 26);

            // Contar la cantidad de COPIA JUDICIAL
            var Cj = primer.Count(e => e.TipoSolicitud == 28 && e.ReIngreso == null);
            //Contar la cantidad de COPIA JUDICIAL REINGRESO
            var CjR = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 28);


            // Contar la cantidad de FIANZA
            var Fia = primer.Count(e => e.TipoSolicitud == 32 && e.ReIngreso == null);
            // Contar la cantidad de FIANZA REINGRESO
            var FiaR = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 32);

            // Contar la cantidad de PROHIBICION DE INNOVAR Y CONTRATAR
            var Pro = primer.Count(e => e.TipoSolicitud == 34 && e.ReIngreso == null);
            // Contar la cantidad de PROHIBICION DE INNOVAR Y CONTRATAR REINGRESO
            var ProR = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 34);


            //PARA MOSTRAR EL TOTAL EN INGRESADOS DE LA VISTA
            var total = ins + insre + adju + adjuRe + trans + transre + reinsri + reinsriRe + Du + DuRe
                       + Co + CoRe + Per + PerRe + Do + DoRe + Usuf + UsufR + Pre + PreR
                       + Lev + LevR + Cer + CerE + In + InE + InJ + InJE + Li + LiRe
                       + AIP + AIPR + EP + EPR + Dp + DpR + Emb + EmbRe + CerCD + CerCDR + +Can + CanR
                       + CamD + CamDR + Rec + RecR + Cj + CjR + Fia + FiaR + Pro + ProR;

            //PARA MOSTRAR EL TOTAL EN REINGRESO DE LA VISTA
            var totalRe = insre + adjuRe + transre + reinsriRe + DuRe
                + CoRe + PerRe + DoRe + UsufR + PreR + LevR + CerE + InE + InJE + LiRe
                + AIPR + EPR + DpR + EmbRe + CerCDR + CanR + CamDR + RecR + CjR + FiaR + ProR;

            //PARA MOSTRAR EL TOTAL DE INSCRIPCIONES SIN REINGRESO
            var inscripTotal = ins + adju + trans + reinsri + Du
                + Co + Per + Do + Usuf + Pre + Lev + Cer + In + InJ + Li
                + AIP + EP + Dp + Emb + CerCD + Can + CamD + Rec + Cj + Fia + Pro;

            //PARA MOSTRAR EL TOTAL DE INSCRIPCIONES ONLINE
            var totalonline = oi + odu + ocer + oinf;

            #endregion

            #region MESA DE ENTRADA REGIONAL

            var queryEnt = from movimiento in _dbContext.RmMovimientosDocs
                           join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                           //join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                           join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                           where movimiento.FechaOperacion <= parametros.FechaHasta &&
                                 movimiento.FechaOperacion >= parametros.FechaDesde
                                 && movimiento.EstadoEntrada == 8 &&
                                 movimiento.CodUsuario == parametros.Usuario

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

            var primerEnt = queryEnt.ToList();

            var insEnt = primerEnt.Count(e => e.TipoSolicitud == 1 && e.ReIngreso == null);
            tituloData.InscripcionEnt = insEnt;
            var insEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 1);
            tituloData.InscripcionEntRe = insEntRe;

            var adjuEnt = primerEnt.Count(e => e.TipoSolicitud == 2 && e.ReIngreso == null);
            tituloData.AdjudicacionEnt = adjuEnt;
            var adjuEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 2);
            tituloData.AdjudicacionEntRe = adjuEntRe;

            var transEnt = primerEnt.Count(e => e.TipoSolicitud == 3 && e.ReIngreso == null);
            tituloData.TransferenciaEnt = transEnt;
            var transEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 3);
            tituloData.TransferenciaEntRe = transEntRe;

            var reinsriEnt = primerEnt.Count(e => e.TipoSolicitud == 4 && e.ReIngreso == null);
            tituloData.ReinscripcionEnt = reinsriEnt;
            var reinsriEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 4);
            tituloData.ReinscripcionEntRe = reinsriEntRe;

            var duEnt = primerEnt.Count(e => e.TipoSolicitud == 5 && e.ReIngreso == null);
            tituloData.DuplicadoEnt = duEnt;
            var duEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 5);
            tituloData.DuplicadoEntRe = duEntRe;

            var coEnt = primerEnt.Count(e => e.TipoSolicitud == 6 && e.ReIngreso == null);
            tituloData.CopiaEnt = coEnt;
            var coEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 6);
            tituloData.CopiaEntRe = coEntRe;

            var perEnt = primerEnt.Count(e => e.TipoSolicitud == 7 && e.ReIngreso == null);
            tituloData.PermutaEnt = perEnt;
            var perEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 7);
            tituloData.PermutaEntRe = perEntRe;

            var doEnt = primerEnt.Count(e => e.TipoSolicitud == 8 && e.ReIngreso == null);
            tituloData.DonacionEnt = doEnt;
            var doEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 8);
            tituloData.DonacionEntRe = doEntRe;

            var usuEnt = primerEnt.Count(e => e.TipoSolicitud == 9 && e.ReIngreso == null);
            tituloData.UsufructoEnt = usuEnt;
            var usuEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 9);
            tituloData.UsufructoEntRe = usuEntRe;

            var preEnt = primerEnt.Count(e => e.TipoSolicitud == 10 && e.ReIngreso == null);
            tituloData.PrendaEnt = preEnt;
            var preEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 10);
            tituloData.PrendaEntRe = preEntRe;

            var leEnt = primerEnt.Count(e => e.TipoSolicitud == 11 && e.ReIngreso == null);
            tituloData.LevantamientoEnt = leEnt;
            var leEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 11);
            tituloData.LevantamientoEntRe = leEntRe;

            var cerEnt = primerEnt.Count(e =>e.ReIngreso == null && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22));
            tituloData.CertificadoEnt = cerEnt;
            var cerEntRe = primerEnt.Count(e => e.ReIngreso == "S" && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22));
            tituloData.CertificadoEntRe = cerEntRe;

            var infEnt = primerEnt.Count(e => e.TipoSolicitud == 13 && e.ReIngreso == null);
            tituloData.InformeEnt = infEnt;
            var infEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 13);
            tituloData.InformeEntRe = infEntRe;

            var ifEnt = primerEnt.Count(e => e.TipoSolicitud == 14 && e.ReIngreso == null);
            tituloData.InformeJudicialEnt = ifEnt;
            var ifEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 14);
            tituloData.InformeJudicialEntRe = ifEntRe;

            var liEnt = primerEnt.Count(e => e.TipoSolicitud == 15 && e.ReIngreso == null);
            tituloData.LitisEnt = liEnt;
            var liEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 15);
            tituloData.LitisEntRe = liEntRe;

            var aipEnt = primerEnt.Count(e => e.TipoSolicitud == 16 && e.ReIngreso == null);
            tituloData.AnotacionDeInscripcionPreventivaEnt = aipEnt;
            var aipEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 16);
            tituloData.AnotacionDeInscripcionPreventivaEntRe = aipEntRe;

            var emPEnt = primerEnt.Count(e => e.TipoSolicitud == 17 && e.ReIngreso == null);
            tituloData.EmbargoPreventivoEnt = emPEnt;
            var emPEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 17);
            tituloData.EmbargoPreventivoEntRe = emPEntRe;

            var epEnt = primerEnt.Count(e => e.TipoSolicitud == 18 && e.ReIngreso == null);
            tituloData.DacionPagoEnt = epEnt;
            var epEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 18);
            tituloData.DacionPagoEntRe = epEntRe;

            var emEEnt = primerEnt.Count(e => e.TipoSolicitud == 19 && e.ReIngreso == null);
            tituloData.EmbargoEjecutivoEnt = emEEnt;
            var emEEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 19);
            tituloData.EmbargoEjecutivoEntRe = emEEntRe;

            var canEnt = primerEnt.Count(e => e.TipoSolicitud == 23 && e.ReIngreso == null);
            tituloData.CancelacionEnt = canEnt;
            var canEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 23);
            tituloData.CancelacionEntRe = canEntRe;

            var CambDEnt = primerEnt.Count(e => e.TipoSolicitud == 25 && e.ReIngreso == null);
            tituloData.CambioDenominacionEnt = CambDEnt;
            var CambDEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 25);
            tituloData.CambioDenominacionEntRe = CambDEntRe;

            var recEnt = primerEnt.Count(e => e.TipoSolicitud == 26 && e.ReIngreso == null);
            tituloData.RectificacionEnt = recEnt;
            var recEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 26);
            tituloData.RectificacionEntRe = recEntRe;

            var cjEnt = primerEnt.Count(e => e.TipoSolicitud == 28 && e.ReIngreso == null);
            tituloData.CopiaJudicialEnt = cjEnt;
            var cjEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 28);
            tituloData.CopiaJudicialEntRe = cjEntRe;

            var fiEnt = primerEnt.Count(e => e.TipoSolicitud == 32 && e.ReIngreso == null);
            tituloData.FianzaEnt = fiEnt;
            var fiEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 32);
            tituloData.FianzaEntRe = fiEntRe;

            var picEnt = primerEnt.Count(e => e.TipoSolicitud == 34 && e.ReIngreso == null);
            tituloData.ProDeIn_y_ConEnt = picEnt;
            var picEntRe = primerEnt.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 34);
            tituloData.ProDeIn_y_ConEntRe = picEntRe;


            var totalEnt = insEnt + adjuEnt + transEnt + reinsriEnt + duEnt + coEnt + perEnt + doEnt + usuEnt
                + preEnt + leEnt + cerEnt + infEnt + ifEnt + liEnt + aipEnt + emPEnt + epEnt + emEEnt + canEnt + CambDEnt + recEnt
                + cjEnt + fiEnt + picEnt;

            var totalEntRe = insEntRe + adjuEntRe + transEntRe + reinsriEntRe + duEntRe + coEntRe + perEntRe + doEntRe + usuEntRe
                + preEntRe + leEntRe + cerEntRe + infEntRe + ifEntRe + liEntRe + aipEntRe + emPEntRe + epEntRe + emEEntRe + canEntRe + CambDEntRe + recEntRe
                + cjEntRe + fiEntRe + picEntRe;

            var totalMesaEntrada = totalEnt + totalEntRe;

            tituloData.TotalEnt = totalEnt;
            tituloData.TotalEntRe = totalEntRe;
            tituloData.TotalMesaEntrada = totalMesaEntrada;

            #endregion

            #region TRABAJOS ENVIADOS A REGIONAL

            var queryEnv = from movimiento in _dbContext.RmMovimientosDocs
                           join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                           //join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                           join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                           where movimiento.FechaOperacion <= parametros.FechaHasta &&
                                 movimiento.FechaOperacion >= parametros.FechaDesde
                                 && movimiento.EstadoEntrada == 27 &&
                                 movimiento.CodUsuario == parametros.Usuario

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

            var primerEnv = queryEnv.ToList();

            var insEnv = primerEnv.Count(e => e.TipoSolicitud == 1 && e.ReIngreso == null);
            tituloData.InscripcionEnv = insEnv;
            var insEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 1);
            tituloData.InscripcionEnvRe = insEnvRe;

            var adjuEnv = primerEnv.Count(e => e.TipoSolicitud == 2 && e.ReIngreso == null);
            tituloData.AdjudicacionEnv = adjuEnv;
            var adjuEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 2);
            tituloData.AdjudicacionEnvRe = adjuEnvRe;

            var transEnv = primerEnv.Count(e => e.TipoSolicitud == 3 && e.ReIngreso == null);
            tituloData.TransferenciaEnv = transEnv;
            var transEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 3);
            tituloData.TransferenciaEnvRe = transEnvRe;

            var reinsriEnv = primerEnv.Count(e => e.TipoSolicitud == 4 && e.ReIngreso == null);
            tituloData.ReinscripcionEnv = reinsriEnv;
            var reinsriEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 4);
            tituloData.ReinscripcionEnvRe = reinsriEnvRe;

            var duEnv = primerEnv.Count(e => e.TipoSolicitud == 5 && e.ReIngreso == null);
            tituloData.DuplicadoEnv = duEnv;
            var duEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 5);
            tituloData.DuplicadoEnvRe = duEnvRe;

            var coEnv = primerEnv.Count(e => e.TipoSolicitud == 6 && e.ReIngreso == null);
            tituloData.CopiaEnv = coEnv;
            var coEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 6);
            tituloData.CopiaEnvRe = coEnvRe;

            var perEnv = primerEnv.Count(e => e.TipoSolicitud == 7 && e.ReIngreso == null);
            tituloData.PermutaEnv = perEnv;
            var perEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 7);
            tituloData.PermutaEnvRe = perEnvRe;

            var doEnv = primerEnv.Count(e => e.TipoSolicitud == 8 && e.ReIngreso == null);
            tituloData.DonacionEnv = doEnv;
            var doEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 8);
            tituloData.DonacionEnvRe = doEnvRe;

            var usuEnv = primerEnv.Count(e => e.TipoSolicitud == 9 && e.ReIngreso == null);
            tituloData.UsufructoEnv = usuEnv;
            var usuEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 9);
            tituloData.UsufructoEnvRe = usuEnvRe;

            var preEnv = primerEnv.Count(e => e.TipoSolicitud == 10 && e.ReIngreso == null);
            tituloData.PrendaEnv = preEnv;
            var preEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 10);
            tituloData.PrendaEnvRe = preEnvRe;

            var leEnv = primerEnv.Count(e => e.TipoSolicitud == 11 && e.ReIngreso == null);
            tituloData.LevantamientoEnv = leEnv;
            var leEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 11);
            tituloData.LevantamientoEnvRe = leEnvRe;

            var cerEnv = primerEnv.Count(e =>e.ReIngreso == null && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22));
            tituloData.CertificadoEnv = cerEnv;
            var cerEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22));
            tituloData.CertificadoEnvRe = cerEnvRe;

            var infEnv = primerEnv.Count(e => e.TipoSolicitud == 13 && e.ReIngreso == null);
            tituloData.InformeEnv = infEnv;
            var infEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 13);
            tituloData.InformeEnvRe = infEnvRe;

            var ifEnv = primerEnv.Count(e => e.TipoSolicitud == 14 && e.ReIngreso == null);
            tituloData.InformeJudicialEnv = ifEnv;
            var ifEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 14);
            tituloData.InformeJudicialEnvRe = ifEnvRe;

            var liEnv = primerEnv.Count(e => e.TipoSolicitud == 15 && e.ReIngreso == null);
            tituloData.LitisEnv = liEnv;
            var liEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 15);
            tituloData.LitisEnvRe = liEnvRe;

            var aipEnv = primerEnv.Count(e => e.TipoSolicitud == 16 && e.ReIngreso == null);
            tituloData.AnotacionDeInscripcionPreventivaEnv = aipEnv;
            var aipEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 16);
            tituloData.AnotacionDeInscripcionPreventivaEnvRe = aipEnvRe;

            var emPEnv = primerEnv.Count(e => e.TipoSolicitud == 17 && e.ReIngreso == null);
            tituloData.EmbargoPreventivoEnv = emPEnv;
            var emPEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 17);
            tituloData.EmbargoPreventivoEnvRe = emPEnvRe;

            var epEnv = primerEnv.Count(e => e.TipoSolicitud == 18 && e.ReIngreso == null);
            tituloData.DacionPagoEnv = epEnv;
            var epEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 18);
            tituloData.DacionPagoEnvRe = epEnvRe;

            var emEEnv = primerEnv.Count(e => e.TipoSolicitud == 19 && e.ReIngreso == null);
            tituloData.EmbargoEjecutivoEnv = emEEnv;
            var emEEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 19);
            tituloData.EmbargoEjecutivoEnvRe = emEEnvRe;

            var canEnv = primerEnv.Count(e => e.TipoSolicitud == 23 && e.ReIngreso == null);
            tituloData.CancelacionEnv = canEnv;
            var canEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 23);
            tituloData.CancelacionEnvRe = canEnvRe;

            var CambDEnv = primerEnv.Count(e => e.TipoSolicitud == 25 && e.ReIngreso == null);
            tituloData.CambioDenominacionEnv = CambDEnv;
            var CambDEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 25);
            tituloData.CambioDenominacionEnvRe = CambDEnvRe;

            var recEnv = primerEnv.Count(e => e.TipoSolicitud == 26 && e.ReIngreso == null);
            tituloData.RectificacionEnv = recEnv;
            var recEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 26);
            tituloData.RectificacionEnvRe = recEnvRe;

            var cjEnv = primerEnv.Count(e => e.TipoSolicitud == 28 && e.ReIngreso == null);
            tituloData.CopiaJudicialEnv = cjEnv;
            var cjEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 28);
            tituloData.CopiaJudicialEnvRe = cjEnvRe;

            var fiEnv = primerEnv.Count(e => e.TipoSolicitud == 32 && e.ReIngreso == null);
            tituloData.FianzaEnv = fiEnv;
            var fiEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 32);
            tituloData.FianzaEnvRe = fiEnvRe;

            var picEnv = primerEnv.Count(e => e.TipoSolicitud == 34 && e.ReIngreso == null);
            tituloData.ProDeIn_y_ConEnv = picEnv;
            var picEnvRe = primerEnv.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 34);
            tituloData.ProDeIn_y_ConEnvRe = picEnvRe;

            var totalEnv = insEnv + adjuEnv + transEnv + reinsriEnv + duEnv + coEnv + perEnv + doEnv + usuEnv
                + preEnv + leEnv + cerEnv + infEnv + ifEnv + liEnv + aipEnv + emPEnv + epEnv + emEEnv + canEnv + CambDEnv + recEnv
                + cjEnv + fiEnv + picEnv;

            var totalEnvRe = insEnvRe + adjuEnvRe + transEnvRe + reinsriEnvRe + duEnvRe + coEnvRe + perEnvRe + doEnvRe + usuEnvRe
                + preEnvRe + leEnvRe + cerEnvRe + infEnvRe + ifEnvRe + liEnvRe + aipEnvRe + emPEnvRe + epEnvRe + emEEnvRe + canEnvRe + CambDEnvRe + recEnvRe
                + cjEnvRe + fiEnvRe + picEnvRe;

            var totalEnviadoRegional = totalEnv + totalEnvRe;

            tituloData.TotalEnv = totalEnv;
            tituloData.TotalEnvRe = totalEnvRe;
            tituloData.TotalEnviadoRegional = totalEnviadoRegional;

            #endregion

            #region TRABAJOS RECEPCIONADOS DE ENTRADA

            var queryRecep = from movimiento in _dbContext.RmMovimientosDocs
                             join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                             //join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                             join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                             where movimiento.FechaOperacion <= parametros.FechaHasta &&
                                   movimiento.FechaOperacion >= parametros.FechaDesde
                                   && movimiento.EstadoEntrada == 26 &&
                                   movimiento.CodUsuario == parametros.Usuario

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

            var primerRecep = queryRecep.ToList();

            var insRecep = primerRecep.Count(e => e.TipoSolicitud == 1 && e.ReIngreso ==null);
            tituloData.InscripcionRecep = insRecep;
            var insRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 1);
            tituloData.InscripcionRecepRe = insRecepRe;

            var adjuRecep = primerRecep.Count(e => e.TipoSolicitud == 2 && e.ReIngreso == null);
            tituloData.AdjudicacionRecep = adjuRecep;
            var adjuRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 2);
            tituloData.AdjudicacionRecepRe = adjuRecepRe;

            var transRecep = primerRecep.Count(e => e.TipoSolicitud == 3 && e.ReIngreso == null);
            tituloData.TransferenciaRecep = transRecep;
            var transRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 3);
            tituloData.TransferenciaRecepRe = transRecepRe;

            var reinsriRecep = primerRecep.Count(e => e.TipoSolicitud == 4 && e.ReIngreso == null);
            tituloData.ReinscripcionRecep = reinsriRecep;
            var reinsriRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 4);
            tituloData.ReinscripcionRecepRe = reinsriRecepRe;

            var duRecep = primerRecep.Count(e => e.TipoSolicitud == 5 && e.ReIngreso == null);
            tituloData.DuplicadoRecep = duRecep;
            var duRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 5);
            tituloData.DuplicadoRecepRe = duRecepRe;

            var coRecep = primerRecep.Count(e => e.TipoSolicitud == 6 && e.ReIngreso == null);
            tituloData.CopiaRecep = coRecep;
            var coRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 6);
            tituloData.CopiaRecepRe = coRecepRe;

            var perRecep = primerRecep.Count(e => e.TipoSolicitud == 7 && e.ReIngreso == null);
            tituloData.PermutaRecep = perRecep;
            var perRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 7);
            tituloData.PermutaRecepRe = perRecepRe;

            var doRecep = primerRecep.Count(e => e.TipoSolicitud == 8 && e.ReIngreso == null);
            tituloData.DonacionRecep = doRecep;
            var doRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 8);
            tituloData.DonacionRecepRe = doRecepRe;

            var usuRecep = primerRecep.Count(e => e.TipoSolicitud == 9 && e.ReIngreso == null);
            tituloData.UsufructoRecep = usuRecep;
            var usuRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 9);
            tituloData.UsufructoRecepRe = usuRecepRe;

            var preRecep = primerRecep.Count(e => e.TipoSolicitud == 10 && e.ReIngreso == null);
            tituloData.PrendaRecep = preRecep;
            var preRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 10);
            tituloData.PrendaRecepRe = preRecepRe;

            var leRecep = primerRecep.Count(e => e.TipoSolicitud == 11 && e.ReIngreso == null);
            tituloData.LevantamientoRecep = leRecep;
            var leRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 11);
            tituloData.LevantamientoRecepRe = leRecepRe;

            var cerRecep = primerRecep.Count(e =>e.ReIngreso == null && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22)  );
            tituloData.CertificadoRecep = cerRecep;
            var cerRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22));
            tituloData.CertificadoRecepRe = cerRecepRe;

            var infRecep = primerRecep.Count(e => e.TipoSolicitud == 13 && e.ReIngreso == null);
            tituloData.InformeRecep = infRecep;
            var infRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 13);
            tituloData.InformeRecepRe = infRecepRe;

            var ifRecep = primerRecep.Count(e => e.TipoSolicitud == 14 && e.ReIngreso == null);
            tituloData.InformeJudicialRecep = ifRecep;
            var ifRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 14);
            tituloData.InformeJudicialRecepRe = ifRecepRe;

            var liRecep = primerRecep.Count(e => e.TipoSolicitud == 15 && e.ReIngreso == null);
            tituloData.LitisRecep = liRecep;
            var liRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 15);
            tituloData.LitisRecepRe = liRecepRe;

            var aipRecep = primerRecep.Count(e => e.TipoSolicitud == 16 && e.ReIngreso == null);
            tituloData.AnotacionDeInscripcionPreventivaRecep = aipRecep;
            var aipRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 16);
            tituloData.AnotacionDeInscripcionPreventivaRecepRe = aipRecepRe;

            var emPRecep = primerRecep.Count(e => e.TipoSolicitud == 17 && e.ReIngreso == null);
            tituloData.EmbargoPreventivoRecep = emPRecep;
            var emPRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 17);
            tituloData.EmbargoPreventivoRecepRe = emPRecepRe;

            var epRecep = primerRecep.Count(e => e.TipoSolicitud == 18 && e.ReIngreso == null);
            tituloData.DacionPagoRecep = epRecep;
            var epRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 18);
            tituloData.DacionPagoRecepRe = epRecepRe;

            var emERecep = primerRecep.Count(e => e.TipoSolicitud == 19 && e.ReIngreso == null);
            tituloData.EmbargoEjecutivoRecep = emERecep;
            var emERecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 19);
            tituloData.EmbargoEjecutivoRecepRe = emERecepRe;

            var canRecep = primerRecep.Count(e => e.TipoSolicitud == 23 && e.ReIngreso == null);
            tituloData.CancelacionRecep = canRecep;
            var canRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 23);
            tituloData.CancelacionRecepRe = canRecepRe;

            var CambDRecep = primerRecep.Count(e => e.TipoSolicitud == 25 && e.ReIngreso == null);
            tituloData.CambioDenominacionRecep = CambDRecep;
            var CambDRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 25);
            tituloData.CambioDenominacionRecepRe = CambDRecepRe;

            var recRecep = primerRecep.Count(e => e.TipoSolicitud == 26 && e.ReIngreso == null);
            tituloData.RectificacionRecep = recRecep;
            var recRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 26);
            tituloData.RectificacionRecepRe = recRecepRe;

            var cjRecep = primerRecep.Count(e => e.TipoSolicitud == 28 && e.ReIngreso == null);
            tituloData.CopiaJudicialRecep = cjRecep;
            var cjRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 28);
            tituloData.CopiaJudicialRecepRe = cjRecepRe;

            var fiRecep = primerRecep.Count(e => e.TipoSolicitud == 32 && e.ReIngreso == null);
            tituloData.FianzaRecep = fiRecep;
            var fiRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 32);
            tituloData.FianzaRecepRe = fiRecepRe;

            var picRecep = primerRecep.Count(e => e.TipoSolicitud == 34 && e.ReIngreso ==null);
            tituloData.ProDeIn_y_ConRecep = picRecep;
            var picRecepRe = primerRecep.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 34);
            tituloData.ProDeIn_y_ConRecepRe = picRecepRe;

            var totalRecep = insRecep + adjuRecep + transRecep + reinsriRecep + duRecep + coRecep + perRecep + doRecep + usuRecep
                + preRecep + leRecep + cerRecep + infRecep + ifRecep + liRecep + aipRecep + emPRecep + epRecep + emERecep + canRecep + CambDRecep + recRecep
                + cjRecep + fiRecep + picRecep;

            var totalRecepRe = insRecepRe + adjuRecepRe + transRecepRe + reinsriRecepRe + duRecepRe + coRecepRe + perRecepRe + doRecepRe + usuRecepRe
                + preRecepRe + leRecepRe + cerRecepRe + infRecepRe + ifRecepRe + liRecepRe + aipRecepRe + emPRecepRe + epRecepRe + emERecepRe + canRecepRe + CambDRecepRe + recRecepRe
                + cjRecepRe + fiRecepRe + picRecepRe;

            var totalRecepEntrada = totalRecep + totalRecepRe;

            tituloData.TotalRecep = totalRecep;
            tituloData.TotalRecepRe = totalRecepRe;
            tituloData.TotalRecepEntrada = totalRecepEntrada;

            #endregion

            #region TRABAJOS DE REMISIÓN DE ENTRADAS A DISEÑO

            var queryReDise = from movimiento in _dbContext.RmMovimientosDocs
                              join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                              //join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                              join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                              where movimiento.FechaOperacion <= parametros.FechaHasta &&
                                    movimiento.FechaOperacion >= parametros.FechaDesde
                                    && movimiento.EstadoEntrada == 42 &&
                                    movimiento.CodUsuario == parametros.Usuario

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

            var primerReDise = queryReDise.ToList();

            // PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS INSCRIPCION
            var insReDise = primerReDise.Count(e => e.TipoSolicitud == 1 && e.ReIngreso == null);
            tituloData.InscripcionReDise = insReDise;
            var insReDiseRe = primerReDise.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 1);
            tituloData.InscripcionReDiseRe = insReDiseRe;

            var totalReDise = insReDise;
            var totalReDiseRe = insReDiseRe;

            var totalRemisionDiseño = totalReDise + totalReDiseRe;

            tituloData.TotalReDise = totalReDise;
            tituloData.TotalReDiseRe = totalReDiseRe;
            tituloData.TotalRemisionDiseño = totalReDise;

            #endregion

            #region TRABAJOS RECEPCIONADOS DE DISEÑO

            var queryDise = from movimiento in _dbContext.RmMovimientosDocs
                            join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                            //join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                            join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                            where movimiento.FechaOperacion <= parametros.FechaHasta &&
                                  movimiento.FechaOperacion >= parametros.FechaDesde
                                  && movimiento.EstadoEntrada == 44 &&
                                 movimiento.CodUsuario == parametros.Usuario
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

            var primerDise = queryDise.ToList();

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS INSCRIPCION
            var insDise = primerDise.Count(e => e.TipoSolicitud == 1 && e.ReIngreso == null);
            tituloData.InscripcionDise = insDise;
            var insDiseRe = primerDise.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 1);
            tituloData.InscripcionDiseRe = insDiseRe;

            var totalDise = insDise;
            var totalDiseRe = insDiseRe;

            var totalRecepDise = totalDise + totalDiseRe;

            tituloData.TotalDise = totalDise;
            tituloData.TotalDiseRe = totalDiseRe;
            tituloData.TotalRecepDiseño = totalDise;

            #endregion

            #region TRABAJOS ASIGNADOS

            var queryAsig = from movimiento in _dbContext.RmMovimientosDocs
                            join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                            //join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                            join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                            where movimiento.FechaOperacion <= parametros.FechaHasta &&
                                  movimiento.FechaOperacion >= parametros.FechaDesde
                                  && movimiento.EstadoEntrada == 6 &&
                                 movimiento.CodUsuario == parametros.Usuario
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

            var primerAsig = queryAsig.ToList();

            var insAsig = primerAsig.Count(e => e.TipoSolicitud == 1 && e.ReIngreso == null);
            tituloData.InscripcionAsig = insAsig;
            var insAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 1);
            tituloData.InscripcionAsigRe = insAsigRe;

            var adjuAsig = primerAsig.Count(e => e.TipoSolicitud == 2 && e.ReIngreso == null);
            tituloData.AdjudicacionAsig = adjuAsig;
            var adjuAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 2);
            tituloData.AdjudicacionAsigRe = adjuAsigRe;

            var transAsig = primerAsig.Count(e => e.TipoSolicitud == 3 && e.ReIngreso == null);
            tituloData.TransferenciaAsig = transAsig;
            var transAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 3);
            tituloData.TransferenciaAsigRe = transAsigRe;

            var reinsriAsig = primerAsig.Count(e => e.TipoSolicitud == 4 && e.ReIngreso == null);
            tituloData.ReinscripcionAsig = reinsriAsig;
            var reinsriAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 4);
            tituloData.ReinscripcionAsigRe = reinsriAsigRe;

            var duAsig = primerAsig.Count(e => e.TipoSolicitud == 5 && e.ReIngreso == null);
            tituloData.DuplicadoAsig = duAsig;
            var duAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 5);
            tituloData.DuplicadoAsigRe = duAsigRe;

            var coAsig = primerAsig.Count(e => e.TipoSolicitud == 6 && e.ReIngreso == null);
            tituloData.CopiaAsig = coAsig;
            var coAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 6);
            tituloData.CopiaAsigRe = coAsigRe;

            var perAsig = primerAsig.Count(e => e.TipoSolicitud == 7 && e.ReIngreso == null);
            tituloData.PermutaAsig = perAsig;
            var perAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 7);
            tituloData.PermutaAsigRe = perAsigRe;

            var doAsig = primerAsig.Count(e => e.TipoSolicitud == 8 && e.ReIngreso == null);
            tituloData.DonacionAsig = doAsig;
            var doAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 8);
            tituloData.DonacionAsigRe = doAsigRe;

            var usuAsig = primerAsig.Count(e => e.TipoSolicitud == 9 && e.ReIngreso == null);
            tituloData.UsufructoAsig = usuAsig;
            var usuAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 9);
            tituloData.UsufructoAsigRe = usuAsigRe;

            var preAsig = primerAsig.Count(e => e.TipoSolicitud == 10 && e.ReIngreso == null);
            tituloData.PrendaAsig = preAsig;
            var preAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 10);
            tituloData.PrendaAsigRe = preAsigRe;

            var leAsig = primerAsig.Count(e => e.TipoSolicitud == 11 && e.ReIngreso == null);
            tituloData.LevantamientoAsig = leAsig;
            var leAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 11);
            tituloData.LevantamientoAsigRe = leAsigRe;

            var cerAsig = primerAsig.Count(e => e.TipoSolicitud == 12 || e.TipoSolicitud == 22);
            tituloData.CertificadoAsig = cerAsig;
            var cerAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22));
            tituloData.CertificadoAsigRe = cerAsigRe;

            var infAsig = primerAsig.Count(e => e.TipoSolicitud == 13 && e.ReIngreso == null);
            tituloData.InformeAsig = infAsig;
            var infAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 13);
            tituloData.InformeAsigRe = infAsigRe;

            var ifAsig = primerAsig.Count(e => e.TipoSolicitud == 14 && e.ReIngreso == null);
            tituloData.InformeJudicialAsig = ifAsig;
            var ifAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 14);
            tituloData.InformeJudicialAsigRe = ifAsigRe;

            var liAsig = primerAsig.Count(e => e.TipoSolicitud == 15 && e.ReIngreso == null);
            tituloData.LitisAsig = liAsig;
            var liAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 15);
            tituloData.LitisAsigRe = liAsigRe;

            var aipAsig = primerAsig.Count(e => e.TipoSolicitud == 16 && e.ReIngreso == null);
            tituloData.AnotacionDeInscripcionPreventivaAsig = aipAsig;
            var aipAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 16);
            tituloData.AnotacionDeInscripcionPreventivaAsigRe = aipAsigRe;

            var emPAsig = primerAsig.Count(e => e.TipoSolicitud == 17 && e.ReIngreso == null);
            tituloData.EmbargoPreventivoAsig = emPAsig;
            var emPAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 17);
            tituloData.EmbargoPreventivoAsigRe = emPAsigRe;

            var epAsig = primerAsig.Count(e => e.TipoSolicitud == 18 && e.ReIngreso == null);
            tituloData.DacionPagoAsig = epAsig;
            var epAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 18);
            tituloData.DacionPagoAsigRe = epAsigRe;

            var emEAsig = primerAsig.Count(e => e.TipoSolicitud == 19 && e.ReIngreso == null);
            tituloData.EmbargoEjecutivoAsig = emEAsig;
            var emEAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 19);
            tituloData.EmbargoEjecutivoAsigRe = emEAsigRe;

            var canAsig = primerAsig.Count(e => e.TipoSolicitud == 23 && e.ReIngreso == null);
            tituloData.CancelacionAsig = canAsig;
            var canAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 23);
            tituloData.CancelacionAsigRe = canAsigRe;

            var CambDAsig = primerAsig.Count(e => e.TipoSolicitud == 25 && e.ReIngreso == null);
            tituloData.CambioDenominacionAsig = CambDAsig;
            var CambDAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 25);
            tituloData.CambioDenominacionAsigRe = CambDAsigRe;

            var recAsig = primerAsig.Count(e => e.TipoSolicitud == 26 && e.ReIngreso == null);
            tituloData.RectificacionAsig = recAsig;
            var recAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 26);
            tituloData.RectificacionAsigRe = recAsigRe;

            var cjAsig = primerAsig.Count(e => e.TipoSolicitud == 28 && e.ReIngreso == null);
            tituloData.CopiaJudicialAsig = cjAsig;
            var cjAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 28);
            tituloData.CopiaJudicialAsigRe = cjAsigRe;

            var fiAsig = primerAsig.Count(e => e.TipoSolicitud == 32 && e.ReIngreso == null);
            tituloData.FianzaAsig = fiAsig;
            var fiAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 32);
            tituloData.FianzaAsigRe = fiAsigRe;

            var picAsig = primerAsig.Count(e => e.TipoSolicitud == 34 && e.ReIngreso == null);
            tituloData.ProDeIn_y_ConAsig = picAsig;
            var picAsigRe = primerAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 34);
            tituloData.ProDeIn_y_ConAsigRe = picAsigRe;

            var totalAsig = insAsig + adjuAsig + transAsig + reinsriAsig + duAsig + coAsig + perAsig + doAsig + usuAsig
                + preAsig + leAsig + cerAsig + infAsig + ifAsig + liAsig + aipAsig + emPAsig + epAsig + emEAsig + canAsig + CambDAsig + recAsig
                + cjAsig + fiAsig + picAsig;

            var totalAsigRe = insAsigRe + adjuAsigRe + transAsigRe + reinsriAsigRe + duAsigRe + coAsigRe + perAsigRe + doAsigRe + usuAsigRe
                + preAsigRe + leAsigRe + cerAsigRe + infAsigRe + ifAsigRe + liAsigRe + aipAsigRe + emPAsigRe + epAsigRe + emEAsigRe + canAsigRe + CambDAsigRe + recAsigRe
                + cjAsigRe + fiAsigRe + picAsigRe;

            var totalAsignados = totalAsig + totalAsigRe;

            tituloData.TotalAsig = totalAsig;
            tituloData.TotalAsigRe = totalAsigRe;
            tituloData.TotalAsignados = totalAsignados;

            #endregion

            #region TRABAJOS REASIGNADOS

            var queryReAsig = from movimiento in _dbContext.RmMovimientosDocs
                              join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                              //join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                              join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                              where movimiento.FechaOperacion <= parametros.FechaHasta &&
                                    movimiento.FechaOperacion >= parametros.FechaDesde
                                    && movimiento.EstadoEntrada == 45 &&
                                 movimiento.CodUsuario == parametros.Usuario
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

            var primerReAsig = queryReAsig.ToList();

            var insReAsig = primerReAsig.Count(e => e.TipoSolicitud == 1 && e.ReIngreso == null);
            tituloData.InscripcionReAsig = insReAsig;
            var insReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 1);
            tituloData.InscripcionReAsigRe = insReAsigRe;

            var adjuReAsig = primerReAsig.Count(e => e.TipoSolicitud == 2 && e.ReIngreso == null);
            tituloData.AdjudicacionReAsig = adjuReAsig;
            var adjuReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 2);
            tituloData.AdjudicacionReAsigRe = adjuReAsigRe;

            var transReAsig = primerReAsig.Count(e => e.TipoSolicitud == 3 && e.ReIngreso == null);
            tituloData.TransferenciaReAsig = transReAsig;
            var transReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 3);
            tituloData.TransferenciaReAsigRe = transReAsigRe;

            var reinsriReAsig = primerReAsig.Count(e => e.TipoSolicitud == 4 && e.ReIngreso == null);
            tituloData.ReinscripcionReAsig = reinsriReAsig;
            var reinsriReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 4);
            tituloData.ReinscripcionReAsigRe = reinsriReAsigRe;

            var duReAsig = primerReAsig.Count(e => e.TipoSolicitud == 5 && e.ReIngreso == null);
            tituloData.DuplicadoReAsig = duReAsig;
            var duReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 5);
            tituloData.DuplicadoReAsigRe = duReAsigRe;

            var coReAsig = primerReAsig.Count(e => e.TipoSolicitud == 6 && e.ReIngreso == null);
            tituloData.CopiaReAsig = coReAsig;
            var coReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 6);
            tituloData.CopiaReAsigRe = coReAsigRe;

            var perReAsig = primerReAsig.Count(e => e.TipoSolicitud == 7 && e.ReIngreso == null);
            tituloData.PermutaReAsig = perReAsig;
            var perReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 7);
            tituloData.PermutaReAsigRe = perReAsigRe;

            var doReAsig = primerReAsig.Count(e => e.TipoSolicitud == 8 && e.ReIngreso == null);
            tituloData.DonacionReAsig = doReAsig;
            var doReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 8);
            tituloData.DonacionReAsigRe = doReAsigRe;

            var usuReAsig = primerReAsig.Count(e => e.TipoSolicitud == 9 && e.ReIngreso == null);
            tituloData.UsufructoReAsig = usuReAsig;
            var usuReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 9);
            tituloData.UsufructoReAsigRe = usuReAsigRe;

            var preReAsig = primerReAsig.Count(e => e.TipoSolicitud == 10 && e.ReIngreso == null);
            tituloData.PrendaReAsig = preReAsig;
            var preReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 10);
            tituloData.PrendaReAsigRe = preReAsigRe;

            var leReAsig = primerReAsig.Count(e => e.TipoSolicitud == 11 && e.ReIngreso == null);
            tituloData.LevantamientoReAsig = leReAsig;
            var leReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 11);
            tituloData.LevantamientoReAsigRe = leReAsigRe;

            var cerReAsig = primerReAsig.Count(e => e.TipoSolicitud == 12 || e.TipoSolicitud == 22);
            tituloData.CertificadoReAsig = cerReAsig;
            var cerReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22));
            tituloData.CertificadoReAsigRe = cerReAsigRe;

            var infReAsig = primerReAsig.Count(e => e.TipoSolicitud == 13 && e.ReIngreso == null);
            tituloData.InformeReAsig = infReAsig;
            var infReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 13);
            tituloData.InformeReAsigRe = infReAsigRe;

            var ifReAsig = primerReAsig.Count(e => e.TipoSolicitud == 14 && e.ReIngreso == null);
            tituloData.InformeJudicialReAsig = ifReAsig;
            var ifReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 14);
            tituloData.InformeJudicialReAsigRe = ifReAsigRe;

            var liReAsig = primerReAsig.Count(e => e.TipoSolicitud == 15 && e.ReIngreso == null);
            tituloData.LitisReAsig = liReAsig;
            var liReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 15);
            tituloData.LitisReAsigRe = liReAsigRe;

            var aipReAsig = primerReAsig.Count(e => e.TipoSolicitud == 16 && e.ReIngreso == null);
            tituloData.AnotacionDeInscripcionPreventivaReAsig = aipReAsig;
            var aipReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 16);
            tituloData.AnotacionDeInscripcionPreventivaReAsigRe = aipReAsigRe;

            var emPReAsig = primerReAsig.Count(e => e.TipoSolicitud == 17 && e.ReIngreso == null);
            tituloData.EmbargoPreventivoReAsig = emPReAsig;
            var emPReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 17);
            tituloData.EmbargoPreventivoReAsigRe = emPReAsigRe;

            var epReAsig = primerReAsig.Count(e => e.TipoSolicitud == 18 && e.ReIngreso == null);
            tituloData.DacionPagoReAsig = epReAsig;
            var epReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 18);
            tituloData.DacionPagoReAsigRe = epReAsigRe;

            var emEReAsig = primerReAsig.Count(e => e.TipoSolicitud == 19 && e.ReIngreso == null);
            tituloData.EmbargoEjecutivoReAsig = emEReAsig;
            var emEReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 19);
            tituloData.EmbargoEjecutivoReAsigRe = emEReAsigRe;

            var canReAsig = primerReAsig.Count(e => e.TipoSolicitud == 23 && e.ReIngreso == null);
            tituloData.CancelacionReAsig = canReAsig;
            var canReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 23);
            tituloData.CancelacionReAsigRe = canReAsigRe;

            var CambDReAsig = primerReAsig.Count(e => e.TipoSolicitud == 25 && e.ReIngreso == null);
            tituloData.CambioDenominacionReAsig = CambDReAsig;
            var CambDReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 25);
            tituloData.CambioDenominacionReAsigRe = CambDReAsigRe;

            var recReAsig = primerReAsig.Count(e => e.TipoSolicitud == 26 && e.ReIngreso == null);
            tituloData.RectificacionReAsig = recReAsig;
            var recReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 26);
            tituloData.RectificacionReAsigRe = recReAsigRe;

            var cjReAsig = primerReAsig.Count(e => e.TipoSolicitud == 28 && e.ReIngreso == null);
            tituloData.CopiaJudicialReAsig = cjReAsig;
            var cjReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 28);
            tituloData.CopiaJudicialReAsigRe = cjReAsigRe;

            var fiReAsig = primerReAsig.Count(e => e.TipoSolicitud == 32 && e.ReIngreso == null);
            tituloData.FianzaReAsig = fiReAsig;
            var fiReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 32);
            tituloData.FianzaReAsigRe = fiReAsigRe;

            var picReAsig = primerReAsig.Count(e => e.TipoSolicitud == 34 && e.ReIngreso == null);
            tituloData.ProDeIn_y_ConReAsig = picReAsig;
            var picReAsigRe = primerReAsig.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 34);
            tituloData.ProDeIn_y_ConReAsigRe = picReAsigRe;

            var totalReAsig = insReAsig + adjuReAsig + transReAsig + reinsriReAsig + duReAsig + coReAsig + perReAsig + doReAsig + usuReAsig
                + preReAsig + leReAsig + cerReAsig + infReAsig + ifReAsig + liReAsig + aipReAsig + emPReAsig + epReAsig + emEReAsig + canReAsig + CambDReAsig + recReAsig
                + cjReAsig + fiReAsig + picReAsig;

            var totalReAsigRe = insReAsigRe + adjuReAsigRe + transReAsigRe + reinsriReAsigRe + duReAsigRe + coReAsigRe + perReAsigRe + doReAsigRe + usuReAsigRe
                + preReAsigRe + leReAsigRe + cerReAsigRe + infReAsigRe + ifReAsigRe + liReAsigRe + aipReAsigRe + emPReAsigRe + epReAsigRe + emEReAsigRe + canReAsigRe + CambDReAsigRe + recReAsigRe
                + cjReAsigRe + fiReAsigRe + picReAsigRe;

            var totalReasignados = totalReAsig + totalReAsigRe;

            tituloData.TotalReAsig = totalReAsig;
            tituloData.TotalReAsigRe = totalReAsigRe;
            tituloData.TotalReasignados = totalReasignados;

            #endregion

            #region TRABAJOS FIRMADOS REGIONAL

            var queryFirm = from movimiento in _dbContext.RmMovimientosDocs
                            join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                            //join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                            join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                            where movimiento.FechaOperacion <= parametros.FechaHasta &&
                                  movimiento.FechaOperacion >= parametros.FechaDesde
                                  && movimiento.EstadoEntrada == 34 &&
                                 movimiento.CodUsuario == parametros.Usuario
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

            var primerFirm = queryFirm.ToList();

            var insFirm = primerFirm.Count(e => e.TipoSolicitud == 1 && e.ReIngreso == null);
            tituloData.InscripcionFirm = insFirm;
            var insFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 1);
            tituloData.InscripcionFirmRe = insFirmRe;

            var adjuFirm = primerFirm.Count(e => e.TipoSolicitud == 2 && e.ReIngreso == null);
            tituloData.AdjudicacionFirm = adjuFirm;
            var adjuFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 2);
            tituloData.AdjudicacionFirmRe = adjuFirmRe;

            var transFirm = primerFirm.Count(e => e.TipoSolicitud == 3 && e.ReIngreso == null);
            tituloData.TransferenciaFirm = transFirm;
            var transFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 3);
            tituloData.TransferenciaFirmRe = transFirmRe;

            var reinsriFirm = primerFirm.Count(e => e.TipoSolicitud == 4 && e.ReIngreso == null);
            tituloData.ReinscripcionFirm = reinsriFirm;
            var reinsriFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 4);
            tituloData.ReinscripcionFirmRe = reinsriFirmRe;

            var duFirm = primerFirm.Count(e => e.TipoSolicitud == 5 && e.ReIngreso == null);
            tituloData.DuplicadoFirm = duFirm;
            var duFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 5);
            tituloData.DuplicadoFirmRe = duFirmRe;

            var coFirm = primerFirm.Count(e => e.TipoSolicitud == 6 && e.ReIngreso == null);
            tituloData.CopiaFirm = coFirm;
            var coFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 6);
            tituloData.CopiaFirmRe = coFirmRe;

            var perFirm = primerFirm.Count(e => e.TipoSolicitud == 7 && e.ReIngreso == null);
            tituloData.PermutaFirm = perFirm;
            var perFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 7);
            tituloData.PermutaFirmRe = perFirmRe;

            var doFirm = primerFirm.Count(e => e.TipoSolicitud == 8 && e.ReIngreso == null);
            tituloData.DonacionFirm = doFirm;
            var doFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 8);
            tituloData.DonacionFirmRe = doFirmRe;

            var usuFirm = primerFirm.Count(e => e.TipoSolicitud == 9 && e.ReIngreso == null);
            tituloData.UsufructoFirm = usuFirm;
            var usuFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 9);
            tituloData.UsufructoFirmRe = usuFirmRe;

            var preFirm = primerFirm.Count(e => e.TipoSolicitud == 10 && e.ReIngreso == null);
            tituloData.PrendaFirm = preFirm;
            var preFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 10);
            tituloData.PrendaFirmRe = preFirmRe;

            var leFirm = primerFirm.Count(e => e.TipoSolicitud == 11 && e.ReIngreso == null);
            tituloData.LevantamientoFirm = leFirm;
            var leFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 11);
            tituloData.LevantamientoFirmRe = leFirmRe;

            var cerFirm = primerFirm.Count(e => e.TipoSolicitud == 12 || e.TipoSolicitud == 22);
            tituloData.CertificadoFirm = cerFirm;
            var cerFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22));
            tituloData.CertificadoFirmRe = cerFirmRe;

            var infFirm = primerFirm.Count(e => e.TipoSolicitud == 13 && e.ReIngreso == null);
            tituloData.InformeFirm = infFirm;
            var infFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 13);
            tituloData.InformeFirmRe = infFirmRe;

            var ifFirm = primerFirm.Count(e => e.TipoSolicitud == 14 && e.ReIngreso == null);
            tituloData.InformeJudicialFirm = ifFirm;
            var ifFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 14);
            tituloData.InformeJudicialFirmRe = ifFirmRe;

            var liFirm = primerFirm.Count(e => e.TipoSolicitud == 15 && e.ReIngreso == null);
            tituloData.LitisFirm = liFirm;
            var liFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 15);
            tituloData.LitisFirmRe = liFirmRe;

            var aipFirm = primerFirm.Count(e => e.TipoSolicitud == 16 && e.ReIngreso == null);
            tituloData.AnotacionDeInscripcionPreventivaFirm = aipFirm;
            var aipFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 16);
            tituloData.AnotacionDeInscripcionPreventivaFirmRe = aipFirmRe;

            var emPFirm = primerFirm.Count(e => e.TipoSolicitud == 17 && e.ReIngreso == null);
            tituloData.EmbargoPreventivoFirm = emPFirm;
            var emPFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 17);
            tituloData.EmbargoPreventivoFirmRe = emPFirmRe;

            var epFirm = primerFirm.Count(e => e.TipoSolicitud == 18 && e.ReIngreso == null);
            tituloData.DacionPagoFirm = epFirm;
            var epFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 18);
            tituloData.DacionPagoFirmRe = epFirmRe;

            var emEFirm = primerFirm.Count(e => e.TipoSolicitud == 19 && e.ReIngreso == null);
            tituloData.EmbargoEjecutivoFirm = emEFirm;
            var emEFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 19);
            tituloData.EmbargoEjecutivoFirmRe = emEFirmRe;

            var canFirm = primerFirm.Count(e => e.TipoSolicitud == 23 && e.ReIngreso == null);
            tituloData.CancelacionFirm = canFirm;
            var canFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 23);
            tituloData.CancelacionFirmRe = canFirmRe;

            var CambDFirm = primerFirm.Count(e => e.TipoSolicitud == 25 && e.ReIngreso == null);
            tituloData.CambioDenominacionFirm = CambDFirm;
            var CambDFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 25);
            tituloData.CambioDenominacionFirmRe = CambDFirmRe;

            var recFirm = primerFirm.Count(e => e.TipoSolicitud == 26 && e.ReIngreso == null);
            tituloData.RectificacionFirm = recFirm;
            var recFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 26);
            tituloData.RectificacionFirmRe = recFirmRe;

            var cjFirm = primerFirm.Count(e => e.TipoSolicitud == 28 && e.ReIngreso == null);
            tituloData.CopiaJudicialFirm = cjFirm;
            var cjFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 28);
            tituloData.CopiaJudicialFirmRe = cjFirmRe;

            var fiFirm = primerFirm.Count(e => e.TipoSolicitud == 32 && e.ReIngreso == null);
            tituloData.FianzaFirm = fiFirm;
            var fiFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 32);
            tituloData.FianzaFirmRe = fiFirmRe;

            var picFirm = primerFirm.Count(e => e.TipoSolicitud == 34 && e.ReIngreso == null);
            tituloData.ProDeIn_y_ConFirm = picFirm;
            var picFirmRe = primerFirm.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 34);
            tituloData.ProDeIn_y_ConFirmRe = picFirmRe;

            var totalFirm = insFirm + adjuFirm + transFirm + reinsriFirm + duFirm + coFirm + perFirm + doFirm + usuFirm
                + preFirm + leFirm + cerFirm + infFirm + ifFirm + liFirm + aipFirm + emPFirm + epFirm + emEFirm + canFirm + CambDFirm + recFirm
                + cjFirm + fiFirm + picFirm;

            var totalFirmRe = insFirmRe + adjuFirmRe + transFirmRe + reinsriFirmRe + duFirmRe + coFirmRe + perFirmRe + doFirmRe + usuFirmRe
                + preFirmRe + leFirmRe + cerFirmRe + infFirmRe + ifFirmRe + liFirmRe + aipFirmRe + emPFirmRe + epFirmRe + emEFirmRe + canFirmRe + CambDFirmRe + recFirmRe
                + cjFirmRe + fiFirmRe + picFirmRe;

            var totalDocFirmadosReg = totalFirm + totalFirmRe;

            tituloData.TotalFirm = totalFirm;
            tituloData.TotalFirmRe = totalFirmRe;
            tituloData.TotalDocFirmadosReg = totalDocFirmadosReg;

            #endregion

            #region TRABAJOS ENVIADOS A SECCIÓN REGIONAL

            var queryEnvSec = from movimiento in _dbContext.RmMovimientosDocs
                              join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                              //join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                              join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                              where movimiento.FechaOperacion <= parametros.FechaHasta &&
                                    movimiento.FechaOperacion >= parametros.FechaDesde
                                    && movimiento.EstadoEntrada == 28 &&
                                 movimiento.CodUsuario == parametros.Usuario
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

            var primerEnvSec = queryEnvSec.ToList();

            // Contando solicitudes iniciales y reingresos para cada tipo de solicitud

            var insEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 1);
            tituloData.InscripcionEnvSec = insEnvSec;
            var insEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 1);
            tituloData.InscripcionEnvSecRe = insEnvSecRe;

            var adjuEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 2);
            tituloData.AdjudicacionEnvSec = adjuEnvSec;
            var adjuEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 2);
            tituloData.AdjudicacionEnvSecRe = adjuEnvSecRe;

            var transEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 3);
            tituloData.TransferenciaEnvSec = transEnvSec;
            var transEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 3);
            tituloData.TransferenciaEnvSecRe = transEnvSecRe;

            var reinsriEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 4);
            tituloData.ReinscripcionEnvSec = reinsriEnvSec;
            var reinsriEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 4);
            tituloData.ReinscripcionEnvSecRe = reinsriEnvSecRe;

            var duEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 5);
            tituloData.DuplicadoEnvSec = duEnvSec;
            var duEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 5);
            tituloData.DuplicadoEnvSecRe = duEnvSecRe;

            var coEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 6);
            tituloData.CopiaEnvSec = coEnvSec;
            var coEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 6);
            tituloData.CopiaEnvSecRe = coEnvSecRe;

            var perEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 7);
            tituloData.PermutaEnvSec = perEnvSec;
            var perEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 7);
            tituloData.PermutaEnvSecRe = perEnvSecRe;

            var doEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 8);
            tituloData.DonacionEnvSec = doEnvSec;
            var doEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 8);
            tituloData.DonacionEnvSecRe = doEnvSecRe;

            var usuEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 9);
            tituloData.UsufructoEnvSec = usuEnvSec;
            var usuEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 9);
            tituloData.UsufructoEnvSecRe = usuEnvSecRe;

            var preEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 10);
            tituloData.PrendaEnvSec = preEnvSec;
            var preEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 10);
            tituloData.PrendaEnvSecRe = preEnvSecRe;

            var leEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 11);
            tituloData.LevantamientoEnvSec = leEnvSec;
            var leEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 11);
            tituloData.LevantamientoEnvSecRe = leEnvSecRe;

            var cerEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 12 || e.TipoSolicitud == 22);
            tituloData.CertificadoEnvSec = cerEnvSec;
            var cerEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22));
            tituloData.CertificadoEnvSecRe = cerEnvSecRe;

            var infEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 13);
            tituloData.InformeEnvSec = infEnvSec;
            var infEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 13);
            tituloData.InformeEnvSecRe = infEnvSecRe;

            var ifEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 14);
            tituloData.InformeJudicialEnvSec = ifEnvSec;
            var ifEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 14);
            tituloData.InformeJudicialEnvSecRe = ifEnvSecRe;

            var liEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 15);
            tituloData.LitisEnvSec = liEnvSec;
            var liEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 15);
            tituloData.LitisEnvSecRe = liEnvSecRe;

            var aipEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 16);
            tituloData.AnotacionDeInscripcionPreventivaEnvSec = aipEnvSec;
            var aipEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 16);
            tituloData.AnotacionDeInscripcionPreventivaEnvSecRe = aipEnvSecRe;

            var emPEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 17);
            tituloData.EmbargoPreventivoEnvSec = emPEnvSec;
            var emPEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 17);
            tituloData.EmbargoPreventivoEnvSecRe = emPEnvSecRe;

            var epEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 18);
            tituloData.DacionPagoEnvSec = epEnvSec;
            var epEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 18);
            tituloData.DacionPagoEnvSecRe = epEnvSecRe;

            var emEEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 19);
            tituloData.EmbargoEjecutivoEnvSec = emEEnvSec;
            var emEEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 19);
            tituloData.EmbargoEjecutivoEnvSecRe = emEEnvSecRe;

            var canEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 23);
            tituloData.CancelacionEnvSec = canEnvSec;
            var canEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 23);
            tituloData.CancelacionEnvSecRe = canEnvSecRe;

            var CambDEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 25);
            tituloData.CambioDenominacionEnvSec = CambDEnvSec;
            var CambDEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 25);
            tituloData.CambioDenominacionEnvSecRe = CambDEnvSecRe;

            var recEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 26);
            tituloData.RectificacionEnvSec = recEnvSec;
            var recEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 26);
            tituloData.RectificacionEnvSecRe = recEnvSecRe;

            var cjEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 28);
            tituloData.CopiaJudicialEnvSec = cjEnvSec;
            var cjEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 28);
            tituloData.CopiaJudicialEnvSecRe = cjEnvSecRe;

            var fiEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 32);
            tituloData.FianzaEnvSec = fiEnvSec;
            var fiEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 32);
            tituloData.FianzaEnvSecRe = fiEnvSecRe;

            var picEnvSec = primerEnvSec.Count(e => e.TipoSolicitud == 34);
            tituloData.ProDeIn_y_ConEnvSec = picEnvSec;
            var picEnvSecRe = primerEnvSec.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 34);
            tituloData.ProDeIn_y_ConEnvSecRe = picEnvSecRe;

            var totalEnvSec = insEnvSec + adjuEnvSec + transEnvSec + reinsriEnvSec + duEnvSec + coEnvSec + perEnvSec + doEnvSec + usuEnvSec
                + preEnvSec + leEnvSec + cerEnvSec + infEnvSec + ifEnvSec + liEnvSec + aipEnvSec + emPEnvSec + epEnvSec + emEEnvSec + canEnvSec + CambDEnvSec + recEnvSec
                + cjEnvSec + fiEnvSec + picEnvSec;

            var totalEnvSecRe = insEnvSecRe + adjuEnvSecRe + transEnvSecRe + reinsriEnvSecRe + duEnvSecRe + coEnvSecRe + perEnvSecRe + doEnvSecRe + usuEnvSecRe
                + preEnvSecRe + leEnvSecRe + cerEnvSecRe + infEnvSecRe + ifEnvSecRe + liEnvSecRe + aipEnvSecRe + emPEnvSecRe + epEnvSecRe + emEEnvSecRe + canEnvSecRe + CambDEnvSecRe + recEnvSecRe
                + cjEnvSecRe + fiEnvSecRe + picEnvSecRe;

            var totalEnvSecRegional = totalEnvSec + totalEnvSecRe;

            tituloData.TotalEnvSec = totalEnvSec;
            tituloData.TotalEnvSecRe = totalEnvSecRe;
            tituloData.TotalEnvSecRegional = totalEnvSecRegional;

            #endregion

            #region TRABAJOS RECIBIDOS MESA SALIDA

            var querySaliReg = from movimiento in _dbContext.RmMovimientosDocs
                               join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                               //join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                               join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                               where movimiento.FechaOperacion <= parametros.FechaHasta &&
                                     movimiento.FechaOperacion >= parametros.FechaDesde
                                     && movimiento.EstadoEntrada == 38 &&
                                 movimiento.CodUsuario == parametros.Usuario
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

            var primerSaliReg = querySaliReg.ToList();

            var insSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 1 && e.ReIngreso == null);
            tituloData.InscripcionSaliReg = insSaliReg;
            var insSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 1);
            tituloData.InscripcionSaliRegRe = insSaliRegRe;

            var adjuSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 2 && e.ReIngreso == null);
            tituloData.AdjudicacionSaliReg = adjuSaliReg;
            var adjuSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 2);
            tituloData.AdjudicacionSaliRegRe = adjuSaliRegRe;

            var transSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 3 && e.ReIngreso == null);
            tituloData.TransferenciaSaliReg = transSaliReg;
            var transSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 3);
            tituloData.TransferenciaSaliRegRe = transSaliRegRe;

            var reinsriSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 4 && e.ReIngreso == null);
            tituloData.ReinscripcionSaliReg = reinsriSaliReg;
            var reinsriSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 4);
            tituloData.ReinscripcionSaliRegRe = reinsriSaliRegRe;

            var duSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 5 && e.ReIngreso == null);
            tituloData.DuplicadoSaliReg = duSaliReg;
            var duSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 5);
            tituloData.DuplicadoSaliRegRe = duSaliRegRe;

            var coSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 6 && e.ReIngreso == null);
            tituloData.CopiaSaliReg = coSaliReg;
            var coSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 6);
            tituloData.CopiaSaliRegRe = coSaliRegRe;

            var perSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 7 && e.ReIngreso == null);
            tituloData.PermutaSaliReg = perSaliReg;
            var perSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 7);
            tituloData.PermutaSaliRegRe = perSaliRegRe;

            var doSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 8 && e.ReIngreso == null);
            tituloData.DonacionSaliReg = doSaliReg;
            var doSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 8);
            tituloData.DonacionSaliRegRe = doSaliRegRe;

            var usuSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 9 && e.ReIngreso == null);
            tituloData.UsufructoSaliReg = usuSaliReg;
            var usuSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 9);
            tituloData.UsufructoSaliRegRe = usuSaliRegRe;

            var preSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 10 && e.ReIngreso == null);
            tituloData.PrendaSaliReg = preSaliReg;
            var preSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 10);
            tituloData.PrendaSaliRegRe = preSaliRegRe;

            var leSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 11 && e.ReIngreso == null);
            tituloData.LevantamientoSaliReg = leSaliReg;
            var leSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 11);
            tituloData.LevantamientoSaliRegRe = leSaliRegRe;

            var cerSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 12 || e.TipoSolicitud == 22);
            tituloData.CertificadoSaliReg = cerSaliReg;
            var cerSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22));
            tituloData.CertificadoSaliRegRe = cerSaliRegRe;

            var infSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 13 && e.ReIngreso == null);
            tituloData.InformeSaliReg = infSaliReg;
            var infSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 13);
            tituloData.InformeSaliRegRe = infSaliRegRe;

            var ifSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 14 && e.ReIngreso == null);
            tituloData.InformeJudicialSaliReg = ifSaliReg;
            var ifSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 14);
            tituloData.InformeJudicialSaliRegRe = ifSaliRegRe;

            var liSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 15 && e.ReIngreso == null);
            tituloData.LitisSaliReg = liSaliReg;
            var liSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 15);
            tituloData.LitisSaliRegRe = liSaliRegRe;

            var aipSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 16 && e.ReIngreso == null);
            tituloData.AnotacionDeInscripcionPreventivaSaliReg = aipSaliReg;
            var aipSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 16);
            tituloData.AnotacionDeInscripcionPreventivaSaliRegRe = aipSaliRegRe;

            var emPSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 17 && e.ReIngreso == null);
            tituloData.EmbargoPreventivoSaliReg = emPSaliReg;
            var emPSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 17);
            tituloData.EmbargoPreventivoSaliRegRe = emPSaliRegRe;

            var epSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 18 && e.ReIngreso == null);
            tituloData.DacionPagoSaliReg = epSaliReg;
            var epSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 18);
            tituloData.DacionPagoSaliRegRe = epSaliRegRe;

            var emESaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 19 && e.ReIngreso == null);
            tituloData.EmbargoEjecutivoSaliReg = emESaliReg;
            var emESaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 19);
            tituloData.EmbargoEjecutivoSaliRegRe = emESaliRegRe;

            var canSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 23 && e.ReIngreso == null);
            tituloData.CancelacionSaliReg = canSaliReg;
            var canSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 23);
            tituloData.CancelacionSaliRegRe = canSaliRegRe;

            var CambDSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 25 && e.ReIngreso == null);
            tituloData.CambioDenominacionSaliReg = CambDSaliReg;
            var CambDSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 25);
            tituloData.CambioDenominacionSaliRegRe = CambDSaliRegRe;

            var recSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 26 && e.ReIngreso == null);
            tituloData.RectificacionSaliReg = recSaliReg;
            var recSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 26);
            tituloData.RectificacionSaliRegRe = recSaliRegRe;

            var cjSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 28 && e.ReIngreso == null);
            tituloData.CopiaJudicialSaliReg = cjSaliReg;
            var cjSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 28);
            tituloData.CopiaJudicialSaliRegRe = cjSaliRegRe;

            var fiSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 32 && e.ReIngreso == null);
            tituloData.FianzaSaliReg = fiSaliReg;
            var fiSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 32);
            tituloData.FianzaSaliRegRe = fiSaliRegRe;

            var picSaliReg = primerSaliReg.Count(e => e.TipoSolicitud == 34 && e.ReIngreso == null);
            tituloData.ProDeIn_y_ConSaliReg = picSaliReg;
            var picSaliRegRe = primerSaliReg.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 34);
            tituloData.ProDeIn_y_ConSaliRegRe = picSaliRegRe;

            var totalSaliReg = insSaliReg + adjuSaliReg + transSaliReg + reinsriSaliReg + duSaliReg + coSaliReg + perSaliReg + doSaliReg + usuSaliReg
                + preSaliReg + leSaliReg + cerSaliReg + infSaliReg + ifSaliReg + liSaliReg + aipSaliReg + emPSaliReg + epSaliReg + emESaliReg + canSaliReg + CambDSaliReg + recSaliReg
                + cjSaliReg + fiSaliReg + picSaliReg;

            var totalSaliRegRe = insSaliRegRe + adjuSaliRegRe + transSaliRegRe + reinsriSaliRegRe + duSaliRegRe + coSaliRegRe + perSaliRegRe + doSaliRegRe + usuSaliRegRe
                + preSaliRegRe + leSaliRegRe + cerSaliRegRe + infSaliRegRe + ifSaliRegRe + liSaliRegRe + aipSaliRegRe + emPSaliRegRe + epSaliRegRe + emESaliRegRe + canSaliRegRe + CambDSaliRegRe + recSaliRegRe
                + cjSaliRegRe + fiSaliRegRe + picSaliRegRe;

            var totalSalidaRegional=totalSaliReg+totalSaliRegRe;

            tituloData.TotalSaliReg = totalSaliReg;
            tituloData.TotalSaliRegRe = totalSaliRegRe;
            tituloData.TotalSalidaRegional = totalSalidaRegional;

            #endregion

            #region TRABAJOS RETIRADOS

            var queryRetir = from movimiento in _dbContext.RmMovimientosDocs
                             join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                             //join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                             join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                             where movimiento.FechaOperacion <= parametros.FechaHasta &&
                                   movimiento.FechaOperacion >= parametros.FechaDesde
                                   && (movimiento.EstadoEntrada == 13 || movimiento.EstadoEntrada == 14 || movimiento.EstadoEntrada == 15) &&
                                 movimiento.CodUsuario == parametros.Usuario
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

            var primerRetir = queryRetir.ToList();

            var insRetir = primerRetir.Count(e => e.TipoSolicitud == 1 && e.ReIngreso == null);
            tituloData.InscripcionRetir = insRetir;
            var insRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 1);
            tituloData.InscripcionRetirRe = insRetirRe;

            var adjuRetir = primerRetir.Count(e => e.TipoSolicitud == 2 && e.ReIngreso == null);
            tituloData.AdjudicacionRetir = adjuRetir;
            var adjuRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 2);
            tituloData.AdjudicacionRetirRe = adjuRetirRe;

            var transRetir = primerRetir.Count(e => e.TipoSolicitud == 3 && e.ReIngreso == null);
            tituloData.TransferenciaRetir = transRetir;
            var transRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 3);
            tituloData.TransferenciaRetirRe = transRetirRe;

            var reinsriRetir = primerRetir.Count(e => e.TipoSolicitud == 4 && e.ReIngreso == null);
            tituloData.ReinscripcionRetir = reinsriRetir;
            var reinsriRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 4);
            tituloData.ReinscripcionRetirRe = reinsriRetirRe;

            var duRetir = primerRetir.Count(e => e.TipoSolicitud == 5 && e.ReIngreso == null);
            tituloData.DuplicadoRetir = duRetir;
            var duRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 5);
            tituloData.DuplicadoRetirRe = duRetirRe;

            var coRetir = primerRetir.Count(e => e.TipoSolicitud == 6 && e.ReIngreso == null);
            tituloData.CopiaRetir = coRetir;
            var coRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 6);
            tituloData.CopiaRetirRe = coRetirRe;

            var perRetir = primerRetir.Count(e => e.TipoSolicitud == 7 && e.ReIngreso == null);
            tituloData.PermutaRetir = perRetir;
            var perRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 7);
            tituloData.PermutaRetirRe = perRetirRe;

            var doRetir = primerRetir.Count(e => e.TipoSolicitud == 8 && e.ReIngreso == null);
            tituloData.DonacionRetir = doRetir;
            var doRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 8);
            tituloData.DonacionRetirRe = doRetirRe;

            var usuRetir = primerRetir.Count(e => e.TipoSolicitud == 9 && e.ReIngreso == null);
            tituloData.UsufructoRetir = usuRetir;
            var usuRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 9);
            tituloData.UsufructoRetirRe = usuRetirRe;

            var preRetir = primerRetir.Count(e => e.TipoSolicitud == 10 && e.ReIngreso == null);
            tituloData.PrendaRetir = preRetir;
            var preRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 10);
            tituloData.PrendaRetirRe = preRetirRe;

            var leRetir = primerRetir.Count(e => e.TipoSolicitud == 11 && e.ReIngreso == null);
            tituloData.LevantamientoRetir = leRetir;
            var leRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 11);
            tituloData.LevantamientoRetirRe = leRetirRe;

            var cerRetir = primerRetir.Count(e => e.TipoSolicitud == 12 || e.TipoSolicitud == 22);
            tituloData.CertificadoRetir = cerRetir;
            var cerRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22));
            tituloData.CertificadoRetirRe = cerRetirRe;

            var infRetir = primerRetir.Count(e => e.TipoSolicitud == 13 && e.ReIngreso == null);
            tituloData.InformeRetir = infRetir;
            var infRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 13);
            tituloData.InformeRetirRe = infRetirRe;

            var ifRetir = primerRetir.Count(e => e.TipoSolicitud == 14 && e.ReIngreso == null);
            tituloData.InformeJudicialRetir = ifRetir;
            var ifRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 14);
            tituloData.InformeJudicialRetirRe = ifRetirRe;

            var liRetir = primerRetir.Count(e => e.TipoSolicitud == 15 && e.ReIngreso == null);
            tituloData.LitisRetir = liRetir;
            var liRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 15);
            tituloData.LitisRetirRe = liRetirRe;

            var aipRetir = primerRetir.Count(e => e.TipoSolicitud == 16 && e.ReIngreso == null);
            tituloData.AnotacionDeInscripcionPreventivaRetir = aipRetir;
            var aipRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 16);
            tituloData.AnotacionDeInscripcionPreventivaRetirRe = aipRetirRe;

            var emPRetir = primerRetir.Count(e => e.TipoSolicitud == 17 && e.ReIngreso == null);
            tituloData.EmbargoPreventivoRetir = emPRetir;
            var emPRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 17);
            tituloData.EmbargoPreventivoRetirRe = emPRetirRe;

            var epRetir = primerRetir.Count(e => e.TipoSolicitud == 18 && e.ReIngreso == null);
            tituloData.DacionPagoRetir = epRetir;
            var epRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 18);
            tituloData.DacionPagoRetirRe = epRetirRe;

            var emERetir = primerRetir.Count(e => e.TipoSolicitud == 19 && e.ReIngreso == null);
            tituloData.EmbargoEjecutivoRetir = emERetir;
            var emERetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 19);
            tituloData.EmbargoEjecutivoRetirRe = emERetirRe;

            var canRetir = primerRetir.Count(e => e.TipoSolicitud == 23 && e.ReIngreso == null);
            tituloData.CancelacionRetir = canRetir;
            var canRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 23);
            tituloData.CancelacionRetirRe = canRetirRe;

            var CambDRetir = primerRetir.Count(e => e.TipoSolicitud == 25 && e.ReIngreso == null);
            tituloData.CambioDenominacionRetir = CambDRetir;
            var CambDRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 25);
            tituloData.CambioDenominacionRetirRe = CambDRetirRe;

            var recRetir = primerRetir.Count(e => e.TipoSolicitud == 26 && e.ReIngreso == null);
            tituloData.RectificacionRetir = recRetir;
            var recRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 26);
            tituloData.RectificacionRetirRe = recRetirRe;

            var cjRetir = primerRetir.Count(e => e.TipoSolicitud == 28 && e.ReIngreso == null);
            tituloData.CopiaJudicialRetir = cjRetir;
            var cjRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 28);
            tituloData.CopiaJudicialRetirRe = cjRetirRe;

            var fiRetir = primerRetir.Count(e => e.TipoSolicitud == 32 && e.ReIngreso == null);
            tituloData.FianzaRetir = fiRetir;
            var fiRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 32);
            tituloData.FianzaRetirRe = fiRetirRe;

            var picRetir = primerRetir.Count(e => e.TipoSolicitud == 34 && e.ReIngreso == null);
            tituloData.ProDeIn_y_ConRetir = picRetir;
            var picRetirRe = primerRetir.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 34);
            tituloData.ProDeIn_y_ConRetirRe = picRetirRe;

            var totalRetir = insRetir + adjuRetir + transRetir + reinsriRetir + duRetir + coRetir + perRetir + doRetir + usuRetir
                + preRetir + leRetir + cerRetir + infRetir + ifRetir + liRetir + aipRetir + emPRetir + epRetir + emERetir + canRetir + CambDRetir + recRetir
                + cjRetir + fiRetir + picRetir;

            var totalRetirRe = insRetirRe + adjuRetirRe + transRetirRe + reinsriRetirRe + duRetirRe + coRetirRe + perRetirRe + doRetirRe + usuRetirRe
                + preRetirRe + leRetirRe + cerRetirRe + infRetirRe + ifRetirRe + liRetirRe + aipRetirRe + emPRetirRe + epRetirRe + emERetirRe + canRetirRe + CambDRetirRe + recRetirRe
                + cjRetirRe + fiRetirRe + picRetirRe;

            var totalRetirado = totalRetir + totalRetirRe;

            tituloData.TotalRetir = totalRetir;
            tituloData.TotalRetirRe = totalRetirRe;
            tituloData.TotalRetirado = totalRetirado;

            #endregion

            #region TRABAJOS ENVIADOS TRIPLICADO

            var queryTrip = from movimiento in _dbContext.RmMovimientosDocs
                            join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                            //join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                            join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                            where movimiento.FechaOperacion <= parametros.FechaHasta &&
                                  movimiento.FechaOperacion >= parametros.FechaDesde
                                  && movimiento.EstadoEntrada == 29 &&
                                 movimiento.CodUsuario == parametros.Usuario
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

            var primerTrip = queryTrip.ToList();

            var insTrip = primerTrip.Count(e => e.TipoSolicitud == 1 && e.ReIngreso == null);
            tituloData.InscripcionTrip = insTrip;
            var insTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 1);
            tituloData.InscripcionTripRe = insTripRe;

            var adjuTrip = primerTrip.Count(e => e.TipoSolicitud == 2 && e.ReIngreso == null);
            tituloData.AdjudicacionTrip = adjuTrip;
            var adjuTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 2);
            tituloData.AdjudicacionTripRe = adjuTripRe;

            var transTrip = primerTrip.Count(e => e.TipoSolicitud == 3 && e.ReIngreso == null);
            tituloData.TransferenciaTrip = transTrip;
            var transTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 3);
            tituloData.TransferenciaTripRe = transTripRe;

            var reinsriTrip = primerTrip.Count(e => e.TipoSolicitud == 4 && e.ReIngreso == null);
            tituloData.ReinscripcionTrip = reinsriTrip;
            var reinsriTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 4);
            tituloData.ReinscripcionTripRe = reinsriTripRe;

            var duTrip = primerTrip.Count(e => e.TipoSolicitud == 5 && e.ReIngreso == null);
            tituloData.DuplicadoTrip = duTrip;
            var duTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 5);
            tituloData.DuplicadoTripRe = duTripRe;

            var coTrip = primerTrip.Count(e => e.TipoSolicitud == 6 && e.ReIngreso == null);
            tituloData.CopiaTrip = coTrip;
            var coTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 6);
            tituloData.CopiaTripRe = coTripRe;

            var perTrip = primerTrip.Count(e => e.TipoSolicitud == 7 && e.ReIngreso == null);
            tituloData.PermutaTrip = perTrip;
            var perTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 7);
            tituloData.PermutaTripRe = perTripRe;

            var doTrip = primerTrip.Count(e => e.TipoSolicitud == 8 && e.ReIngreso == null);
            tituloData.DonacionTrip = doTrip;
            var doTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 8);
            tituloData.DonacionTripRe = doTripRe;

            var usuTrip = primerTrip.Count(e => e.TipoSolicitud == 9 && e.ReIngreso == null);
            tituloData.UsufructoTrip = usuTrip;
            var usuTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 9);
            tituloData.UsufructoTripRe = usuTripRe;

            var preTrip = primerTrip.Count(e => e.TipoSolicitud == 10 && e.ReIngreso == null);
            tituloData.PrendaTrip = preTrip;
            var preTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 10);
            tituloData.PrendaTripRe = preTripRe;

            var leTrip = primerTrip.Count(e => e.TipoSolicitud == 11 && e.ReIngreso == null);
            tituloData.LevantamientoTrip = leTrip;
            var leTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 11);
            tituloData.LevantamientoTripRe = leTripRe;

            var cerTrip = primerTrip.Count(e => e.TipoSolicitud == 12 || e.TipoSolicitud == 22);
            tituloData.CertificadoTrip = cerTrip;
            var cerTripRe = primerTrip.Count(e => e.ReIngreso == "S" && (e.TipoSolicitud == 12 || e.TipoSolicitud == 22));
            tituloData.CertificadoTripRe = cerTripRe;

            var infTrip = primerTrip.Count(e => e.TipoSolicitud == 13 && e.ReIngreso == null);
            tituloData.InformeTrip = infTrip;
            var infTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 13);
            tituloData.InformeTripRe = infTripRe;

            var ifTrip = primerTrip.Count(e => e.TipoSolicitud == 14 && e.ReIngreso == null);
            tituloData.InformeJudicialTrip = ifTrip;
            var ifTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 14);
            tituloData.InformeJudicialTripRe = ifTripRe;

            var liTrip = primerTrip.Count(e => e.TipoSolicitud == 15 && e.ReIngreso == null);
            tituloData.LitisTrip = liTrip;
            var liTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 15);
            tituloData.LitisTripRe = liTripRe;

            var aipTrip = primerTrip.Count(e => e.TipoSolicitud == 16 && e.ReIngreso == null);
            tituloData.AnotacionDeInscripcionPreventivaTrip = aipTrip;
            var aipTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 16);
            tituloData.AnotacionDeInscripcionPreventivaTripRe = aipTripRe;

            var emPTrip = primerTrip.Count(e => e.TipoSolicitud == 17 && e.ReIngreso == null);
            tituloData.EmbargoPreventivoTrip = emPTrip;
            var emPTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 17);
            tituloData.EmbargoPreventivoTripRe = emPTripRe;

            var epTrip = primerTrip.Count(e => e.TipoSolicitud == 18 && e.ReIngreso == null);
            tituloData.DacionPagoTrip = epTrip;
            var epTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 18);
            tituloData.DacionPagoTripRe = epTripRe;

            var emETrip = primerTrip.Count(e => e.TipoSolicitud == 19 && e.ReIngreso == null);
            tituloData.EmbargoEjecutivoTrip = emETrip;
            var emETripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 19);
            tituloData.EmbargoEjecutivoTripRe = emETripRe;

            var canTrip = primerTrip.Count(e => e.TipoSolicitud == 23 && e.ReIngreso == null);
            tituloData.CancelacionTrip = canTrip;
            var canTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 23);
            tituloData.CancelacionTripRe = canTripRe;

            var CambDTrip = primerTrip.Count(e => e.TipoSolicitud == 25 && e.ReIngreso == null);
            tituloData.CambioDenominacionTrip = CambDTrip;
            var CambDTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 25);
            tituloData.CambioDenominacionTripRe = CambDTripRe;

            var recTrip = primerTrip.Count(e => e.TipoSolicitud == 26 && e.ReIngreso == null);
            tituloData.RectificacionTrip = recTrip;
            var recTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 26);
            tituloData.RectificacionTripRe = recTripRe;

            var cjTrip = primerTrip.Count(e => e.TipoSolicitud == 28 && e.ReIngreso == null);
            tituloData.CopiaJudicialTrip = cjTrip;
            var cjTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 28);
            tituloData.CopiaJudicialTripRe = cjTripRe;

            var fiTrip = primerTrip.Count(e => e.TipoSolicitud == 32 && e.ReIngreso == null);
            tituloData.FianzaTrip = fiTrip;
            var fiTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 32);
            tituloData.FianzaTripRe = fiTripRe;

            var picTrip = primerTrip.Count(e => e.TipoSolicitud == 34 && e.ReIngreso == null);
            tituloData.ProDeIn_y_ConTrip = picTrip;
            var picTripRe = primerTrip.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 34);
            tituloData.ProDeIn_y_ConTripRe = picTripRe;

            var totalTrip = insTrip + adjuTrip + transTrip + reinsriTrip + duTrip + coTrip + perTrip + doTrip + usuTrip
                + preTrip + leTrip + cerTrip + infTrip + ifTrip + liTrip + aipTrip + emPTrip + epTrip + emETrip + canTrip + CambDTrip + recTrip
                + cjTrip + fiTrip + picTrip;

            var totalTripRe = insTripRe + adjuTripRe + transTripRe + reinsriTripRe + duTripRe + coTripRe + perTripRe + doTripRe + usuTripRe
                + preTripRe + leTripRe + cerTripRe + infTripRe + ifTripRe + liTripRe + aipTripRe + emPTripRe + epTripRe + emETripRe + canTripRe + CambDTripRe + recTripRe
                + cjTripRe + fiTripRe + picTripRe;

            var totalTriplicado = totalTrip + totalTripRe;

            tituloData.TotalTrip = totalTrip;
            tituloData.TotalTripRe = totalTripRe;
            tituloData.TotalTriplicado = totalTriplicado;

            #endregion

            #region TRABAJOS FINALIZADOS

            var query1 = from movimiento in _dbContext.RmMovimientosDocs
                         join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                         join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                         join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                         where mesa.FechaSalida <= parametros.FechaHasta &&
                               mesa.FechaSalida >= parametros.FechaDesde
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


            var primer1 = query1.ToList();


            //PARA MOSTRAR EL TOTAL TRABAJOS FINALIZADOS INSCRIPCIONES
            var ins1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 1);

            //PARA MOSTRAR EL TOTAL TRABAJOS FINALIZADOS ADJUDICACION
            var adju1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 2);

            //PARA MOSTRAR EL TOTAL TRABAJOS FINALIZADOS TRANSFERENCIA
            var trans1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 3);

            //PARA MOSTRAR EL TOTAL TRABAJOS FINALIZADOS REINSCRIPCION
            var reinsri1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 4);

            //PARA MOSTRAR EL TOTAL TRABAJOS FINALIZADOS DUPLICADO
            var du1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 5);

            //PARA MOSTRAR EL TOTAL TRABAJOS FINALIZADOS COPIA
            var co1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 6);

            //PARA MOSTRAR EL TOTAL TRABAJOS FINALIZADOS PERMUTA
            var per1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 7);

            //PARA MOSTRAR EL TOTAL TRABAJOS FINALIZADOS DONACION
            var do1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 8);

            //PARA MOSTRAR EL TOTAL TRABAJOS USUFRUCTO
            var usu1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 9);

            //PARA MOSTRAR EL TOTAL TRABAJOS PRENDA
            var pre1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 10);

            //PARA MOSTRAR EL TOTAL TRABAJOS LEVANTAMIENTO
            var le1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 11);

            //PARA MOSTRAR EL TOTAL TRABAJOS CERTIFICADO
            var cer1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 12);
            //PARA MOSTRAR EL TOTAL TRABAJOS CERTIFICADO DE CONDOMINIO DE DOMINIO
            var cerCD1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 22);

            cerCD1 = cerCD1 + cer1;

            //PARA MOSTRAR EL TOTAL TRABAJOS INFORME
            var inf1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 13);

            //PARA MOSTRAR EL TOTAL TRABAJOS INFORME JUDICIAL
            var ij1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 14);

            //PARA MOSTRAR EL TOTAL TRABAJOS FINALIZADOS LITIS
            var li1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 15);

            //PARA MOSTRAR EL TOTAL TRABAJOS ANOTACION DE INSCRIPCION PREVENTIVO
            var aip1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 16);

            //PARA MOSTRAR EL TOTAL TRABAJOS EMBARGO PREVENTIVO
            var ep1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 17);

            //PARA MOSTRAR EL TOTAL TRABAJOS DACION EN PAGO
            var dp1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 18);

            //PARA MOSTRAR EL TOTAL TRABAJOS FINALIZADOS EMBARGO EJECUTIVO
            var Emb1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 19);



            //PARA MOSTRAR EL TOTAL TRABAJOS CANCELACION
            var can1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 23);

            //PARA MOSTRAR EL TOTAL TRABAJOS FINALIZADOS CAMBIO DENOMINACION
            var CambD1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 25);

            //PARA MOSTRAR EL TOTAL TRABAJOS RECTIFICACION
            var rec1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 26);

            //PARA MOSTRAR EL TOTAL TRABAJOS COPIA JUDICIAL
            var cj1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 28);

            //PARA MOSTRAR EL TOTAL TRABAJOS FINALIZADO FIANZA
            var fia1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 32);

            //PARA MOSTRAR EL TOTAL TRABAJOS FINALIZADOS PROHIBICION DE INNOVAR Y CONTRATAR
            var pic1 = primer1.Count(e => e.EstadoEntrada == 13 && e.TipoSolicitud == 34);

            var totalFinalizado = ins1 + adju1 + trans1 + reinsri1 + du1 + co1 + per1 + do1 + usu1 + pre1
                + le1 + cer1 + inf1 + ij1 + aip1 + li1 + ep1 + dp1 + Emb1 + can1 + CambD1 + rec1 + cj1 + fia1 + pic1;
            #endregion

            #region TRABAJOS OBSERVADOS

            var queryO = from movimiento in _dbContext.RmMovimientosDocs
                         join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                         join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                         join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                         where mesa.FechaSalida <= parametros.FechaHasta &&
                               mesa.FechaSalida >= parametros.FechaDesde
                               && movimiento.EstadoEntrada == 14
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


            var primerO = queryO.ToList();


            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS INSCRIPCION
            var insO = primerO.Count(e => e.TipoSolicitud == 1);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS ADJUDICACION
            var adjuO = primerO.Count(e => e.TipoSolicitud == 2);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS TRANSFERENCIA
            var transO = primerO.Count(e => e.TipoSolicitud == 3);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS REINSCRIPCION
            var reinsriO = primerO.Count(e => e.TipoSolicitud == 4);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS DUPLICADO
            var duO = primerO.Count(e => e.TipoSolicitud == 5);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVAODS COPIA
            var coO = primerO.Count(e => e.TipoSolicitud == 6);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS PERMUTA
            var perO = primerO.Count(e => e.TipoSolicitud == 7);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS DONACION
            var doO = primerO.Count(e => e.TipoSolicitud == 8 && e.ReIngreso == null);

            var usuO = primerO.Count(e => e.TipoSolicitud == 9 && e.ReIngreso == null);

            var preO = primerO.Count(e => e.TipoSolicitud == 10 && e.ReIngreso == null);

            var leO = primerO.Count(e => e.TipoSolicitud == 11 && e.ReIngreso == null);

            var cerO = primerO.Count(e => e.TipoSolicitud == 12 || e.TipoSolicitud == 22);

            var infO = primerO.Count(e => e.TipoSolicitud == 13 && e.ReIngreso == null);

            var ifO = primerO.Count(e => e.TipoSolicitud == 14 && e.ReIngreso == null);

            var liO = primerO.Count(e => e.TipoSolicitud == 15 && e.ReIngreso == null);

            var aipO = primerO.Count(e => e.TipoSolicitud == 16 && e.ReIngreso == null);

            var emPO = primerO.Count(e => e.TipoSolicitud == 17 && e.ReIngreso == null);

            var epO = primerO.Count(e => e.TipoSolicitud == 18 && e.ReIngreso == null);

            var emE = primerO.Count(e => e.TipoSolicitud == 19 && e.ReIngreso == null);

            var canO = primerO.Count(e => e.TipoSolicitud == 23 && e.ReIngreso == null);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS CAMBIO DENOMINACION
            var CambDO = primerO.Count(e => e.TipoSolicitud == 25 && e.ReIngreso == null);

            var recO = primerO.Count(e => e.TipoSolicitud == 26 && e.ReIngreso == null);
            var cjO = primerO.Count(e => e.TipoSolicitud == 28 && e.ReIngreso == null);
            var fiO = primerO.Count(e => e.TipoSolicitud == 32 && e.ReIngreso == null);
            var picO = primerO.Count(e => e.TipoSolicitud == 34 && e.ReIngreso == null);

            var totalObservado = insO + adjuO + transO + reinsriO + duO + coO + perO + doO + usuO
                + preO + leO + cerO + infO + ifO + liO + aipO + emPO + epO + emE + canO + CambDO + recO
                + cjO + fiO + picO;

            #endregion

            #region TRABAJOS NOTA NEGATIVA

            var query2 = from movimiento in _dbContext.RmMovimientosDocs
                         join mesa in queryMesaEntrada on movimiento.NroEntrada equals mesa.NumeroEntrada
                         join transaccion in _dbContext.RmTransacciones on mesa.NumeroEntrada equals transaccion.NumeroEntrada
                         join oficina in _dbContext.RmOficinasRegistrales on mesa.CodigoOficina equals oficina.CodigoOficina
                         where mesa.FechaSalida <= parametros.FechaHasta &&
                               mesa.FechaSalida >= parametros.FechaDesde
                               && movimiento.EstadoEntrada == 15
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


            var primer2 = query2.ToList();


            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS INSCRIPCION
            var ins2 = primer2.Count(e => e.TipoSolicitud == 1);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS ADJUDICACION
            var adju2 = primer2.Count(e => e.TipoSolicitud == 2);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS TRANSFERENCIA
            var trans2 = primer2.Count(e => e.TipoSolicitud == 3);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS REINSCRIPCION
            var reinsri2 = primer2.Count(e => e.TipoSolicitud == 4);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS DUPLICADO
            var du2 = primer2.Count(e => e.TipoSolicitud == 5);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS COPIA
            var co2 = primer2.Count(e => e.TipoSolicitud == 6);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS PERMUTA
            var per2 = primer2.Count(e => e.TipoSolicitud == 7);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS DONACION
            var do2 = primer2.Count(e => e.TipoSolicitud == 8 && e.ReIngreso == null);

            var usu2 = primer2.Count(e => e.TipoSolicitud == 9 && e.ReIngreso == null);

            var pre2 = primer2.Count(e => e.TipoSolicitud == 10 && e.ReIngreso == null);

            var le2 = primer2.Count(e => e.TipoSolicitud == 11 && e.ReIngreso == null);

            var cer2 = primer2.Count(e => e.TipoSolicitud == 12 || e.TipoSolicitud == 22);

            var inf2 = primer2.Count(e => e.TipoSolicitud == 13 && e.ReIngreso == null);

            var if2 = primer2.Count(e => e.TipoSolicitud == 14 && e.ReIngreso == null);

            var li2 = primer2.Count(e => e.TipoSolicitud == 15 && e.ReIngreso == null);

            var aip2 = primer2.Count(e => e.TipoSolicitud == 16 && e.ReIngreso == null);

            var emP2 = primer2.Count(e => e.TipoSolicitud == 17 && e.ReIngreso == null);

            var ep2 = primer2.Count(e => e.TipoSolicitud == 18 && e.ReIngreso == null);

            var emE2 = primer2.Count(e => e.TipoSolicitud == 19 && e.ReIngreso == null);

            var can2 = primer2.Count(e => e.TipoSolicitud == 23 && e.ReIngreso == null);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS CAMBIO DENOMINACION
            var CambD2 = primer2.Count(e => e.TipoSolicitud == 25 && e.ReIngreso == null);

            var rec2 = primer2.Count(e => e.TipoSolicitud == 26 && e.ReIngreso == null);
            var cj2 = primer2.Count(e => e.TipoSolicitud == 28 && e.ReIngreso == null);
            var fi2 = primer2.Count(e => e.TipoSolicitud == 32 && e.ReIngreso == null);
            var pic2 = primer2.Count(e => e.TipoSolicitud == 34 && e.ReIngreso == null);

            var totalNotaNegativa = ins2 + adju2 + trans2 + reinsri2 + du2 + co2 + per2 + do2 + usu2
                + pre2 + le2 + cer2 + inf2 + if2 + li2 + aip2 + emP2 + ep2 + emE2 + can2 + CambD2 + rec2
                + cj2 + fi2 + pic2;


            #endregion

            // Asigna los valores de FechaActual y TotalIngresados desde el objeto parametros
            tituloData.FechaActual = DateTime.Now;
            tituloData.Usuario = parametros.Usuario;
            tituloData.NombreUsuario = parametros.NombreUsuario;

            return tituloData;
        }

    }
}
