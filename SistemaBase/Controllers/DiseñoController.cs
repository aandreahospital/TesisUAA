using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistroLogin.Filters;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;

namespace SistemaBase.Controllers
{
    public class DiseñoController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public DiseñoController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMRECDIS", "Index" , "Diseño" })]
        public async Task<IActionResult> Index()
        {
            var disenho = (from me in _dbContext.RmMesaEntrada 
                           join ts in _dbContext.RmTipoSolicituds on me.TipoSolicitud equals ts.TipoSolicitud
                           join ee in _dbContext.RmEstadosEntrada on me.EstadoEntrada equals ee.CodigoEstado
                           where ee.DescripEstado== "Enviado a Diseño" && ts.DescripSolicitud== "INSCRIPCION"
                           //( me.CodigoOficina==1 &&   ee.DescripEstado== "Mesa entrada" && (ts.DescripSolicitud == "INSCRIPCION" || ts.DescripSolicitud == "REINSCRIPCION"))
                           //|| ( me.CodigoOficina!=1 && ee.DescripEstado== "Entrada Div Regional"  && (ts.DescripSolicitud== "INSCRIPCION" || ts.DescripSolicitud== "REINSCRIPCION"))
                           //&& me.CodigoOficina !=null
                           orderby me.NumeroEntrada ascending
                           select new Diseño()
                           {
                               //NroEntrada = me.NumeroEntrada,
                               TipoSolicitud = ts.DescripSolicitud,
                               FechaIngreso = me.FechaEntrada,
                               StrNroEntrada = Math.Round(me.NumeroEntrada).ToString()
                           });
            return View(await disenho.AsNoTracking().ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado([FromBody] List<decimal> selectedNroEntradas)
        {
            try
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
                        var estadoEntrada = await _dbContext.RmEstadosEntrada.FirstOrDefaultAsync(p => p.DescripEstado == "Diseño");
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
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar/actualizar: " + ex.Message);
                return BadRequest("Error al agregar/actualizar: " + ex.Message);
            }
        }

        //[HttpPost]
        //public JsonResult CustomServerSideSearchAction([FromBody] DataTableAjaxPostModel model)
        //{
        //    int filteredResultsCount;
        //    int totalResultsCount;
        //    var res = YourCustomSearchFunc(model, out filteredResultsCount, out totalResultsCount);

        //    var re = Json(new
        //    {
        //        draw = model.Draw,
        //        recordsTotal = totalResultsCount,
        //        recordsFiltered = filteredResultsCount,
        //        data = res.ToList()
        //    });

        //    return re;
        //}

        //public IList<Diseño> YourCustomSearchFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        //{
        //    var searchBy = (model.Search != null) ? model.Search.Value : null;
        //    var take = model.Length;
        //    var skip = model.Start;

        //    string sortBy = "";
        //    bool sortDir = true;

        //    var result = GetDataFromDbase(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
        //    if (result == null)
        //    {
        //        return new List<Diseño>();
        //    }
        //    return result;
        //}

        //public List<Diseño> GetDataFromDbase(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        //{

        //    if (String.IsNullOrEmpty(searchBy))
        //    {
        //        sortBy = "NroEntrada";
        //        sortDir = true;
        //    }
        //    IQueryable<RmMesaEntradum> queryEntrada = _dbContext.RmMesaEntrada.AsQueryable();
        //    IQueryable<RmTipoSolicitud> querySolicitud = _dbContext.RmTipoSolicituds.AsQueryable();
        //    IQueryable<RmAsigDi> queryAsigDis = _dbContext.RmAsigDis.AsQueryable();

        //    //Para Obetener el Nro Entrada y Tipo Solicitud 

        //    var queryEntradaAsigDis = queryAsigDis.Where(ad => ad.Recibido == "N" && (ad.Desasignado != "S" || ad.Desasignado == "")).AsQueryable().Join(queryEntrada,
        //       asigDis => asigDis.NroEntrada,
        //       entrada => entrada.NumeroEntrada,
        //       (asigDis, entrada) => new { NroEntrada = entrada.NumeroEntrada, FechaIngreso = entrada.FechaEntrada, TipoSolicitud = entrada.TipoSolicitud });


        //    //Para obtener la descripcion del tipo solicitud 
        //    var queryFinal = queryEntradaAsigDis.AsQueryable().Join(querySolicitud,
        //        dis => dis.TipoSolicitud,
        //        sol => sol.TipoSolicitud,
        //        (dis, sol) => new Diseño { NroEntrada = dis.NroEntrada, TipoSolicitud = sol.DescripSolicitud, FechaIngreso = dis.FechaIngreso });


        //    if (!string.IsNullOrEmpty(searchBy))
        //    {
        //        queryFinal = queryFinal.AsQueryable().Where(item =>
        //                item.NroEntrada.ToString().Contains(searchBy)
        //            );
        //    }


        //    var result = queryFinal
        //                   .Select(m => new Diseño
        //                   {
        //                       NroEntrada = m.NroEntrada,
        //                       TipoSolicitud = m.TipoSolicitud,
        //                       FechaIngreso = m.FechaIngreso
        //                   })
        //                   .Skip(skip)
        //                  .Take(take)
        //                  .ToList();

        //    filteredResultsCount = queryFinal.Count();
        //    totalResultsCount = queryFinal.Count();

        //    return result;
        //}



    }
}
