using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;

namespace SistemaBase.Controllers
{
    public class CambioEstadoController : Controller
    {
        private readonly DbvinDbContext _dbContext;

        public CambioEstadoController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSCAMB", "Index", "CambioEstado" })]

        public IActionResult Index()
        {
            try
            {
                ViewBag.NuevoEstado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado");
                CambioEstadoCustom cambioEsta = new()
                {
                    FechaOperacion= DateTime.Now
                };
                return View(cambioEsta);
            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrió un error: " + ex.Message);

            }
        }
        public async Task<IActionResult> Get(decimal nEntrada)
        {
            try
            {
                var rmMesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                if (rmMesaEntrada != null)
                {
                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", rmMesaEntrada?.EstadoEntrada ?? 0);
                        ViewBag.NuevoEstado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado");
                        ViewBag.TipoSolicitud = new SelectList(_dbContext.RmTipoSolicituds, "TipoSolicitud", "DescripSolicitud", rmMesaEntrada?.TipoSolicitud);
                        // cargar para edicion
                        CambioEstadoCustom inscripcion = new()
                        {
                            NumeroEntrada = nEntrada,
                            NroDocPresentador = rmMesaEntrada?.NroDocumentoPresentador??"",
                            NomPresentador = rmMesaEntrada?.NombrePresentador,
                            NroDocRetirador = rmMesaEntrada?.NroDocumentoRetirador,
                            NomRetirador = rmMesaEntrada?.NombreRetirador,
                            NroDocTitular= rmMesaEntrada?.NroDocumentoTitular,
                            NomTitular = rmMesaEntrada?.NomTitular,
                            TipoSolicitud = rmMesaEntrada?.TipoSolicitud,
                            NroFormulario = rmMesaEntrada?.NroFormulario,
                            EstadoEntrada = rmMesaEntrada?.EstadoEntrada ?? 0,
                            NroLiquidacion = rmMesaEntrada?.NumeroLiquidacionLetras,
                            FechaOperacion= DateTime.Now
                            
                        };
                        return View("Index", inscripcion);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error: " + ex.Message);

            }
        }

        [HttpPost]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSCAMB", "Create", "CambioEstado" })]

        public async Task<IActionResult> Create(CambioEstadoCustom cambioEstado)
        {
            try
            {
                var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m => m.NumeroEntrada == cambioEstado.NumeroEntrada);
              
                    RmCambiosEstado rmCambio = new()
                    {
                        NroMovimiento = _dbContext.RmCambiosEstados.Max(m => m.NroMovimiento) + 1,
                        NroEntrada = cambioEstado.NumeroEntrada.ToString(),
                        CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                        FechaOperacion = DateTime.Now,
                        CodOperacion = "09", //parametro para mesa salida
                        EstadoAnterior = mesaEntrada.EstadoEntrada.ToString(),
                        EstadoNuevo = cambioEstado.NuevoEstado.ToString(),
                        Observacion = cambioEstado.Observacion
                    };
                    await _dbContext.AddAsync(rmCambio);

                    var transaccion = _dbContext.RmTransacciones.OrderByDescending(o=>o.FechaAlta).FirstOrDefault(c => c.NumeroEntrada == cambioEstado.NumeroEntrada);
                    if (transaccion != null && cambioEstado.NuevoEstado==9)
                    {
                        transaccion.EstadoTransaccion = cambioEstado.NuevoEstado.ToString();
                        _dbContext.Update(transaccion);
                    }
                    mesaEntrada.EstadoEntrada = cambioEstado.NuevoEstado;
                        _dbContext.Update(mesaEntrada);
                RmMovimientosDoc movimientos = new()
                {
                    NroEntrada = cambioEstado.NumeroEntrada,
                    CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                    FechaOperacion = DateTime.Now,
                    CodOperacion = "09", //parametro para mesa salida
                    NroMovimientoRef = cambioEstado.NumeroEntrada.ToString(),
                    EstadoEntrada = cambioEstado.NuevoEstado
                };
                await _dbContext.AddAsync(movimientos);
                await _dbContext.SaveChangesAsync();




            }
            catch (Exception ex)
            {
                return View("Error");

            }
            return RedirectToAction("Index");
        }

    }
}
