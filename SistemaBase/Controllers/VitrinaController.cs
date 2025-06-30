using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using System.Security.Claims;

namespace SistemaBase.Controllers
{
    public class VitrinaController : Controller
    {
        private readonly Models.UAADbContext _context;

        public VitrinaController(Models.UAADbContext context)
        {
            _context = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "SCVITRI", "Index", "Vitrina" })]

        public async Task<IActionResult> Index()
        {
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            string displayStyle = (roleClaim == "ADMIN") ? "" : "display:none;";

            ViewBag.StyleAdmin = displayStyle;

            return _context.OfertaAcademicas != null ?
              View(await _context.OfertaAcademicas.AsNoTracking().ToListAsync()) :
              Problem("Entity set 'DbvinDbContext.OfertaAcademicas'  is null.");
        }



        public IActionResult AbrirAdjunto(int IdOfertaAcademica)
        {
            var foroAdjunto = _context.OfertaAcademicas.SingleOrDefault(me => me.IdOfertaAcademica == IdOfertaAcademica);

            if (foroAdjunto != null && foroAdjunto.Adjunto != null && foroAdjunto.Adjunto.Length > 0)
            {
                string contentType = "application/octet-stream"; // Tipo genérico por defecto
                string defaultFileName = "archivo_desconocido";
                bool esVisualizable = false;

                // Detectar el tipo de archivo por los primeros bytes
                if (foroAdjunto.Adjunto.Length > 4)
                {
                    byte[] headerBytes = foroAdjunto.Adjunto.Take(4).ToArray();
                    string headerHex = BitConverter.ToString(headerBytes).Replace("-", "").ToUpper();

                    contentType = headerHex switch
                    {
                        "25504446" => "application/pdf", // PDF
                        "FFD8FFDB" or "FFD8FFE0" => "image/jpeg", // JPG
                        "89504E47" => "image/png", // PNG
                        "47494638" => "image/gif", // GIF
                        "D0CF11E0" => "application/msword", // Word o Excel antiguos (DOC, XLS)
                        "504B0304" => DetectarTipoOpenXml(foroAdjunto.Adjunto), // DOCX, XLSX o PPTX
                        _ => "application/octet-stream"
                    };

                    esVisualizable = contentType is "application/pdf" or "image/jpeg" or "image/png" or "image/gif";

                    defaultFileName = contentType switch
                    {
                        "application/pdf" => "documento.pdf",
                        "image/jpeg" => "imagen.jpg",
                        "image/png" => "imagen.png",
                        "image/gif" => "imagen.gif",
                        "application/msword" => "documento.doc",
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => "documento.docx",
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => "hoja_calculo.xlsx",
                        "application/vnd.openxmlformats-officedocument.presentationml.presentation" => "presentacion.pptx",
                        _ => "archivo_descargado"
                    };
                }

                if (esVisualizable)
                {
                    // Retorna el archivo para abrirlo en el navegador
                    return File(foroAdjunto.Adjunto, contentType);
                }
                else
                {
                    // Retorna el archivo forzando la descarga
                    return File(foroAdjunto.Adjunto, contentType, defaultFileName);
                }
            }

            return NotFound();
        }


        private string DetectarTipoOpenXml(byte[] archivo)
        {
            string contenidoTexto = System.Text.Encoding.ASCII.GetString(archivo);
            if (contenidoTexto.Contains("word"))
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; // DOCX
            if (contenidoTexto.Contains("xl/") || contenidoTexto.Contains("xl\\"))
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; // XLSX
            if (contenidoTexto.Contains("ppt"))
                return "application/vnd.openxmlformats-officedocument.presentationml.presentation"; // PPTX

            return "application/octet-stream";
        }
        public ActionResult AbrirOferta()
        {

            return View("AddOferta");

        }

        [HttpPost]
        public async Task<IActionResult> AddOferta(OfertaAcademica ofertaAcademica, IFormFile ArchivoPDF)
        {
            try
            {
                OfertaAcademica addofertaAcademica = new()
                {
                    CodUsuario = User.Identity.Name,
                    Titulo = ofertaAcademica?.Titulo,
                    Descripcion = ofertaAcademica?.Descripcion,
                    FechaCierre = ofertaAcademica.FechaCierre,
                    FechaCreacion = DateTime.Now,
                    Estado = "S"
                };

                const int maxFileSize = 5 * 1024 * 1024; // 5 MB
                //if (ArchivoPDF == null || ArchivoPDF.Length == 0)
                //{
                //    return Json(new { Success = false, ErrorMessage = "Por favor, selecciona una archivo en PDF" });
                //}
                var allowedExtensions = new[] { ".pdf" };
                if (ArchivoPDF != null && ArchivoPDF.Length > 0)
                {
                    var fileExtension = Path.GetExtension(ArchivoPDF.FileName).ToLower();
                    //if (!allowedExtensions.Contains(fileExtension))
                    //{
                    //    return Json(new { Success = false, ErrorMessage = "Error, el archivo debe ser en PDF." });
                    //}
                    //if (ArchivoPDF.Length > maxFileSize)
                    //{
                    //    return Json(new { Success = false, ErrorMessage = "El tamaño del archivo no debe ser mayor de 5MB" });
                    //}
                    using (var memoryStream = new MemoryStream())
                    {
                        await ArchivoPDF.CopyToAsync(memoryStream);

                        addofertaAcademica.Adjunto = memoryStream.ToArray();
                    }
                }

                _context.Add(addofertaAcademica);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AbrirEditOferta(int id)
        {
            var oferta = _context.OfertaAcademicas.Find(id);
            if (oferta == null)
            {
                return NotFound();
            }
            else
            {
                OfertaAcademica ofertaAcademica = new()
                {
                    IdOfertaAcademica = oferta.IdOfertaAcademica,
                    CodUsuario = oferta?.CodUsuario ?? "",
                    Titulo = oferta?.Titulo ?? "",
                    Descripcion = oferta?.Descripcion ?? "",
                    FechaCreacion = oferta?.FechaCreacion,
                    FechaCierre = oferta?.FechaCierre,
                    Adjunto = oferta?.Adjunto,
                    Estado = oferta?.Estado ?? ""

                };

                return View("EditOfertaAca", ofertaAcademica);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddEditOferta(OfertaAcademica oferta, IFormFile ArchivoPDF)
        {
            try
            {
                var ofertas = _context.OfertaAcademicas.FirstOrDefault(m => m.IdOfertaAcademica == oferta.IdOfertaAcademica);
                if (ofertas != null)
                {
                    ofertas.CodUsuario = oferta.CodUsuario;
                    ofertas.Titulo = oferta?.Titulo;
                    ofertas.Descripcion = oferta?.Descripcion;
                    ofertas.FechaCierre = oferta?.FechaCierre;
                    //ofertas.Adjunto = oferta?.Adjunto;
                    //ofertas.FechaCreacion = oferta?.FechaCreacion;

                    ofertas.Estado = oferta?.Estado;

                    if  (oferta.FechaCierre != null && oferta.FechaCierre <= DateTime.Now) {
                        ofertas.Estado = "N";
                    }
                    const int maxFileSize = 5 * 1024 * 1024; // 5 MB
                   
                    var allowedExtensions = new[] { ".pdf" };
                    if (ArchivoPDF != null && ArchivoPDF.Length > 0)
                    {
                        var fileExtension = Path.GetExtension(ArchivoPDF.FileName).ToLower();
                        //if (!allowedExtensions.Contains(fileExtension))
                        //{
                        //    return Json(new { Success = false, ErrorMessage = "Error, el archivo debe ser en PDF." });
                        //}
                        //if (ArchivoPDF.Length > maxFileSize)
                        //{
                        //    return Json(new { Success = false, ErrorMessage = "El tamaño del archivo no debe ser mayor de 5MB" });
                        //}
                        using (var memoryStream = new MemoryStream())
                        {
                            await ArchivoPDF.CopyToAsync(memoryStream);

                            ofertas.Adjunto = memoryStream.ToArray();
                        }
                    }
                     _context.SaveChanges();
                }

               
                


            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteForo(int id)
        {
            var ofertas = _context.OfertaAcademicas.Find(id);
            if (ofertas == null)
            {
                return NotFound();
            }
            _context.OfertaAcademicas.Remove(ofertas);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int IdOfertaAcademica)
        {

            var foro = await _context.OfertaAcademicas
            .FindAsync(IdOfertaAcademica);
            if (foro == null)
            {
                return NotFound();
            }

            return View("Delete", foro);
            //return View(foro);
        }

        // POST: DatosLaborale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdOfertaAcademica)
        {

            var foros = _context.OfertaAcademicas.Find(IdOfertaAcademica);
            if (foros == null)
            {
                return NotFound();
            }
            _context.OfertaAcademicas.Remove(foros);
            _context.SaveChanges();
            return RedirectToAction("Index");


        }






    }
}
