using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;

namespace SistemaBase.Controllers
{
    public class DireccionEntradaController : Controller
    {
        private readonly DbvinDbContext _context;

        public DireccionEntradaController(DbvinDbContext context)
        {
            _context = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMDIREN", "Index", "DireccionEntrada" })]

        public async Task<IActionResult> Index()
        {
            var direccionEntrada = (from  rme in _context.RmMesaEntrada 
                                        join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                        join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                        where (ee.DescripEstado == "Aprobado/JefeRegistral/Firma" && (ts.DescripSolicitud == "INSCRIPCION" 
                                        || ts.DescripSolicitud == "DUPLICADO" 
                                        || ts.DescripSolicitud == "REINSCRIPCION" 
                                        || ts.DescripSolicitud == "ADJUDICACION" 
                                        || ts.DescripSolicitud == "CAMBIO DE DENOMINACION" 
                                        || ts.DescripSolicitud == "DACION EN PAGO" 
                                        || ts.DescripSolicitud == "DONACION" 
                                        || ts.DescripSolicitud == "PERMUTA" 
                                        || ts.DescripSolicitud == "TRANSFERENCIA"))
                                        || ee.DescripEstado == "Aprobado/JefeSup/Firma"
                                    //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                    //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                    //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                    orderby rme.FechaEntrada descending
                                        select new Direccion
                                        {
                                            NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                            DescSolicitud = ts.DescripSolicitud,
                                            FechaAlta = rme.FechaEntrada

                                        });

            return View(await direccionEntrada.AsNoTracking().ToListAsync());
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
                    var estadoEntrada = await _context.RmEstadosEntrada.FirstOrDefaultAsync(p => p.DescripEstado == "Recibido Direccion");

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
