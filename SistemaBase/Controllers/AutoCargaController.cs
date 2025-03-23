using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SistemaBase.Models;

namespace SistemaBase.Controllers
{
    public class AutoCargaController : Controller
    {

        private readonly DbvinDbContext _context;

        public AutoCargaController(DbvinDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
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


        [HttpGet]
        public IActionResult CargarExcel()
        {
            return View();
        }




    }
}
