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
using System.Diagnostics.Metrics; // Asegúrate de ajustar el espacio de nombres según tu aplicación

public class IPMdivRegistralController : Controller
{
    private readonly DbvinDbContext _context; // Reemplaza 'YourDbContext' con el nombre de tu DbContext

    public IPMdivRegistralController(DbvinDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GenerarPdf(Reportes parametros)
    {
        // Obtiene los datos filtrados según los parámetros del modelo
        var datosReporte = GetReporteData(parametros);

        ViewBag.FechaActual = parametros.FechaActual;
        ViewBag.TotalIngresado = parametros.TotalIngresado;
        ViewBag.FechaDesde = parametros.FechaDesde;
        ViewBag.FechaHasta = parametros.FechaHasta;
        ViewBag.Usuario = parametros.Usuario;

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

    // Método para obtener los datos del reporte filtrados
    private Reportes GetReporteData(Reportes parametros)
    {
        // Obtener el código del grupo OPREG
        var codGrupoOPREG = "OPREG";

        // Filtrar movimientos por grupo de usuarios (OPREG), estados y rango de fechas
        var movimientos = _context.RmMovimientosDocs
            .Where(m => m.FechaOperacion >= parametros.FechaDesde
                     && m.FechaOperacion <= parametros.FechaHasta
                     && (m.EstadoEntrada == 4
                         || m.EstadoEntrada == 3 || m.EstadoEntrada == 2)
                     && _context.Usuarios.Any(u => u.CodUsuario == m.CodUsuario
                                                   && u.CodGrupo == codGrupoOPREG))
            .ToList();

        // Agrupar y contar los movimientos por estado
        var resultados = movimientos
            .Join(_context.Usuarios, m => m.CodUsuario, u => u.CodUsuario, (m, u) => new { Movimiento = m, Usuario = u })
            .Join(_context.Personas, mu => mu.Usuario.CodPersona, p => p.CodPersona, (mu, p) => new { Movimiento = mu.Movimiento, Persona = p })
            .GroupBy(mu => mu.Movimiento.CodUsuario)
            .Select(g => new Reportes.ResultadoReporte
            {
                Usuario = g.Key,
                NombreUsuario = g.FirstOrDefault()?.Persona.Nombre, // Obtener el nombre del primer usuario encontrado (debería ser el mismo para todos los registros agrupados),
                AprobRegistrador = g.Count(m => m.Movimiento.EstadoEntrada == 4),
                NNRegistrador = g.Count(m => m.Movimiento.EstadoEntrada == 3),
                ObsRegistrador = g.Count(m => m.Movimiento.EstadoEntrada == 2)
            }).ToList();

        var usuariosOPREG = _context.Usuarios
            .Where(u => u.CodGrupo == codGrupoOPREG)
            .ToList();

        // Mapear los datos al modelo de vista ReportesViewModel
        var datosReporte = new Reportes
        {
            FechaActual = DateTime.Now,
            Resultados = resultados,
            UsuariosOPREG = usuariosOPREG,
            Usuario = parametros.Usuario,
        };

        return datosReporte;
    }

}
