using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;

namespace SistemaBase.Controllers
{
    public class ForoControlController : Controller
    {

        private readonly DbvinDbContext _context;

        public ForoControlController(DbvinDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return _context.ForoDebates != null ?
               View(await _context.ForoDebates.AsNoTracking().ToListAsync()) :
               Problem("Entity set 'DbvinDbContext.Formas'  is null.");
        }


        public ActionResult AbrirForo()
        {

            return View("AddForo");

        }

        [HttpPost]
        public async Task<IActionResult> AddForo(ForoDebate foroDebate)
        {
            try
            {
                ForoDebate addforoDebate = new()
                {
                    CodUsuario = User.Identity.Name,
                    Titulo = foroDebate?.Titulo,
                    Descripcion = foroDebate?.Descripcion,
                    Adjunto = foroDebate?.Adjunto,
                    Estado = "S"
                };
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
                    Adjunto = experiencia?.Adjunto ?? "",
                    Estado = experiencia?.Estado ?? ""

                };

                return View("EditForo", foro);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddEditForo(ForoDebate foroDebate)
        {
            try
            {
                var foros = _context.ForoDebates.FirstOrDefault(m => m.IdForoDebate == foroDebate.IdForoDebate);
                if (foros != null)
                {
                    foros.CodUsuario = foroDebate.CodUsuario;
                    foros.Titulo = foroDebate?.Titulo;
                    foros.Descripcion = foroDebate?.Descripcion;
                    foros.Adjunto = foroDebate?.Adjunto;
                    foros.Estado = foroDebate?.Estado;
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



    }
}
