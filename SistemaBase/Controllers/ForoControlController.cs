using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using System.Security.Claims;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace SistemaBase.Controllers
{
    public class ForoControlController : Controller
    {

        private readonly Models.UAADbContext _context;

        public ForoControlController(Models.UAADbContext context)
        {
            _context = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "SCFORO", "Index", "ForoControl" })]

        public async Task<IActionResult> Index()
        {

            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            string displayStyle = (roleClaim == "ADMIN") ? "" : "display:none;";

            ViewBag.StyleAdmin = displayStyle;

            return _context.ForoDebates != null ?
               View(await _context.ForoDebates.AsNoTracking().ToListAsync()) :
               Problem("Entity set 'DbvinDbContext.ForoDebates'  is null.");
        }


        public ActionResult AbrirForo()
        {

            return View("AddForo");

        }

        [HttpPost]
        public async Task<IActionResult> AddForo(ForoDebate foroDebate, IFormFile ArchivoPDF)
        {
            try
            {
                ForoDebate addforoDebate = new()
                {
                    CodUsuario = User.Identity.Name,
                    Titulo = foroDebate?.Titulo,
                    Descripcion = foroDebate?.Descripcion,
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

                        addforoDebate.Adjunto = memoryStream.ToArray();
                    }
                }

                _context.Add(addforoDebate);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                    return NotFound();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AbrirEditForo(int id)
        {
            var experiencia = _context.ForoDebates.Find(id);
            if (experiencia == null)
            {
                return NotFound();
            }
            else
            {
                ForoDebate foro = new()
                {
                    IdForoDebate = experiencia.IdForoDebate,
                    CodUsuario = experiencia?.CodUsuario ?? "",
                    Titulo = experiencia?.Titulo ?? "",
                    Descripcion = experiencia?.Descripcion ?? "",
                    Adjunto = experiencia?.Adjunto,
                    Estado = experiencia?.Estado ?? ""

                };

                return View("EditForo", foro);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddEditForo(ForoDebate foroDebate, IFormFile ArchivoPDF)
        {
            try
            {
                var foros = _context.ForoDebates.FirstOrDefault(m => m.IdForoDebate == foroDebate.IdForoDebate);
                if (foros != null)
                {
                    foros.CodUsuario = foroDebate.CodUsuario;
                    foros.Titulo = foroDebate?.Titulo;
                    foros.Descripcion = foroDebate?.Descripcion;
                   // foros.Adjunto = foroDebate?.Adjunto;
                    foros.Estado = foroDebate?.Estado;



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

                            foros.Adjunto = memoryStream.ToArray();
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
            var foros = _context.ForoDebates.Find(id);
            if (foros == null)
            {
                return NotFound();
            }
            _context.ForoDebates.Remove(foros);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: DatosLaborale/Delete/5
        public async Task<IActionResult>Delete(int IdForoDebate)
        {

            var foro = await _context.ForoDebates
            .FindAsync(IdForoDebate);
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
        public async Task<IActionResult> DeleteConfirmed(int IdForoDebate)
        {

            var foros = _context.ForoDebates.Find(IdForoDebate);
            if (foros == null)
            {
                return NotFound();
            }
            _context.ForoDebates.Remove(foros);
            _context.SaveChanges();
            return RedirectToAction("Index");
           

        }



    }
}
