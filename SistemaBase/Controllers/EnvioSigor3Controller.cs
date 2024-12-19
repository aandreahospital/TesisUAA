using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static SistemaBase.ModelsCustom.Reportes;

namespace SistemaBase.Controllers
{
    public class EnvioSigor3Controller : Controller
    {
        private readonly DbvinDbContext _context;

        public EnvioSigor3Controller(DbvinDbContext context)
        {
            _context = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
       [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSSIGO", "Index", "EnvioSigor3" })]

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerarTxt(Reportes parametros)
        {
            IQueryable<RmMesaEntradum> queryMesaEntrada = _context.RmMesaEntrada.Where(t => t.FechaEntrada >= parametros.FechaDesde && t.FechaEntrada <= parametros.FechaHasta).AsQueryable();
            IQueryable<RmMarcasSenale> queryMarcas = _context.RmMarcasSenales.AsQueryable();
            IQueryable<Persona> queryPersona = _context.Personas.AsQueryable();
            IQueryable<RmBoletasMarca> queryBoletas = _context.RmBoletasMarcas.AsQueryable();
            IQueryable<Paise> queryPaises = _context.Paises.AsQueryable();


            var queryPersonas = queryMesaEntrada.AsQueryable().Join(queryPersona,
           en => en.NroDocumentoTitular,
           tipo => tipo.CodPersona,
           (en, tipo) => new EnvioSigor
           {
               NumeroEntrada = en.NumeroEntrada,
               NomTitular = en.NomTitular,
               NroDocumento = en.NroDocumentoTitular,
               TipoDocumento = en.TipoDocumentoTitular,
               Nacionalidad = tipo.CodPais
           }).ToList();

            var queryPais = queryPersonas.AsQueryable().Join(queryPaises,
          en => en.Nacionalidad,
          tipo => tipo.CodPais,
          (en, tipo) => new EnvioSigor
          {
              NumeroEntrada = en.NumeroEntrada,
              NomTitular = en.NomTitular,
              NroDocumento = en.NroDocumento,
              TipoDocumento = en.TipoDocumento,
              Nacionalidad = tipo.Nacionalidad
          }).ToList();

            var queryEnMarcas = queryPais.AsQueryable().Join(queryMarcas,
           en => en.NumeroEntrada,
           tipo => tipo.NumeroEntrada,
           (en, tipo) => new EnvioSigor
           {
               NumeroEntrada = en.NumeroEntrada,
               NomTitular = en.NomTitular,
               NroMarca = tipo.NroBoleta,
               MarcaImagen = tipo.MarcaNombre,
               SenalImagen = tipo.SenalNombre,
               NroSenal = tipo.NroBoleta,
               TipoDocumento = en.TipoDocumento,
               Nacionalidad = en.Nacionalidad,
               NroDocumento = en.NroDocumento
           }).ToList();

            var queryFinal = queryEnMarcas.AsQueryable().Join(queryBoletas,
                en => en.NroMarca,
                tipo=> tipo.NroBoleta.ToString(),
                (en,tipo)=> new EnvioSigor
                {
                    NumeroEntrada = en.NumeroEntrada,
                    NomTitular = en.NomTitular,
                    NroMarca = tipo.Descripcion,
                    MarcaImagen = en.MarcaImagen,
                    SenalImagen = en.SenalImagen,
                    NroSenal = tipo.Descripcion,
                    TipoDocumento = en.TipoDocumento,
                    Nacionalidad = en.Nacionalidad,
                    NroDocumento = en.NroDocumento

                });

            // Realizamos la conversión de tipo fuera de la expresión LINQ to Entities
            //foreach (var item in queryFinal)
            //{
            //    if (!string.IsNullOrEmpty(item.NroMarca)) // Verifica si NroBoleta no es nulo ni vacío
            //    {
            //        if (!decimal.TryParse(item.NroMarca, out decimal nroBoletaDecimal))
            //        {
            //            // Si no se puede convertir a decimal, asumimos que ya es alfanumérico y no hacemos ninguna conversión
            //            continue; // Pasamos al siguiente item
            //        }

            //        // Obtenemos la descripción de la boleta correspondiente al número de boleta
            //        var descripcionBoleta = _context.RmBoletasMarcas.FirstOrDefault(b => b.NroBoleta == Convert.ToInt32(nroBoletaDecimal))?.Descripcion;
            //        if (descripcionBoleta != null)
            //        {
            //            item.NroMarca = descripcionBoleta; // Reemplazamos el número de boleta con la descripción
            //        }
            //    }
                
            //}
            var resultados = queryFinal.ToList();
            return Ok();
        }


        //[HttpPost]
        //public IActionResult GenerarPdfPrueb(Reportes parametros)
        //{
        //    // Obtiene los datos filtrados según los parámetros del modelo
        //   var datosReporte = GetReporteData(parametros);

        //    // Lee la imagen y conviértela en una cadena de datos URI codificada en base64
        //    string imagePath = "wwwroot/assets/img/PJ/PoderJudicial.png"; // Ruta a la imagen en tu proyecto
        //    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
        //    string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

        //    // Pasa la cadena de datos URI a la vista a través del modelo de vista
        //    datosReporte.ImageDataUri = imageDataUri;

        //    // Renderiza la vista Razor a una cadena HTML
        //    string viewHtml = RenderViewToString("ReportePDF", datosReporte);

        //    if (string.IsNullOrWhiteSpace(viewHtml))
        //    {
        //        return BadRequest("La vista HTML está vacía o nula.");
        //    }

        //    // Crear un documento PDF utilizando iText7
        //    MemoryStream memoryStream = new MemoryStream();
        //    PdfDocument pdfDoc = new PdfDocument(new PdfWriter(memoryStream));
        //    Document document = new Document(pdfDoc);

        //    // Agregar el contenido HTML convertido al documento PDF
        //    HtmlConverter.ConvertToPdf(viewHtml, pdfDoc, new ConverterProperties());

        //    document.Close();

        //    // Convertir el MemoryStream a un arreglo de bytes
        //    byte[] pdfBytes = memoryStream.ToArray();
        //    memoryStream.Close();

        //    // Configurar el encabezado Content-Disposition
        //    Response.Headers["Content-Disposition"] = "inline; filename=Reporte-de-Entrada.pdf";

        //    // Devolver el PDF como un archivo descargable con el nuevo nombre
        //    return File(pdfBytes, "application/pdf");
        //}

        //private string RenderViewToString(string viewName, object model)
        //{
        //    ViewData.Model = model;
        //    using (var sw = new StringWriter())
        //    {
        //        var engine = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine)) as Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine;
        //        var viewResult = engine.FindView(ControllerContext, viewName, false);

        //        var viewContext = new ViewContext(
        //            ControllerContext,
        //            viewResult.View,
        //            ViewData,
        //            TempData,
        //            sw,
        //            new Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelperOptions()
        //        );

        //        viewResult.View.RenderAsync(viewContext);
        //        return sw.GetStringBuilder().ToString();
        //    }
        //}


        [HttpPost]
        public IActionResult GenerarPdf(Reportes parametros)
        {
            IQueryable<RmMesaEntradum> queryMesaEntrada = _context.RmMesaEntrada.Where(t => t.FechaEntrada >= parametros.FechaDesde && t.FechaEntrada <= parametros.FechaHasta && t.TipoSolicitud==1).AsQueryable();
            IQueryable<RmMarcasSenale> queryMarcas = _context.RmMarcasSenales.AsQueryable();
            IQueryable<Persona> queryPersona = _context.Personas.AsQueryable();
            IQueryable<RmBoletasMarca> queryBoletas = _context.RmBoletasMarcas.AsQueryable();
            IQueryable<Paise> queryPaises = _context.Paises.AsQueryable();
            IQueryable<RmTiposDocumento> queryTiposDoc = _context.RmTiposDocumentos.AsQueryable();

            var UsuSigor = _context.ParametrosGenerales.FirstOrDefault(p => p.Parametro == "USUARIO_SIGOR3");
            var ContrseSigor = _context.ParametrosGenerales.FirstOrDefault(p => p.Parametro == "CONTRASENA_SIGOR3");


            var queryPersonas = queryMesaEntrada.Join(queryPersona,
                en => en.NroDocumentoTitular,
                tipo => tipo.CodPersona,
                (en, tipo) => new EnvioSigor
                {
                    NumeroEntrada = en.NumeroEntrada,
                    NomTitular = en.NomTitular,
                    NroDocumento = en.NroDocumentoTitular,
                    TipoDocumento = en.TipoDocumentoTitular,
                    Nacionalidad = tipo.CodPais
                }).ToList();

          
            var queryEnMarcas = queryPersonas.Join(queryMarcas,
                en => en.NumeroEntrada,
                tipo => tipo.NumeroEntrada,
                (en, tipo) => new EnvioSigor
                {
                    NumeroEntrada = en.NumeroEntrada,
                    NomTitular = en.NomTitular,
                    NroMarca = tipo.NroBoleta,
                    MarcaImagen = tipo.MarcaNombre.Replace(@"\\172.30.150.12\inventiva\imagenes_marcas\", @"C:\inventiva\imagenes_marcas\"),
                    SenalImagen = tipo.SenalNombre.Replace(@"\\172.30.150.12\inventiva\imagenes_senales\", @"C:\inventiva\imagenes_senales\"),
                    NroSenal = tipo.NroBoleta,
                    TipoDocumento = en.TipoDocumento,
                    Nacionalidad = en.Nacionalidad,
                    NroDocumento = en.NroDocumento
                }).ToList();

            var queryFin = queryEnMarcas.Join(queryBoletas,
                en => en.NroMarca,
                tipo => tipo.NroBoleta.ToString(),
                (en, tipo) => new EnvioSigor
                {
                    NumeroEntrada = en.NumeroEntrada,
                    NomTitular = en.NomTitular,
                    NroMarca = tipo.Descripcion,
                    MarcaImagen = en.MarcaImagen,
                    SenalImagen = en.SenalImagen,
                    NroSenal = tipo.Descripcion,
                    TipoDocumento = en.TipoDocumento,
                    Nacionalidad = en.Nacionalidad,
                    NroDocumento = en.NroDocumento,
                    TipoImagen = "BMP"
                }).ToList();

            var queryFinal = queryFin.Join(queryTiposDoc,
               en => en.TipoDocumento,
               tipo => tipo.TipoDocumento.ToString(),
               (en, tipo) => new EnvioSigor
               {
                   NumeroEntrada = en.NumeroEntrada,
                   Usuario = UsuSigor?.Valor??"" ,
                   Contrasena = ContrseSigor?.Valor ?? "",
                   NomTitular = en.NomTitular,
                   NroMarca = en.NroMarca,
                   MarcaImagen = en.MarcaImagen,
                   SenalImagen = en.SenalImagen,
                   NroSenal = en.NroSenal,
                   TipoDocumento = tipo.TipoDocumento == 1 ? "CI" : tipo.TipoDocumento == 2 ? "PAS" : tipo.TipoDocumento == 3 ? "RUC" : tipo.TipoDocumento == 4 ? "DNI" : "DNI",
                   Nacionalidad = en.Nacionalidad,
                   NroDocumento = en.NroDocumento,
                   DigitoVerificador = tipo.TipoDocumento == 3 ? en.NroDocumento.Substring(en.NroDocumento.IndexOf('-') + 1) : "0",
                   TipoImagen = "BMP"
               }).ToList();

            string fechaActual = DateTime.Now.ToString("yyyyMMdd");
            string filePath = $"resulSigor_{fechaActual}.txt";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var resultado in queryFinal)
                {
                    string linea = "";
                    linea += !string.IsNullOrEmpty(resultado.Usuario) ? $"\"{resultado.Usuario}\" " : "\"\" ";
                    linea += !string.IsNullOrEmpty(resultado.Contrasena) ? $"\"{resultado.Contrasena}\" " : "\"\" ";
                    linea += !string.IsNullOrEmpty(resultado.NomTitular) ? $"\"{resultado.NomTitular}\" " : "\"\" ";
                    linea += !string.IsNullOrEmpty(resultado.NomTitular) ? $"\"{resultado.NomTitular}\" " : "\"\" ";
                    linea += !string.IsNullOrEmpty(resultado.NomTitular) ? $"\"{resultado.NomTitular}\" " : "\"\" ";
                    linea += !string.IsNullOrEmpty(resultado.TipoDocumento) ? $"\"{resultado.TipoDocumento}\" " : "\"\" ";
                    linea += !string.IsNullOrEmpty(resultado.NroDocumento) ? $"\"{resultado.NroDocumento}\" " : "\"\" ";
                    linea += !string.IsNullOrEmpty(resultado.Nacionalidad) ? $"\"{resultado.Nacionalidad}\" " : "\"\" ";
                    linea += !string.IsNullOrEmpty(resultado.DigitoVerificador) ? $"\"{resultado.DigitoVerificador}\" " : "\"\" ";
                    linea += !string.IsNullOrEmpty(resultado.MarcaImagen) ? $"\"{resultado.MarcaImagen}\" " : "\"\" ";
                    linea += !string.IsNullOrEmpty(resultado.NroMarca) ? $"\"{resultado.NroMarca}\" " : "\"\" ";
                    linea += !string.IsNullOrEmpty(resultado.TipoImagen) ? $"\"{resultado.TipoImagen}\" " : "\"\" ";
                    linea += !string.IsNullOrEmpty(resultado.SenalImagen) ? $"\"{resultado.SenalImagen}\" " : "\"\" ";
                    linea += !string.IsNullOrEmpty(resultado.NroSenal) ? $"\"{resultado.NroSenal}\" " : "\"\" ";
                    linea += !string.IsNullOrEmpty(resultado.TipoImagen) ? $"\"{resultado.TipoImagen}\" " : "\"\" ";
                    writer.WriteLine(linea);
                }
            }

            EjecutarComandosJava(filePath);

            var fileStream = new FileStream(filePath, FileMode.Open);
            return File(fileStream, "text/plain", "resultados.txt");
        }

        private void EjecutarComandosJava(string filePath)
        {
            try
            {

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            Console.WriteLine("Advertencia: Se encontró una línea vacía en el archivo.");
                            continue;
                        }
                        var rutaJava = _context.ParametrosGenerales.FirstOrDefault(m => m.Parametro == "RUTA_ARCHIVO_JAVA");
                        // Ejecutar el comando Java -jar para cada línea
                        string javaCommand = $"java -jar "+rutaJava.Valor +" " +line;
                        ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + javaCommand);
                        processInfo.RedirectStandardError = true;
                        processInfo.RedirectStandardOutput = true;
                        processInfo.UseShellExecute = false;

                        using (Process process = Process.Start(processInfo))
                        {
                            string output = process.StandardOutput.ReadToEnd();
                            string error = process.StandardError.ReadToEnd();

                            process.WaitForExit();

                            // Verificar si hubo algún error durante la ejecución
                            //if (!string.IsNullOrEmpty(error))
                            //{
                            //    Console.WriteLine($"Error al ejecutar el comando Java: {error}");
                            //    return Json(new { success = false, message = "Ocurrio un error" });
                            //}

                            Console.WriteLine($"Salida del comando Java: {output}");
                        }

                    }
                }
                //return Ok(); // Si todo salió bien
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ejecutar comandos Java: {ex.Message}");
                //return Json(new { success = false, message = ex.Message });
            }
        }


    }
}
