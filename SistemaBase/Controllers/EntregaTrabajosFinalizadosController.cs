using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scryber.Components;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;

namespace SistemaBase.Controllers
{
    public class EntregaTrabajosFinalizadosController : Controller
    {
        private readonly DbvinDbContext _context;

        public EntregaTrabajosFinalizadosController(DbvinDbContext context)
        {
            _context = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMTRAFIN", "Index", "EntregaTrabajosFinalizados" })]

        public async Task<IActionResult> Index()
        {

            var EntregasTrabajosJoin = (from rme in _context.RmMesaEntrada
                                        join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                        join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                        join or in _context.RmOficinasRegistrales on rme.CodOficinaRetiro equals or.CodigoOficina
                                        //Condicion para que filtre por Documentos con titulo con los estados requeridos
                                        where (
                                        (ee.DescripEstado == "Salida Direccion" || ee.DescripEstado == "Nota Negativa/JefeRegistral" || ee.DescripEstado == "Observado/JefeRegistral") &&

                                        (ts.DescripSolicitud == "INSCRIPCION" || ts.DescripSolicitud == "DUPLICADO" || ts.DescripSolicitud == "REINSCRIPCION" || ts.DescripSolicitud == "ADJUDICACION" || ts.DescripSolicitud == "CAMBIO DE DENOMINACION" || ts.DescripSolicitud == "DACION EN PAGO" || ts.DescripSolicitud == "DONACION" || ts.DescripSolicitud == "PERMUTA" || ts.DescripSolicitud == "TRANSFERENCIA")
                                        ) && or.DescripOficina == "Asunción"
                                        //Condicion para que filtre por Documentos sin titulo con los estados requeridos
                                        ||
                                        (ee.DescripEstado == "Aprobado/JefeRegistral/Firma" || ee.DescripEstado == "Nota Negativa/JefeRegistral" || ee.DescripEstado == "Observado/JefeRegistral") &&

                                        (ts.DescripSolicitud == "CERTIFICADO" ||
                                        ts.DescripSolicitud == "CERTIFICADO DE CONDICION DE DOMINIO" ||
                                        ts.DescripSolicitud == "ANOTACION DE INSCRIPCION PREVENTIVA" ||
                                        ts.DescripSolicitud == "CANCELACION" ||
                                        ts.DescripSolicitud == "COPIA" ||
                                        ts.DescripSolicitud == "COPIA JUDICIAL" ||
                                        ts.DescripSolicitud == "EMBARGO EJECUTIVO" ||
                                        ts.DescripSolicitud == "EMBARGO PREVENTIVO" ||
                                        ts.DescripSolicitud == "FIANZA" ||
                                        ts.DescripSolicitud == "INFORME" ||
                                        ts.DescripSolicitud == "INFORME JUDICIAL" ||
                                        ts.DescripSolicitud == "LITIS" ||
                                        ts.DescripSolicitud == "PROHIBICIÓN DE INNOVAR Y CONTRATAR" ||
                                        ts.DescripSolicitud == "RECTIFICACION" ||
                                        ts.DescripSolicitud == "USUFRUCTO" ||
                                        ts.DescripSolicitud == "LEVANTAMIENTO" ||
                                        ts.DescripSolicitud == "PRENDA") && or.DescripOficina == "Asunción"

                                        orderby rme.FechaEntrada descending
                                        select new

                                        {
                                            NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                            DescSolicitud = ts.DescripSolicitud,
                                            FechaAlta = rme.FechaEntrada

                                        }).AsEnumerable().GroupBy(result => result.NroEntrada).Select(group => group.OrderByDescending(f => f.FechaAlta).First())
                                        .Select(result => new EntregaTrabajosFinalizado()
                                        {
                                            NroEntrada = result.NroEntrada,
                                            DescSolicitud = result.DescSolicitud,
                                            FechaAlta = result.FechaAlta
                                        });

            return View(EntregasTrabajosJoin.ToList());
        }


        [HttpPost]
        public async Task<IActionResult> CambiarEstado([FromBody] List<decimal> selectedNroEntradas)
        {

            foreach (decimal nroEntrada in selectedNroEntradas)
            {
                var mesaEntrada = await _context.RmMesaEntrada.FindAsync(nroEntrada);

                if (mesaEntrada == null)
                {
                    // Manejo si la mesaEntrada no se encuentra
                    return NotFound();
                }
                else
                {
                    var estadoEntrada = await _context.RmEstadosEntrada.FirstOrDefaultAsync(p => p.DescripEstado == "Recibido Mesa de Salida");
                    mesaEntrada.FechaRecSalida = DateTime.Now;
                    mesaEntrada.EstadoEntrada = estadoEntrada.CodigoEstado;
                    _context.Update(mesaEntrada);
                    await _context.SaveChangesAsync();

                    RmMovimientosDoc movimientos = new()
                    {
                        NroEntrada = nroEntrada,
                        CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                        FechaOperacion = DateTime.Now,
                        CodOperacion = "09", //parametro para cambio de estado 
                        NroMovimientoRef = nroEntrada.ToString(),
                        EstadoEntrada = mesaEntrada.EstadoEntrada
                    };
                    await _context.AddAsync(movimientos);
                    await _context.SaveChangesAsync();


                }
                
            }
            return View("Index");
        }



    }
}
