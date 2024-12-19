using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Windows.Compatibility;

namespace SistemaBase.Controllers
{
    [Authorize]
    public class EntradasOnlineController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public EntradasOnlineController( DbvinDbContext context)
        {
            _dbContext = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMREO", "Index", "EntradasOnline" })]

        public async Task<IActionResult> Index()
        {
            
                var recepcionDis = (from me in _dbContext.RmMesaEntrada
                                    join ee in _dbContext.RmEstadosEntrada on me.EstadoEntrada equals ee.CodigoEstado
                                    join ts in _dbContext.RmTipoSolicituds on me.TipoSolicitud equals ts.TipoSolicitud
                                    join of in _dbContext.RmOficinasRegistrales on me.CodigoOficina equals of.CodigoOficina
                                    where ee.DescripEstado == "Entrada Online"
                                    orderby me.FechaEntrada descending
                                    select new Online()
                                    {
                                        NroEntrada = Convert.ToInt32(me.NumeroEntrada),
                                        DescTipoSolicitud = ts.DescripSolicitud,
                                        DescOficina = of.DescripOficina,
                                        FechaEntrada = me.FechaEntrada,
                                        ArchivoPDF = me.ArchivoPDF,
                                        AnexoPDF = me.AnexoPDF
                                    });
                return View(await recepcionDis.AsNoTracking().ToListAsync());
          
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(decimal nroEntrada)
        {

            var mesaEntrada = await _dbContext.RmMesaEntrada.FindAsync(nroEntrada);
            var entradaCapital = await _dbContext.RmEstadosEntrada.Where(p => p.DescripEstado == "Mesa entrada").FirstOrDefaultAsync();
            var entradaInterior = await _dbContext.RmEstadosEntrada.Where(p => p.DescripEstado == "Entrada Div Regional").FirstOrDefaultAsync();
            if (mesaEntrada == null)
            {
                // Manejo si la mesaEntrada no se encuentra
                return NotFound();
            }
            else
            {
                if (mesaEntrada.CodigoOficina == 1)
                {
                    mesaEntrada.EstadoEntrada = entradaCapital.CodigoEstado;
                    _dbContext.Update(mesaEntrada);
                }
                else
                {
                    mesaEntrada.EstadoEntrada = entradaInterior.CodigoEstado;
                    _dbContext.Update(mesaEntrada);

                }

                await _dbContext.SaveChangesAsync();
                RmMovimientosDoc movimientos = new()
                {
                    NroEntrada = nroEntrada,
                    CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                    FechaOperacion = DateTime.Now,
                    CodOperacion = "09", //parametro para cambio de estado 
                    NroMovimientoRef = nroEntrada.ToString(),
                    EstadoEntrada = mesaEntrada.EstadoEntrada
                };
                await _dbContext.AddAsync(movimientos);
                await _dbContext.SaveChangesAsync();
            }

            return View("Index");
        }
        public IActionResult DescargarPDF(int numeroEntrada)
        {
            var mesaEntrada = _dbContext.RmMesaEntrada.SingleOrDefault(me => me.NumeroEntrada == numeroEntrada);

            if (mesaEntrada != null && mesaEntrada.ArchivoPDF != null && mesaEntrada.ArchivoPDF.Length > 0)
            {
                return File(mesaEntrada.ArchivoPDF, "application/pdf", $"Entrada_{numeroEntrada}.pdf");
            }

            return NotFound();
        }

        public IActionResult AbrirPDF(int numeroEntrada)
        {
            var mesaEntrada = _dbContext.RmMesaEntrada.SingleOrDefault(me => me.NumeroEntrada == numeroEntrada);

            if (mesaEntrada != null && mesaEntrada.ArchivoPDF != null && mesaEntrada.ArchivoPDF.Length > 0)
            {
                // Retorna el archivo PDF como FileResult
                return File(mesaEntrada.ArchivoPDF, "application/pdf");
            }

            return NotFound();
        }
        public IActionResult AbrirAnexoPDF(int numeroEntrada)
        {
            var mesaEntrada = _dbContext.RmMesaEntrada.SingleOrDefault(me => me.NumeroEntrada == numeroEntrada);

            if (mesaEntrada != null && mesaEntrada.AnexoPDF != null && mesaEntrada.AnexoPDF.Length > 0)
            {
                // Retorna el archivo PDF como FileResult
                return File(mesaEntrada.AnexoPDF, "application/pdf");
            }

            return NotFound();
        }
        public IActionResult AbrirArchivo(string nombreArchivo)
        {
            var rutaAlmacenamiento = Path.Combine(Directory.GetCurrentDirectory(), "ArchivosPDF", nombreArchivo);

            // Verifica que el archivo exista
            if (System.IO.File.Exists(rutaAlmacenamiento))
            {
                // Retorna el archivo como un FileResult
                return File(System.IO.File.ReadAllBytes(rutaAlmacenamiento), "application/pdf");
            }

            // Si el archivo no existe, retorna un NotFound
            return NotFound();
        }


        [HttpPost]
        public IActionResult GenerarPdf(decimal nroEntrada)
        {
            try
            {
                var mesaSalida = GetEntradaPorLiquidacionData(nroEntrada);


                //var rmMesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m=>m.NumeroEntrada == mesaEntrada.NumeroEntrada);

                //rmMesaEntrada.Impreso = "S";
                //_dbContext.Update(rmMesaEntrada);
                //_dbContext.SaveChanges();
                // Lee la imagen y conviértela en una cadena de datos URI codificada en base64
                string imagePath = "wwwroot/assets/img/PJ/PoderJudicial.png"; // Ruta a la imagen en tu proyecto
                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

                // Pasa la cadena de datos URI a la vista a través del modelo de vista
                mesaSalida.ImageDataUri = imageDataUri;

                // Renderizar la vista Razor a una cadena HTML
                string viewHtml = RenderViewToString("TicketPDF", mesaSalida);

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

                // Generar el nombre del archivo PDF con el formato deseado
                string fileName = $"EntradaPorLiquidación-Nro{nroEntrada}-{DateTime.Now:dd-MM-yyyy}.pdf";

                // Devolver el PDF como un archivo descargable con el nuevo nombre
                return File(pdfBytes, "application/pdf", fileName);

            }
            catch (Exception ex)
            {
                // Manejar el error de generación de PDF de alguna manera
                return BadRequest("Error al generar el PDF: " + ex.Message);
            }
        }



        // Método para renderizar una vista Razor a una cadena HTML
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
        public EntradaPorLiquidacionCustom GetEntradaPorLiquidacionData(decimal nroEntrada)
        {
            
            var existingMesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(me => me.NumeroEntrada == nroEntrada);
            string nombreAutorizante = "";
            if (existingMesaEntrada.IdAutorizante!=null)
            {
                var autorizante = _dbContext.RmAutorizantes.FirstOrDefault(a=>a.IdAutorizante== existingMesaEntrada.IdAutorizante);
                if(autorizante!=null && autorizante.DescripAutorizante!="")
                {
                    nombreAutorizante = autorizante?.DescripAutorizante??"";
                }
            }

            EntradaPorLiquidacionCustom entradaPorLiquidacion = new()
            {
                NumeroEntrada = nroEntrada,
                Barcode = GetCodigoBarra(nroEntrada.ToString()),
                FechaEntrada = existingMesaEntrada?.FechaEntrada,
                Oficinas = GetOficinasRegistrales(),
                CodigoOficina = existingMesaEntrada?.CodigoOficina ?? 0,
                NroBoleta = existingMesaEntrada?.NroBoleta ?? "",
                NumeroLiquidacion = existingMesaEntrada?.NumeroLiquidacionLetras ?? "",
                MontoLiquidacion = existingMesaEntrada?.MontoLiquidacion ?? 0,
                TiposSolicitud = GetTiposSolicitud(),
                TipoSolicitud = existingMesaEntrada?.TipoSolicitud ?? 0,
                Autorizantes = GetAutorizantes(),
                NombreAutorizante = nombreAutorizante,
                NomTitular = existingMesaEntrada?.NomTitular ?? "",
                TipoDocumentoTitular = GetTiposDocumentos(),
                IdTipoDocumentoTitular = existingMesaEntrada?.TipoDocumentoTitular ?? "",
                NroDocumentoTitular = existingMesaEntrada?.NroDocumentoTitular ?? "",
                //EsPresentador = true,
                NombrePresentador = existingMesaEntrada?.NombrePresentador ?? "",
                DocumentoPresentador = GetTiposDocumentos(),
                TipoDocumentoPresentador = existingMesaEntrada?.TipoDocumentoPresentador ?? 0,
                NroDocumentoPresentador = existingMesaEntrada?.NroDocumentoPresentador ?? "",
               // Comentario = mesaEntrada?.Comentario ?? "",
                UsuarioEntrada = existingMesaEntrada?.UsuarioEntrada ?? User.Identity.Name
            };
            return entradaPorLiquidacion;

        }

        private string GetCodigoBarra(string nEntrada)
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.CODE_128; // Puedes cambiar el formato según tus necesidades
            barcodeWriter.Options = new ZXing.Common.EncodingOptions
            {
                Width = 275,
                Height = 75,
                PureBarcode = true
            };

            var barcodeBitmap = barcodeWriter.Write(nEntrada);

            using (var ms = new MemoryStream())
            {
                barcodeBitmap.Save(ms, ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return "data:image/png;base64," + base64String;
            }
        }

        private List<SelectListItem> GetOficinasRegistrales()
        {
            using var dbContext = new DbvinDbContext();

            var oficinas = dbContext.RmOficinasRegistrales
                .Select(o => new SelectListItem
                {
                    Text = $"{o.DescripOficina}",
                    Value = o.CodigoOficina.ToString()
                })
                .ToList();

            return oficinas;
        }

        private List<SelectListItem> GetTiposSolicitud()
        {
            using var dbContext = new DbvinDbContext();

            var tiposSolicitud = dbContext.RmTipoSolicituds
                .Select(t => new SelectListItem
                {
                    Text = $"{t.DescripSolicitud}",
                    Value = t.TipoSolicitud.ToString()
                })
                .ToList();

            return tiposSolicitud;
        }

        private List<SelectListItem> GetTiposDocumentos()
        {
            using var dbContext = new DbvinDbContext();

            var tiposDocumentos = dbContext.RmTiposDocumentos
                .Select(d => new SelectListItem
                {
                    Text = $"{d.DescripTipoDocumento}",
                    Value = d.TipoDocumento.ToString()
                })
                .ToList();

            return tiposDocumentos;
        }

        private List<SelectListItem> GetAutorizantes()
        {
            using var dbContext = new DbvinDbContext();

            var autorizante = dbContext.RmAutorizantes
                .Select(d => new SelectListItem
                {
                    Text = $"{d.DescripAutorizante}",
                    Value = d.IdAutorizante.ToString()
                })
                .ToList();

            return autorizante;
        }


    }
}
