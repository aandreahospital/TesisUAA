using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using System.Security.Claims;

namespace SistemaBase.Controllers
{
    [Authorize]
    public class DiseñoRecibidoController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public DiseñoRecibidoController(DbvinDbContext context)
        {
            _dbContext = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMDISASI", "Index", "DiseñoRecibido" })]

        public async Task<IActionResult> Index()
        {
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleClaim == "ADMIN")
            {
                var queryFinal = (from ad in _dbContext.RmAsigDis
                                  join me in _dbContext.RmMesaEntrada on ad.NroEntrada equals me.NumeroEntrada
                                  join ts in _dbContext.RmTipoSolicituds on me.TipoSolicitud equals ts.TipoSolicitud
                                  join ee in _dbContext.RmEstadosEntrada on me.EstadoEntrada equals ee.CodigoEstado
                                  where ee.DescripEstado == "Asignado Diseño" && ad.Desasignado == null /*&& ad.IdUsuarioAsignado == User.Identity.Name*/
                                  orderby ad.FechaAsignada descending
                                  select new
                                  {
                                      NroEntrada = Convert.ToInt32(ad.NroEntrada),
                                      TipoSolicitud = ts.DescripSolicitud,
                                      FechaIngreso = ad.FechaAsignada,
                                      UsuarioAsignado = ad.IdUsuarioAsignado
                                  }).AsEnumerable()
                             .GroupBy(result => result.NroEntrada)
                             .Select(group => group.OrderByDescending(g => g.FechaIngreso).First())
                             .Select(result => new Diseño()
                             {
                                 NroEntrada = result.NroEntrada,
                                 TipoSolicitud = result.TipoSolicitud,
                                 FechaIngreso = result.FechaIngreso,
                                 UsuarioAsignado = result.UsuarioAsignado
                             });

                return View(queryFinal.ToList());
            }
            else
            {
                var queryFinal = (from ad in _dbContext.RmAsigDis
                                  join me in _dbContext.RmMesaEntrada on ad.NroEntrada equals me.NumeroEntrada
                                  join ts in _dbContext.RmTipoSolicituds on me.TipoSolicitud equals ts.TipoSolicitud
                                  join ee in _dbContext.RmEstadosEntrada on me.EstadoEntrada equals ee.CodigoEstado
                                  where ee.DescripEstado == "Asignado Diseño" && ad.IdUsuarioAsignado == User.Identity.Name && ad.Desasignado==null
                                  orderby ad.FechaAsignada descending
                                  select new
                                  {
                                      NroEntrada = Convert.ToInt32(ad.NroEntrada),
                                      TipoSolicitud = ts.DescripSolicitud,
                                      FechaIngreso = ad.FechaAsignada,
                                      UsuarioAsignado = ad.IdUsuarioAsignado
                                  }).AsEnumerable()
                             .GroupBy(result => result.NroEntrada)
                             .Select(group => group.OrderByDescending(g => g.FechaIngreso).First())
                             .Select(result => new Diseño()
                             {
                                 NroEntrada = result.NroEntrada,
                                 TipoSolicitud = result.TipoSolicitud,
                                 FechaIngreso = result.FechaIngreso,
                                 UsuarioAsignado = result.UsuarioAsignado
                             });

                return View(queryFinal.ToList());

            }

        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado([FromBody] List<decimal> selectedNroEntradas)
        {

            foreach (decimal nroEntrada in selectedNroEntradas)
            {
                var mesaEntrada = await _dbContext.RmMesaEntrada.FindAsync(nroEntrada);

                if (mesaEntrada == null)
                {
                    // Manejo si la mesaEntrada no se encuentra
                    return NotFound();
                }
                else
                {
                    var estadoEntrada = await _dbContext.RmEstadosEntrada.FirstOrDefaultAsync(p => p.DescripEstado == "Recepcionado Diseñador");
                    mesaEntrada.EstadoEntrada = estadoEntrada.CodigoEstado;
                    _dbContext.Update(mesaEntrada);
                    await _dbContext.SaveChangesAsync();

                    RmMovimientosDoc movimientos = new()
                    {
                        NroEntrada = nroEntrada,
                        CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                        FechaOperacion = DateTime.Now,
                        CodOperacion = "01", //Parametro para ingreso a mesa entrada ya que no hay de dise;o
                        NroMovimientoRef = nroEntrada.ToString(),
                        EstadoEntrada = mesaEntrada.EstadoEntrada
                    };
                    await _dbContext.AddAsync(movimientos);
                    await _dbContext.SaveChangesAsync();

                }
                //var asigDis = await _dbContext.RmAsigDis.FirstOrDefaultAsync(p => p.NroEntrada == nroEntrada);
                //if (asigDis == null)
                //{
                //    // Manejo si la asigDis no se encuentra
                //    return NotFound();
                //}
                //else
                //{
                //    asigDis.Recibido = "S";
                //    _dbContext.Update(asigDis);
                //    await _dbContext.SaveChangesAsync();
                //}
            }
            return View("Index");
        }

    }
}
