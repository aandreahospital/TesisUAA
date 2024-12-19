using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;

namespace SistemaBase.Controllers
{
    public class EntregaTrabajosArchivoController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public EntregaTrabajosArchivoController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMARCHI", "Index", "EntregaTrabajosArchivo" })]
        public async Task<IActionResult> Index()
        {
            //var EntregasTrabajosJoin = (from rt in _dbContext.RmTransacciones
            //                            join rme in _dbContext.RmMesaEntrada on rt.NumeroEntrada equals rme.NumeroEntrada
            //                            join ee in _dbContext.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
            //                            join ts in _dbContext.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
            //                            join r in _dbContext.RmOficinasRegistrales on rme.CodOficinaRetiro equals r.CodigoOficina
            //                            join b in _dbContext.RmBoletasMarcas on rt.NroBoleta equals b.NroBoleta.ToString() into boletaJoin
            //                            from boleta in boletaJoin.DefaultIfEmpty() // Left join para permitir que la boleta sea null
            //                                                                       // Filtramos los documentos que emite título con el estado Enviado a Archivo que sean de Asunción
            //                                                                       // Caso contrario filtramos por el estado de Enviado Triplicado que sean diferentes a Asunción
            //                            where ((ee.DescripEstado == "Enviado a Archivo" && r.DescripOficina == "Asunción") ||
            //                                   (ee.DescripEstado == "Enviado Triplicado" && r.DescripOficina != "Asunción"))
            //                                   && (ts.DescripSolicitud == "INSCRIPCION" || ts.DescripSolicitud == "DUPLICADO" || ts.DescripSolicitud == "REINSCRIPCION" || ts.DescripSolicitud == "ADJUDICACION" || ts.DescripSolicitud == "CAMBIO DE DENOMINACION" || ts.DescripSolicitud == "DACION EN PAGO" || ts.DescripSolicitud == "DONACION" || ts.DescripSolicitud == "PERMUTA" || ts.DescripSolicitud == "TRANSFERENCIA")
            //                            orderby rt.FechaAlta descending
            //                            select new
            //                            {
            //                                NUMERO_BOLETA = boleta != null ? boleta.Descripcion : rt.NroBoleta, // Si la boleta es null, asigna el valor que esta en transaccion
            //                                NUMERO_ENTRADA = Convert.ToInt32(rme.NumeroEntrada),
            //                                TIPO_SOLICITUD = ts.DescripSolicitud,
            //                                FECHA_ASIGNACION = rt.FechaAlta
            //                            }).AsEnumerable()
            //                  .GroupBy(result => result.NUMERO_ENTRADA)
            //                  .Select(group => group.OrderByDescending(g => g.FECHA_ASIGNACION).First())
            //                  .Select(result => new EntregaTrabajoArchivoCustom
            //                  {
            //                      NUMERO_BOLETA = result.NUMERO_BOLETA, 
            //                      NUMERO_ENTRADA = result.NUMERO_ENTRADA,
            //                      TIPO_SOLICITUD = result.TIPO_SOLICITUD,
            //                      FECHA_ASIGNACION = result.FECHA_ASIGNACION
            //                  });
            //return View( EntregasTrabajosJoin.ToList());
            
            return View();


            //var EntregasTrabajosJoin = (from rt in _dbContext.RmTransacciones
            //                            join rme in _dbContext.RmMesaEntrada on rt.NumeroEntrada equals rme.NumeroEntrada
            //                            join ee in _dbContext.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
            //                            join ts in _dbContext.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
            //                            join r in _dbContext.RmOficinasRegistrales on rme.CodOficinaRetiro equals r.CodigoOficina
            //                            join b in _dbContext.RmBoletasMarcas on rt.NroBoleta equals b.NroBoleta.ToString()
            //                            //Filtramos los documentos que emite titulo con el estado Enviado a Archivo que sean de Asuncion
            //                            //Caso contrario filtramos por el estado de Enviado Triplicado que sean diferentes a Asuncion
            //                            where ((ee.DescripEstado == "Enviado a Archivo") &&
            //                                   (ts.DescripSolicitud == "INSCRIPCION" || ts.DescripSolicitud == "DUPLICADO" || ts.DescripSolicitud == "REINSCRIPCION" || ts.DescripSolicitud == "ADJUDICACION" || ts.DescripSolicitud == "CAMBIO DE DENOMINACION" || ts.DescripSolicitud == "DACION EN PAGO" || ts.DescripSolicitud == "DONACION" || ts.DescripSolicitud == "PERMUTA" || ts.DescripSolicitud == "TRANSFERENCIA")
            //                                   && r.DescripOficina == "Asunción")
            //                                   ||
            //                                  ((ee.DescripEstado == "Enviado Triplicado") &&
            //                                   (ts.DescripSolicitud == "INSCRIPCION" || ts.DescripSolicitud == "DUPLICADO" || ts.DescripSolicitud == "REINSCRIPCION" || ts.DescripSolicitud == "ADJUDICACION" || ts.DescripSolicitud == "CAMBIO DE DENOMINACION" || ts.DescripSolicitud == "DACION EN PAGO" || ts.DescripSolicitud == "DONACION" || ts.DescripSolicitud == "PERMUTA" || ts.DescripSolicitud == "TRANSFERENCIA")
            //                                   && r.DescripOficina != "Asunción")
            //                            orderby rt.FechaAlta descending

                                        
            //                            select new EntregaTrabajoArchivoCustom
            //                            {
            //                                NUMERO_BOLETA = b.Descripcion,
            //                                NUMERO_ENTRADA = Convert.ToInt32(rme.NumeroEntrada),
            //                                TIPO_SOLICITUD = ts.DescripSolicitud,
            //                                FECHA_ASIGNACION = rt.FechaAlta
            //                            });
            //return View(await EntregasTrabajosJoin.AsNoTracking().ToListAsync());
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
                    var estadoEntrada = await _dbContext.RmEstadosEntrada.FirstOrDefaultAsync(p => p.DescripEstado == "Recepcionado Archivo");
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

            }
            return View("Index");
        }

        public List<EntregaTrabajoArchivoCustom> GetDataFromDbaseTransaccion(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "NroEntrada";
                sortDir = true;
            }
            List<string> descripcionesPermitidas = new List<string> {
                    "INSCRIPCION",
                    "DUPLICADO",
                    "REINSCRIPCION",
                    "ADJUDICACION",
                    "CAMBIO DE DENOMINACION",
                    "DACION EN PAGO",
                    "DONACION",
                    "PERMUTA",
                    "TRANSFERENCIA"
            };
            IQueryable<RmMesaEntradum> queryMesaEntrada = _dbContext.RmMesaEntrada.OrderByDescending(o => o.FechaEntrada).Where(o => (o.EstadoEntrada == 43 && o.CodOficinaRetiro == 1) || (o.EstadoEntrada == 29 && o.CodOficinaRetiro != 1)).AsQueryable();
            IQueryable<RmTipoSolicitud> queryTipoSolicitud = _dbContext.RmTipoSolicituds.Where(ts => descripcionesPermitidas.Contains(ts.DescripSolicitud)).AsQueryable();
            IQueryable<RmTransaccione> queryTransaccion = _dbContext.RmTransacciones.AsQueryable();


            var primer = queryMesaEntrada.Join(queryTipoSolicitud,
                en => en.TipoSolicitud,
                tipo => tipo.TipoSolicitud,
                (en, tipo) => new EntregaTrabajoArchivoCustom
                {
                    NUMERO_ENTRADA = Convert.ToInt32(en.NumeroEntrada),
                    TIPO_SOLICITUD = tipo.DescripSolicitud,
                    NUMERO_BOLETA = en.NroBoleta,
                    FECHA_ASIGNACION = en.FechaEntrada,
                    NOM_TITULAR = en.NomTitular
                }
                );

            var segundo = primer.AsQueryable().Join(queryTransaccion,
               en => en.NUMERO_ENTRADA,
               tipo => tipo.NumeroEntrada,
               (en, tipo) => new EntregaTrabajoArchivoCustom
               {
                   NUMERO_ENTRADA = en.NUMERO_ENTRADA,
                   TIPO_SOLICITUD = en.TIPO_SOLICITUD,
                   NUMERO_BOLETA = tipo.NroBoleta,
                   FECHA_ASIGNACION = tipo.FechaAlta,
                   NOM_TITULAR = en.NOM_TITULAR
               }).ToList();

            // Realizamos la conversión de tipo fuera de la expresión LINQ to Entities
            foreach (var item in segundo)
            {
                if (!string.IsNullOrEmpty(item.NUMERO_BOLETA)) // Verifica si NroBoleta no es nulo ni vacío
                {
                    if (!decimal.TryParse(item.NUMERO_BOLETA, out decimal nroBoletaDecimal))
                    {
                        // Si no se puede convertir a decimal, asumimos que ya es alfanumérico y no hacemos ninguna conversión
                        continue; // Pasamos al siguiente item
                    }

                    // Obtenemos la descripción de la boleta correspondiente al número de boleta
                    var descripcionBoleta = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.NroBoleta == Convert.ToInt32(nroBoletaDecimal))?.Descripcion;
                    if (descripcionBoleta != null)
                    {
                        item.NUMERO_BOLETA = descripcionBoleta; // Reemplazamos el número de boleta con la descripción
                    }
                }
            }
            // Continuamos con la lógica de tu consulta
            //var resultado = segundo.GroupBy(x => x.NUMERO_ENTRADA)
            //                           .Select(g => g.OrderByDescending(e => e.FECHA_ASIGNACION).First())
            //                           .ToList();

            //if (!string.IsNullOrEmpty(searchBy))
            //{
            //    var searchTerm = searchBy;
            //    segundo = segundo.Where(item =>
            //        item.NUMERO_ENTRADA.ToString().Contains(searchTerm)
            //    );
            //}

            if (!string.IsNullOrEmpty(searchBy))
            {
                var searchTerm = searchBy;
                segundo = segundo.Where(item =>
                    item.NUMERO_ENTRADA.ToString().Contains(searchTerm)    // Si se desea buscar en NUMERO_BOLETA
                ).ToList();
            }

            var result = segundo
                           .Select(m => new EntregaTrabajoArchivoCustom
                           {
                               NUMERO_BOLETA = m.NUMERO_BOLETA,
                               NUMERO_ENTRADA = m.NUMERO_ENTRADA,
                               TIPO_SOLICITUD = m.TIPO_SOLICITUD,
                               FECHA_ASIGNACION = m.FECHA_ASIGNACION,
                               NOM_TITULAR = m.NOM_TITULAR
                           }).GroupBy(x => x.NUMERO_ENTRADA)
                            .Select(g => g.OrderByDescending(e => e.FECHA_ASIGNACION).First())
                           .Skip(skip)
                          .Take(take)
                          .ToList();

            filteredResultsCount = segundo.Count();
            totalResultsCount = segundo.Count();

            return result;
        }

        public IList<EntregaTrabajoArchivoCustom> SearchFuncTransacciones(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.Search != null) ? model.Search.Value : null;
            var take = model.Length;
            var skip = model.Start;

            string sortBy = "";
            bool sortDir = true;

            var result = GetDataFromDbaseTransaccion(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            if (result == null)
            {
                return new List<EntregaTrabajoArchivoCustom>();
            }
            return result;
        }

        [HttpPost]
        public JsonResult SearchTransacciones([FromBody] DataTableAjaxPostModel model)
        {
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchFuncTransacciones(model, out filteredResultsCount, out totalResultsCount);

            var re = Json(new
            {
                draw = model.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res.ToList()
            });

            return re;
        }


    }
}
