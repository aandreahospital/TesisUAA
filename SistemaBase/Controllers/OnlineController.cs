using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using System.Data;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using ZXing;
using ZXing.Windows.Compatibility;

namespace SistemaBase.Controllers
{
    [Authorize]
    [AllowAnonymous]
    public class OnlineController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        private readonly DbPercepcionesContext _dbPercepContext;
        public OnlineController(DbPercepcionesContext percepContex, DbvinDbContext context) 
        {
            _dbContext = context;
            _dbPercepContext = percepContex;
        }
        public async Task<IActionResult> Index()
        {
            //var nroEntrada = _dbContext.RmMesaEntrada.Max(m => m.NumeroEntrada) + 1;
            ViewBag.Oficina = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina");
            ViewBag.TipoSolicitud = new SelectList(_dbContext.RmTipoSolicituds, "TipoSolicitud", "DescripSolicitud");
            //ViewBag.Autorizante = new SelectList(_dbContext.RmAutorizantes, "IdAutorizante", "DescripAutorizante");
            ViewBag.TipoDocumento = new SelectList(_dbContext.RmTiposDocumentos, "TipoDocumento", "DescripTipoDocumento");
            ViewBag.Documento = new SelectList(_dbContext.RmTiposDocumentos, "TipoDocumento", "DescripTipoDocumento");
            Online EntradaOnline = new Online();
            //EntradaOnline.NumeroEntrada = nroEntrada;
            return View(await Task.FromResult(EntradaOnline));
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(Online mesaEntrada, IFormFile ArchivoPDF, IFormFile AnexoPDF)
        {
            try
            {
                //if (mesaEntrada.NumeroLiquidacion!=null)
                //{
                    var parametroNroDocumento = new SqlParameter("@codigo", SqlDbType.VarChar)
                    {
                        Value = mesaEntrada.NumeroLiquidacion
                    };

                    var results = await _dbPercepContext.ObtenerLiquidacionMarcasSenales
                        .FromSqlRaw("EXEC PS_ObtenerLiquidacionMarcasSenales @codigo", parametroNroDocumento)
                        .ToListAsync();

                    var result = results.FirstOrDefault();
                    //var parametro1 = new SqlParameter("@codigo", NroLiquidacion);
                    //var results = _dbPercepContext.ObtenerLiquidacionMarcasSenales
                    //.FromSqlRaw("EXEC PS_ObtenerLiquidacionMarcasSenales @codigo", parametro1).ToList();

                    var totalLiquidacion = result?.totalLiquidacion ?? 0;
                    var montoLiquidacion = result?.totalLiquidacion ?? 0;
                    var tipoSolicitud = result?.conceptoId ?? 0;
                    var tipoFiscalizacion = result?.IdTipoFiscalizacion ?? 0;
                    var nroSolicitante = result?.nroDocumentoSolicitante ?? "";
                    var tipoDocSolicitante = result?.tipoDocumentoSolicitante ?? "";
                    var nombreSolicitante = result?.nombreSolicitante ?? "";
                    var estadoLiquidacion = result?.estadoLiquidacion ?? "";
                    var matriculaProfe = result?.matriculaProfesional ?? "0";
                    var nombreProfe = result?.nombreProfesional ?? "";
                    var codigo = result?.codigo ?? "";

                    var idMesaEntrada = _dbContext.RmMesaEntrada.Max(m => m.NumeroEntrada) + 1;
                    var existingLiquidacion = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroLiquidacionLetras == mesaEntrada.NumeroLiquidacion);
                    if (existingLiquidacion != null)
                    {
                        return Json(new { Success = false, ErrorMessage = "Error, la liquidacion " + codigo + " ya se encuentra registrado" });
                    }
                    var listaConceptos = new List<decimal> { 1074, 1308, 602, 601 };
                    if (!listaConceptos.Contains(tipoSolicitud))
                    {
                        return Json(new { Success = false, ErrorMessage = "Error, la liquidacion no tiene el concepto requerido para este tramite" });
                    }
                    if (estadoLiquidacion != "Fiscalizado electrónicamente")
                    {
                        return Json(new { Success = false, ErrorMessage = "Error, la liquidacion no se encuentra fiscalizado" });
                    }
                    if (tipoFiscalizacion != 156)
                    {
                        return Json(new { Success = false, ErrorMessage = "Error, la liquidacion no corresponde al tipo de fiscalizacion para este tramite" });
                    }
                //}
              
                var idAuto = mesaEntrada?.NombreAutorizante??"";
                var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(a => a.DescripAutorizante == idAuto);

                if (idAuto != "")
                {
                    if (autorizante == null)
                    {
                        RmAutorizante rmAutorizante = new()
                        {
                            DescripAutorizante = idAuto
                        };
                        _dbContext.Add(rmAutorizante);
                        await _dbContext.SaveChangesAsync();
                        autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(a => a.DescripAutorizante == idAuto);
                    }
                }
                if (mesaEntrada != null)
                {
                    var estadoEntrada = await _dbContext.RmEstadosEntrada.FirstOrDefaultAsync(p => p.DescripEstado == "Entrada Online");
                    RmMesaEntradum rmMesaEntrada = new()
                    {
                        NumeroLiquidacionLetras = mesaEntrada.NumeroLiquidacion,
                        MontoLiquidacion = mesaEntrada.MontoLiquidacion,
                        EstadoEntrada = estadoEntrada?.CodigoEstado,
                        UsuarioEntrada = "USERONLINE",
                        TipoSolicitud = mesaEntrada.TipoSolicitud,
                        TipoDocumentoTitular = mesaEntrada.IdTipoDocumentoTitular,
                        NroDocumentoTitular = mesaEntrada.NroDocumentoTitular,
                        NomTitular = mesaEntrada.NomTitular,
                        TipoDocumentoPresentador = mesaEntrada.TipoDocumentoPresentador,
                        NroDocumentoPresentador = mesaEntrada.NroDocumentoPresentador,
                        NombrePresentador = mesaEntrada.NombrePresentador,
                        CodigoOficina = mesaEntrada.CodigoOficina,
                        CodOficinaRetiro = mesaEntrada.CodigoOficina,
                        IdAutorizante = autorizante?.IdAutorizante
                    };
                    const int maxFileSize = 1000 * 1024;
                    if (ArchivoPDF == null || ArchivoPDF.Length == 0)
                    {
                        return Json(new { Success = false, ErrorMessage = "Por favor, selecciona una liquidacion en PDF" });
                        //ModelState.AddModelError("ArchivoPDF", "Por favor, selecciona un archivo PDF.");
                        //return View();  // Reemplaza "View()" con la vista a la que deseas redirigir
                    }

                    //if (AnexoPDF == null || AnexoPDF.Length == 0)
                    //{
                    //    return Json(new { Success = false, ErrorMessage = "Por favor, selecciona un archivo anexo en PDF" });
                    //    //ModelState.AddModelError("ArchivoPDF", "Por favor, selecciona un archivo PDF.");
                    //    //return View();  // Reemplaza "View()" con la vista a la que deseas redirigir
                    //}

                    // Verifica la extensión del archivo
                    var allowedExtensions = new[] { ".pdf" };
                    if (ArchivoPDF != null && ArchivoPDF.Length > 0)
                    {
                       
                        var fileExtension = Path.GetExtension(ArchivoPDF.FileName).ToLower();
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            return Json(new { Success = false, ErrorMessage = "Error, la liquidacion debe ser en PDF." });
                            //ModelState.AddModelError("ArchivoPDF", "Solo se permiten archivos PDF.");
                            //return View();  // Reemplaza "View()" con la vista a la que deseas redirigir
                        }
                        if (ArchivoPDF.Length > maxFileSize)
                        {
                            return Json(new { Success = false, ErrorMessage = "El tamaño del archivo PDF no debe ser mayor de 1000 KB" });
                        }
                       
                        //Para guardar en la base de datos el archivo pdf
                        using (var memoryStream = new MemoryStream())
                        {
                            // Copia el contenido del archivo a un MemoryStream
                            await ArchivoPDF.CopyToAsync(memoryStream);

                            // Asigna el contenido del archivo al campo ArchivoPDF del modelo
                            rmMesaEntrada.ArchivoPDF = memoryStream.ToArray();
                        }

                        ////En caso de almacenar solo la ruta y el arhivo que sea en almacenamiento externo
                        //// Configura la ruta de almacenamiento (puedes ajustar la ruta según tu estructura)
                        //var rutaAlmacenamiento = Path.Combine(Directory.GetCurrentDirectory(), "ArchivosPDF");

                        //// Verifica si el directorio de almacenamiento existe, si no, créalo
                        //if (!Directory.Exists(rutaAlmacenamiento))
                        //{
                        //    Directory.CreateDirectory(rutaAlmacenamiento);
                        //}

                        //// Construye la ruta completa del archivo
                        //var nombreArchivo = $"{Guid.NewGuid().ToString()}.pdf";
                        //var rutaCompleta = Path.Combine(rutaAlmacenamiento, nombreArchivo);

                        //// Guarda el archivo físicamente en el sistema de archivos
                        //using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                        //{
                        //    await ArchivoPDF.CopyToAsync(stream);
                        //}
                        //rmMesaEntrada.RutaPdf = nombreArchivo;
                    }
                    if (AnexoPDF != null && AnexoPDF.Length > 0)
                    {
                        var Extension = Path.GetExtension(AnexoPDF.FileName).ToLower();
                        if (!allowedExtensions.Contains(Extension))
                        {
                            return Json(new { Success = false, ErrorMessage = "Error, el archivo anexo debe ser en PDF." });
                            //ModelState.AddModelError("ArchivoPDF", "Solo se permiten archivos PDF.");
                            //return View();  // Reemplaza "View()" con la vista a la que deseas redirigir
                        }
                        if (AnexoPDF.Length > maxFileSize)
                        {
                            return Json(new { Success = false, ErrorMessage = "El tamaño del archivo anexo PDF no debe ser mayor de 1000 KB" });
                        }
                        //Para guardar en la base de datos el archivo pdf
                        using (var memoryStream = new MemoryStream())
                        {
                            // Copia el contenido del archivo a un MemoryStream
                            await AnexoPDF.CopyToAsync(memoryStream);

                            // Asigna el contenido del archivo al campo ArchivoPDF del modelo
                            rmMesaEntrada.AnexoPDF = memoryStream.ToArray();
                        }
                    }
                    DateTime fechaActual =  DateTime.Now;
                    DateTime limiteHora = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day, 13, 0, 0);
                    DateTime primeraHora = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day, 7, 0, 0);

                    int contadorEntradasFueraDeHorario = _dbContext.RmMesaEntrada.Where(m=>m.FechaEntrada> fechaActual).Count(); // Implementa la lógica para obtener el contador

                    if (fechaActual > limiteHora)
                    {
                        // La fecha actual es mayor a la hora límite o menor a la primera hora
                        // Modificar la fecha a las 07:00 del día siguiente
                        fechaActual = fechaActual.AddDays(1).Date.AddHours(7);

                        // Incrementar el contador de entradas fuera de horario
                        //contadorEntradasFueraDeHorario++;

                        // Agregar el tiempo adicional según el contador (aquí he asumido 5 minutos por entrada fuera de horario)
                        fechaActual = fechaActual.AddMinutes(contadorEntradasFueraDeHorario);
                        rmMesaEntrada.FechaEntrada = fechaActual;
                    }else 
                    if (fechaActual < primeraHora)
                    {
                        // La fecha actual es mayor a la hora límite o menor a la primera hora
                        // Modificar la fecha a las 07:00 del día siguiente
                        fechaActual = fechaActual.Date.AddHours(7);

                        // Incrementar el contador de entradas fuera de horario
                        //contadorEntradasFueraDeHorario++;

                        // Agregar el tiempo adicional según el contador (aquí he asumido 5 minutos por entrada fuera de horario)
                        fechaActual = fechaActual.AddMinutes(contadorEntradasFueraDeHorario);
                        rmMesaEntrada.FechaEntrada = fechaActual;
                    }
                    else
                    {
                        rmMesaEntrada.FechaEntrada = DateTime.Now;
                    }

                    _dbContext.Add(rmMesaEntrada);
                    await _dbContext.SaveChangesAsync();

                    var nroEntrada = _dbContext.RmMesaEntrada.Max(m => m.NumeroEntrada);
                    var existente = _dbContext.RmMesaEntrada.Where(m=>m.UsuarioEntrada== "USERONLINE").FirstOrDefault(m=>m.NumeroEntrada== nroEntrada);

                    RmMovimientosDoc movimientos = new()
                    {
                        NroEntrada = existente.NumeroEntrada,
                        CodUsuario = "USERONLINE",
                        FechaOperacion = existente.FechaEntrada,
                        CodOperacion = "01",
                        NroMovimientoRef = existente.NumeroEntrada.ToString(),
                        EstadoEntrada = estadoEntrada?.CodigoEstado
                    };
                    await _dbContext.AddAsync(movimientos);
                    await _dbContext.SaveChangesAsync();

                    //if (mesaEntrada.NumeroLiquidacion != null)
                    //{
                        var parametroEntrada = new SqlParameter("@codigo", mesaEntrada.NumeroLiquidacion); // Valor de entrada
                        var origen = "ERP";
                        var parametroEntrada2 = new SqlParameter("@origen", origen);
                        var parametroSalida = new SqlParameter
                        {
                            ParameterName = "@codigoRetorno",
                            SqlDbType = SqlDbType.Int,
                            Direction = ParameterDirection.Output // Declarar como parámetro de salida
                        };
                        var parametroSalida2 = new SqlParameter
                        {
                            ParameterName = "@mensajeRetorno",
                            SqlDbType = SqlDbType.VarChar,
                            Size = 200,
                            Direction = ParameterDirection.Output // Declarar como parámetro de salida
                        };

                        _dbPercepContext.Database.ExecuteSqlRaw("EXEC PS_UtilizarLiquidacion @codigo, @origen,@codigoRetorno OUTPUT,@mensajeRetorno OUTPUT ", parametroEntrada, parametroEntrada2, parametroSalida2, parametroSalida);
                    //}
                }

                return Ok("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar/actualizar la mesa de entrada: " + ex.Message);
                return BadRequest("Error al agregar/actualizar la mesa de entrada: " + ex.Message);
            }
        }


        public async Task<IActionResult> GetLiquidacion(string NroLiquidacion)
        {
            try
            {
                //LLAMADA AL PROCEDIMIENTO ALMACENADO EN OTRA BASE DE DATOS.
                var parametroNroDocumento = new SqlParameter("@codigo", SqlDbType.VarChar)
                {
                    Value = NroLiquidacion
                };

                var results = await _dbPercepContext.ObtenerLiquidacionMarcasSenales
                    .FromSqlRaw("EXEC PS_ObtenerLiquidacionMarcasSenales @codigo", parametroNroDocumento)
                    .ToListAsync();

                var result = results.FirstOrDefault();
                //var parametro1 = new SqlParameter("@codigo", NroLiquidacion);
                //var results = _dbPercepContext.ObtenerLiquidacionMarcasSenales
                //.FromSqlRaw("EXEC PS_ObtenerLiquidacionMarcasSenales @codigo", parametro1).ToList();

                var totalLiquidacion = result?.totalLiquidacion ?? 0;
                var montoLiquidacion = result?.totalLiquidacion ?? 0;
                var tipoSolicitud = result?.conceptoId ?? 0;
                var tipoFiscalizacion = result?.IdTipoFiscalizacion ?? 0;
                var nroSolicitante = result?.nroDocumentoSolicitante ?? "";
                var tipoDocSolicitante = result?.tipoDocumentoSolicitante ?? "";
                var nombreSolicitante = result?.nombreSolicitante ?? "";
                var estadoLiquidacion = result?.estadoLiquidacion ?? "";
                var matriculaProfe = result?.matriculaProfesional ?? "0";
                var nombreProfe = result?.nombreProfesional ?? "";
                var codigo = result?.codigo ?? "";

                var idMesaEntrada = _dbContext.RmMesaEntrada.Max(m => m.NumeroEntrada) + 1;
                var mesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroLiquidacionLetras == NroLiquidacion);
                if (mesaEntrada != null)
                {
                    return Json(new { Success = false, ErrorMessage = "Error, la liquidacion " + NroLiquidacion + " ya se encuentra registrado" });
                }
                var listaConceptos = new List<decimal> { 1074, 1308, 602, 601 };
                if (!listaConceptos.Contains(tipoSolicitud))
                {
                    return Json(new { Success = false, ErrorMessage = "Error, la liquidacion no tiene el concepto requerido para este tramite" });
                }
                if (estadoLiquidacion != "Fiscalizado electrónicamente")
                {
                    return Json(new { Success = false, ErrorMessage = "Error, la liquidacion no se encuentra fiscalizado" });
                }
                if (tipoFiscalizacion != 156)
                {
                    return Json(new { Success = false, ErrorMessage = "Error, la liquidacion no corresponde al tipo de fiscalizacion para este tramite" });
                }
                ViewBag.TipoDocumento = new SelectList(_dbContext.RmTiposDocumentos, "TipoDocumento", "DescripTipoDocumento");
                ViewBag.Documento = new SelectList(_dbContext.RmTiposDocumentos, "TipoDocumento", "DescripTipoDocumento");
                ViewBag.Oficina = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina");
                Online entradaPorLiquidacion = new()
                {
                    NumeroEntrada = idMesaEntrada,
                    NumeroLiquidacion = codigo,
                    TotalBruto = totalLiquidacion,
                    Exoneracion = estadoLiquidacion,
                    MontoLiquidacion = Convert.ToDecimal(montoLiquidacion),
                    NombreAutorizante = nombreProfe,
                    MatriculaAutorizante = Convert.ToInt32(matriculaProfe),
                    NomTitular = nombreSolicitante,
                    IdTipoDocumentoTitular = tipoDocSolicitante,
                    NroDocumentoTitular = nroSolicitante

                };

                if (tipoSolicitud== 1074)
                {
                    entradaPorLiquidacion.TipoSolicitud = 5;
                    ViewBag.TipoSolicitud = new SelectList(_dbContext.RmTipoSolicituds, "TipoSolicitud", "DescripSolicitud", entradaPorLiquidacion.TipoSolicitud);
                }
                if (tipoSolicitud== 1308)
                {
                    entradaPorLiquidacion.TipoSolicitud = 1;
                    ViewBag.TipoSolicitud = new SelectList(_dbContext.RmTipoSolicituds, "TipoSolicitud", "DescripSolicitud", entradaPorLiquidacion.TipoSolicitud);

                }
                if (tipoSolicitud== 602)
                {
                    entradaPorLiquidacion.TipoSolicitud = 13;
                    ViewBag.TipoSolicitud = new SelectList(_dbContext.RmTipoSolicituds, "TipoSolicitud", "DescripSolicitud", entradaPorLiquidacion.TipoSolicitud);

                }
                if (tipoSolicitud== 601)
                {
                    entradaPorLiquidacion.TipoSolicitud = 22;
                    ViewBag.TipoSolicitud = new SelectList(_dbContext.RmTipoSolicituds, "TipoSolicitud", "DescripSolicitud", entradaPorLiquidacion.TipoSolicitud);

                }
                return View("Index", entradaPorLiquidacion);
                

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar/actualizar la mesa de entrada: " + ex.Message);
                return View("Index");


            }
        }
        [HttpPost]
        public IActionResult GenerarPdf(EntradaPorLiquidacionCustom mesaEntrada)
        {
            try
            {
                var mesaSalida = GetEntradaPorLiquidacionData(mesaEntrada);


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
                string fileName = $"EntradaPorLiquidación-Nro{mesaEntrada.NumeroEntrada}-{DateTime.Now:dd-MM-yyyy}.pdf";

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
        public EntradaPorLiquidacionCustom GetEntradaPorLiquidacionData(EntradaPorLiquidacionCustom mesaEntrada)
        {
            //var nroEntrada = _dbContext.RmMesaEntrada.Max(m=>m.NumeroEntrada);
            //var existingMesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(me => me.NumeroEntrada == mesaEntrada.NumeroEntrada);
            var ultimaEntrada = _dbContext.RmMesaEntrada.OrderByDescending(o => o.FechaEntrada).FirstOrDefault(m => m.UsuarioEntrada == "USERONLINE");
            //nroEntrada = ultimaEntrada.NumeroEntrada;
            mesaEntrada.FechaEntrada = ultimaEntrada.FechaEntrada;

         
                EntradaPorLiquidacionCustom entradaPorLiquidacion = new()
                {
                    NumeroEntrada = ultimaEntrada.NumeroEntrada,
                    Barcode = GetCodigoBarra(ultimaEntrada.NumeroEntrada.ToString()),
                    FechaEntrada = ultimaEntrada?.FechaEntrada,
                    Oficinas = GetOficinasRegistrales(),
                    CodigoOficina = mesaEntrada?.CodigoOficina ?? 0,
                    NroBoleta = mesaEntrada?.NroBoleta,
                    NumeroLiquidacion = mesaEntrada?.NumeroLiquidacion ?? "",
                    MontoLiquidacion = mesaEntrada?.MontoLiquidacion ?? 0,
                    TiposSolicitud = GetTiposSolicitud(),
                    TipoSolicitud = mesaEntrada?.TipoSolicitud ?? 0,
                    Autorizantes = GetAutorizantes(),
                    NombreAutorizante = mesaEntrada?.NombreAutorizante ?? "",
                    NomTitular = mesaEntrada?.NomTitular ?? "",
                    TipoDocumentoTitular = GetTiposDocumentos(),
                    IdTipoDocumentoTitular = mesaEntrada?.IdTipoDocumentoTitular ?? "",
                    NroDocumentoTitular = mesaEntrada?.NroDocumentoTitular ?? "",
                    //EsPresentador = true,
                    NombrePresentador = mesaEntrada?.NombrePresentador ?? "",
                    DocumentoPresentador = GetTiposDocumentos(),
                    TipoDocumentoPresentador = mesaEntrada?.TipoDocumentoPresentador ?? 0,
                    NroDocumentoPresentador = mesaEntrada?.NroDocumentoPresentador ?? "",
                    Comentario = mesaEntrada?.Comentario,
                    UsuarioEntrada = ultimaEntrada?.UsuarioEntrada
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
