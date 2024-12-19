using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using System.Security.Claims;

namespace SistemaBase.Controllers
{
    [Authorize]
    public class MedidaCautelarCustomController : Controller
    {
        private readonly DbvinDbContext _context;

        public MedidaCautelarCustomController(DbvinDbContext context)
        {
            _context = context;
        }
        // GET: MedidasCautelaresController
        public ActionResult Index()
        {
            var idMedida = _context.RmMedidasPrendas.Max(m => m.IdMedida) + 1;
            var usuario = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fechaacto = DateTime.Now;
            var nroentrada = _context.RmMesaEntrada.Max(m => m.NumeroEntrada) + 1;

            //Mientras le pongo instrumentos genericos en una lista
            var instrumentos = new List<SelectListItem>();
            instrumentos.Add(new SelectListItem() { Text = "Instrumento 1", Value = "1" });
            instrumentos.Add(new SelectListItem() { Text = "Instrumento 2", Value = "2" });

            var embargos = new List<SelectListItem>();
            embargos.Add(new SelectListItem() { Text = "Embargo 1", Value = "1" });
            embargos.Add(new SelectListItem() { Text = "Embargo 2", Value = "2" });

            MedidaCautelarCustom medidasCautelaresCustom = new MedidaCautelarCustom();
            medidasCautelaresCustom.IdMedida = idMedida;
            medidasCautelaresCustom.CodUsuario = usuario;
            medidasCautelaresCustom.Fecha_Operacion = fechaacto;
            medidasCautelaresCustom.Nro_Entrada = nroentrada.ToString();
            medidasCautelaresCustom.Instrumento = instrumentos;
            medidasCautelaresCustom.Tipo_Embargo = embargos;

            return View(medidasCautelaresCustom);


        }
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RmMedidasPrenda rmMedidas)
        {
            try
            {
                var id = _context.RmMedidasPrendas.Max(m => m.IdMedida) + 1;

                rmMedidas.IdMedida = id;

                _context.Add(rmMedidas);
                await _context.SaveChangesAsync();
                return Ok("Se han guardado los cambios");



            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar la medida cautelar: " + ex.Message);
                return BadRequest("Error al agregar la medida cautelar " + ex.Message);
            }
        }
    }
}
