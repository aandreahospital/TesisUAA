using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Html2pdf;
using iText.Commons.Actions.Contexts;
using ZXing;
using ZXing.Windows.Compatibility;
using ZXing.Common;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using System.Diagnostics.Metrics;
using iText.Html2pdf.Css.Apply.Impl;

namespace SistemaBase.Controllers
{
    public class PrendaController : Controller
    {
        #region Instancias
        // Esta instancia de DbContext representa una conexión a la base de datos DbvinDbContext.
        private readonly DbvinDbContext _context;
        #endregion
        
        #region Constructor
        public PrendaController(DbvinDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Index

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMMEDPRE", "Index","Prenda" })]

        public IActionResult Index()
        {
           
            ViewBag.Distrito = new SelectList(_context.RmDistritos.OrderBy(p=>p.DescripDistrito), "CodigoDistrito", "DescripDistrito", "1");

            return View();
        }
        #endregion

        #region Metodo Get
        // Este método se encarga de buscar y mostrar información relacionada con una entrada específica en la mesa de entrada.
        // Recibe el número de entrada (nEntrada) como parámetro.
        public async Task<IActionResult> Get(decimal nEntrada)
        {
            try
            {
                // Busca la entrada en la base de datos en función de su número de entrada.
                var mesaEntrada = await _context.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

                // Cargar información sobre los distritos para mostrar en la vista.

                if (mesaEntrada == null)
                {
                    return Json(new { Success = false, ErrorMessage = "El número de entrada no existe." });
                }
                var tipoSolicitud = mesaEntrada?.TipoSolicitud ?? 0;
                var listaConceptos = new List<decimal> { 10 };
                if (!listaConceptos.Contains(tipoSolicitud))
                {
                    return Json(new { Success = false, ErrorMessage = "Error, el tipo de solicitud no corresponde" });
                }
                //Si se encuentra la mesa de entrada:
                //Si también se encuentra la prenda, crea un objeto "PrendaCustom" con los detalles y muestra la vista "Index" con estos detalles.
                //Si no se encuentra la prenda, muestra la vista "Index" sin detalles de la prenda.
                if (mesaEntrada != null)
                {
                    var nroBoletaEn = mesaEntrada?.NroBoleta??"";
                    var boltaMarca = _context.RmBoletasMarcas.FirstOrDefault(b=>b.Descripcion== nroBoletaEn);
                    var nroBoleta = boltaMarca?.NroBoleta??0;
                    var rmTransaccion = await _context.RmTransacciones.FirstOrDefaultAsync(t=>t.NumeroEntrada == nEntrada);
                    //Busca información sobre la prenda asociada a la mesa de entrada.
                    var rmPrenda = await _context.RmMedidasPrendas.FirstOrDefaultAsync(p => p.NroEntrada == mesaEntrada.NumeroEntrada);

                    // Se busca la información de la prenda asociada a la mesa de entrada y la informacion en la tabla RmMarcasSenales.
                    var estadoRegistrador = _context.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Registrador");
                    var enrevision = await _context.RmEstadosEntrada.FirstOrDefaultAsync(e => e.DescripEstado == "En Revisión");
                    var transaccione = await _context.RmTransacciones.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                    var estadoRetiradoObs = _context.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Retirado/Observado");
                    var estadoRetiradoNN = _context.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Retirado/Nota negativa");
                    var estadoRetiradoAprob = _context.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Retirado/Aprobado");

                    if (transaccione != null)
                    {
                        if (estadoRegistrador.CodigoEstado != mesaEntrada.EstadoEntrada)
                        {
                            return Json(new { Success = false, ErrorMessage = "Error, debe recepcionar el trabajo" });
                        }
                        if ((mesaEntrada.Reingreso == "S" && estadoRetiradoAprob.CodigoEstado.ToString() != rmTransaccion.EstadoTransaccion) &&
                            (mesaEntrada.Reingreso == "S" && estadoRetiradoObs.CodigoEstado.ToString() != rmTransaccion.EstadoTransaccion) &&
                            (mesaEntrada.Reingreso == "S" && estadoRetiradoNN.CodigoEstado.ToString() != rmTransaccion.EstadoTransaccion))
                        {
                            return Json(new { Success = false, ErrorMessage = "Error, el trabajo ya se encuentra resgistrado" });
                        }

                    }
                    var reingreso = _context.RmReingresos.FirstOrDefault(r => r.NroEntrada == nEntrada);
                    //if (enrevision.CodigoEstado != mesaEntrada.EstadoEntrada && reingreso == null)
                    //{
                    //    return Json(new { Success = false, ErrorMessage = "Error, " + nEntrada + " ya se encuentra registrado" });
                    //}


                    if (rmPrenda != null)
                    {
                        //Obtiene detalles del distrito y el departamento asociados a la prenda.
                        var distrito = await _context.RmDistritos.FirstOrDefaultAsync(d => d.CodigoDistrito.ToString() == rmPrenda.CodDistrito);
                        var codDepto = distrito?.CodigoDepto??0;

                        var departamento = await _context.AvDepartamentos.FirstOrDefaultAsync(d => d.CodDepartamento == codDepto.ToString());
                        var transaccion = await _context.RmTransacciones.FirstOrDefaultAsync(t=>t.NumeroEntrada== nEntrada);
                        ViewBag.Distrito = new SelectList(_context.RmDistritos.OrderBy(p=>p.DescripDistrito), "CodigoDistrito", "DescripDistrito", rmPrenda?.CodDistrito??"");

                        PrendaCustom prenda = new()
                        {
                            Libro = rmPrenda?.Libro,
                            Folio = rmPrenda?.Folio,
                            NroEntrada = rmPrenda?.NroEntrada,
                            FechaInscripcion = rmPrenda?.FechaInscripcion,
                            FechaOperacion = rmPrenda?.FechaOperacion,
                            Acreedor = rmPrenda?.Acreedor,
                            Deudor = rmPrenda?.Deudor,
                            FechaVencimiento = rmPrenda?.FechaVencimiento,
                            MontoPrenda = rmPrenda?.MontoPrenda,
                            MontoDeJusticia = rmPrenda?.MontoDeJusticia,
                            NroBoleta = mesaEntrada?.NroBoleta,
                            NroBoletaSenal = rmPrenda?.NroBoletaSenal,
                            CodDistrito = rmPrenda?.CodDistrito,
                            Departamento = departamento?.Descripcion ?? "",
                            //Distrito = await GetDistritosAsync(),
                            Instrumento = rmPrenda?.Instrumento,
                            Comentario = transaccion?.Comentario
                        };
                        return View("Index", prenda);
                    }
                    else
                    {
                        ViewBag.Distrito = new SelectList(_context.RmDistritos, "CodigoDistrito", "DescripDistrito");

                        PrendaCustom prenda = new()
                        {
                            NroEntrada = mesaEntrada.NumeroEntrada,
                            NroBoleta = mesaEntrada?.NroBoleta ?? "",
                            //FechaInscripcion = rmTransaccion?.FechaAlta
                            //Distrito = await GetDistritosAsync(),
                            //Instrumento = rmPrenda.Instrumento
                        };
                        return View("Index", prenda);
                    }

                    //return View("Index");
                }
                //Si la mesa de entrada no se encuentra, devuelve un resultado "NotFound".

                else
                {
                    return NotFound();
                }


            }
            //Captura y lanza excepciones en caso de errores.

            catch (Exception ex)
            {

                throw new Exception("Ocurrió un error: " + ex.Message);

            }
        }
        public async Task<IActionResult> AddPrenda(decimal nEntrada)
        {
            try
            {
                // Busca la entrada en la base de datos en función de su número de entrada.
                var mesaEntrada = await _context.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

                // Cargar información sobre los distritos para mostrar en la vista.

                //Si se encuentra la mesa de entrada:
                //Si también se encuentra la prenda, crea un objeto "PrendaCustom" con los detalles y muestra la vista "Index" con estos detalles.
                //Si no se encuentra la prenda, muestra la vista "Index" sin detalles de la prenda.
                if (mesaEntrada != null)
                {
                    var rmTransaccion = await _context.RmTransacciones.OrderByDescending(o => o.FechaAlta).FirstOrDefaultAsync(t => t.NumeroEntrada == mesaEntrada.NumeroEntrada);
                    //Busca información sobre la prenda asociada a la mesa de entrada.
                    var rmPrenda = await _context.RmMedidasPrendas.FirstOrDefaultAsync(p => p.NroEntrada == mesaEntrada.NumeroEntrada);

                    if (rmPrenda != null)
                    {
                        //Obtiene detalles del distrito y el departamento asociados a la prenda.
                        var codDistrito = rmPrenda?.CodDistrito ?? "";
                        var distrito = await _context.RmDistritos.FirstOrDefaultAsync(d => d.CodigoDistrito.ToString() == codDistrito);
                        var codDepto = distrito?.CodigoDepto ?? 0;

                        var departamento = await _context.AvDepartamentos.FirstOrDefaultAsync(d => d.CodDepartamento == codDepto.ToString());

                        //var codAcreedor = rmPrenda?.Acreedor??"";
                        //var acreedor = await _dbContext.Personas.FirstOrDefaultAsync(p=>p.CodPersona== codAcreedor);
                        //var codDeudor = rmPrenda?.Deudor ?? "";
                        //var deudor = await _dbContext.Personas.FirstOrDefaultAsync(p=>p.CodPersona == codDeudor);

                        ViewBag.Distrito = new SelectList(_context.RmDistritos, "CodigoDistrito", "DescripDistrito", rmPrenda?.CodDistrito ?? "");

                        PrendaCustom prenda = new()
                        {
                            Libro = rmPrenda?.Libro,
                            Folio = rmPrenda?.Folio,
                            NroEntrada = rmPrenda?.NroEntrada,
                            FechaInscripcion = rmPrenda?.FechaInscripcion,
                            FechaOperacion = rmPrenda?.FechaOperacion,
                            Acreedor = rmPrenda?.Acreedor,
                            Deudor = rmPrenda?.Deudor,
                            FechaVencimiento = rmPrenda?.FechaVencimiento,
                            MontoPrenda = rmPrenda?.MontoPrenda,
                            MontoDeJusticia = rmPrenda?.MontoDeJusticia,
                            NroBoleta = rmPrenda?.NroBoleta,
                            NroBoletaSenal = rmPrenda?.NroBoletaSenal,
                            CodDistrito = rmPrenda?.CodDistrito,
                            Departamento = departamento?.Descripcion ?? "",
                            //Distrito = await GetDistritosAsync(),
                            Instrumento = rmPrenda?.Instrumento,
                            Comentario = rmTransaccion?.Comentario
                        };
                        return View("Index", prenda);
                    }
                    else
                    {
                        ViewBag.Distrito = new SelectList(_context.RmDistritos, "CodigoDistrito", "DescripDistrito");
                        PrendaCustom prenda = new()
                        {
                            NroEntrada = mesaEntrada.NumeroEntrada,
                            NroBoleta = mesaEntrada?.NroBoleta ?? ""
                            //Distrito = await GetDistritosAsync(),
                            //Instrumento = rmPrenda.Instrumento
                        };
                        return View("Index", prenda);
                    }

                    //return View("Index");
                }
                //Si la mesa de entrada no se encuentra, devuelve un resultado "NotFound".

                else
                {
                    return NotFound();
                }


            }
            //Captura y lanza excepciones en caso de errores.

            catch (Exception ex)
            {

                throw new Exception("Ocurrió un error: " + ex.Message);

            }


        }
        #endregion

        [HttpPost]
        public IActionResult GenerarPdf(PrendaCustom rmMedPrenda)
        {
            var mesaSalida = GetPrendaData(rmMedPrenda);
        
            // Lee la imagen y conviértela en una cadena de datos URI codificada en base64
            string imagePath2 = "wwwroot/assets/img/PJ/CorteSuprema.png"; // Ruta a la imagen en tu proyecto
            byte[] imageBytes2 = System.IO.File.ReadAllBytes(imagePath2);
            string imageDataUri2 = "data:image/png;base64," + Convert.ToBase64String(imageBytes2);

            // Pasa la cadena de datos URI a la vista a través del modelo de vista
            mesaSalida.ImageDataUri2 = imageDataUri2;

            // Lee la imagen y conviértela en una cadena de datos URI codificada en base64
            string imagePath3 = "wwwroot/assets/img/PJ/Compromiso.png"; // Ruta a la imagen en tu proyecto
            byte[] imageBytes3 = System.IO.File.ReadAllBytes(imagePath3);
            string imageDataUri3 = "data:image/png;base64," + Convert.ToBase64String(imageBytes3);

            // Pasa la cadena de datos URI a la vista a través del modelo de vista
            mesaSalida.ImageDataUri3 = imageDataUri3;
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

            // Configurar el encabezado Content-Disposition
            Response.Headers["Content-Disposition"] = "inline; filename=Prenda.pdf";

            // Devolver el PDF como un archivo descargable con el nuevo nombre
            return File(pdfBytes, "application/pdf");
        }

        // Otros métodos del controlador...

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
        public PrendaCustom GetPrendaData(PrendaCustom rmMedPrenda)
        {
            var mesaEntrada = _context.RmMesaEntrada.FirstOrDefault(p => p.NumeroEntrada == rmMedPrenda.NroEntrada);
            
            var rmTransaccion = _context.RmTransacciones.FirstOrDefault(t => t.NumeroEntrada == mesaEntrada.NumeroEntrada);
            var rmPrenda = _context.RmMedidasPrendas.FirstOrDefault(p => p.NroEntrada == mesaEntrada.NumeroEntrada);
            var distrito = _context.RmDistritos.FirstOrDefault(d => d.CodigoDistrito.ToString() == rmPrenda.CodDistrito);
            var codDistrito = distrito?.CodigoDistrito ?? 0;

            var departamento = _context.AvDepartamentos.FirstOrDefault(d => d.CodDepartamento == codDistrito.ToString());
            var codDeudor = rmPrenda?.Deudor ?? "";
            var deudor = _context.Personas.FirstOrDefault(p=>p.CodPersona== codDeudor);
            var codAcreeder = rmPrenda?.Acreedor??"";
            var acreerdor = _context.Personas.FirstOrDefault(p => p.CodPersona== codAcreeder);
            ViewBag.Distrito = new SelectList(_context.RmDistritos, "CodigoDistrito", "DescripDistrito", rmPrenda?.CodDistrito ?? "");
           
            PrendaCustom prenda = new()
            {
                Libro = rmPrenda?.Libro,
                FechaEntrada = mesaEntrada?.FechaEntrada,
                Folio = rmPrenda?.Folio,
                NroBoleta= mesaEntrada.NroBoleta,
                NroEntrada = rmPrenda?.NroEntrada,
                Acreedor = acreerdor?.Nombre,
                Deudor = deudor?.Nombre,
                NroBoletaSenal = rmPrenda?.NroBoletaSenal,
                CodDistrito = distrito?.DescripDistrito,
                Departamento = departamento?.Descripcion ?? "",
                Instrumento = rmPrenda?.Instrumento,
                Comentario = rmTransaccion?.Comentario
            };
            return prenda;


        }

        #region Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrendaCustom transaccion)
        {
            try
            {
                var rmMesaEntrada = await _context.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == transaccion.NroEntrada);
             
                var rmTransaccion = await _context.RmTransacciones.OrderBy(o=>o.FechaAlta).FirstOrDefaultAsync(p => p.NumeroEntrada == transaccion.NroEntrada);
                var boletaMarca = _context.RmBoletasMarcas.FirstOrDefault(b=>b.Descripcion==transaccion.NroBoleta);
                var nroBoleta = boletaMarca?.NroBoleta;
                if (rmTransaccion != null)
                {
                    rmTransaccion.Comentario = transaccion.Comentario;
                    rmTransaccion.NroBoleta = nroBoleta.ToString();
                    //rmTransaccion.EstadoTransaccion = rmMesaEntrada.EstadoEntrada.ToString();
                    _context.Update(rmTransaccion);

                     var rmPrenda = await _context.RmMedidasPrendas.FirstOrDefaultAsync(p=>p.NroEntrada == transaccion.NroEntrada);
                    if (rmPrenda != null)
                    {
                        rmPrenda.Libro = transaccion?.Libro ?? "";
                        rmPrenda.Folio = transaccion?.Folio ?? "";
                        rmPrenda.NroEntrada = transaccion?.NroEntrada;
                        rmPrenda.TipoOperacion = "26";//Codigo de Prenda 
                        rmPrenda.FechaInscripcion = transaccion?.FechaInscripcion;
                        rmPrenda.Acreedor = transaccion?.Acreedor;
                        rmPrenda.FechaOperacion = transaccion?.FechaOperacion;
                        rmPrenda.Deudor = transaccion?.Deudor;
                        rmPrenda.FechaVencimiento = transaccion?.FechaVencimiento;
                        rmPrenda.MontoPrenda = transaccion?.MontoPrenda;
                        rmPrenda.NroBoleta = transaccion?.NroBoleta;
                        rmPrenda.NroBoletaSenal = transaccion?.NroBoletaSenal;
                        rmPrenda.MontoDeJusticia = transaccion?.MontoDeJusticia;
                        rmPrenda.CodDistrito = transaccion?.CodDistrito;
                        rmPrenda.Instrumento = transaccion?.Instrumento;
                        _context.Update(rmPrenda);
                    }
                    else { 
                        RmMedidasPrenda prenda = new()
                        {
                            Libro = transaccion?.Libro ?? "",
                            Folio = transaccion?.Folio ?? "",
                            NroEntrada = transaccion?.NroEntrada,
                            TipoOperacion = "26",//Codigo de Prenda 
                            FechaInscripcion = transaccion?.FechaInscripcion,
                            Acreedor = transaccion?.Acreedor,
                            Deudor = transaccion?.Deudor,
                            FechaOperacion = transaccion?.FechaOperacion,
                            FechaVencimiento = transaccion?.FechaVencimiento,
                            MontoPrenda = transaccion?.MontoPrenda,
                            NroBoleta = transaccion?.NroBoleta,
                            NroBoletaSenal = transaccion?.NroBoletaSenal,
                            CodDistrito = transaccion?.CodDistrito,
                            MontoDeJusticia = transaccion?.MontoDeJusticia,
                            Instrumento = transaccion?.Instrumento,
                            IdAutorizante = rmMesaEntrada?.IdAutorizante

                        };

                        await _context.AddAsync(prenda);

                    }
               
                await _context.SaveChangesAsync();
                }

                return Ok();
            }
            catch
            {
                // Manejo de errores
                return View("Error");
            }
        }
        #endregion

        #region Metodo Cambiar Estado
        [HttpPost]
        public async Task<IActionResult> CambiarEstado(decimal id, string nuevoEstado, string comentario)
        {
            try
            {
                var mesaEntradum = await _context.RmMesaEntrada.FindAsync(id);
                var estadoEntrada = await _context.RmEstadosEntrada.FirstOrDefaultAsync(e => e.DescripEstado == nuevoEstado);


                if (mesaEntradum != null)
                {
                    decimal? nroEntrada = null;
                    var enrevision = await _context.RmEstadosEntrada.FirstOrDefaultAsync(e => e.DescripEstado == "En Revisión");
                    var EntradaRevision = _context.RmMesaEntrada.Where(m => m.EstadoEntrada == enrevision.CodigoEstado).FirstOrDefault(m => m.NumeroEntrada == id);
                    nroEntrada = EntradaRevision?.NumeroEntrada ?? 0;
                    var transaccion = await _context.RmTransacciones.OrderByDescending(o => o.FechaAlta).FirstOrDefaultAsync(p => p.NumeroEntrada == nroEntrada);
                    if (transaccion == null)
                    {
                        if (nuevoEstado == "Nota Negativa/Registrador") //si es nota negativa se agrega en la tabla
                        {
                            var notasNegativas = _context.RmNotasNegativas.FirstOrDefault(n => n.IdEntrada == id);
                            if (notasNegativas == null)
                            {
                                RmNotasNegativa notaNegativa = new()
                                {
                                    IdEntrada = id,
                                    FechaAlta = DateTime.Now,
                                    DescripNotaNegativa = comentario,
                                    IdUsuario = User.Identity.Name
                                };
                                await _context.AddAsync(notaNegativa);
                            }
                            else
                            {
                                notasNegativas.DescripNotaNegativa = comentario;
                                notasNegativas.IdUsuario = User.Identity.Name;
                                notasNegativas.FechaAlta = DateTime.Now;
                                _context.SaveChanges();
                            }

                        }
                        RmTransaccione bdTransaccion = new()
                        {
                            NumeroEntrada = id,
                            FechaAlta = DateTime.Now,
                            Observacion = comentario,
                            EstadoTransaccion = estadoEntrada.CodigoEstado.ToString(),
                            IdUsuario = User.Identity.Name,
                            TipoOperacion = "26"
                        };
                        await _context.AddAsync(bdTransaccion);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (nuevoEstado != "Aprobado/Registrador")
                        { //Eliminar el registro cargado de esa transaccion
                            if (nuevoEstado == "Nota Negativa/Registrador") //si es nota negativa se agrega en la tabla
                            {
                                var notasNegativas = _context.RmNotasNegativas.FirstOrDefault(n=>n.IdEntrada==id);
                                if (notasNegativas==null)
                                {
                                    RmNotasNegativa notaNegativa = new()
                                    {
                                        IdEntrada = id,
                                        FechaAlta = DateTime.Now,
                                        DescripNotaNegativa = comentario,
                                        IdUsuario = User.Identity.Name
                                    };
                                    await _context.AddAsync(notaNegativa);
                                }
                                else
                                {
                                    notasNegativas.DescripNotaNegativa = comentario;
                                    notasNegativas.IdUsuario = User.Identity.Name;
                                    notasNegativas.FechaAlta = DateTime.Now;
                                    _context.SaveChanges();
                                }
                              

                            }
                            transaccion.EstadoTransaccion = estadoEntrada.CodigoEstado.ToString();
                            transaccion.NroBoleta = null;
                            transaccion.Observacion = comentario;
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            transaccion.EstadoTransaccion = estadoEntrada.CodigoEstado.ToString();
                            transaccion.Observacion = comentario;

                            _context.Update(transaccion);

                        }

                    }

                    mesaEntradum.EstadoEntrada = estadoEntrada.CodigoEstado;
                    _context.Update(mesaEntradum);
                    await _context.SaveChangesAsync();

                    RmMovimientosDoc movimientos = new()
                    {
                        NroEntrada = id,
                        CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                        FechaOperacion = DateTime.Now,
                        CodOperacion = "09", //Parametro para cambio de estado 
                        NroMovimientoRef = id.ToString(),
                        EstadoEntrada = mesaEntradum.EstadoEntrada
                    };
                    await _context.AddAsync(movimientos);
                    await _context.SaveChangesAsync();


                }
               else {
                    return Json(new { Success = false, ErrorMessage = "Numero de entrada no existe" });
                }

            }
            catch
            {
                // Manejo de errores
                return View("Error");
            }
            return Ok();
        }
        #endregion

        #region Departamentos
        public IActionResult GetDepartamento(string CodDistrito)
        {
            if (CodDistrito != null)
            {
                var distrito = _context.RmDistritos.FirstOrDefault(p => p.CodigoDistrito.ToString() == CodDistrito);
                var departamento = _context.AvDepartamentos.FirstOrDefault(d => d.CodDepartamento == distrito.CodigoDepto.ToString());

                if (departamento != null)
                {
                    return Json(new { Success = true, nombre = departamento.Descripcion });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Error" });
                }
            }
            else
            {
                return NotFound();
            }


        }

        #endregion

        #region Metodo Distritos
        private async Task<List<SelectListItem>> GetDistritosAsync()
        {
            using var dbContext = new DbvinDbContext();
            var distritos = await dbContext.RmDistritos
            .Select(d => new SelectListItem
            {
                Text = $"{d.CodigoDistrito}-{d.DescripDistrito}",
                Value = d.CodigoDistrito.ToString()
            })
            .ToListAsync();

            return distritos;
        }

        #endregion

        #region Metodo Buscar imagen

        // Este método busca una imagen y datos relacionados con una boleta en la base de datos
        // y responde con estos datos en formato JSON.
        public IActionResult BuscarImagen(string nroBoleta)
        {

            if (nroBoleta != null)
            {
                var transaccion = _context.RmTransacciones.Where(t => t.NroBoleta == nroBoleta).FirstOrDefault(t => t.TipoOperacion == "6");
                var result = _context.RmMarcasSenales.FirstOrDefault(p => p.NumeroEntrada.Equals(transaccion.NumeroEntrada));

                if (result != null)
                {
                    try
                    {
                        string imagePath = result.MarcaNombre; // Ruta a la imagen en tu proyecto
                        byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                        string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                        string imagePath2 = result.SenalNombre; // Ruta a la imagen en tu proyecto
                        byte[] imageBytes2 = System.IO.File.ReadAllBytes(imagePath2);
                        string imageDataUri2 = "data:image/png;base64," + Convert.ToBase64String(imageBytes2);

                        var estableDis = _context.RmMarcasXEstabs.FirstOrDefault(m => m.IdTransaccion == transaccion.IdTransaccion);
                        var distrito = _context.RmDistritos.FirstOrDefault(d => d.CodigoDistrito.ToString() == estableDis.CodDistrito);

                        return Json(new { Success = true, srcmarca = imageDataUri, srcsenhal = imageDataUri2, distrito = distrito.DescripDistrito, departamento = estableDis.Descripcion });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Success = false, ErrorMessage = ex.Message });
                    }
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Error" });
                }
            }
            else
            {
                return NotFound();
            }

        }

        #endregion
    }
}
