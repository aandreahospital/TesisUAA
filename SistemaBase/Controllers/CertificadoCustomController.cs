using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Ini;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using System.Data;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Html2pdf;
using iText.Commons.Actions.Contexts;
using static SistemaBase.ModelsCustom.CertificadoCustom;
using System.Linq;

namespace SistemaBase.Controllers
{
    public class CertificadoCustomController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        
        public CertificadoCustomController(DbvinDbContext context)
        {
            _dbContext = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMCERTIF", "Index", "CertificadoCustom" })]

        public IActionResult Index()
        {
            try
            {
                ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", "18");
                //ViewBag.Autorizante = new SelectList(_dbContext.RmAutorizantes, "IdAutorizante", "DescripAutorizante");
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error: " + ex.Message);
            }
        }

        /*public async Task<IActionResult> Get2(decimal nEntrada)
        {
            var mesaEntrada = await _dbContext.RmMesaEntrada.SingleOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
            var transaccion = await _dbContext.RmTransacciones.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
            var rmMarcasSenale = await _dbContext.RmMarcasSenales.SingleOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
            var nomRepresentante = await _dbContext.Personas.FirstOrDefaultAsync(p => p.CodPersona == transaccion.Representante);
            if (mesaEntrada == null)
            {
                return NotFound();
            }

            ViewBag.TipoSolicitud = new SelectList(_dbContext.RmTipoSolicituds, "TipoSolicitud", "DescripSolicitud", mesaEntrada.TipoSolicitud);
            ViewBag.Autorizante = new SelectList(_dbContext.RmAutorizantes, "IdAutorizante", "DescripAutorizante", mesaEntrada.IdAutorizante);


            CertificadoCustom mesaSalida = new()
            {
                NumeroEntrada = transaccion.NumeroEntrada,
                EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0,
                NroBoleta = transaccion.NroBoleta,
                IdTransaccion = transaccion.IdTransaccion,
                CantidadGanado = rmMarcasSenale.CantidadGanado,
                TipoSolicitud = mesaEntrada.TipoSolicitud,
                FechaActoJuridico = transaccion.FechaActoJuridico,
                IdAutorizante = mesaEntrada.IdAutorizante,
                Serie = transaccion.Serie,
                FormaConcurre = transaccion.FormaConcurre,
                Representante = transaccion.Representante,
                NombreRepresentante=nomRepresentante?.Nombre??""

            };
            return View("Index", mesaSalida);
        }*/

        public async Task<IActionResult> Get(decimal nEntrada)
        {
            try
            {
                var administrador = _dbContext.Usuarios.FirstOrDefault(u => u.CodUsuario == User.Identity.Name);
                var codGrupo = administrador?.CodGrupo ?? "";
               
                    var mesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

                if (mesaEntrada == null)
                {
                    return Json(new { Success = false, ErrorMessage = "El número de entrada no existe." });
                }
                var tipoSolicitud = mesaEntrada?.TipoSolicitud ?? 0;
                var listaConceptos = new List<decimal> { 12, 22};
                if (!listaConceptos.Contains(tipoSolicitud))
                {
                    return Json(new { Success = false, ErrorMessage = "Error, el tipo de solicitud no corresponde" });
                }
                if (mesaEntrada != null)
                {
                    var estadoRegistrador = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Registrador");
                    var enrevision = await _dbContext.RmEstadosEntrada.FirstOrDefaultAsync(e => e.DescripEstado == "En Revisión");
                    var rmTransaccion = await _dbContext.RmTransacciones.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                    if (codGrupo != "ADMIN")
                    {
                        if (rmTransaccion != null)
                        {
                            if (mesaEntrada.Reingreso != "S" && enrevision.CodigoEstado != mesaEntrada.EstadoEntrada)
                            {
                                return Json(new { Success = false, ErrorMessage = "Error, el trabajo ya se encuentra resgistrado" });
                            }
                        }
                    }


                    var boletaMarca = await _dbContext.RmBoletasMarcas.FirstOrDefaultAsync(b=>b.Descripcion == mesaEntrada.NroBoleta);
                    var nroBoletaMarca = boletaMarca?.NroBoleta??0;
                    var marcasSenal = _dbContext.RmMarcasSenales.FirstOrDefault(t=>t.NroBoleta == nroBoletaMarca.ToString());
                    var nroEntradaIm = marcasSenal?.NumeroEntrada??0;
                    var transaccion = await _dbContext.RmTransacciones.FirstOrDefaultAsync(p => p.NumeroEntrada == nroEntradaIm);
                    var idAutorizante = rmTransaccion?.IdAutorizante ?? 0;
                    var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(p => p.IdAutorizante == idAutorizante);

                    if (transaccion != null)
                    {
                      
                        var nomRepresentante = await _dbContext.Personas.FirstOrDefaultAsync(p => p.CodPersona == transaccion.Representante);
                        //var tipoSolicitud = await _dbContext.RmTipoSolicituds.FirstOrDefaultAsync(t=>t.TipoSolicitud == mesaEntrada.TipoSolicitud);
                        //var tipoOperacion = await _dbContext.RmTiposOperaciones.FirstOrDefaultAsync(o=>o.DescripTipoOperacion == tipoSolicitud.DescripSolicitud);
                        var rmMarcasSenale = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(p => p.NumeroEntrada == transaccion.NumeroEntrada);
                        //Que muestre como primer dato la descripcion CERTIFICADO
                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", "18");
                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada?.EstadoEntrada ?? 0);
                        var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);

                        // cargar para edicion
                        CertificadoCustom inscripcion = new()
                        {
                            NumeroEntrada = mesaEntrada.NumeroEntrada,
                            NroEntradaTrans = transaccion.NumeroEntrada,
                            EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0,
                            NroBoleta = mesaEntrada?.NroBoleta,
                            IdTransaccion = transaccion.IdTransaccion,
                            CantidadGanado = rmMarcasSenale?.CantidadGanado??0,
                            FechaActoJuridico = transaccion?.FechaActoJuridico,
                            FechaAlta= transaccion?.FechaAlta,
                            NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                            Serie = transaccion?.Serie??"",
                            FormaConcurre = transaccion?.FormaConcurre??"",
                            Representante = transaccion?.Representante ?? "",
                            NombreRepresentante = nomRepresentante?.Nombre ?? "",
                            Comentario = rmTransaccion?.Comentario??""
                        };
                        return View("Index", inscripcion);
                    }
                    else
                    {
                        if (estadoRegistrador.CodigoEstado != mesaEntrada.EstadoEntrada)
                        {
                            return Json(new { Success = false, ErrorMessage = "Error, debe recepcionar el trabajo" });
                        }

                        // cargar nuevo
                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", "18");
                       
                        CertificadoCustom inscripcion = new();
                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada?.EstadoEntrada ?? 0);
                        inscripcion.NumeroEntrada = nEntrada;
                        inscripcion.EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0;
                        inscripcion.NroBoleta = mesaEntrada?.NroBoleta ?? "";


                        return View("Index", inscripcion);
                        //return View("Index");
                    }
                }
                else
                {
                    return NotFound();
                }



            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrió un error: " + ex.Message);

            }
        }

        public async Task<IActionResult> AddCertificado(decimal nEntrada)
        {
            try
            {
                var mesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                var nuevaTrans = await _dbContext.RmTransacciones.OrderByDescending(o => o.FechaAlta).FirstOrDefaultAsync(t => t.NumeroEntrada == nEntrada);
                if (mesaEntrada != null)
                {
                    var estadoRegistrador = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Registrador");
                    var enrevision = await _dbContext.RmEstadosEntrada.FirstOrDefaultAsync(e => e.DescripEstado == "En Revisión");
                    var rmTransaccion = await _dbContext.RmTransacciones.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                    //if (rmTransaccion != null)
                    //{
                    //    var reingreso = _dbContext.RmReingresos.FirstOrDefault(r => r.NroEntrada == nEntrada);
                    //    if (enrevision.CodigoEstado != mesaEntrada.EstadoEntrada && reingreso == null)
                    //    {
                    //        return Json(new { Success = false, ErrorMessage = "Error, " + nEntrada + " ya se encuentra registrado" });
                    //    }
                    //}
                    //if (estadoRegistrador.CodigoEstado != mesaEntrada.EstadoEntrada)
                    //{
                    //    return Json(new { Success = false, ErrorMessage = "Error, debe recepcionar el trabajo" });
                    //}

                    var boletaMarca = await _dbContext.RmBoletasMarcas.FirstOrDefaultAsync(b => b.Descripcion == mesaEntrada.NroBoleta);
                    var nroBoletaMarca = boletaMarca?.NroBoleta ?? 0;
                    var marcasSenal = _dbContext.RmMarcasSenales.FirstOrDefault(t => t.NroBoleta == nroBoletaMarca.ToString());
                    var nroEntradaIm = marcasSenal?.NumeroEntrada ?? 0;
                    var transaccion = await _dbContext.RmTransacciones.FirstOrDefaultAsync(p => p.NumeroEntrada == nroEntradaIm);
                    var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(p => p.IdAutorizante == mesaEntrada.IdAutorizante);

                    if (transaccion != null)
                    {

                        var nomRepresentante = await _dbContext.Personas.FirstOrDefaultAsync(p => p.CodPersona == transaccion.Representante);
                        //var tipoSolicitud = await _dbContext.RmTipoSolicituds.FirstOrDefaultAsync(t=>t.TipoSolicitud == mesaEntrada.TipoSolicitud);
                        //var tipoOperacion = await _dbContext.RmTiposOperaciones.FirstOrDefaultAsync(o=>o.DescripTipoOperacion == tipoSolicitud.DescripSolicitud);
                        var rmMarcasSenale = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(p => p.NumeroEntrada == transaccion.NumeroEntrada);
                        //Que muestre como primer dato la descripcion CERTIFICADO
                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", "18");
                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada?.EstadoEntrada ?? 0);
                        var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);

                        // cargar para edicion
                        CertificadoCustom inscripcion = new()
                        {
                            NumeroEntrada = mesaEntrada.NumeroEntrada,
                            NroEntradaTrans = transaccion.NumeroEntrada,
                            EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0,
                            NroBoleta = mesaEntrada?.NroBoleta,
                            IdTransaccion = transaccion.IdTransaccion,
                            CantidadGanado = rmMarcasSenale?.CantidadGanado ?? 0,
                            FechaActoJuridico = transaccion?.FechaActoJuridico,
                            FechaAlta = transaccion?.FechaAlta,
                            NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                            Serie = transaccion?.Serie ?? "",
                            FormaConcurre = transaccion?.FormaConcurre ?? "",
                            Representante = transaccion?.Representante ?? "",
                            NombreRepresentante = nomRepresentante?.Nombre ?? "",
                            Comentario = nuevaTrans?.Comentario ?? "",
                            Observacion = nuevaTrans?.Observacion ?? ""

                        };
                        return View("Index", inscripcion);
                    }
                    else
                    {

                        // cargar nuevo
                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", "18");

                        CertificadoCustom inscripcion = new();
                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada?.EstadoEntrada ?? 0);
                        inscripcion.NumeroEntrada = nEntrada;
                        inscripcion.EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0;
                        inscripcion.NroBoleta = mesaEntrada?.NroBoleta ?? "";


                        return View("Index", inscripcion);
                        //return View("Index");
                    }
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                // Manejar el error de generación de PDF de alguna manera
                return BadRequest("Error al cargar la pagina " + ex.Message);
            }


        }
        public async Task<JsonResult> CargarDistritoBoleta(string nroBoleta)
        {
            IQueryable<RmDistrito> queryDistritos = _dbContext.RmDistritos.AsQueryable();
            IQueryable<RmMarcasXEstab> queryTransaccion = _dbContext.RmMarcasXEstabs.AsQueryable();
            IQueryable<RmTransaccione> queryMovimientos = _dbContext.RmTransacciones.AsQueryable();
            IQueryable<RmMarcasSenale> queryMarcasSenal = _dbContext.RmMarcasSenales.AsQueryable();

            var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(o=>o.Descripcion== nroBoleta);
            var nroBoletaMarca = boletaMarca?.NroBoleta??0;

            var marcaSenal = _dbContext.RmMarcasSenales.FirstOrDefault(o=>o.NroBoleta== nroBoletaMarca.ToString());
            var nroEntrada = marcaSenal?.NumeroEntrada??0;
            var transaccion = _dbContext.RmTransacciones.FirstOrDefault(t=>t.NumeroEntrada== nroEntrada);


            var queryEstableDistrito = queryTransaccion
                 .Where(m => m.IdTransaccion == transaccion.IdTransaccion)
                 .Join(
                     queryDistritos,
                     mov => mov.CodDistrito,
                     trans => trans.CodigoDistrito.ToString(),
                     (mov, trans) => new Distrito
                     {
                         CodDistrito = trans.CodigoDistrito.ToString(),
                         Descripcion = trans.DescripDistrito,
                         DescripcionEstable = mov.Descripcion
                     }
                 );
            var observacions = await queryEstableDistrito.ToArrayAsync();

            return Json(observacions);
        }
        public async Task<JsonResult> CargarDistrito(decimal idTransaccion)
        {
            IQueryable<RmDistrito> queryDistritos=_dbContext.RmDistritos.AsQueryable();
            IQueryable<RmMarcasXEstab> queryTransaccion = _dbContext.RmMarcasXEstabs.AsQueryable();
            IQueryable<RmTransaccione> queryMovimientos = _dbContext.RmTransacciones.AsQueryable();


            var queryEstableDistrito = queryTransaccion
                 .Where(m => m.IdTransaccion == idTransaccion)
                 .Join(
                     queryDistritos,
                     mov => mov.CodDistrito,
                     trans => trans.CodigoDistrito.ToString(),
                     (mov, trans) => new Distrito
                     {
                         CodDistrito = trans.CodigoDistrito.ToString(),
                         Descripcion = trans.DescripDistrito, 
                         DescripcionEstable=mov.Descripcion 
                     }
                 );
            var observacions = await queryEstableDistrito.ToArrayAsync();

            return Json(observacions);
        }


        public async Task<JsonResult> CargarPropietarioBoleta(string nroBoleta)
        {

            IQueryable<RmTitularesMarca> queryTitularMarca = _dbContext.RmTitularesMarcas.AsQueryable();
            IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();
            IQueryable<RmTiposPropiedad> queryTipoPropiedad = _dbContext.RmTiposPropiedads.AsQueryable();

            var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(o => o.Descripcion == nroBoleta);
            var nroBoletaMarca = boletaMarca?.NroBoleta ?? 0;

            var marcaSenal = _dbContext.RmMarcasSenales.FirstOrDefault(o => o.NroBoleta == nroBoletaMarca.ToString());
            var nroEntrada = marcaSenal?.NumeroEntrada ?? 0;
            var transaccion = _dbContext.RmTransacciones.FirstOrDefault(t => t.NumeroEntrada == nroEntrada);
            //Para obtener el nombre del usuario
            var queryTitularPersona = queryTitularMarca.Where(t => t.IdTransaccion == transaccion.IdTransaccion).AsQueryable().Join(queryPersona,
                  propietario => propietario.IdPropietario,
                  people => people.CodPersona,
                  (propietario, people) =>
                      new PersonaDireccion { IdPropietario = propietario.IdPropietario, Nombre = people.Nombre, TipoPropiedad = propietario.IdTipoPropiedad.ToString() });
            queryTitularPersona = queryTitularPersona.AsQueryable().Join(queryTipoPropiedad,
                persona => persona.TipoPropiedad,
                tipo => tipo.IdTipoPropiedad,
                (persona, tipo) =>
                    new PersonaDireccion { IdPropietario = persona.IdPropietario, Nombre = persona.Nombre, Descripcion = tipo.DescripcionTipoPropiedad, TipoPropiedad = persona.TipoPropiedad });

            var titularesMarcas = await queryTitularPersona.ToArrayAsync();

            return Json(titularesMarcas);
        }

        public async Task<JsonResult> CargarPropietario(int idTransaccion)
        {

            IQueryable<RmTitularesMarca> queryTitularMarca = _dbContext.RmTitularesMarcas.AsQueryable();
            IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();
            IQueryable<RmTiposPropiedad> queryTipoPropiedad = _dbContext.RmTiposPropiedads.AsQueryable();

            //Para obtener el nombre del usuario
            var queryTitularPersona = queryTitularMarca.Where(t => t.IdTransaccion == idTransaccion).AsQueryable().Join(queryPersona,
                  propietario => propietario.IdPropietario,
                  people => people.CodPersona,
                  (propietario, people) =>
                      new PersonaDireccion { IdPropietario = propietario.IdPropietario, Nombre = people.Nombre, TipoPropiedad = propietario.IdTipoPropiedad.ToString() });
            queryTitularPersona = queryTitularPersona.AsQueryable().Join(queryTipoPropiedad,
                persona => persona.TipoPropiedad,
                tipo => tipo.IdTipoPropiedad,
                (persona, tipo) =>
                    new PersonaDireccion { IdPropietario = persona.IdPropietario, Nombre = persona.Nombre, Descripcion = tipo.DescripcionTipoPropiedad, TipoPropiedad = persona.TipoPropiedad });

            var titularesMarcas = await queryTitularPersona.ToArrayAsync();

            return Json(titularesMarcas);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(decimal id, string nuevoEstado, string comentario, string tipoOperacion)
        {
            try
            {
                var mesaEntradum = await _dbContext.RmMesaEntrada.FindAsync(id);
                var estadoEntrada = await _dbContext.RmEstadosEntrada.FirstOrDefaultAsync(e => e.DescripEstado == nuevoEstado);
                var administrador = _dbContext.Usuarios.FirstOrDefault(u => u.CodUsuario == User.Identity.Name);
                var codGrupo = administrador?.CodGrupo ?? "";

                if (mesaEntradum != null)
                {
                    var transaccion = await _dbContext.RmTransacciones.FirstOrDefaultAsync(p => p.NumeroEntrada == id);
                    if (transaccion != null)
                    {
                        //var enrevision = await _dbContext.RmEstadosEntrada.FirstOrDefaultAsync(e => e.DescripEstado == "En Revisión");
                        //if (transaccion.EstadoTransaccion == enrevision.CodigoEstado.ToString())
                        //{
                            transaccion.NumeroEntrada = id;
                          //  transaccion.FechaAlta = DateTime.Now;
                            transaccion.Observacion = comentario;
                            if (codGrupo!="ADMIN")
                            {
                                transaccion.EstadoTransaccion = estadoEntrada.CodigoEstado.ToString();
                            }
                            transaccion.TipoOperacion = tipoOperacion;

                            _dbContext.Update(transaccion);
                            await _dbContext.SaveChangesAsync();
                        //}
                    }
                    else
                    {
                        RmTransaccione bdTransaccion = new()
                        {
                            NumeroEntrada = id,
                            FechaAlta = DateTime.Now,
                            Observacion = comentario,
                            EstadoTransaccion = estadoEntrada.CodigoEstado.ToString(),
                            TipoOperacion = tipoOperacion
                        };
                        await _dbContext.AddAsync(bdTransaccion);
                        await _dbContext.SaveChangesAsync();
                    }

                    mesaEntradum.EstadoEntrada = estadoEntrada.CodigoEstado;
                    _dbContext.Update(mesaEntradum);
                    await _dbContext.SaveChangesAsync();

                    RmMovimientosDoc movimientos = new()
                    {
                        NroEntrada = id,
                        CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                        FechaOperacion = DateTime.Now,
                        CodOperacion = "09", //Parametro para cambio de estado 
                        NroMovimientoRef = id.ToString(),
                        EstadoEntrada = mesaEntradum.EstadoEntrada
                    };
                    await _dbContext.AddAsync(movimientos);
                    await _dbContext.SaveChangesAsync();

                }

            }
            catch
            {
                // Manejo de errores
                return View("Error");
            }
            return Ok();
        }

        [HttpPost]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMCERTIF", "Create", "CertificadoCustom" })]

        public async Task<IActionResult> Create( CertificadoCustom transaccion)
        {
            var rmTransaccion = await _dbContext.RmTransacciones.OrderByDescending(o=>o.FechaAlta).Where(t=>t.NumeroEntrada== transaccion.NumeroEntrada).FirstOrDefaultAsync();
            var administrador = _dbContext.Usuarios.FirstOrDefault(u => u.CodUsuario == User.Identity.Name);
            var codGrupo = administrador?.CodGrupo ?? "";
            var nroBoleta = transaccion?.NroBoleta ?? "";
            var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.Descripcion == nroBoleta);
            if (rmTransaccion != null)
            {
                var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m=>m.NumeroEntrada== transaccion.NumeroEntrada);
                var idAuto = transaccion?.NombreAutorizante ?? "";
                var autorizantes = _dbContext.RmAutorizantes.FirstOrDefault(a=>a.DescripAutorizante == idAuto);
                //var interviniente = await _dbContext.RmIntervinientes.FirstOrDefaultAsync(a => a.Nombre == descAuto);
                if (autorizantes == null && idAuto != "")
                {
                    RmAutorizante rmAutorizante = new()
                    {
                        DescripAutorizante = idAuto
                    };
                    _dbContext.Add(rmAutorizante);
                    await _dbContext.SaveChangesAsync();
                    autorizantes = _dbContext.RmAutorizantes.FirstOrDefault(a => a.DescripAutorizante == idAuto);
                }
                    //rmTransaccion.NumeroEntrada = transaccion.NumeroEntrada;
                    rmTransaccion.Comentario = transaccion.Comentario;
                    //rmTransaccion.TipoOperacion = transaccion.TipoOperacion;
                    //rmTransaccion.Representante = transaccion.Representante;
                    //rmTransaccion.FormaConcurre = transaccion.FormaConcurre;
                    //rmTransaccion.FechaActoJuridico = transaccion.FechaActoJuridico;
                    rmTransaccion.IdAutorizante = autorizantes?.IdAutorizante??0 ;
                    if (codGrupo!="ADMIN")
                    {
                        rmTransaccion.EstadoTransaccion = mesaEntrada.EstadoEntrada.ToString();
                    }
                    //rmTransaccion.Serie = transaccion.Serie;
                    rmTransaccion.NroBoleta = boletaMarca.NroBoleta.ToString();
                    rmTransaccion.NombreRepresentante = transaccion.NombreRepresentante;
                    rmTransaccion.IdUsuario = transaccion.IdUsuario;
                    rmTransaccion.FechaAlta = DateTime.Now;

                    _dbContext.Update(rmTransaccion);


                    //var rmTitularesMarca = await _dbContext.RmTitularesMarcas.Where(p => p.IdTransaccion == transaccion.IdTransaccion).FirstOrDefaultAsync();
                    //if (rmTitularesMarca != null)
                    //{
                    //    RmTitularesMarca titulares = new()
                    //    {
                    //        IdTransaccion = rmTransaccion.IdTransaccion,
                    //        IdPropietario = rmTitularesMarca.IdPropietario,
                    //        IdTitular = rmTitularesMarca.IdPropietario,
                    //        FechaRegistro = DateTime.Now
                    //    };
                    //    await _dbContext.AddAsync(titulares);
                    //    await _dbContext.SaveChangesAsync();
                    //}


                    //var rmMarcasxEstable = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(t => t.IdTransaccion == transaccion.IdTransaccion);

                    //if (rmMarcasxEstable != null)
                    //{
                    //    RmMarcasXEstab ubicacion = new()
                    //    {
                    //        CodDistrito = rmMarcasxEstable.CodDistrito,
                    //        IdTransaccion = rmTransaccion.IdTransaccion,
                    //        Descripcion = rmMarcasxEstable.Descripcion,
                    //        GpsH = rmMarcasxEstable.GpsH,
                    //        GpsS = rmMarcasxEstable.GpsS,
                    //        GpsSc = rmMarcasxEstable.GpsSc,
                    //        GpsV = rmMarcasxEstable.GpsV,
                    //        GpsW = rmMarcasxEstable.GpsW

                    //    };

                    //    await _dbContext.AddAsync(ubicacion);
                    //}


                    await _dbContext.SaveChangesAsync();


            }

            //return RedirectToAction("Index");
            return View();
        }



        public IActionResult GenerarPdf(CertificadoCustom certificado)
        {
            var mesaSalida = GetTituloData(certificado);
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

            var transacion = _dbContext.RmTransacciones.OrderByDescending(o => o.FechaAlta).FirstOrDefault(t => t.NumeroEntrada == certificado.NumeroEntrada);
            var estadoNotaNegativa = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Nota Negativa/Registrador");
            var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m=>m.NumeroEntrada == certificado.NumeroEntrada);
            string viewHtml;
            var usuario = _dbContext.Usuarios.FirstOrDefault(u => u.CodUsuario == User.Identity.Name);
            var codGrupo = usuario?.CodGrupo ?? "";
            if (codGrupo != "ADMIN")
            {
                DateTime FechaActual = DateTime.Now.AddMinutes(-5);
                if (transacion.FechaAlta < FechaActual)
                {
                    viewHtml = null;

                }
                else
                {
                    if (mesaEntrada.EstadoEntrada == estadoNotaNegativa.CodigoEstado)
                    {
                        mesaSalida.Comentario = transacion.Observacion;
                        viewHtml = RenderViewToString("NotaNegativaPDF", mesaSalida);
                    }
                    else
                    {
                        // Renderizar la vista Razor a una cadena HTML
                        viewHtml = RenderViewToString("TicketPDF", mesaSalida);
                    }
                }
            }
            else
            {
                if (mesaEntrada.EstadoEntrada == estadoNotaNegativa.CodigoEstado)
                {
                    mesaSalida.Comentario = transacion.Observacion;
                    viewHtml = RenderViewToString("NotaNegativaPDF", mesaSalida);
                }
                else
                {
                    // Renderizar la vista Razor a una cadena HTML
                    viewHtml = RenderViewToString("TicketPDF", mesaSalida);
                }
            }
                   
            

            // Crear un documento PDF utilizando iText7
            MemoryStream memoryStream = new MemoryStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(memoryStream));
            Document document = new Document(pdfDoc);
            if (viewHtml != null)
            {
                // Agregar el contenido HTML convertido al documento PDF
                HtmlConverter.ConvertToPdf(viewHtml, pdfDoc, new ConverterProperties());
            }
            else
            {
                // Manejar el caso en que viewHtml es nulo
                return BadRequest("El contenido HTML es nulo");
            }
            document.Close();

            // Convertir el MemoryStream a un arreglo de bytes
            byte[] pdfBytes = memoryStream.ToArray();
            memoryStream.Close();

            // Configurar el encabezado Content-Disposition
            Response.Headers["Content-Disposition"] = "inline; filename=Titulo-de-Propiedad.pdf";

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

        private DatosTituloCustom GetTituloData(CertificadoCustom certificado)
        {
            var mesaEntrada = _dbContext.RmMesaEntrada.SingleOrDefault(p => p.NumeroEntrada == certificado.NumeroEntrada);
            var NroBoletaEntda = mesaEntrada?.NroBoleta ?? "";
            var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.Descripcion == NroBoletaEntda);
            var nroBolImg = boletaMarca?.NroBoleta ?? 0;
            var result = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NroBoleta == nroBolImg.ToString());
            var EntradaResul = result?.NumeroEntrada ?? 0;
            var transaccion = _dbContext.RmTransacciones.FirstOrDefault(p => p.NumeroEntrada == EntradaResul);
            var nuevaTransaccion = _dbContext.RmTransacciones.FirstOrDefault(t=>t.NumeroEntrada== certificado.NumeroEntrada);
            //var titularMarca = _dbContext.RmTitularesMarcas.FirstOrDefault(p=>p.IdTransaccion == transaccion.IdTransaccion);
            //var nomTitular = _dbContext.Personas.FirstOrDefault(p=>p.CodPersona == titularMarca.IdTitular.ToString());
            var dataEstablecimiento = _dbContext.RmMarcasXEstabs.FirstOrDefault(p => p.IdTransaccion == transaccion.IdTransaccion);
            var boletaSenal = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NumeroEntrada == transaccion.NumeroEntrada);
            //var autorizante = _dbContext.RmAutorizantes.FirstOrDefault(a=>a.IdAutorizante== nuevaTransaccion.IdAutorizante);
            if (mesaEntrada == null)
            {
                return null; // Manejar el caso cuando no se encuentra la entrada
            }


            DatosTituloCustom tituloData = new()
            {
                NumeroEntrada = mesaEntrada.NumeroEntrada,
                FechaActual = GetFechaActual(),
                FechaEntrada = mesaEntrada?.FechaEntrada ?? DateTime.MinValue,
                FechaEntradaFecha = GetFechaEntradaFecha(DateOnly.FromDateTime(mesaEntrada?.FechaEntrada ?? DateTime.MinValue)),
                FechaEntradaHora = GetFechaEntradaHora(mesaEntrada?.FechaEntrada ?? DateTime.MinValue),
                NroBoleta = mesaEntrada?.NroBoleta ?? "",
                NomTitular = mesaEntrada?.NomTitular ?? "",
                NroDocumentoTitular = mesaEntrada?.NroDocumentoTitular ?? "",
                Distritos = GetDistritos(),
                DistritoEstable = dataEstablecimiento?.CodDistrito ?? "",
                NombreAutorizante = certificado?.NombreAutorizante ?? "",
                MatriculaAutorizante = certificado?.MatriculaAutorizante ?? 0,
                Autorizantes = GetAutorizante(),
                IdAutorizante = mesaEntrada?.IdAutorizante ?? 0,
                FechaActoJuridico = DateOnly.FromDateTime(transaccion?.FechaAlta ?? DateTime.MinValue),
                NroBoletaSenal = boletaSenal?.NroBoleta ?? "",
                Operaciones = GetOperaciones(),
                TipoOperacion = transaccion?.TipoOperacion ?? "",
                Oficinas = GetOficinasRegistrales(),
                CodigoOficina = mesaEntrada?.CodigoOficina ?? 0,
                Comentario = certificado?.Comentario ?? ""

            };
            return tituloData;
        }


        private List<SelectListItem> GetOficinasRegistrales()
        {
            using var dbContext = new DbvinDbContext();

            var oficinas = dbContext.RmOficinasRegistrales
                .Select(o => new SelectListItem
                {
                    Text = $"{o.DescripOficina}",
                    Value = o.CodigoOficina.ToString()
                })
                .ToList();

            return oficinas;
        }

        private List<SelectListItem> GetOperaciones()
        {
            using var dbContext = new DbvinDbContext();

            var oficinas = dbContext.RmTiposOperaciones
                .Select(o => new SelectListItem
                {
                    Text = $"{o.DescripTipoOperacion}",
                    Value = o.TipoOperacion.ToString()
                })
                .ToList();

            return oficinas;
        }

        private List<SelectListItem> GetCodCiudad()
        {
            using var dbContext = new DbvinDbContext();

            var oficinas = dbContext.RmAutorizantes
                .Select(o => new SelectListItem
                {
                    Text = $"{o.CodCiudad}",
                    Value = o.IdAutorizante.ToString()
                })
                .ToList();

            return oficinas;
        }


        private List<SelectListItem> GetCiudades()
        {
            using var dbContext = new DbvinDbContext();

            var oficinas = dbContext.Ciudades
                .Select(o => new SelectListItem
                {
                    Text = $"{o.Descripcion}",
                    Value = o.CodCiudad.ToString()
                })
                .ToList();

            return oficinas;
        }


        private string GetFechaActual()
        {
            // Obtener la fecha actual
            DateTime fechaActual = DateTime.Now;

            // Crear una cadena con el formato deseado
            string fechaFormateada = fechaActual.ToString("dd 'DE' MMMM 'DE' yyyy", new System.Globalization.CultureInfo("es-ES"));

            return fechaFormateada;
        }

        private List<SelectListItem> GetDistritos()
        {
            using var dbContext = new DbvinDbContext();

            var oficinas = dbContext.RmDistritos
                .Select(o => new SelectListItem
                {
                    Text = $"{o.DescripDistrito}",
                    Value = o.CodigoDistrito.ToString()
                })
                .ToList();

            return oficinas;
        }

        private List<SelectListItem> GetMatriculaRegistro()
        {
            using var dbContext = new DbvinDbContext();

            var tiposDocumentos = dbContext.RmAutorizantes
                .Select(d => new SelectListItem
                {
                    Text = $"{d.MatriculaRegistro}",
                    Value = d.IdAutorizante.ToString()
                })
                .ToList();

            return tiposDocumentos;
        }

        private List<SelectListItem> GetAutorizante()
        {
            using var dbContext = new DbvinDbContext();

            var autorizante = dbContext.RmIntervinientes
                .Select(d => new SelectListItem
                {
                    Text = $"{d.Nombre}",
                    Value = d.IdProfesional.ToString()
                })
                .ToList();

            return autorizante;
        }

        //private string ConvertImageToDataUri(string imagePath)
        //{
        //    if (string.IsNullOrEmpty(imagePath))
        //    {
        //        return null; // Manejar el caso cuando la ruta de la imagen no existe o es nula
        //    }

        //    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
        //    string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

        //    return imageDataUri;
        //}

        private DateOnly GetFechaEntradaFecha(DateOnly fechaEntrada)
        {
            // Extrae solo la fecha de la FechaEntrada
            return fechaEntrada;
        }

        private TimeSpan GetFechaEntradaHora(DateTime fechaEntrada)
        {
            // Extrae solo la hora de la FechaEntrada
            return fechaEntrada.TimeOfDay;
        }
    }
}
