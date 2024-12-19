using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;

namespace SistemaBase.Controllers
{
    public class CambioOficinaController : Controller
    {
        private readonly DbvinDbContext _dbContext;

        public CambioOficinaController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
       //[TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSCAMBO", "Index", "CambioOficina" })]
        public IActionResult Index()
        {

            ViewBag.Oficina = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina"); 
            ViewBag.NuevaOficina = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina");

            ViewBag.TipoSolicitud = new SelectList(_dbContext.RmTipoSolicituds, "TipoSolicitud", "DescripSolicitud");
            //ViewBag.Autorizante = new SelectList(_dbContext.RmAutorizantes, "IdAutorizante", "DescripAutorizante");
            ViewBag.TipoDocumento = new SelectList(_dbContext.RmTiposDocumentos, "TipoDocumento", "DescripTipoDocumento");
            ViewBag.Documento = new SelectList(_dbContext.RmTiposDocumentos, "TipoDocumento", "DescripTipoDocumento");
            return View();
        }

        public async Task<IActionResult> Get(decimal nEntrada)
        {
            try
            {
                var rmMesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                if (rmMesaEntrada != null)
                {
                    ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", rmMesaEntrada?.EstadoEntrada ?? 0);
                    ViewBag.Oficina = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina",rmMesaEntrada?.CodigoOficina??0);
                    ViewBag.NuevaOficina = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina", rmMesaEntrada?.CodOficinaRetiro ?? 0);
                    ViewBag.TipoSolicitud = new SelectList(_dbContext.RmTipoSolicituds, "TipoSolicitud", "DescripSolicitud", rmMesaEntrada?.TipoSolicitud);
                    // cargar para edicion
                    CambioEstadoCustom inscripcion = new()
                    {
                        NumeroEntrada = nEntrada,
                        NroDocPresentador = rmMesaEntrada?.NroDocumentoPresentador ?? "",
                        CodigoOficina = rmMesaEntrada?.CodigoOficina ?? 0,
                        CodOficinaRetiro = rmMesaEntrada?.CodOficinaRetiro ??0,
                        NomPresentador = rmMesaEntrada?.NombrePresentador,
                        NroDocRetirador = rmMesaEntrada?.NroDocumentoRetirador,
                        NomRetirador = rmMesaEntrada?.NombreRetirador,
                        NroDocTitular = rmMesaEntrada?.NroDocumentoTitular,
                        NomTitular = rmMesaEntrada?.NomTitular,
                        TipoSolicitud = rmMesaEntrada?.TipoSolicitud,
                        NroFormulario = rmMesaEntrada?.NroFormulario,
                        EstadoEntrada = rmMesaEntrada?.EstadoEntrada ?? 0,
                        NroLiquidacion = rmMesaEntrada?.NumeroLiquidacionLetras??"",
                        FechaOperacion = DateTime.Now

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

        public async Task<IActionResult> Create(CambioEstadoCustom cambioEstado)
        {
            try
            {
               var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m => m.NumeroEntrada == cambioEstado.NumeroEntrada);
                if (mesaEntrada != null) {
                   
                    // Obtener los movimientos relacionados con la entrada
                    var movimientos = _dbContext.RmMovimientosDocs
                        .OrderByDescending(o => o.FechaOperacion)
                        .Where(m => m.NroEntrada == cambioEstado.NumeroEntrada)
                        .ToList();
                  
                    // Verificar el estado actual de la entrada y retroceder según lo necesario
                    if (mesaEntrada.EstadoEntrada == 22 && movimientos.Count > 0)
                    {
                        decimal? estadoAnterior = null;

                        // Iterar a través de los movimientos para encontrar el estado anterior válido
                        foreach (var movimiento in movimientos)
                        {
                            if (movimiento.EstadoEntrada != null && movimiento.EstadoEntrada!=22)
                            {
                                estadoAnterior = movimiento.EstadoEntrada;
                                break;
                            }
                        }
                        // Retroceder un estado de la lista de movimientos
                        if (estadoAnterior != null)
                        {
                            mesaEntrada.EstadoEntrada = estadoAnterior;
                        }
                    }
                    else if (mesaEntrada.EstadoEntrada == 38 && movimientos.Count > 2)
                    {
                        // Retroceder tres estados de la lista de movimientos
                        mesaEntrada.EstadoEntrada = movimientos[3].EstadoEntrada;
                    }
                    else if (mesaEntrada.EstadoEntrada == 28 && movimientos.Count > 1)
                    {
                        // Retroceder dos estados de la lista de movimientos
                        mesaEntrada.EstadoEntrada = movimientos[2].EstadoEntrada;
                    }
                    else if (mesaEntrada.EstadoEntrada == 34 && movimientos.Count > 0)
                    {
                        // Retroceder un estado de la lista de movimientos
                        mesaEntrada.EstadoEntrada = movimientos[1].EstadoEntrada;
                    }

                    RmCambiosOficina rmCambio = new()
                    {
                        NroEntrada = cambioEstado.NumeroEntrada.ToString(),
                        CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                        FechaOperacion = DateTime.Now,
                        CodOperacion = "09", //parametro para mesa salida
                        OficRetiroAnt = mesaEntrada?.CodOficinaRetiro,
                        OficRetiroNuev = cambioEstado?.CodOficinaRetiro,
                        Observacion = cambioEstado?.Observacion
                    };
                    await _dbContext.AddAsync(rmCambio);

                    mesaEntrada.CodOficinaRetiro = cambioEstado.CodOficinaRetiro;
                    _dbContext.Update(mesaEntrada);

                    RmMovimientosDoc rmmovimientos = new()
                    {
                        NroEntrada = cambioEstado.NumeroEntrada,
                        CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                        FechaOperacion = DateTime.Now,
                        CodOperacion = "09", //parametro para mesa salida
                        NroMovimientoRef = cambioEstado.NumeroEntrada.ToString(),
                        EstadoEntrada = 48 //Cambio Oficina de Retiro
                    };
                    await _dbContext.AddAsync(rmmovimientos);
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                return View("Error");

            }
            return RedirectToAction("Index");
        }

    }
}
