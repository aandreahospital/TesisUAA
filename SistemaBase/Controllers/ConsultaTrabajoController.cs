using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using SistemaBase.Models;
using SistemaBase.Models.Dtos;
using SistemaBase.ModelsCustom;
using static SistemaBase.ModelsCustom.ConsultaTrabajo;
using static SistemaBase.ModelsCustom.DatosTituloCustom;

namespace SistemaBase.Controllers
{
    public class ConsultaTrabajoController : Controller
    {
        private readonly DbvinDbContext _dbContext;

        public ConsultaTrabajoController(DbvinDbContext context)
        {
            _dbContext = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
       [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMCONTRA", "Index", "ConsultaTrabajo" })]

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Get(decimal nEntrada)
        {
            try
            {
                var movimientos = await _dbContext.RmMovimientosDocs.Where(p => p.NroEntrada == nEntrada).OrderByDescending(p => p.FechaOperacion).FirstOrDefaultAsync();
                var mesaEntrada = await _dbContext.RmMesaEntrada.Where(p => p.NumeroEntrada == nEntrada).FirstOrDefaultAsync();
                ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", movimientos?.EstadoEntrada);
                ViewBag.TipoSolicitud = new SelectList(_dbContext.RmTipoSolicituds, "TipoSolicitud", "DescripSolicitud", mesaEntrada?.TipoSolicitud??0);
                ViewBag.Oficina = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina", mesaEntrada?.CodigoOficina??0);
                ViewBag.OficinaRetiro = new SelectList(_dbContext.RmOficinasRegistrales, "CodigoOficina", "DescripOficina", mesaEntrada?.CodOficinaRetiro ?? 0);

                var marcasBoleta = _dbContext.RmMarcasSenales.FirstOrDefault(m=>m.NumeroEntrada== nEntrada);
                string nroBoleta = "";

                if (mesaEntrada?.NumeroEntrada == null)
                {
                    return Json(new { Success = false, ErrorMessage = "El número de entrada no existe" });
                }

                if (marcasBoleta != null)
                {
                   
                    var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b=>b.NroBoleta== Convert.ToInt64(marcasBoleta.NroBoleta));
                    nroBoleta = boletaMarca?.Descripcion??"";
                }else
                {
                    nroBoleta = mesaEntrada?.NroBoleta??"";
                }
              //  var cambios = _dbContext.RmCambiosOficinas.OrderByDescending(p => p.FechaOperacion).FirstOrDefault(c=>c.NroEntrada==nEntrada.ToString());
                var transaccion = await _dbContext.RmTransacciones.Where(p => p.NumeroEntrada == nEntrada).FirstOrDefaultAsync();
                //Para sacar el ultimo usuario asignado
                var notaNegativa = await _dbContext.RmNotasNegativas.Where(p => p.IdEntrada == nEntrada).FirstOrDefaultAsync();
                var Asignaciones = await _dbContext.RmAsignaciones.Where(x=>x.TipoAsignacion=="R").OrderByDescending(o=>o.FechaAsignada).FirstOrDefaultAsync(a=>a.NroEntrada == nEntrada);

                //var nroBoletaMarca = mesaEntrada?.NroBoleta??"0";
                //var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b=>b.NroBoleta== Convert.ToInt64(nroBoletaMarca));
                // cargar para edicion
                ConsultaTrabajo consultaTrabajo = new()
                {
                    //Datos de Mesa Entrada
                    NroEntrada = mesaEntrada?.NumeroEntrada ?? 0,
                    NroEntradaOriginal = mesaEntrada?.NroEntradaOriginal ?? 0,
                    NombrePresentador =mesaEntrada?.NombrePresentador??"",
                    NroDocumentoPresentador =mesaEntrada?.NroDocumentoPresentador??"",
                    NombreRetirador = mesaEntrada?.NombreRetirador??"",
                    NroDocumentoRetirador=mesaEntrada?.NroDocumentoRetirador??"",
                    NumeroLiquidacionLetras = mesaEntrada?.NumeroLiquidacionLetras??"",
                    NroFormulario = mesaEntrada?.NroFormulario??0,
                    EstadoEntrada = movimientos?.EstadoEntrada??0,
                    FechaEntrada = mesaEntrada?.FechaEntrada?? DateTime.MinValue,
                    FechaSalida = mesaEntrada?.FechaSalida?? DateTime.MinValue,
                    CodigoOficina = mesaEntrada?.CodigoOficina??0,
                    NomTitular = mesaEntrada?.NomTitular??"",
                    TipoSolicitud = mesaEntrada?.TipoSolicitud??0,
                    NroOficio = mesaEntrada?.NroOficio??"",
                    TipoDocumento = mesaEntrada?.TipoDocumento??"",
                    NroBoleta = nroBoleta,
                    CodOficinaRetiro= mesaEntrada?.CodOficinaRetiro??0,
                    //UsuarioAsignado
                    CodUsuario = Asignaciones?.IdUsuarioAsignado??"",
                    NroMovimiento = movimientos?.NroMovimiento??0
                    //ObserCambio = cambios?.Observacion??""

                };
                return View("Index", consultaTrabajo);



            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrió un error: " + ex.Message);

            }
        }

        public ActionResult MostrarBuscador()
        {
            //var operacionSupervisor = (from me in _dbContext.RmMesaEntrada
            //                           join ts in _dbContext.RmTipoSolicituds on me.TipoSolicitud equals ts.TipoSolicitud
            //                           join p in _dbContext.Personas on me.NroDocumentoTitular equals p.CodPersona
            //                           where me.TipoSolicitud ==2
            //                           orderby me.FechaEntrada descending
            //                           select new ConsultaTrabajo()
            //                           {
            //                               NumeroEntrada = Convert.ToInt32(me.NumeroEntrada),
            //                               DescSolicitud = ts.DescripSolicitud,
            //                               FechaEntrada = me.FechaEntrada,
            //                               NomTitular = p.Nombre
            //                           });

            //return View("Buscador",  operacionSupervisor.AsNoTracking().ToListAsync());
            return View("Buscador");
        }

        public async Task<JsonResult> CargarPorDocumento(string codPersona)
        {
            IQueryable<RmTitularesMarca> queryTitulares = _dbContext.RmTitularesMarcas.AsQueryable();
            IQueryable<RmTransaccione> queryTransaccion = _dbContext.RmTransacciones.AsQueryable();
            IQueryable<RmMesaEntradum> mesaEntrada =_dbContext.RmMesaEntrada.AsQueryable();
            IQueryable<RmTipoSolicitud> tipoSolicituds = _dbContext.RmTipoSolicituds.AsQueryable();
            IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();

            var queryFinal = mesaEntrada.OrderByDescending(m=>m.FechaEntrada).Where(m=>m.NroDocumentoTitular== codPersona).AsQueryable().Join(tipoSolicituds,
                entrada => entrada.TipoSolicitud,
                tipo => tipo.TipoSolicitud,
                (entrada, tipo) =>
               new BuscadorCustom { NomTitular = entrada.NomTitular, NroDocTitular = entrada.NroDocumentoTitular, TipoSolicitud = tipo.DescripSolicitud, NumeroEntrada = entrada.NumeroEntrada });

            var titularesMarcas = await queryFinal.ToArrayAsync();

            return Json(titularesMarcas);
        }

        //public async Task<JsonResult> CargarEntradas()
        //{
        //    IQueryable<RmTitularesMarca> queryTitulares = _dbContext.RmTitularesMarcas.AsQueryable();
        //    IQueryable<RmTransaccione> queryTransaccion = _dbContext.RmTransacciones.AsQueryable();
        //    IQueryable<RmMesaEntradum> mesaEntrada = _dbContext.RmMesaEntrada.AsQueryable();
        //    IQueryable<RmTipoSolicitud> tipoSolicituds = _dbContext.RmTipoSolicituds.AsQueryable();
        //    IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();

        //    var queryFinal = mesaEntrada.OrderByDescending(m => m.FechaEntrada).AsQueryable().Join(tipoSolicituds,
        //        entrada => entrada.TipoSolicitud,
        //        tipo => tipo.TipoSolicitud,
        //        (entrada, tipo) =>
        //       new BuscadorCustom { NomTitular = entrada.NomTitular, NroDocTitular = entrada.NroDocumentoTitular, TipoSolicitud = tipo.DescripSolicitud, NumeroEntrada = entrada.NumeroEntrada });

        //    var titularesMarcas = await queryFinal.ToArrayAsync();

        //    return Json(titularesMarcas);
        //}


        public async Task<JsonResult> CargarEntradas(int draw, int start, int length)
        {
            IQueryable<RmTitularesMarca> queryTitulares = _dbContext.RmTitularesMarcas.AsQueryable();
            IQueryable<RmTransaccione> queryTransaccion = _dbContext.RmTransacciones.AsQueryable();
            IQueryable<RmMesaEntradum> mesaEntrada = _dbContext.RmMesaEntrada.AsQueryable();
            IQueryable<RmTipoSolicitud> tipoSolicituds = _dbContext.RmTipoSolicituds.AsQueryable();
            IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();

            var queryFinal = mesaEntrada.OrderByDescending(m => m.FechaEntrada).AsQueryable().Join(tipoSolicituds,
                entrada => entrada.TipoSolicitud,
                tipo => tipo.TipoSolicitud,
                (entrada, tipo) =>
                new BuscadorCustom { NomTitular = entrada.NomTitular, NroDocTitular = entrada.NroDocumentoTitular, TipoSolicitud = tipo.DescripSolicitud, NumeroEntrada = entrada.NumeroEntrada });

            draw = 1;
            start = 0;
            length = 10;
            // Aplicar paginación
            var filteredResults = queryFinal
                .Skip(start)  // Salta la cantidad de registros de inicio
                .Take(length) // Toma la cantidad de registros especificada por 'length'
                .ToArray();

            var totalResultsCount = queryFinal.Count(); // Obtener el recuento total de resultados sin paginación

            var response = new
            {
                draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = totalResultsCount,
                data = filteredResults
            };

            return Json(response);
        }


        public async Task<JsonResult> CargarPorNombre(string nombre)
        {
            IQueryable<RmTitularesMarca> queryTitulares = _dbContext.RmTitularesMarcas.AsQueryable();
            IQueryable<RmTransaccione> queryTransaccion = _dbContext.RmTransacciones.AsQueryable();
            IQueryable<RmMesaEntradum> mesaEntrada = _dbContext.RmMesaEntrada.AsQueryable();
            IQueryable<RmTipoSolicitud> tipoSolicituds = _dbContext.RmTipoSolicituds.AsQueryable();
            IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();

            var queryFinal = mesaEntrada.OrderByDescending(m => m.FechaEntrada).Where(m => m.NomTitular.Contains(nombre)).AsQueryable().Join(tipoSolicituds,
                entrada => entrada.TipoSolicitud,
                tipo => tipo.TipoSolicitud,
                (entrada, tipo) =>
               new BuscadorCustom { NomTitular = entrada.NomTitular, NroDocTitular = entrada.NroDocumentoTitular, TipoSolicitud = tipo.DescripSolicitud, NumeroEntrada = entrada.NumeroEntrada });

            var titularesMarcas = await queryFinal.ToArrayAsync();

            return Json(titularesMarcas);
        }

        public async Task<JsonResult> CargarTitulares(decimal nroEntrada)
        {
            IQueryable<RmTitularesMarca> queryTitulares = _dbContext.RmTitularesMarcas.AsQueryable();
            IQueryable<RmTransaccione> queryTransaccion = _dbContext.RmTransacciones.AsQueryable();
            IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();

            var queryTituMesa = queryTransaccion.Where(t=>t.NumeroEntrada== nroEntrada).AsQueryable().Join(queryTitulares,
                trans => trans.IdTransaccion,
                titu => titu.IdTransaccion,
                (trans, titu ) =>
                new Titulares { CodPersona = titu.IdTitular, FechaRegistro= titu.FechaRegistro });

            var queryFinal = queryTituMesa.AsQueryable().Join(queryPersona,
                titu => titu.CodPersona,
                persona => persona.CodPersona,
                (titu, persona) =>
               new Titulares { CodPersona = titu.CodPersona, Nombre = persona.Nombre, FechaRegistro = titu.FechaRegistro}).ToList();

           // var titularesMarcas = await queryFinal.ToArrayAsync();
            var resultados = queryFinal.GroupBy(x => x.CodPersona)
                                      .Select(g => g.OrderByDescending(e => e.FechaRegistro).First())
                                      .ToList();

            return Json(resultados);
        }


        public async Task<JsonResult> CargarAsignaciones(decimal nroEntrada)
        {
            IQueryable<RmAsignacione> queryAsignaciones = _dbContext.RmAsignaciones.Where(a=>a.TipoAsignacion=="R").AsQueryable();
            IQueryable<RmMesaEntradum> queryEntrada= _dbContext.RmMesaEntrada.AsQueryable();
            IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();

            var queryFinal = queryAsignaciones
                 .Where(t => t.NroEntrada == nroEntrada)
                 .Join(queryEntrada,
                     trans => trans.NroEntrada,
                     titu => titu.NumeroEntrada,
                     (trans, titu) =>
                         new
                         {
                             FechaAsignacion = trans.FechaAsignada,
                             Nombre = trans.IdUsuarioAsignado,
                             Desasignado = trans.UsuarioDesasignacion,
                             Reingreso = titu.Reingreso
                         })
                 .Select(result =>
                 new
                     {
                         FechaAsignacion = result.FechaAsignacion,
                         Nombre = result.Nombre,
                         Desasignado = result.Desasignado,
                         Reingreso = result.Reingreso == "S" ? "Reingreso" : ""
                     });


            //var primero = queryTituMesa.ToList();

            //var queryFinal = queryTituMesa.AsQueryable().Join(queryPersona,
            //    titu => titu.Nombre,
            //    persona => persona.CodPersona,
            //    (titu, persona) =>
            //   new Asignaciones { FechaAsignacion = titu.FechaAsignacion, Nombre = persona.Nombre, Desasignado = titu.i }).ToList();

            // var titularesMarcas = await queryFinal.ToArrayAsync();
            //var resultados = queryFinal.GroupBy(x => x.CodPersona)
            //                          .Select(g => g.OrderByDescending(e => e.FechaRegistro).First())
            //                          .ToList();

            var resultados = queryFinal.ToList();

            return Json(resultados);
        }

        public async Task<JsonResult> CargarMovimientos(decimal nroEntrada)
        {
            IQueryable<RmMovimientosDoc> queryMovimientos = _dbContext.RmMovimientosDocs.AsQueryable();
            IQueryable<RmEstadosEntradum> queryEstado = _dbContext.RmEstadosEntrada.AsQueryable();
            IQueryable<Usuario> queryUsuario = _dbContext.Usuarios.AsQueryable();
            IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();

            var queryMovEstado = queryMovimientos.Where(m => m.NroEntrada == nroEntrada).AsQueryable().Join(queryEstado,
                mov => mov.EstadoEntrada,
                estado => estado.CodigoEstado,
                (mov,estado)=> 
                new Movimientos {CodUsuario= mov.CodUsuario, Estado = estado.DescripEstado, FechaOperacion =mov.FechaOperacion});

           var queryMovimientosUsuario = queryMovEstado.AsQueryable().Join(queryUsuario,
               mov => mov.CodUsuario,
               usuario => usuario.CodUsuario,
               (mov, usuario)=> 
              new Movimientos { CodUsuario = mov.CodUsuario, Nombre = usuario.CodPersona, FechaOperacion = mov.FechaOperacion, Estado = mov.Estado });

            var queryFinal = queryMovimientosUsuario.AsQueryable().Join(queryPersona,
               mov => mov.Nombre,
               persona => persona.CodPersona,
               (mov ,persona)=> 
               new Movimientos { CodUsuario = mov.CodUsuario, Nombre = persona.Nombre,  FechaOperacion = mov.FechaOperacion , Estado = mov.Estado });
            // Después de la línea que obtiene los resultados
            var titularesMarcas = await queryFinal.Where(o => o.FechaOperacion != null).OrderBy(o => o.FechaOperacion).ToListAsync();

            
            return Json(titularesMarcas);
        }


        public async Task<JsonResult> CargarObservacion(decimal nroEntrada)
        {
            var observado = _dbContext.RmEstadosEntrada.FirstOrDefault(o=>o.DescripEstado== "Observado/Registrador");
            IQueryable<RmMovimientosDoc> queryMovimientos = _dbContext.RmMovimientosDocs.AsQueryable().Where(m=>m.EstadoEntrada==observado.CodigoEstado); 
            IQueryable<RmTransaccione> queryTransaccion = _dbContext.RmTransacciones.AsQueryable();
        
            var queryMovTrans = queryMovimientos.Where(m => m.NroEntrada == nroEntrada).AsQueryable().Join(queryTransaccion,
                mov => mov.NroEntrada,
                trans => trans.NumeroEntrada,
                (mov, trans) =>
                new Observacion { UsuarioSup = trans.IdUsuario, FechaAlta = trans.FechaAlta, ObservacionSup= trans.Observacion }).Distinct();

            var observacions = await queryMovTrans.ToArrayAsync();

            return Json(observacions);
        }

        public async Task<JsonResult> CargarNotaNegativa(decimal nroEntrada)
        {
            var observado = _dbContext.RmEstadosEntrada.FirstOrDefault(o => o.DescripEstado == "Nota Negativa/Registrador");
            IQueryable<RmMovimientosDoc> queryMovimientos = _dbContext.RmMovimientosDocs.AsQueryable().Where(m => m.EstadoEntrada == observado.CodigoEstado);
            IQueryable<RmTransaccione> queryTransaccion = _dbContext.RmTransacciones.AsQueryable();

            var queryMovTrans = queryMovimientos.Where(m => m.NroEntrada == nroEntrada).AsQueryable().Join(queryTransaccion,
                mov => mov.NroEntrada,
                trans => trans.NumeroEntrada,
                (mov, trans) =>
                new NotaNegativa { DescripNotaNegativa = trans.Observacion, NomPersona = trans.IdUsuario, FechaAlta = trans.FechaAlta }).Distinct();

            var observacions = await queryMovTrans.ToArrayAsync();

            return Json(observacions);


            //IQueryable<RmMovimientosDoc> queryMovimientos = _dbContext.RmMovimientosDocs.AsQueryable();
            //IQueryable<RmNotasNegativa> queryNotaNegativa = _dbContext.RmNotasNegativas.AsQueryable();
            //IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();
            //IQueryable<Usuario> queryUsuario = _dbContext.Usuarios.AsQueryable();

            //var queryMovNota = queryMovimientos.Where(m => m.NroEntrada == nroEntrada).AsQueryable().Join(queryNotaNegativa,
            //    mov => mov.NroEntrada,
            //    nota => nota.IdEntrada,
            //    (mov, nota) =>
            //    new NotaNegativa { DescripNotaNegativa = nota.DescripNotaNegativa, NomPersona = nota.IdUsuario, FechaAlta= nota.FechaAlta  });

            //var queryNotaUsuario = queryMovNota.AsQueryable().Join(queryUsuario,
            //    nota => nota.NomPersona,
            //    usuario => usuario.CodUsuario,
            //    (nota, usuario) => new NotaNegativa { DescripNotaNegativa = nota.DescripNotaNegativa, NomPersona = usuario.CodPersona, FechaAlta = nota.FechaAlta });

            //var queryFinal = queryNotaUsuario.AsQueryable().Join(queryPersona,
            //    nota => nota.NomPersona,
            //    persona => persona.CodPersona,
            //    (nota, persona)=> new NotaNegativa { DescripNotaNegativa = nota.DescripNotaNegativa, NomPersona = persona.Nombre, FechaAlta = nota.FechaAlta  }).Distinct();

            //var notaNegativas = await queryFinal.ToArrayAsync();

            //return Json(notaNegativas);
        }

        //[HttpGet("ObtenerDatos")]
        //public async Task<IActionResult> ObtenerDatos(int IdMovimiento)
        //{
        //    try
        //    {
        //        var dataMovimiento = await _context.RmMesaEntrada.FirstOrDefaultAsync(x =>
        //        x.NumeroEntrada == IdMovimiento);

        //        if (dataMovimiento == null)
        //        {
        //            return BadRequest("No se encontraron datos");
        //        }
        //        var dataMesaEntrada = from me in _context.RmMesaEntrada
        //                              join ee in _context.RmEstadosEntrada on me.EstadoEntrada equals ee.CodigoEstado
        //                              join ofi in _context.RmOficinasRegistrales on me.CodigoOficina equals ofi.CodigoOficina
        //                              join sol in _context.RmTipoSolicituds on me.TipoSolicitud equals sol.TipoSolicitud
        //                              join ag in _context.RmAsignaciones on me.NumeroEntrada equals ag.NroEntrada
        //                              where me.NumeroEntrada == dataMovimiento.NumeroEntrada
        //                              select new
        //                              {
        //                                  me.NroDocumentoPresentador,
        //                                  me.NombrePresentador,
        //                                  me.NroDocumentoRetirador,
        //                                  me.NombreRetirador,
        //                                  me.NumeroLiquidacionLetras,
        //                                  me.NroFormulario,
        //                                  ee.DescripEstado,
        //                                  me.FechaEntrada,
        //                                  me.FechaSalida,
        //                                  ofi.DescripOficina,
        //                                  me.NomTitular,
        //                                  sol.DescripSolicitud,
        //                                  me.NroOficio,
        //                                  me.TipoDocumento,
        //                                  me.NroBoleta,
        //                                  ag.IdUsuarioAsignado

        //                              };

        //        var dataTransaccion = from t in _context.RmTransacciones
        //                              where t.NumeroEntrada == dataMovimiento.NumeroEntrada
        //                              select new
        //                              {
        //                                  t.UsuarioSup,
        //                                  t.FechaAlta,
        //                                  t.ObservacionSup
        //                              };
        //        //if (dataTransaccion == null)
        //        //{
        //        //    return BadRequest("No se encontraron datos");
        //        //}
        //        //Obtener la ultima observacion de la entrada
        //        var obs = dataTransaccion.OrderByDescending(x => x.FechaAlta).FirstOrDefault().ObservacionSup;

        //        var dataNotaNegativa = from nt in _context.RmNotasNegativas
        //                               join u in _context.Usuarios on nt.IdUsuario equals u.CodUsuario
        //                               join p in _context.Personas on u.CodPersona equals p.CodPersona
        //                               where nt.IdEntrada == dataMovimiento.NumeroEntrada
        //                               select new
        //                               {
        //                                   nt.DescripNotaNegativa,
        //                                   p.Nombre,
        //                                   nt.FechaAlta
        //                               };
        //        var obsNota = dataNotaNegativa.OrderByDescending(x => x.FechaAlta).FirstOrDefault().DescripNotaNegativa;

        //        //if (dataNotaNegativa == null)
        //        //{
        //        //    return BadRequest("No se encontraron datos");
        //        //}

        //        var datatablaNota = from nt in _context.RmNotasNegativas
        //                            join u in _context.Usuarios on nt.IdUsuario equals u.CodUsuario
        //                            join p in _context.Personas on u.CodPersona equals p.CodPersona
        //                            join me in _context.RmMovimientosDocs on nt.IdEntrada equals me.NroEntrada
        //                            join ee in _context.RmEstadosEntrada on me.EstadoEntrada equals ee.CodigoEstado
        //                            where nt.IdEntrada == dataMovimiento.NumeroEntrada
        //                            select new
        //                            {
        //                                nt.IdUsuario,
        //                                p.Nombre,
        //                                me.FechaOperacion,
        //                                me.EstadoEntrada,
        //                                ee.DescripEstado
        //                            };

        //        //if (datatablaNota == null)
        //        //{
        //        //    return BadRequest("No se encontraron datos");
        //        //}


        //        var data = new
        //        {
        //            dataMesaEntrada,
        //            dataTransaccion,
        //            obs,
        //            dataNotaNegativa,
        //            obsNota,
        //            datatablaNota
        //        };

        //        return Ok(data);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //Este seria mi controlador completo 
        [HttpPost]
        public JsonResult SearchMesaEntrada([FromBody] DataTableAjaxPostModel model)
        {
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchFuncMesaEntrada(model, out filteredResultsCount, out totalResultsCount);

            var re = Json(new
            {
                draw = model.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res.ToList()
            });

            return re;
        }
        public IList<BuscadorCustom> SearchFuncMesaEntrada(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.Search != null) ? model.Search.Value : null;
            var take = model.Length;
            var skip = model.Start;

            string sortBy = "";
            bool sortDir = true;

            var result = GetDataFromDbaseEntradas(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            if (result == null)
            {
                return new List<BuscadorCustom>();
            }
            return result;
        }


        public List<BuscadorCustom> GetDataFromDbaseEntradas(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {

            if (String.IsNullOrEmpty(searchBy))
            {
                sortBy = "NumeroEntrada";
                sortDir = true;
            }
            //IQueryable<RmAsignacione> queryAsignaciones = _dbContext.RmAsignaciones.AsQueryable();
            IQueryable<RmMesaEntradum> queryMesaEntrada = _dbContext.RmMesaEntrada.AsQueryable();
            IQueryable<RmEstadosEntradum> queryEstado = _dbContext.RmEstadosEntrada.AsQueryable();
            IQueryable<RmTipoSolicitud> queryTipoSolicitud = _dbContext.RmTipoSolicituds.AsQueryable();
            IQueryable<RmMarcasSenale> queryMarcasSenal = _dbContext.RmMarcasSenales.AsQueryable();
            IQueryable<RmBoletasMarca> queryBoletaMarca = _dbContext.RmBoletasMarcas.AsQueryable();

            var queryFi = queryMesaEntrada.OrderByDescending(m => m.FechaEntrada).Join(queryTipoSolicitud,
                me => me.TipoSolicitud,
                ts => ts.TipoSolicitud,
                (me, ts) =>
            new BuscadorCustom
            {
                TipoSolicitud = ts.DescripSolicitud,
                NumeroEntrada = me.NumeroEntrada,
                NroDocTitular = me.NroDocumentoTitular,
                NomTitular = me.NomTitular,
                NroBoleta =me.NroBoleta
            });
           
            var queryFin = queryFi.GroupJoin(queryMarcasSenal,
               me => me.NumeroEntrada,
               ts => ts.NumeroEntrada,
               (me, ts) => new { me, ts })
                            .SelectMany(
                                x => x.ts.DefaultIfEmpty(),
                                (me, ts) => new { me.me, ts })
                            .Select(
                                x =>
                           new BuscadorCustom
                           {
                               TipoSolicitud = x.me.TipoSolicitud,
                               NumeroEntrada = x.me.NumeroEntrada,
                               NroDocTitular = x.me.NroDocTitular,
                               NomTitular = x.me.NomTitular,
                               NroBoleta = x.ts.NroBoleta
                           });

            var queryFinal = queryFin.GroupJoin(queryBoletaMarca,
             me => me.NroBoleta,
             ts => ts.NroBoleta.ToString(),
             (me, ts) => new { me, ts })
                          .SelectMany(
                              x => x.ts.DefaultIfEmpty(),
                              (me, ts) => new { me.me, ts })
                          .Select(
                              x =>
                         new BuscadorCustom
                         {
                             TipoSolicitud = x.me.TipoSolicitud,
                             NumeroEntrada = x.me.NumeroEntrada,
                             NroDocTitular = x.me.NroDocTitular,
                             NomTitular = x.me.NomTitular,
                             NroBoleta = x.ts.Descripcion
                         });


            //var resultado = queryFinal.ToArray();

            if (!string.IsNullOrEmpty(searchBy))
            {
                queryFinal = queryFinal.AsQueryable().Where(item =>
                        item.TipoSolicitud.Contains(searchBy) ||
                        item.NumeroEntrada.ToString().Contains(searchBy) ||
                        item.NroDocTitular.Contains(searchBy) ||
                        item.NomTitular.Contains(searchBy) ||
                        item.NroBoleta.Contains(searchBy)
                    );
            }

            var result = queryFinal
                          .Skip(skip)
                          .Take(take)
                          .ToList();

            filteredResultsCount = queryFinal.Count();
            totalResultsCount = queryFinal.Count();

            return result;
        }



        [HttpPost]
        public IActionResult GenerarPdf(ConsultaTrabajo nroEntrada)
        {
            var mesaSalida = GetTituloData(nroEntrada);
            // Lee la imagen y conviértela en una cadena de datos URI codificada en base64
            string imagePath = "wwwroot/assets/img/PJ/PoderJudicial.png"; // Ruta a la imagen en tu proyecto
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

            // Pasa la cadena de datos URI a la vista a través del modelo de vista
            mesaSalida.ImageDataUri = imageDataUri;

            // Lee la imagen y conviértela en una cadena de datos URI codificada en base64
            string imagePath2 = "wwwroot/assets/img/PJ/CorteSuprema.png"; // Ruta a la imagen en tu proyecto
            byte[] imageBytes2 = System.IO.File.ReadAllBytes(imagePath2);
            string imageDataUri2 = "data:image/png;base64," + Convert.ToBase64String(imageBytes2);

            // Pasa la cadena de datos URI a la vista a través del modelo de vista
            mesaSalida.ImageDataUri2 = imageDataUri2;

            // Lee la imagen y conviértela en una cadena de datos URI codificada en base64
            string imagePath3 = "wwwroot/assets/img/PJ/Compromiso.png"; // Ruta a la imagen en tu proyecto
            byte[] imageBytes3 = System.IO.File.ReadAllBytes(imagePath3);
            string imageDataUri3 = "data:image/png;base64," + Convert.ToBase64String(imageBytes3);

            // Pasa la cadena de datos URI a la vista a través del modelo de vista
            mesaSalida.ImageDataUri3 = imageDataUri3;
           
                // Renderizar la vista Razor a una cadena HTML
           var viewHtml = RenderViewToString("Diseño", mesaSalida);
           

            // Crear un documento PDF utilizando iText7
            MemoryStream memoryStream = new MemoryStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(memoryStream));
            Document document = new Document(pdfDoc);

            // Agregar el contenido HTML convertido al documento PDF
            HtmlConverter.ConvertToPdf(viewHtml, pdfDoc, new ConverterProperties());

            document.Close();

            // Convertir el MemoryStream a un arreglo de bytes
            byte[] pdfBytes = memoryStream.ToArray();
            memoryStream.Close();

            // Configurar el encabezado Content-Disposition
            Response.Headers["Content-Disposition"] = "inline; filename=Diseño-de-Titulo.pdf";

            // Devolver el PDF como un archivo descargable con el nuevo nombre
            return File(pdfBytes, "application/pdf");
        }

        // Otros métodos del controlador...

        private string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var engine = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine)) as Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine;
                var viewResult = engine.FindView(ControllerContext, viewName, false);

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelperOptions()
                );

                viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpPost]
        private DatosTituloCustom GetTituloData(ConsultaTrabajo nroEntrada)
        {
            try
            {
                var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(p => p.NumeroEntrada == nroEntrada.NumeroEntrada);
                var transaccionAnt = _dbContext.RmTransacciones.OrderBy(t => t.FechaAlta).Where(t => t.NroBoleta == mesaEntrada.NroBoleta).FirstOrDefault();
              
                var dataTitular = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == mesaEntrada.NroDocumentoTitular);
                var transaccionNew = _dbContext.RmTransacciones.FirstOrDefault(t=>t.NumeroEntrada== transaccionAnt.NumeroEntrada);              
                var result = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NumeroEntrada.Equals(transaccionNew.NumeroEntrada));
                //var autorizante = _dbContext.RmAutorizantes.FirstOrDefault(p => p.MatriculaRegistro == inscripcion.MatriculaAutorizante);

                string imagenSenhalPath = result?.SenalNombre ?? "";
                string imagenMarcaPath = result?.MarcaNombre ?? "";


                string imagenSenhalDataUri = ConvertImageToDataUri(imagenSenhalPath);
                string imagenMarcaDataUri = ConvertImageToDataUri(imagenMarcaPath);

                DatosTituloCustom tituloData = new()
                {
                    NumeroEntrada = nroEntrada.NumeroEntrada,
                    FechaEntrada = mesaEntrada.FechaEntrada,
                    ImagenMarca = imagenMarcaDataUri,
                    ImagenSenhal = imagenSenhalDataUri,
                    CodigoOficina = mesaEntrada?.CodigoOficina ?? 0,
                    NomTitular = mesaEntrada?.NomTitular ?? "",
                    NroDocumentoTitular = mesaEntrada?.NroDocumentoTitular ?? "",
                    CodPais = dataTitular?.CodPais ?? "",
                    CodProfesion = dataTitular?.Profesion ?? "",
                    FecNacimiento = dataTitular?.FecNacimiento ?? DateTime.MinValue,
                    CodEstadoCivil = dataTitular?.CodEstadoCivil ?? ""
                };
               
             

                return tituloData;


            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrió un error: " + ex.Message);

            }

        }
        private string ConvertImageToDataUri(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return null; // Manejar el caso cuando la ruta de la imagen no existe o es nula
            }

            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

            return imageDataUri;
        }

    }
}
