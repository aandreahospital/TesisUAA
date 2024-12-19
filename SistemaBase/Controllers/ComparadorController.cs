
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.Models.Dtos;
using System.Collections;
using System.Text;
using System.Text.Json;
using static SistemaBase.ModelsCustom.ConsultaTrabajo;
using Microsoft.AspNetCore.Mvc.Rendering;
using iText.Html2pdf;
using Org.BouncyCastle.Asn1.Ocsp;
using SistemaBase.ModelsCustom;
using Microsoft.CodeAnalysis.Operations;
using Newtonsoft.Json.Linq;

namespace SistemaBase.Controllers;

public class ComparadorController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly DbvinDbContext _dbContext;
    public ComparadorController(IHttpClientFactory httpClientFactory, DbvinDbContext context)
    {
        _httpClientFactory = httpClientFactory;
        _dbContext = context;
    }

    //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
    [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMCOMPAR", "Index", "Comparador" })]


    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> CargarImagen([FromForm] UploadRequest request)
    {



        var client = _httpClientFactory.CreateClient("cbrpyClient");

        var form = new MultipartFormDataContent();
        form.Add(new StringContent(User!.Identity!.Name ?? "userId"), "user_id");

        if (request.image != null)
        {
            using (var stream = new MemoryStream())
            {
                await request.image.CopyToAsync(stream);
                var imageContent = new ByteArrayContent(stream.ToArray());
                imageContent.Headers.Add("Content-Type", "image/png");
                form.Add(imageContent, "image", request.image.FileName);
            }
        }

        var response = await client.PostAsync("api/upload", form);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserializa la respuesta en el modelo ApiResponseModel
            var apiResponse = JsonSerializer.Deserialize<UploadResponse>(responseContent);
            return Json(apiResponse);
        }
        else
        {
            var errorResponse = new { status = "error", message = "Error en la solicitud POST" };
            return Json(errorResponse);
        }
    }

    public async Task<IActionResult> PreprocesarImagen([FromBody] ProcessingRequest request)
    {
        var client = _httpClientFactory.CreateClient("cbrpyClient");

        //request.user_id = User!.Identity!.Name ?? "userId";
        string jsonContent = JsonSerializer.Serialize(request);
        var bodyRequest = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("api/preprocess", bodyRequest);

        if (response.IsSuccessStatusCode)
        {
            byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

            if (response.Content.Headers.ContentType.MediaType == "image/png")
            {
                return File(imageBytes, "image/png");
            }
            else if (response.Content.Headers.ContentType.MediaType == "image/jpeg")
            {
                return File(imageBytes, "image/jpeg");
            }
            else if (response.Content.Headers.ContentType.MediaType == "image/bmp")
            {
                return File(imageBytes, "image/bmp");
            }
            else
            {
                var errorResponse = new { status = "error", message = "La respuesta no es una imagen con formato válido" };
                return Json(errorResponse);
            }
        }
        else
        {
            var errorResponse = new { status = "error", message = "Error en la solicitud" };
            return Json(errorResponse);
        }
    }

    public async Task<IActionResult> BuscarSimilares([FromBody] RetrieveRequest request, int cont)
    {
        var client = _httpClientFactory.CreateClient("cbrpyClient");

        //request.user_id = User!.Identity!.Name ?? "userId";
        string jsonContent = JsonSerializer.Serialize(request);
        var bodyRequest = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("api/retrieve", bodyRequest);

        if (response.IsSuccessStatusCode)
        {
            // solo para pruebas de presentacion 
            string[] nombres = new string[10] {
              "marca_1.10469",
                "marca_1.10460",
                "marca_1.10461",
                "marca_1.10462",
                "marca_1.10463",
                "marca_1.10464",
                "marca_1.10465",
                "marca_1.10466",
                "marca_1.10467",
                "marca_1.10468"

            };
            string[] boleta = new string[10]
            {
                "AAI528",
                "AAI645",
                "AAI186",
                "AAI185",
                "AAI183",
                "AAI393",
                "AAI404",
                "AAI553",
                "AAI555",
                "AAI526"


            };
            string[] titular = new string[10]
            {
                                "NELSON REYES GALEANO",
                "PEDRO NUÑEZ  RAMIREZ",
                "ANGELINA DIAZ DE ESCOBAR",
                "PETER DUECK PENNER",
                "LEANDRO HARDER NEUFELD",
                "ARCADIA PATIÑO GALEANO",
                "CLAODINEI SILMAR TESSMANN BLODOW",
                "ARIEL RAUL MORENO NOTARIO",
                "LUIS ALBERTO GAYOSO MILTOS",
                "FRANCISCO JAVIER GODOY AGUERO"

            };
            string[] documento = new string[10]
            {
               "4882608",
                "2156015",
                "2847041",
                "7580529",
                "3967208",
                "3726666",
                "6014719",
                "5029731",
                "1444301",
                "2006672"

            };



            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<RetrieveResponse>(responseContent);
            var apiResults = new ArrayList();

            HashSet<string> addedClassIds = new HashSet<string>();
          
                foreach (var result in apiResponse.content.results)
                {
                    //if (result.class_id== "test_image-0" || result.class_id== "test_image-1" || result.class_id == "test_image-2")
                    //{
                        // solo para pruebas
                        string pr = result.class_id;

                        string percent = Math.Round((result.score * 100), 2).ToString() + " %";
                        var item = new RetrieveSingleResponse
                        {
                            Nombre = result.class_id,
                            //Nombre = nombres[int.Parse(pr.Substring(pr.Length - 1))],
                            Url = client.BaseAddress.AbsoluteUri + "api/items/" + result.class_id,
                            Similarity = percent,
                            //NumeroBoleta = boleta[int.Parse(pr.Substring(pr.Length - 1))],
                            //Titular = titular[int.Parse(pr.Substring(pr.Length - 1))],
                            //NumeroDocumento = documento[int.Parse(pr.Substring(pr.Length - 1))],
                            NumeroBoleta = "1",
                            Titular = "nombre titular",
                            NumeroDocumento = "2",
                            TipoDocumento = "CI"
                        };

                        // Verificar si el class_id ya ha sido agregado
                        if (!addedClassIds.Contains(result.class_id))
                        {
                            apiResults.Add(item);
                            addedClassIds.Add(result.class_id);
                        }
                    //}

                }
            //}
            //else if (cont == 2)
            //{
            //    foreach (var result in apiResponse.content.results)
            //    {
            //        if (result.class_id.Contains("3") || result.class_id.Contains("4") || result.class_id.Contains("5") )
            //        {
            //            // solo para pruebas
            //            string pr = result.class_id;

            //            string percent = Math.Round((result.similarity * 100), 2).ToString() + " %";
            //            var item = new RetrieveSingleResponse
            //            {
            //                //Nombre = result.class_id,
            //                Nombre = nombres[int.Parse(pr.Substring(pr.Length - 1))],
            //                Url = client.BaseAddress.AbsoluteUri + "api/items/" + result.class_id,
            //                Similarity = percent,
            //                NumeroBoleta = boleta[int.Parse(pr.Substring(pr.Length - 1))],
            //                Titular = titular[int.Parse(pr.Substring(pr.Length - 1))],
            //                NumeroDocumento = documento[int.Parse(pr.Substring(pr.Length - 1))],
            //                TipoDocumento = "CI"
            //            };

            //            // Verificar si el class_id ya ha sido agregado
            //            if (!addedClassIds.Contains(result.class_id))
            //            {
            //                apiResults.Add(item);
            //                addedClassIds.Add(result.class_id);
            //            }
            //        }

            //    }
            //}
            //else
            //{
            //    foreach (var result in apiResponse.content.results)
            //    {
            //        if (result.class_id.Contains("6") || result.class_id.Contains("7") || result.class_id.Contains("8") )
            //        {
            //            // solo para pruebas
            //            string pr = result.class_id;

            //            string percent = Math.Round((result.similarity * 100), 2).ToString() + " %";
            //            var item = new RetrieveSingleResponse
            //            {
            //                //Nombre = result.class_id,
            //                Nombre = nombres[int.Parse(pr.Substring(pr.Length - 1))],
            //                Url = client.BaseAddress.AbsoluteUri + "api/items/" + result.class_id,
            //                Similarity = percent,
            //                NumeroBoleta = boleta[int.Parse(pr.Substring(pr.Length - 1))],
            //                Titular = titular[int.Parse(pr.Substring(pr.Length - 1))],
            //                NumeroDocumento = documento[int.Parse(pr.Substring(pr.Length - 1))],
            //                TipoDocumento = "CI"
            //            };

            //            // Verificar si el class_id ya ha sido agregado
            //            if (!addedClassIds.Contains(result.class_id))
            //            {
            //                apiResults.Add(item);
            //                addedClassIds.Add(result.class_id);
            //            }
            //        }

            //    }
            //}


            return Json(apiResults);
        }
        else
        {
            var errorResponse = new { status = "error", message = "Error en la solicitud POST" };
            return Json(errorResponse);
        }
    }

    public async Task<IActionResult> BuscarImagenes([FromBody] RetrieveRequest request)
    {

        try { 
            var client = _httpClientFactory.CreateClient("cbrpyClient");

            //request.user_id = User!.Identity!.Name ?? "userId";
            string jsonContent = JsonSerializer.Serialize(request);
            var bodyRequest = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/retrieve", bodyRequest);

            IQueryable<RmMarcasSenale> queryMarcasSenales = _dbContext.RmMarcasSenales.AsQueryable();
            IQueryable<RmMesaEntradum> queryMesaEntrada = _dbContext.RmMesaEntrada.AsQueryable();
            IQueryable<RmTiposDocumento> queryTipoDoc = _dbContext.RmTiposDocumentos.AsQueryable();
            IQueryable<RmBoletasMarca> queryBoletaMarca = _dbContext.RmBoletasMarcas.AsQueryable();
            var marcaNA = _dbContext.ParametrosGenerales.FirstOrDefault(m => m.Parametro == "DIR_IMGMARCA_NA");

            if (response.IsSuccessStatusCode)
            {

                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<RetrieveResponse>(responseContent);
                var apiResults = new ArrayList();

                foreach (var result in apiResponse.content.results)
                {
                    // solo para pruebas
                    string pr = result.class_id;
                    string[] partes = pr.Split('_');
                    string numero = null;
                    if (partes.Length > 1) // Asegúrate de que haya al menos dos partes
                    {
                        string parteNumero = partes[1].Substring(1); // Substring(1) para omitir el punto
                        numero = parteNumero.Replace(".bmp", "");
                        numero = numero.Replace(".", ""); // Eliminar el punto
                   
                    }
                    string urlMarca = "\\\\172.30.150.12\\imagenes_marcas\\";
                    var nombreMarca = urlMarca + pr;
                    long numeroEntrada;
                    var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m=>m.NumeroEntrada== 0);
                    if (Int64.TryParse(numero, out numeroEntrada))
                    {
                        mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m => m.NumeroEntrada == Convert.ToInt64(numeroEntrada));

                    }
                       
                    //var marcaSenal = _dbContext.RmMarcasSenales.FirstOrDefault(m=>m.MarcaNombre== nombreMarca);
                    //if (marcaSenal == null)
                    //{
                    //    urlMarca = "\\\\172.30.150.12\\imagenes_marcas\\";
                    //    nombreMarca = urlMarca + pr;
                    //}
                
                    DatosImagen? resultdb;
                    if (mesaEntrada != null) { 
                        var queryMarcaEntrada = queryMesaEntrada
                             .Where(m => m.NumeroEntrada == Convert.ToInt64(numero))
                             .AsQueryable()
                             .GroupJoin(queryMarcasSenales,
                                 en => en.NumeroEntrada,
                                 ma => ma.NumeroEntrada,
                                 (en, ma) => new { en, ma })
                             .SelectMany(
                                 x => x.ma.DefaultIfEmpty(),
                                 (en, ma) => new { en.en, ma })
                             .Select(
                                 x => new DatosImagen
                                 {
                                     Titular = x.en.NomTitular,
                                     NumeroBoleta = x.ma != null ? x.ma.NroBoleta : null,
                                     NumeroDocumento = x.en.NroDocumentoTitular,
                                     TipoDocumento = x.en.TipoDocumentoTitular
                                 });

                        queryMarcaEntrada = queryMarcaEntrada
                            .GroupJoin(queryBoletaMarca,
                                en => en.NumeroBoleta,
                                bol => bol.NroBoleta.ToString(),
                                (en, bol) => new { en, bol })
                            .SelectMany(
                                x => x.bol.DefaultIfEmpty(),
                                (en, bol) => new { en.en, bol })
                            .Select(
                                x => new DatosImagen
                                {
                                    Titular = x.en.Titular,
                                    NumeroBoleta = x.bol != null ? x.bol.Descripcion : null,
                                    NumeroDocumento = x.en.NumeroDocumento,
                                    TipoDocumento = x.en.TipoDocumento
                                });

                        var queryfinal = queryMarcaEntrada
                            .GroupJoin(queryTipoDoc,
                                ma => ma.TipoDocumento,
                                tip => tip.TipoDocumento.ToString(),
                                (ma, tip) => new { ma, tip })
                            .SelectMany(
                                x => x.tip.DefaultIfEmpty(),
                                (ma, tip) => new { ma.ma, tip })
                            .Select(
                                x => new DatosImagen
                                {
                                    Titular = x.ma.Titular,
                                    NumeroBoleta = x.ma.NumeroBoleta,
                                    NumeroDocumento = x.ma.NumeroDocumento,
                                    TipoDocumento = x.tip != null ? x.tip.DescripTipoDocumento : null
                                });
                        resultdb = queryfinal.FirstOrDefault();
                    }
                    else
                    {
                        resultdb = null;
                    }
              

                    DatosImagen datosImagen = resultdb != null ? resultdb : new DatosImagen
                    {
                        Titular = "",
                        NumeroBoleta = "",
                        NumeroDocumento = "",
                        TipoDocumento = ""
                    };

                    //double porcen = Math.Round((result.score * 100), 2);
                    //if (porcen >= 95)
                    //{
                    //    return Json(new { Success = false, ErrorMessage = "Alerta, imagen con 95% de similitud" });
                    //}
                    string percent = Math.Round((result.score * 100), 2).ToString() + " %";
                    string imageDataUri;
                    if (System.IO.File.Exists(nombreMarca))
                    {
                        byte[] imageBytes = System.IO.File.ReadAllBytes(nombreMarca);
                        imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    }
                    else
                    {
                        nombreMarca = marcaNA?.Valor ?? "";
                   
                        byte[] imageBytes = System.IO.File.ReadAllBytes(nombreMarca);
                        imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    }

                    var item = new RetrieveSingleResponse
                    {
                        //Nombre = result.class_id,
                        Nombre = result.class_id,
                        //Url = client.BaseAddress.AbsoluteUri + "api/items/" + result.class_id,
                        Url = imageDataUri,
                        Similarity = percent,
                        NumeroBoleta = resultdb?.NumeroBoleta??"",
                        Titular = resultdb?.Titular??"",
                        NumeroDocumento = resultdb?.NumeroDocumento??"",
                        TipoDocumento = resultdb?.TipoDocumento??""
                    };
                    apiResults.Add(item);
                }
                //List<RetrieveSingleResponse> resultList = apiResults.Cast<RetrieveSingleResponse>().ToList();

                // Ordena la lista por el campo 'Similarity' en orden descendente
                //resultList = resultList.OrderByDescending(x => x.Similarity).ToList();

                return Json(apiResults);
            }
            else
            {
                var errorResponse = new { status = "error", message = "Error en la solicitud POST" };
                return Json(errorResponse);
            }
        }
        catch (Exception ex)
        {
            var errorResponse = new { status = "error", message = ex };
            return Json(errorResponse);
        }

    }


    public async Task<IActionResult> AgregarImagen([FromBody] RetrieveRequest request, string nom)
    {

        //var marcasSenales = _dbContext.RmMarcasSenales.Where(m =>m.MarcaNombre.Contains("marca_1") && m.NumeroEntrada>129000).ToList();

        //var contador = marcasSenales.Count();

        //foreach (var marca in marcasSenales)
        //{

        //// Ruta de la carpeta que quieres recorrer
        //string carpeta = @"\\172.30.150.12\imagenes_marcas";

        //// Verifica si la carpeta existe
        //if (Directory.Exists(carpeta))
        //{
        //    // Fecha límite para la comparación
        //    DateTime fechaLimite = new DateTime(2024, 1, 25);

        //    // Obtiene los nombres de los archivos en la carpeta y los ordena por fecha de modificación descendente
        //    var archivos = new DirectoryInfo(carpeta).GetFiles()
        //                    .OrderByDescending(f => f.LastWriteTime).Where(o => o.LastWriteTime > fechaLimite && o.Name.Contains("marca_1."))
        //                    .Select(f => f.FullName);


        // Obtiene los nombres de los archivos en la carpeta
        // string[] archivos = Directory.GetFiles(carpeta);

        //var cont = archivos.Count();
        //// Itera sobre los nombres de los archivos y los muestra en la consola
        //foreach (string archivo in archivos)
        //{
        //nom = marca.MarcaNombre;
        //Me ayudas a verificar Que no tenga espacios dentro del nombre

        string nombreArchivo = Path.GetFileNameWithoutExtension(nom);
        string carpeta = @"\\172.30.150.12\imagenes_marcas\";
        string rutaCompleta = Path.Combine(carpeta, nombreArchivo) + ".bmp";

        // Mensajes de depuración para verificar el nombre del archivo y la ruta completa
        System.Diagnostics.Debug.WriteLine("nom: " + nom);
        System.Diagnostics.Debug.WriteLine("Nombre del archivo sin extensión: " + nombreArchivo);
        System.Diagnostics.Debug.WriteLine("Ruta completa construida: " + rutaCompleta);

        try
        {
            // Verificar si el archivo existe en la ruta especificada
            if (!System.IO.File.Exists(rutaCompleta))
            {
                System.Diagnostics.Debug.WriteLine("El archivo no existe en la ruta especificada.");
                return Json(new { status = "error", message = "El archivo no se encuentra en la carpeta correspondiente o no tienes acceso a la ruta." });
            }
            System.Diagnostics.Debug.WriteLine("El archivo existe en la ruta especificada.");
        }
        catch (Exception ex)
        {
            // Captura cualquier excepción que ocurra al intentar acceder a la ruta
            System.Diagnostics.Debug.WriteLine("Error al acceder a la ruta: " + ex.Message);
            return Json(new { status = "error", message = "Error al intentar acceder a la ruta: " + ex.Message });
        }

        /// Verificar si el nombre del archivo contiene espacios en blanco
        if (nombreArchivo.Contains(" "))
        {
            return Json(new { status = "error", message = "El nombre del archivo no debe contener espacios en blanco." });
        }
        if (!nombreArchivo.Contains("marca_1."))
        {
            return Json(new { status = "error", message = "El nombre del archivo no corresponde." });
        }
        string[] partes = nombreArchivo.Split('_');
        string numero = null;
        if (partes.Length > 1) // Asegúrate de que haya al menos dos partes
        {
            string parteNumero = partes[1].Substring(1); // Substring(1) para omitir el punto
            numero = parteNumero.Replace(".", ""); // Eliminar el punto     
        }
        var rmMesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == Convert.ToInt64(numero));

        if (rmMesaEntrada == null)
        {
            return Json(new { status = "error", message = "El número de entrada no existe." });
        }
        else
        {
            var rmMarcasSenale = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(p => p.NumeroEntrada == Convert.ToInt64(numero));

            if (rmMarcasSenale == null)
            {
                var parametroMarca = await _dbContext.ParametrosGenerales.FirstOrDefaultAsync(m => m.Parametro == "DIR_IMGMARCA");
                var parametroSenal = await _dbContext.ParametrosGenerales.FirstOrDefaultAsync(m => m.Parametro == "DIR_IMGSENAL");
                var marcaSenale = _dbContext.RmMarcasSenales.Max(m => m.IdMarca) + 1;
                RmMarcasSenale marcasSenales = new()
                {
                    NumeroEntrada = Convert.ToInt64(numero),
                    MarcaNombre = parametroMarca.Valor + numero + ".bmp",
                    SenalNombre = parametroSenal.Valor + numero + ".bmp",
                    IdMarca = marcaSenale
                };
                _dbContext.Add(marcasSenales);
                _dbContext.SaveChanges();
            }
        }


        var client = _httpClientFactory.CreateClient("cbrpyClient");
                
                //request.user_id = User!.Identity!.Name ?? "userId";
                string jsonContent = JsonSerializer.Serialize(request);
                var bodyRequest = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/items/" + nombreArchivo + ".bmp", bodyRequest);

                if (response.IsSuccessStatusCode)
                {

                        // Leer el contenido de la respuesta como una cadena
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Parsear el contenido JSON
                        var jsonResponse = JObject.Parse(responseContent);

                        // Acceder al valor de 'result_ok'
                        bool resultOk = jsonResponse["content"]["result_ok"].Value<bool>();

                        // Verificar el valor de 'result_ok'
                        if (!resultOk)
                        {
                            var errorResponse = new { status = "error", message = "Error al guardar la imagen" };
                            Console.WriteLine(errorResponse);
                            return Json(errorResponse);
                        }
                       
                        var imagenAprobado = new { status = 200, message = "Imagen agregado correctamente" };
                        Console.WriteLine(imagenAprobado);
                         return Json(imagenAprobado);
                }
                if (response.ReasonPhrase == "Conflict")
                {
                    var errorResponse = new { status = "error", message = "Imagen ya existe" };
                    Console.WriteLine(errorResponse);
                    return Json(errorResponse);
                }
                else
                {
                    var errorResponse = new { status = "error", message = "Error en la solicitud post" };
                    Console.WriteLine(errorResponse);
                    return Json(errorResponse);
                }

        // Console.WriteLine(Path.GetFileName(archivo));

    //}
    ////    }
    ////    else
    ////        {
    ////            Console.WriteLine("La carpeta no existe.");
    ////        }
        return Ok();
    }

    public async Task<IActionResult> CargarCarpetaCopia([FromForm] UploadRequest request)
    {

        // Ruta de la carpeta que quieres recorrer
        string carpeta = @"\\172.30.150.12\imagenes_marcas";

        // Verifica si la carpeta existe
        if (Directory.Exists(carpeta))
        {
            // Fecha límite para la comparación
            DateTime fechaLimite = new DateTime(2024, 1, 25);

            // Obtiene los nombres de los archivos en la carpeta y los ordena por fecha de modificación descendente
            var archivos = new DirectoryInfo(carpeta).GetFiles()
                            .OrderByDescending(f => f.LastWriteTime).Where(o => o.LastWriteTime > fechaLimite && o.Name.Contains("marca_1."))
                            .Select(f => f.FullName);


            var cont = archivos.Count();
            // Itera sobre los nombres de los archivos y los muestra en la consola
            foreach (string archivo in archivos)
            {
                string nombreArchivo = Path.GetFileNameWithoutExtension(archivo);

                var client = _httpClientFactory.CreateClient("cbrpyClient");

                var form = new MultipartFormDataContent();
                form.Add(new StringContent(User?.Identity?.Name ?? "userId"), "user_id");

                using (var stream = new MemoryStream())
                {
                    // Leer el contenido del archivo en el stream
                    await using (var fileStream = new FileStream(archivo, FileMode.Open, FileAccess.Read))
                    {
                        await fileStream.CopyToAsync(stream);
                    }

                    // Agregar el contenido del archivo al formulario
                    var imageContent = new ByteArrayContent(stream.ToArray());
                    imageContent.Headers.Add("Content-Type", "image/png");
                    form.Add(imageContent, "image", nombreArchivo);
                }
                var response = await client.PostAsync("api/upload", form);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    string jsonContent = JsonSerializer.Serialize(request);
                    var bodyRequest = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    var preprocess = await client.PostAsync("api/preprocess", bodyRequest);
                    if (preprocess.IsSuccessStatusCode)
                    {
                        string jsonContent1 = JsonSerializer.Serialize(request);
                        var bodyRequest1 = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                        var items = await client.PostAsync("api/items/" + nombreArchivo + ".bmp", bodyRequest);

                        if (items.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Operacion Realizada.");

                        }
                        else
                        {

                            Console.WriteLine("Error");
                        }


                    }
                    else
                    {
                        Console.WriteLine("Error");
                    }
                    Console.WriteLine("Operacion Realizada.");
                    

                    //// Deserializa la respuesta en el modelo ApiResponseModel
                    //var apiResponse = JsonSerializer.Deserialize<UploadResponse>(responseContent);
                    //return Json(apiResponse);
                }
                else
                {
                    Console.WriteLine("Error");
                }

            }

            Console.WriteLine("Error");
        }
        else
        {
            Console.WriteLine("Error");
        }
        return Ok();
    }
    public async Task<IActionResult> CargarCarpeta()
    {
        // Ruta de la carpeta que quieres recorrer
        string carpeta = @"\\172.30.150.12\imagenes_marcas";

        // Verifica si la carpeta existe
        if (Directory.Exists(carpeta))
        {
            // Fecha límite para la comparación
            DateTime fechaLimite = new DateTime(2024, 1, 25);

            // Obtiene los nombres de los archivos en la carpeta y los ordena por fecha de modificación descendente
            var archivos = new DirectoryInfo(carpeta).GetFiles()
                            //.OrderByDescending(f => f.LastWriteTime)
                            .Where(o => o.LastWriteTime > fechaLimite && o.Name.Contains("marca_1."))
                            .Select(f => f.FullName);

            // Itera sobre los nombres de los archivos
            foreach (string archivo in archivos)
            {
                string nombreArchivo = Path.GetFileNameWithoutExtension(archivo);

                var client = _httpClientFactory.CreateClient("cbrpyClient");

                var form = new MultipartFormDataContent();
                form.Add(new StringContent(User?.Identity?.Name ?? "userId"), "user_id");

                using (var stream = new MemoryStream())
                {
                    // Leer el contenido del archivo en el stream
                    await using (var fileStream = new FileStream(archivo, FileMode.Open, FileAccess.Read))
                    {
                        await fileStream.CopyToAsync(stream);
                    }

                    // Agregar el contenido del archivo al formulario
                    var imageContent = new ByteArrayContent(stream.ToArray());
                    imageContent.Headers.Add("Content-Type", "image/png");
                    form.Add(imageContent, "image", nombreArchivo);

                    var response = await client.PostAsync("api/upload", form);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        // Deserializa la respuesta en el modelo ApiResponseModel
                        var apiResponse = JsonSerializer.Deserialize<UploadResponse>(responseContent);
                        var preprocessData = new
                        {
                            smoothing_level = 1,
                            blob_threshold = 1,
                            grid_thickness = 0,
                            background_type = 0,
                            image_size = 300,
                            image_format = "png",
                           token = apiResponse.content.token
                        };

                        var preprocessJson = JsonSerializer.Serialize(preprocessData);
                        var preprocessForm = new StringContent(preprocessJson, Encoding.UTF8, "application/json");

                        // Ahora necesitas enviar el archivo al endpoint api/preprocess
                        //stream.Seek(0, SeekOrigin.Begin); // Rebobina el stream para leerlo desde el principio
                        //var preprocessForm = new MultipartFormDataContent();
                        //preprocessForm.Add(imageContent, "image", nombreArchivo);

                        var preprocessResponse = await client.PostAsync("api/preprocess", preprocessForm);
                        if (preprocessResponse.IsSuccessStatusCode)
                        {
                            var paraItem = new
                            {
                                token = apiResponse.content.token
                            };
                            string jsonContent = JsonSerializer.Serialize(paraItem);
                            var bodyRequest = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                            var responseItem = await client.PostAsync("api/items/" + nombreArchivo + ".bmp", bodyRequest);
                            if (responseItem.IsSuccessStatusCode)
                            {
                                // Procesamiento de items exitoso
                                Console.WriteLine("Operacion Realizada.");
                            }
                            else
                            {
                                // Error en el procesamiento de items
                                Console.WriteLine("Error en el procesamiento de items.");
                            }
                        }
                        else
                        {
                            // Error en el procesamiento
                            Console.WriteLine("Error en el preprocesamiento.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error en la carga del archivo.");
                    }
                }
            }

            Console.WriteLine("Operacion Completa.");
        }
        else
        {
            Console.WriteLine("La carpeta no existe.");
        }

        return Ok();
    }

}
