using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;

namespace SistemaBase.Controllers
{
    public class ComentarioForoController : Controller
    {


        private readonly DbvinDbContext _context;

        public ComentarioForoController(DbvinDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> VerComentarios(int idForo)
        {
            try
            {
                ///var dbComentarios = await _context.Comentarios.FirstOrDefaultAsync(t => t.IdForoDebate == idForo);
                var dbForo = await _context.ForoDebates.FirstOrDefaultAsync(t => t.IdForoDebate == idForo);
                var dbComentarios = await _context.Comentarios.Where(t => t.IdForoDebate == idForo).ToListAsync();

                if (dbComentarios != null && dbForo != null)
                {

                    ForoComentario comentarios = new()
                    {
                        Titulo = dbForo.Titulo,
                        IdForoDebate = idForo,
                        DescripcionForo = dbForo.Descripcion,
                        Adjunto = dbForo.Adjunto,
                        Comentarios = dbComentarios.Select(c => new Comentario
                        {
                            IdComentario = c.IdComentario,
                            CodUsuario = c.CodUsuario,
                            Descripcion = c.Descripcion,
                            FechaComentario = c.FechaComentario
                        }).ToList()

                    };
                    return View("Index", comentarios);

                }

                return Ok();


            }

            catch (Exception ex)
            {
                // Manejar el error de generación de PDF de alguna manera
                return BadRequest("Error al cargar la pagina " + ex.Message);
            }



        }


        public IActionResult AbrirAdjunto(int IdForoDebate)
        {
            var foroAdjunto = _context.ForoDebates.SingleOrDefault(me => me.IdForoDebate == IdForoDebate);

            if (foroAdjunto != null && foroAdjunto.Adjunto != null && foroAdjunto.Adjunto.Length > 0)
            {
                // Retorna el archivo PDF como FileResult
                return File(foroAdjunto.Adjunto, "application/pdf");
            }

            //var foroAdjunto = _context.ForoDebates.SingleOrDefault(me => me.IdForoDebate == IdForoDebate);

            //if (foroAdjunto != null && foroAdjunto.Adjunto != null && foroAdjunto.Adjunto.Length > 0)
            //{
            //    string contentType = "application/octet-stream"; // Tipo genérico por defecto
            //    string defaultFileName = "archivo_desconocido";

            //    // Leer los primeros bytes del archivo para intentar detectar su tipo
            //    if (foroAdjunto.Adjunto.Length > 4)
            //    {
            //        byte[] headerBytes = foroAdjunto.Adjunto.Take(4).ToArray();
            //        string headerHex = BitConverter.ToString(headerBytes).Replace("-", "").ToUpper();

            //        contentType = headerHex switch
            //        {
            //            "25504446" => "application/pdf", // PDF (%PDF)
            //            "D0CF11E0" => "application/vnd.ms-excel", // .xls (OLE format, igual a .doc y .ppt antiguos)
            //            "504B0304" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", // XLSX (ZIP format, igual a .docx y .pptx)
            //            _ => "application/octet-stream"
            //        };

            //        // Asignar un nombre por defecto basado en el tipo de archivo
            //        defaultFileName = contentType switch
            //        {
            //            "application/pdf" => "documento.pdf",
            //            "application/msword" => "documento.doc",
            //            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => "documento.docx",
            //            "application/vnd.ms-powerpoint" => "presentacion.ppt",
            //            "application/vnd.openxmlformats-officedocument.presentationml.presentation" => "presentacion.pptx",
            //            "application/vnd.ms-excel" => "hoja_calculo.xls",
            //            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => "hoja_calculo.xlsx",
            //            _ => "archivo_desconocido"
            //        };
            //    }

            //    // Retorna el archivo con el tipo de contenido detectado
            //    return File(foroAdjunto.Adjunto, contentType, defaultFileName);
        ///}

            return NotFound();
        }


        [HttpGet]
        public IActionResult AbrirAddComentario()
        {

                return View("AddComentario");
            
        }

        [HttpPost]
        public async Task<IActionResult> AddComentario(Comentario comentario)
        {
            try
            {
                Comentario addcomentario = new()
                {
                    CodUsuario = User.Identity.Name,
                    IdForoDebate = comentario.IdForoDebate,
                    Descripcion = comentario?.Descripcion,
                    FechaComentario = DateTime.Now
                };
                _context.Add(addcomentario);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return Ok();
        }


        [HttpGet]
        public IActionResult AbrirEditComentario(int id)
        {
            var dbComen = _context.Comentarios.Find(id);
            if (dbComen == null)
            {
                return NotFound();
            }
            else
            {
                Comentario comen = new()
                {
                    IdForoDebate = dbComen.IdForoDebate,
                    CodUsuario = dbComen?.CodUsuario ?? "",
                    Descripcion = dbComen?.Descripcion ?? "",
                    FechaComentario = dbComen?.FechaComentario

                };

                return View("EditForo", comen);
            }
        }

        [HttpGet]
        public IActionResult DeleteComentario(int id)
        {
            var comen = _context.Comentarios.Find(id);
            if (comen == null)
            {
                return NotFound();
            }
            _context.Comentarios.Remove(comen);
            _context.SaveChanges();
            return Ok();
        }

    }
}
