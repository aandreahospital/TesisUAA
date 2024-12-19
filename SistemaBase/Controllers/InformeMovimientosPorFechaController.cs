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


namespace SistemaBase.Controllers
{
    public class InformeMovimientosPorFechaController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public InformeMovimientosPorFechaController(DbvinDbContext context)
        {
            _dbContext = context;
        }


        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMIMPF", "Index", "InformeMovimientosPorFecha" })]

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

            
            var queryMesaEntrada = _dbContext.RmMesaEntrada.AsQueryable();

            if(parametros.CodigoOficina== null)
            {
                 queryMesaEntrada = _dbContext.RmMesaEntrada.Where(m=>m.CodigoOficina!=1).AsQueryable();
            }
            else
            {
                queryMesaEntrada = _dbContext.RmMesaEntrada.Where(m => m.CodigoOficina ==parametros.CodigoOficina).AsQueryable();
            }



            var query = from mesa  in queryMesaEntrada
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
            var oi= primer.Count(e => e.TipoSolicitud == 1 && e.UsuarioEntrada == "USERONLINE");


            // Contar la cantidad de ADJUDICACION
            var adju = primer.Count(e => e.TipoSolicitud == 2 && e.ReIngreso == null);

            // Contar la cantidad de ADJUDICACION REINGRESO
            var adjuRe = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 2);

            // Contar la cantidad de TRANSFERENCIA
            var trans = primer.Count(e => e.TipoSolicitud == 3 && e.ReIngreso == null);

            // Contar la cantidad de TRANSFERENCIA REINGRESO
            var transre = primer.Count(e =>e.ReIngreso == "S" && e.TipoSolicitud == 3);

            // Contar la cantidad de REINSCRIPCION 
            var reinsri = primer.Count(e => e.TipoSolicitud == 4 && e.ReIngreso == null);

            // Contar la cantidad de REINSCRIPCION REINGRESO
            var reinsriRe = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 4);

            // Contar la cantidad de DUPLICADO
            var Du = primer.Count(e => e.TipoSolicitud == 5 && e.ReIngreso ==  null);
            //Contar la cantidad de DUPLICADO REINGRESO
            var DuRe = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 5);
            //Contar la cantidad de DUPLICADO ONLINE
            var odu = primer.Count(e => e.TipoSolicitud == 5 && e.UsuarioEntrada == "USERONLINE");

            // Contar la cantidad de COPIA
            var Co = primer.Count(e => e.TipoSolicitud == 6 && e.ReIngreso == null);
            //Contar la cantidad de COPIA REINGRESO
            var CoRe = primer.Count(e => e.ReIngreso == "S" && e.TipoSolicitud == 6);

            // Contar la cantidad de PERMUTA
            var Per = primer.Count(e => e.TipoSolicitud == 7 && e.ReIngreso== null);
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
                       + AIP + AIPR + EP + EPR + Dp + DpR + Emb + EmbRe+CerCD+CerCDR+ + Can + CanR
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
            var doO = primerO.Count(e => e.TipoSolicitud == 8);

            var usuO = primerO.Count(e => e.TipoSolicitud == 9);

            var preO = primerO.Count(e => e.TipoSolicitud == 10);

            var leO = primerO.Count(e => e.TipoSolicitud == 11);

            var cerO = primerO.Count(e => e.TipoSolicitud == 12 || e.TipoSolicitud==22);

            var infO = primerO.Count(e => e.TipoSolicitud == 13);

            var ifO = primerO.Count(e => e.TipoSolicitud == 14);

            var liO = primerO.Count(e => e.TipoSolicitud == 15);

            var aipO = primerO.Count(e => e.TipoSolicitud == 16);

            var emPO = primerO.Count(e => e.TipoSolicitud == 17);

            var epO = primerO.Count(e => e.TipoSolicitud == 18);

            var emE = primerO.Count(e => e.TipoSolicitud == 19);

            var canO = primerO.Count(e => e.TipoSolicitud == 23);

            //PARA MOSTRAR EL TOTAL TRABAJOS OBSERVADOS CAMBIO DENOMINACION
            var CambDO = primerO.Count(e => e.TipoSolicitud == 25);

            var recO = primerO.Count(e => e.TipoSolicitud == 26);
            var cjO = primerO.Count(e => e.TipoSolicitud == 28);
            var fiO = primerO.Count(e => e.TipoSolicitud == 32);
            var picO = primerO.Count(e => e.TipoSolicitud == 34);

            var totalObservado = insO + adjuO + transO + reinsriO + duO + coO + perO + doO + usuO 
                + preO + leO + cerO + infO + ifO + liO + aipO + emPO + epO + emE + canO + CambDO + recO 
                + cjO + fiO + picO;

            #endregion



            // Asigna los valores de FechaActual y TotalIngresados desde el objeto parametros
            parametros.FechaActual = DateTime.Now;
            parametros.Usuario = User.Identity.Name;

         

            Reportes tituloData = new()
            {
                //DescripOficina = DescOficina,
                FechaDesde = parametros.FechaDesde,
                FechaHasta = parametros.FechaHasta,
                FechaActual = DateTime.Now,
                Usuario = User.Identity.Name,
                //Ingresos y Reingreso
                Inscripcion = ins,
                InscripcionRe = insre,
                Adjudicacion = adju,
                AdjudicacionRe = adjuRe,
                Transferencia = trans,
                TransferenciaRe = transre,
                Reinscripcion = reinsri,
                ReinscripcionRe = reinsriRe,
                Duplicado = Du,
                DuplicadoRe = DuRe,
                Copia = Co,
                CopiaRe = CoRe,
                Permuta = Per,
                PermutaRe = PerRe,
                Donacion = Do,
                DonacionRe = DoRe,
                Usufructo = Usuf,
                UsufructoRe = UsufR,
                Prenda = Pre,
                PrendaRe = PreR,
                Levantamiento = Lev,
                LevantamientoRe = LevR,
                //Certificado = Cer,
                //CertificadoRe = CerE,
                Informe = In,
                InformeRe = InE,
                InformeJudicial = InJ,
                InformeJudicialRe = InJE,
                Litis = Li,
                LitisRe = LiRe,
                AnotacionDeInscripcionPreventiva = AIP,
                AnotacionDeInscripcionPreventivaRe = AIPR,
                EmbargoPreventivo = EP,
                EmbargoPreventivoRe = EPR,
                DacionPago = Dp,
                DacionPagoRe = DpR,
                EmbargoEjecutivo = Emb,
                EmbargoEjecutivoRe = EmbRe,
                CertificadoCondominioDominio=CerCD,
                CertificadoCondominioDominioR=CerCDR,
                Cancelacion = Can,
                CancelacionRe = CanR,
                CambioDenominacion = CamD,
                CambioDenominacionRe = CamDR,
                Rectificacion = Rec,
                RectificacionRe = RecR,
                CopiaJudicial = Cj,
                CopiaJudicialRe = CjR,
                Fianza = Fia,
                FianzaRe = FiaR,
                ProDeIn_y_Con = Pro,
                ProDeIn_y_ConRe = ProR,
                //Total Ingresados
                TotalIn = total,
                //Total Reingresos
                TotalRe = totalRe,
                //Total Sin reingresos
                TotalInscriptos = inscripTotal,

                //Trabajos finalizados propiedades
                Inscripcion1 = ins1,
                Adjudicacion1 = adju1,
                Transferencia1 = trans1,
                Reinscripcion1 = reinsri1,
                Duplicado1 = du1,
                Copia1 = co1,
                Permuta1 = per1,
                Donacion1 = do1,
                Usufructo1 = usu1,
                Prenda1 = pre1,
                Levantamiento1 = le1,
                //Certificado1 = cer1,
                Informe1 = inf1,
                InformeJudicial1 = ij1,
                Litis1 = li1,
                AnotacionInscripcionPreventiva1 = aip1,
                EmbargoPreventivo1 = ep1,
                DacionPago1 = dp1,
                EmbargoEjecutivo1 = Emb1,
                CertificadoCondominioDominio1 = cerCD1,
                Cancelacion1 = can1,
                CambioDenominacion1 = CambD1,
                Rectificacion1 = rec1,
                CopiaJudicial1 = cj1,
                Fianza1 = fia1,
                ProhibicionInnovarContratar1 = pic1,
                TotalF = totalFinalizado,
                //Trabajos Observados propiedades
                InscripcionO = insO,
                AdjudicacionO = adjuO,

                TransferenciaO = transO,
                ReinscripcionO = reinsriO,
                DuplicadoO = duO,
                CopiaO = coO,
                PermutaO = perO,
                DonacionO = doO,
                UsufructoO = usuO,
                PrendaO= preO,
                levantamientoO = leO,
                CertificadoO = cerO,
                InformeO = infO,
                InformeJudicialO = ifO,
                LitisO = liO,
                AnotacionInscripcionPreventivoO = aipO,
                EmbargoPreventivoO = emPO,
                DacionPagoO = epO,
                EmbargoEO = emE,
                CancelacionO = canO,
                CambioDO = CambDO,
                RectificacionO = recO,
                CopiaJudicialO = cjO,
                FianzaO = fiO,
                ProhibicionInnovarContratarO = picO,


                TotalO = totalObservado,

                //INGRESADOS ONLINE
                InscripcionOnline = oi,
                DuplicadoOnline = odu,
                CertificadoOnline = ocer,
                InformeOnline = oinf,

                TotalOnline = totalonline

            };

            if (parametros.CodigoOficina != null)
            {
                var Oficina = _dbContext.RmOficinasRegistrales.FirstOrDefault(o => o.CodigoOficina == parametros.CodigoOficina);

                tituloData.DescripOficina = Oficina.DescripOficina;

            }
            else
            {
                tituloData.DescripOficina = "REGIONALES";
            }
          

            ViewBag.Oficina = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina");

            return tituloData;
        }

    }
}
