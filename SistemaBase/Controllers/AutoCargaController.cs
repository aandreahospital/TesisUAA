using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using OfficeOpenXml;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using System;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SistemaBase.Controllers
{
    public class AutoCargaController : Controller
    {

        private readonly UAADbContext _context;

        public AutoCargaController(UAADbContext context)
        {
            _context = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
       // [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSAUTCA", "Index", "AutoCarga" })]


        public IActionResult Index()
        {
            ViewBag.Carrera = new SelectList(_context.Carreras, "IdCarrera", "Descripcion");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CargarExcel(IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
            {
                TempData["Mensaje"] = "Por favor seleccione un archivo válido.";
                return RedirectToAction("CargarExcel");
            }

            var listaPersonas = new List<Persona>();
            var listaUsuarios = new List<Usuario>();

            using (var stream = new MemoryStream())
            {
                await archivoExcel.CopyToAsync(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        return BadRequest("El archivo Excel no contiene hojas.");
                    }

                    ExcelWorksheet hoja = package.Workbook.Worksheets.First();
                    if (hoja.Dimension == null)
                    {
                        return BadRequest("La hoja está vacía.");
                    }

                    int filas = hoja.Dimension.Rows;

                    // Obtener todos los códigos de persona del Excel
                    var codPersonasExcel = new HashSet<string>();
                    for (int fila = 2; fila <= filas; fila++)
                    {
                        string codPersona = hoja.Cells[fila, 1].GetValue<string>();
                        if (!string.IsNullOrWhiteSpace(codPersona))
                        {
                            codPersonasExcel.Add(codPersona);
                        }
                    }

                    // Consultar en la BD cuáles ya existen
                    var personasExistentes = _context.Personas
                        .Where(p => codPersonasExcel.Contains(p.CodPersona))
                        .Select(p => p.CodPersona)
                        .ToHashSet();

                    var usuariosExistentes = _context.Usuarios
                        .Where(u => codPersonasExcel.Contains(u.CodPersona))
                        .Select(u => u.CodPersona)
                        .ToHashSet();

                    for (int fila = 2; fila <= filas; fila++)
                    {
                        string codPersonaExcel = hoja.Cells[fila, 1].GetValue<string>();

                        if (string.IsNullOrWhiteSpace(codPersonaExcel))
                            continue; // Saltar filas vacías

                        if (!personasExistentes.Contains(codPersonaExcel))
                        {
                            var nuevaPersona = new Persona
                            {
                                CodPersona = codPersonaExcel,
                                Nombre = hoja.Cells[fila, 2].GetValue<string>(),
                                Sexo = hoja.Cells[fila, 3].GetValue<string>(),
                                FecNacimiento = hoja.Cells[fila, 4].GetValue<DateTime>(),
                                EstadoCivil = hoja.Cells[fila, 5].GetValue<string>(),
                                Email = hoja.Cells[fila, 6].GetValue<string>(),
                                FecAlta = DateTime.Now
                            };
                            listaPersonas.Add(nuevaPersona);
                           

                        }

                        if (!usuariosExistentes.Contains(codPersonaExcel))
                        {
                            var nuevoUsuario = new Usuario
                            {
                                CodUsuario = codPersonaExcel,
                                CodPersona = codPersonaExcel,
                                Clave = codPersonaExcel,
                                CodGrupo = hoja.Cells[fila, 8].GetValue<string>(),
                                FecCreacion = DateTime.Now
                            };
                            listaUsuarios.Add(nuevoUsuario);
                        }
                    }
                }
            }

            if (listaPersonas.Any())
            {
                _context.AddRange(listaPersonas);
            }

            if (listaUsuarios.Any())
            {
                _context.AddRange(listaUsuarios);
            }

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Carga masiva realizada con éxito.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarDatos([FromBody] ProcesarDatosRequest request)
        {
            var personasDto = request.Personas;
            decimal carrera = request.Carrera;

            if (personasDto == null || !personasDto.Any())
            {
                return BadRequest("No se recibieron datos para procesar.");
            }
            if (personasDto == null || !personasDto.Any())
            {
                return BadRequest("No se recibieron datos para procesar.");
            }

            var listaPersonas = new List<Persona>();
            var listaUsuarios = new List<Usuario>();
            var listaDatoAcademico = new List<DatosAcademico>();
            var errores = new List<object>(); // Lista para errores por fila

            // Obtener todos los códigos de persona del JSON
            var codPersonasExcel = personasDto.Select(p => p.CodPersona).ToHashSet();

            // Consultar en la BD cuáles ya existen
            var datoAcademicoExis = _context.DatosAcademicos
                .Where(p => codPersonasExcel.Contains(p.CodUsuario) && p.IdCarrera == carrera)
                .Select(p => p.CodUsuario)
                .ToHashSet();

            var personasExistentes = _context.Personas
                .Where(p => codPersonasExcel.Contains(p.CodPersona))
                .Select(p => p.CodPersona)
                .ToHashSet();

            var usuariosExistentes = _context.Usuarios
                .Where(u => codPersonasExcel.Contains(u.CodPersona))
                .Select(u => u.CodPersona)
                .ToHashSet();

            int fila = 1;

            foreach (var personaDto in personasDto)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(personaDto.CodPersona))
                        throw new Exception("CodPersona vacío.");

                    if (string.IsNullOrWhiteSpace(personaDto.Nombre))
                        throw new Exception("Nombre vacío.");

                    if (!string.IsNullOrWhiteSpace(personaDto.Email) &&
                        !Regex.IsMatch(personaDto.Email, @"^\S+@\S+\.\S+$"))
                        throw new Exception("Email inválido.");

                    if (!personaDto.FechaNacimiento.HasValue || personaDto.FechaNacimiento.Value == DateTime.MinValue)
                        throw new Exception("Fecha de nacimiento vacía o inválida.");


                    if (!personasExistentes.Contains(personaDto.CodPersona))
                    {
                        var nuevaPersona = new Persona
                        {
                            CodPersona = personaDto.CodPersona,
                            Nombre = personaDto.Nombre,
                            Sexo = personaDto.Sexo,
                            FecNacimiento = personaDto.FechaNacimiento,
                            EstadoCivil = personaDto.EstadoCivil,
                            Email = personaDto.Email,
                            FecAlta = DateTime.Now
                        };
                        listaPersonas.Add(nuevaPersona);

                        await EnviarCorreoAsync(nuevaPersona.Email, nuevaPersona.Nombre, nuevaPersona.CodPersona);
                    }

                    if (!usuariosExistentes.Contains(personaDto.CodPersona))
                    {
                        var nuevoUsuario = new Usuario
                        {
                            CodUsuario = personaDto.CodPersona,
                            CodPersona = personaDto.CodPersona,
                            Clave = personaDto.CodPersona,
                            CodGrupo = personaDto.CodGrupo,
                            FecCreacion = DateTime.Now
                        };
                        listaUsuarios.Add(nuevoUsuario);
                    }

                    if (!datoAcademicoExis.Contains(personaDto.CodPersona))
                    {
                        var nuevoUserCarrera = new DatosAcademico
                        {
                            CodUsuario = personaDto.CodPersona,
                            IdCentroEstudio = 1,
                            IdCarrera = Convert.ToInt32(carrera)
                        };
                        listaDatoAcademico.Add(nuevoUserCarrera);
                    }
                }
                catch (Exception ex)
                {
                    errores.Add(new
                    {
                        Fila = fila,
                        CodPersona = personaDto.CodPersona,
                        Error = ex.Message
                    });
                }

                fila++;
            }

            if (listaPersonas.Any())
            {
                _context.AddRange(listaPersonas);
            }

            if (listaUsuarios.Any())
            {
                _context.AddRange(listaUsuarios);
            }
            if (listaDatoAcademico.Any())
            {
                _context.AddRange(listaDatoAcademico);
            }
            await _context.SaveChangesAsync();
            if (errores.Any())
            {
                return Ok(new
                {
                    mensaje = "Algunos registros fallaron.",
                    errores
                });
            }

            return Ok(new { mensaje = "Carga masiva realizada con éxito." });
        }


        [HttpGet]
        public IActionResult FormatoBase()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // EPPlus requiere esto

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Formato Base de la Planilla Masiva");

                // Encabezados
                worksheet.Cells["A1"].Value = "CodPersona";
                worksheet.Cells["B1"].Value = "Nombre";
                worksheet.Cells["C1"].Value = "Sexo"; 
                worksheet.Cells["D1"].Value = "FechaNacimiento";
                worksheet.Cells["E1"].Value = "EstadoCivil";
                worksheet.Cells["F1"].Value = "Email";
                worksheet.Cells["G1"].Value = "Clave";
                worksheet.Cells["H1"].Value = "CodGrupo";
                //worksheet.Cells["I1"].Value = "Carrera";

                // Datos de ejemplo
                worksheet.Column(4).Style.Numberformat.Format = "dd/MM/yyyy";
                // worksheet.Cells["A2"].Value = "";
                //worksheet.Cells["G2"].Value = "=A1";

                worksheet.Cells["A1:I1"].Style.Font.Bold = true; // Negrita para encabezados

                // Convertimos el archivo a bytes
                var excelBytes = package.GetAsByteArray();

                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Formato_Base_Planilla_Masiva.xlsx");
            }
        }

        [HttpGet]
        public IActionResult CargarExcel()
        {
            return View();
        }


        public static async Task EnviarCorreoAsync(string destinatario, string nombreUsuario, string ci)
        {
            try
            {
                var remitente = "uaafacyt0@gmail.com"; 
                var contrasenaApp = "lizz wiif vfww xhhs"; 

                var mensaje = new MailMessage();
                mensaje.From = new MailAddress(remitente, "EnvioCorreoUAA");
                mensaje.To.Add(destinatario);
                mensaje.Subject = "Bienvenido a nuestra plataforma";
                ///mensaje.Body = $"Hola {nombreUsuario},\n\nTu usuario ha sido creado exitosamente.\nSaludos.";
                mensaje.Body = $"Hola {nombreUsuario},\n\n" +
               $"Tu usuario ha sido creado exitosamente en la plataforma para Graduados de la UAA.\n" +
               $"Podés iniciar sesión en el siguiente enlace:\n" +
               $"https://localhost:7062/Login\n\n" +
               $"Usuario: {ci}\n" +
               $"Contraseña: {ci}\n\n" +
               $"Debes cambiar tu contraseña en el primer ingreso.\n\n" +
               $"Saludos.";

                mensaje.IsBodyHtml = false;

                var smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential(remitente, contrasenaApp);
                smtp.EnableSsl = true;

                await smtp.SendMailAsync(mensaje);

                Console.WriteLine("Correo enviado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo:");
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
