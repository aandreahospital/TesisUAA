using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Html2pdf;
using iText.Commons.Actions.Contexts;
using ZXing;

namespace SistemaBase.Controllers
{
    public class MesaSalidaCustomController : BaseRyMController
    {
        private readonly DbvinDbContext _dbContext;

        public MesaSalidaCustomController(DbvinDbContext context, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {
            ViewBag.TiposDocumentos = new SelectList(_dbContext.RmTiposDocumentos, "TipoDocumento", "DescripTipoDocumento");

            return View();
        }
        public async Task<IActionResult> Get(decimal nEntrada)
        {
            var mesaEntrada = await _dbContext.RmMesaEntrada.SingleOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
            if (mesaEntrada == null)
            {
                return NotFound();
            }

            ViewBag.TiposDocumentos = new SelectList(_dbContext.RmTiposDocumentos, "TipoDocumento", "DescripTipoDocumento", mesaEntrada.TipoDocumento);
            ViewBag.Oficinas = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina", mesaEntrada.CodigoOficina);
            ViewBag.TiposSolicitud = new SelectList(_dbContext.RmTipoSolicituds, "TipoSolicitud", "DescripSolicitud", mesaEntrada.TipoSolicitud);
            ViewBag.EstadosEntrada = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada.EstadoEntrada);


            MesaSalidaCustom mesaSalida = new()
            {
                NumeroEntrada = mesaEntrada.NumeroEntrada,
                FechaSalida = mesaEntrada.FechaSalida,
                NroFormulario = mesaEntrada.NroFormulario,
                Oficinas = await GetOficinasRegistralesAsync(),
                CodigoOficina = mesaEntrada.CodigoOficina,
                TiposSolicitud = await GetTiposSolicitudAsync(),
                TipoSolicitud = mesaEntrada.TipoSolicitud,
                EstadoEntrada = mesaEntrada.EstadoEntrada,
                EstadosEntrada = await GetEstadosEntradaAsync(),
                NomTitular = mesaEntrada.NomTitular,
                NombreRetirador = mesaEntrada.NombreRetirador,
                TiposDocumentos = await GetTiposDocumentosAsync(),
                TipoDocumentoRetirador =mesaEntrada.TipoDocumentoRetirador,
                NroDocumentoRetirador = mesaEntrada.NroDocumentoRetirador

            };
            return View("Index", mesaSalida);
        }

        private async Task<List<SelectListItem>> GetOficinasRegistralesAsync()
        {
            using var dbContext = new DbvinDbContext();

            var oficinas = await dbContext.RmOficinasRegistrales
            .Select(o => new SelectListItem
            {
                Text = $"{o.DescripOficina}",
                Value = o.CodigoOficina.ToString()
            })
            .ToListAsync();

            return oficinas;
        }

        private async Task<List<SelectListItem>> GetTiposSolicitudAsync()
        {
            using var dbContext = new DbvinDbContext();
            var tiposSolicitud = await dbContext.RmTipoSolicituds
            .Select(t => new SelectListItem
            {
                Text = $"{t.TipoSolicitud}-{t.DescripSolicitud}",
                Value = t.TipoSolicitud.ToString()
            })
            .ToListAsync();

            return tiposSolicitud;

        }
        private async Task<List<SelectListItem>> GetEstadosEntradaAsync()
        {
            using var dbContext = new DbvinDbContext();
            var estadosEntrada = await dbContext.RmEstadosEntrada
            .Select(d => new SelectListItem
            {
                Text = $"{d.CodigoEstado}-{d.DescripEstado}",
                Value = d.CodigoEstado.ToString()
            })
            .ToListAsync();

            return estadosEntrada;
        }

        private async Task<List<SelectListItem>> GetTiposDocumentosAsync()
        {
            using var dbContext = new DbvinDbContext();
            var tiposDocumentos = await dbContext.RmTiposDocumentos
            .Select(d => new SelectListItem
            {
                Text = $"{d.TipoDocumento}-{d.DescripTipoDocumento}",
                Value = d.TipoDocumento.ToString()
            })
            .ToListAsync();

            return tiposDocumentos;
        }

        public IActionResult GenerarPdf(decimal nEntrada)
        {
            var mesaSalida = GetTituloData(nEntrada);
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

            // Configurar el encabezado Content-Disposition
            Response.Headers["Content-Disposition"] = "inline; filename=Titulo-de-Propiedad.pdf";

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

        private DatosTituloCustom GetTituloData(decimal nEntrada)
        {
            var mesaEntrada = _dbContext.RmMesaEntrada.SingleOrDefault(p => p.NumeroEntrada == nEntrada);
            var transaccion = _dbContext.RmTransacciones.FirstOrDefault(p => p.NumeroEntrada == nEntrada);
            var dataRepresentante = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == transaccion.Representante);
            var dataTitular = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == mesaEntrada.NroDocumentoTitular);
            var dataEstablecimiento = _dbContext.RmMarcasXEstabs.FirstOrDefault(p => p.IdMarca == mesaEntrada.NumeroEntrada);
            var ciudadTitu = _dbContext.DirecPersonas.FirstOrDefault(p => p.CodPersona == mesaEntrada.NroDocumentoTitular);
            var ciudadRep = _dbContext.DirecPersonas.FirstOrDefault(p => p.CodPersona == transaccion.Representante);
            var fecReingreso = _dbContext.RmReingresos.FirstOrDefault(p => p.NroEntrada == mesaEntrada.NumeroEntrada);
            //var result = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NumeroEntrada.Equals(nEntrada));
            //string imagenMarcaPath = result?.MarcaNombre;
            //string imagenSenhalPath = result?.SenalNombre;

            //string imagenMarcaDataUri = ConvertImageToDataUri(imagenMarcaPath);
            //string imagenSenhalDataUri = ConvertImageToDataUri(imagenSenhalPath);

            if (mesaEntrada == null)
            {
                return null; // Manejar el caso cuando no se encuentra la entrada
            }


            DatosTituloCustom tituloData = new()
            {
                NumeroEntrada = mesaEntrada.NumeroEntrada,
                FechaActual = GetFechaActual(),
                //ImagenMarca = result.MarcaNombre,
                //ImagenSenhal = result.SenalNombre,
                NroBoleta = mesaEntrada?.NroBoleta ?? "",
                Oficinas = GetOficinasRegistrales(),
                CodigoOficina = mesaEntrada?.CodigoOficina ?? 0,
                NomTitular = mesaEntrada?.NomTitular ?? "",
                NroDocumentoTitular = mesaEntrada?.NroDocumentoTitular ?? "",
                DireccionTitular = transaccion?.DireccionTitular ?? "",
                CiudadesTitu = GetCiudades(),
                CiudadTitular = ciudadTitu?.CodCiudad ?? "",
                Nacionalidades = GetNacionalidad(),
                CodPais = dataTitular?.CodPais ?? "",
                Profesiones = GetProfesion(),
                CodProfesion = dataTitular?.Profesion ?? "",
                FecNacimiento = dataTitular?.FecNacimiento ?? DateTime.MinValue,
                Edad = CalcularEdad(dataTitular?.FecNacimiento ?? DateTime.MinValue), // Calcula la edad aquí y usa DateTime.MinValue como valor predeterminado si FecNacimiento es nulo
                EstadoCivil = GetEstadoCivil(),
                CodEstadoCivil = dataTitular?.CodEstadoCivil ?? "",
                Distritos = GetDistritos(),
                DistritoEstable = dataEstablecimiento?.CodDistrito ?? "",
                DepartamentoEstable = dataEstablecimiento?.Descripcion ?? "",
                NombreRepresentante = dataRepresentante?.Nombre ?? "",
                Representante = transaccion?.Representante ?? "",
                NacionalidadesRep = GetNacionalidad(),
                CodPaisRep = dataRepresentante?.CodPais ?? "",
                EstadoCivilRep = GetEstadoCivil(),
                CodEstadoCivilRep = dataRepresentante?.CodEstadoCivil ?? "",
                ProfesionesRep = GetProfesion(),
                CodProfesionRep = dataRepresentante?.Profesion ?? "",
                DirecRep = ciudadRep?.Detalle ?? "",
                CiudadesRep = GetCiudades(),
                CiudadRep = ciudadRep?.CodCiudad ?? "",
                MatriculaRegistro = GetMatriculaRegistro(),
                Autorizantes = GetAutorizante(),
                CodCiudadAuto = GetCodCiudad(),
                DescripCiudadAuto = GetCiudades(),
                IdAutorizante = mesaEntrada?.IdAutorizante ?? 0,
                FechaActoJuridico = DateOnly.FromDateTime(transaccion?.FechaActoJuridico ?? DateTime.MinValue),
                FechaReingreso = fecReingreso?.FechaReingreso ?? DateTime.MinValue
            };

            return tituloData;
        }

        private string ConvertImageToDataUri(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return null; // Manejar el caso cuando la ruta de la imagen no existe o es nula
            }

            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

            return imageDataUri;
        }

        private List<SelectListItem> GetCodCiudad()
        {
            using var dbContext = new DbvinDbContext();

            var oficinas = dbContext.RmAutorizantes
                .Select(o => new SelectListItem
                {
                    Text = $"{o.CodCiudad}",
                    Value = o.IdAutorizante.ToString()
                })
                .ToList();

            return oficinas;
        }


        private List<SelectListItem> GetCiudades()
        {
            using var dbContext = new DbvinDbContext();

            var oficinas = dbContext.Ciudades
                .Select(o => new SelectListItem
                {
                    Text = $"{o.Descripcion}",
                    Value = o.CodCiudad.ToString()
                })
                .ToList();

            return oficinas;
        }


        private string GetFechaActual()
        {
            // Obtener la fecha actual
            DateTime fechaActual = DateTime.Now;

            // Crear una cadena con el formato deseado
            string fechaFormateada = fechaActual.ToString("dd 'DE' MMMM 'DE' yyyy", new System.Globalization.CultureInfo("es-ES"));

            return fechaFormateada;
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

        private List<SelectListItem> GetDistritos()
        {
            using var dbContext = new DbvinDbContext();

            var oficinas = dbContext.RmDistritos
                .Select(o => new SelectListItem
                {
                    Text = $"{o.DescripDistrito}",
                    Value = o.CodigoDistrito.ToString()
                })
                .ToList();

            return oficinas;
        }

        private List<SelectListItem> GetNacionalidad()
        {
            using var dbContext = new DbvinDbContext();

            var tiposSolicitud = dbContext.Paises
                .Select(t => new SelectListItem
                {
                    Text = $"{t.Nacionalidad}",
                    Value = t.CodPais.ToString()
                })
                .ToList();

            return tiposSolicitud;
        }

        private List<SelectListItem> GetMatriculaRegistro()
        {
            using var dbContext = new DbvinDbContext();

            var tiposDocumentos = dbContext.RmAutorizantes
                .Select(d => new SelectListItem
                {
                    Text = $"{d.MatriculaRegistro}",
                    Value = d.IdAutorizante.ToString()
                })
                .ToList();

            return tiposDocumentos;
        }

        private List<SelectListItem> GetAutorizante()
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

        private List<SelectListItem> GetProfesion()
        {
            using var dbContext = new DbvinDbContext();
            var estadosEntrada = dbContext.Profesiones
            .Select(d => new SelectListItem
            {
                Text = $"{d.Descripcion}",
                Value = d.CodProfesion.ToString()
            })
            .ToList();

            return estadosEntrada;
        }

        private List<SelectListItem> GetEstadoCivil()
        {
            using var dbContext = new DbvinDbContext();
            var estadosEntrada = dbContext.EstadosCiviles
            .Select(d => new SelectListItem
            {
                Text = $"{d.Descripcion}",
                Value = d.CodEstadoCivil.ToString()
            })
            .ToList();

            return estadosEntrada;
        }


        // Este método calcula la edad a partir de la fecha de nacimiento
        private int CalcularEdad(DateTime fechaNacimiento)
        {
            DateTime fechaActual = DateTime.Now;
            int edad = fechaActual.Year - fechaNacimiento.Year;

            // Ajustar la edad si aún no ha tenido su cumpleaños este año
            if (fechaNacimiento > fechaActual.AddYears(-edad))
            {
                edad--;
            }

            return edad;
        }



        [HttpPost]
        public async Task<IActionResult> Edit(decimal NumeroEntrada, RmMesaEntradum rmMesaEntradum)
        {
            try
            {
                var estadoApro = await _dbContext.RmEstadosEntrada.Where(p => p.DescripEstado == "Retirado/Aprobado").FirstOrDefaultAsync();
                var estadoNota = await _dbContext.RmEstadosEntrada.Where(p => p.DescripEstado == "Retirado/Nota negativa").FirstOrDefaultAsync();
                var estadoObser = await _dbContext.RmEstadosEntrada.Where(p => p.DescripEstado == "Retirado/Observado").FirstOrDefaultAsync();

                var transaccion = await _dbContext.RmTransacciones.FirstOrDefaultAsync(t=>t.NumeroEntrada == rmMesaEntradum.NumeroEntrada);
               
                var dbToUpdate = await _dbContext.RmMesaEntrada.FindAsync(NumeroEntrada);

                dbToUpdate.NombreRetirador = rmMesaEntradum.NombreRetirador;
                dbToUpdate.TipoDocumentoRetirador = rmMesaEntradum.TipoDocumentoRetirador;
                dbToUpdate.NroDocumentoRetirador = rmMesaEntradum.NroDocumentoRetirador;
                dbToUpdate.FechaSalida = DateTime.Now;
                dbToUpdate.UsuarioSalida = User.Identity.Name;

                if (transaccion.EstadoTransaccion == "31" || transaccion.EstadoTransaccion== "4")
                {
                    dbToUpdate.EstadoEntrada = estadoNota.CodigoEstado;
                }else if (transaccion.EstadoTransaccion == "30" || transaccion.EstadoTransaccion == "2")
                {
                    dbToUpdate.EstadoEntrada = estadoObser.CodigoEstado;
                }else if (transaccion.EstadoTransaccion == "20" || transaccion.EstadoTransaccion == "3")
                {
                    dbToUpdate.EstadoEntrada = estadoApro.CodigoEstado;
                }
                //dbToUpdate.EstadoEntrada = estadoEntrada.CodigoEstado;
                await _dbContext.SaveChangesAsync();

                RmMovimientosDoc movimientos = new()
                {
                    NroEntrada = rmMesaEntradum.NumeroEntrada,
                    CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                    FechaOperacion = DateTime.Now,
                    CodOperacion = "08", //parametro para mesa salida
                    NroMovimientoRef = rmMesaEntradum.NumeroEntrada.ToString(),
                    EstadoEntrada = dbToUpdate.EstadoEntrada
                };
                await _dbContext.AddAsync(movimientos);
                await _dbContext.SaveChangesAsync();


            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RmMesaEntradumExists(rmMesaEntradum.NumeroEntrada))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }
        private bool RmMesaEntradumExists(decimal id)
        {
            return (_dbContext.RmMesaEntrada?.Any(e => e.NumeroEntrada == id)).GetValueOrDefault();
        }
    

    }
}
