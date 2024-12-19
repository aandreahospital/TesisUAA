using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;

namespace SistemaBase.Controllers
{
    public class AnulacionEntradaController : Controller
    {
        private readonly DbvinDbContext _dbContext;

        public AnulacionEntradaController(DbvinDbContext context)
        {
            _dbContext = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSANUL", "Index", "AnulacionEntrada" })]
        public IActionResult Index()
        {
            ViewBag.MotivoAnulacion = new SelectList(_dbContext.RmMotivosAnulacions, "IdMotivoAnulacion", "DescripMotivo");

            return View();
        }
        public async Task<IActionResult> Get(decimal nEntrada)
        {
            var mesaEntrada = await _dbContext.RmMesaEntrada.SingleOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
            if (mesaEntrada == null)
            {
                return NotFound();
            }

            ViewBag.TiposDocumentos = new SelectList(_dbContext.RmTiposDocumentos, "TipoDocumento", "DescripTipoDocumento", mesaEntrada.TipoDocumento);
            ViewBag.Oficinas = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina", mesaEntrada.CodigoOficina);
            ViewBag.TiposSolicitud = new SelectList(_dbContext.RmTipoSolicituds, "TipoSolicitud", "DescripSolicitud", mesaEntrada.TipoSolicitud);
            ViewBag.EstadosEntrada = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada.EstadoEntrada);
            ViewBag.MotivoAnulacion = new SelectList(_dbContext.RmMotivosAnulacions, "IdMotivoAnulacion", "DescripMotivo", mesaEntrada.IdMotivoAnulacion);

            AnulacionEntrada anulacion = new()
            {
                NroEntrada = mesaEntrada.NumeroEntrada,
                FechaAnulacion = mesaEntrada.FechaAnulacion,
                NroFormulario = mesaEntrada.NroFormulario,
                CodOficina = mesaEntrada.CodigoOficina,
                TipoSolicitud = mesaEntrada.TipoSolicitud,
                EstadoEntrada = mesaEntrada.EstadoEntrada,
                NombreTit = mesaEntrada.NomTitular,
                IdMotivo = mesaEntrada.IdMotivoAnulacion

            };
            return View("Index", anulacion);
        }



        [HttpPost]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
       // [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMANUENT", "Create", "AnulacionEntrada" })]


        public async Task<IActionResult> Create( AnulacionEntrada anulacionEntrada)
        {
            try
            {
                var marcaNA = _dbContext.ParametrosGenerales.FirstOrDefault(m => m.Parametro == "DIR_IMGMARCA_NA");
                var senalNA = _dbContext.ParametrosGenerales.FirstOrDefault(m => m.Parametro == "DIR_IMGSENAL_NA");
                var motivoAnulacion = _dbContext.RmMotivosAnulacions.FirstOrDefault(m=>m.DescripMotivo== "RENUNCIA EXPRESA DEL TITULAR");
                if(motivoAnulacion.IdMotivoAnulacion== anulacionEntrada.IdMotivo)
                {
                    var result = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NumeroEntrada == anulacionEntrada.NroEntrada);
                    result.MarcaNombre = marcaNA?.Valor??"";
                    result.SenalNombre = senalNA?.Valor??"";
                    _dbContext.Update(result);
                    var idMarca = result?.IdMarca ?? 0;
                    var anulaciones = _dbContext.RmAnulacionesMarcas.FirstOrDefault(a => a.IdMarca == idMarca);
                    if (anulaciones == null)
                    {
                        RmAnulacionesMarca Addanulaciones = new()
                        {
                            IdMarca = idMarca,
                            IdMotivoAnulacion = anulacionEntrada.IdMotivo,
                            IdUsuario = User.Identity.Name,
                            FechaAlta = DateTime.Now
                        };
                        await _dbContext.AddAsync(Addanulaciones);
                        await  _dbContext.SaveChangesAsync();
                    }


                }
                
                
                var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m=>m.NumeroEntrada== anulacionEntrada.NroEntrada);
                var estado = _dbContext.RmEstadosEntrada.FirstOrDefault(e=>e.DescripEstado== "Anulado");
                mesaEntrada.IdMotivoAnulacion= anulacionEntrada.IdMotivo; 
                mesaEntrada.EstadoEntrada = estado.CodigoEstado;
                _dbContext.Update(mesaEntrada);

                RmMovimientosDoc movimientos = new()
                {
                    NroEntrada = anulacionEntrada.NroEntrada,
                    CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                    FechaOperacion = DateTime.Now,
                    CodOperacion = "08", //parametro para mesa salida
                    NroMovimientoRef = anulacionEntrada.NroEntrada.ToString(),
                    EstadoEntrada = mesaEntrada.EstadoEntrada
                };
                await _dbContext.AddAsync(movimientos);
                await _dbContext.SaveChangesAsync();


            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RmMesaEntradumExists(anulacionEntrada.NroEntrada))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }
        private bool RmMesaEntradumExists(decimal id)
        {
            return (_dbContext.RmMesaEntrada?.Any(e => e.NumeroEntrada == id)).GetValueOrDefault();
        }

        //[HttpGet("Get")]
        //public IActionResult Get(int numeroEntrada)
        //{
        //    try
        //    {
        //        var data = from rm in _context.RmMesaEntrada
        //                   join ts in _context.RmTipoSolicituds on rm.TipoSolicitud equals ts.TipoSolicitud
        //                   join est in _context.RmEstadosEntrada on rm.EstadoEntrada equals est.CodigoEstado
        //                   join of in _context.RmOficinasRegistrales on rm.CodigoOficina equals of.CodigoOficina
        //                   join mn in _context.RmMotivosAnulacions on rm.IdMotivoAnulacion equals mn.IdMotivoAnulacion
        //                   where rm.NumeroEntrada == numeroEntrada
        //                   select new
        //                   {
        //                       rm.FechaAnulacion,
        //                       rm.NroFormulario,
        //                       rm.CodigoOficina,
        //                       of.DescripOficina,
        //                       rm.TipoSolicitud,
        //                       ts.DescripSolicitud,
        //                       rm.EstadoEntrada,
        //                       est.DescripEstado,
        //                       rm.NomTitular,
        //                       rm.IdMotivoAnulacion,
        //                       mn.DescripMotivo
        //                   };



        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
