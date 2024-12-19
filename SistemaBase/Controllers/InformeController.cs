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
using static SistemaBase.ModelsCustom.InformeCustom;
using static SistemaBase.ModelsCustom.TransaccionCustom;
//using AspNetCore;

namespace SistemaBase.Controllers
{
    public class InformeController : Controller
    {
        private readonly DbvinDbContext _dbContext;

        public InformeController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMINFORM", "Index", "Informe" })]

        public IActionResult Index()
        {
            try
            {
                  ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", "24");
                 return View();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error: " + ex.Message);
            }
        }

        public IActionResult BuscarImagen(string nroBoleta)
        {

            if (nroBoleta != null)
            {

                var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b=>b.Descripcion== nroBoleta);
                var nroBoletaMarca = boletaMarca?.NroBoleta??0;
                var result = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NroBoleta ==nroBoletaMarca.ToString());
                var nroTrans = result?.NumeroEntrada;
                var transaccion = _dbContext.RmTransacciones.Where(t => t.NumeroEntrada == nroTrans).FirstOrDefault();
                if (result != null)
                {
                    try
                    {
                        string imagePath = result?.MarcaNombre??""; // Ruta a la imagen en tu proyecto
                        byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                        string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                        string imagePath2 = result?.SenalNombre??""; // Ruta a la imagen en tu proyecto
                        byte[] imageBytes2 = System.IO.File.ReadAllBytes(imagePath2);
                        string imageDataUri2 = "data:image/png;base64," + Convert.ToBase64String(imageBytes2);
                        var idTransaccion = transaccion?.IdTransaccion??0;
                        var estableDis = _dbContext.RmMarcasXEstabs.FirstOrDefault(m=>m.IdTransaccion== idTransaccion);
                        var codDistrito = estableDis?.CodDistrito??"";
                        var distrito = _dbContext.RmDistritos.FirstOrDefault(d=>d.CodigoDistrito.ToString() == codDistrito);

                        return Json(new { Success = true, srcmarca = imageDataUri, srcsenhal = imageDataUri2, distrito=distrito?.DescripDistrito??"", departamento= estableDis?.Descripcion??"" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Success = false, ErrorMessage = ex.Message });
                    }
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Error" });
                }
            }
            else
            {
                return NotFound();
            }

        }
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

                if (mesaEntrada != null)
                {
                    var boletaMarca = await _dbContext.RmBoletasMarcas.FirstOrDefaultAsync(b=>b.Descripcion == mesaEntrada.NroBoleta);
                    var nroBoletaMarca = boletaMarca?.NroBoleta??0;
                  
                    var estadoRegistrador = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Registrador");
                    var enrevision = await _dbContext.RmEstadosEntrada.FirstOrDefaultAsync(e => e.DescripEstado == "En Revisión");
                    var rmTransaccion = await _dbContext.RmTransacciones.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                    var idAutorizante = rmTransaccion?.IdAutorizante ?? 0;
                    var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(p => p.IdAutorizante == idAutorizante);
                    var estadoRetiradoObs = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Retirado/Observado");
                    var estadoRetiradoNN = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Retirado/Nota negativa");
                    var estadoRetiradoAprob = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Retirado/Aprobado");

                    if (codGrupo != "ADMIN")
                    {
                        if (rmTransaccion != null)
                        {
                            if (mesaEntrada.Reingreso != "S" && enrevision.CodigoEstado != mesaEntrada.EstadoEntrada)
                            {
                                return Json(new { Success = false, ErrorMessage = "Error, el trabajo ya se encuentra resgistrado" });
                            }
                            if ((mesaEntrada.Reingreso == "S" && estadoRetiradoAprob.CodigoEstado.ToString() != rmTransaccion.EstadoTransaccion) &&
                                (mesaEntrada.Reingreso == "S" && estadoRetiradoObs.CodigoEstado.ToString() != rmTransaccion.EstadoTransaccion) &&
                                (mesaEntrada.Reingreso == "S" && estadoRetiradoNN.CodigoEstado.ToString() != rmTransaccion.EstadoTransaccion))
                            {
                                return Json(new { Success = false, ErrorMessage = "Error, el trabajo ya se encuentra resgistrado" });
                            }

                        }
                    }

                    var marcasSenal = _dbContext.RmMarcasSenales.FirstOrDefault(m=>m.NroBoleta== nroBoletaMarca.ToString());
                    var NroEntradaBoleta = marcasSenal?.NumeroEntrada??0;
                    var transaccion = await _dbContext.RmTransacciones.FirstOrDefaultAsync(p => p.NumeroEntrada== NroEntradaBoleta);
                  
                    if (rmTransaccion != null)
                    {
                        //var nomRepresentante = await _dbContext.Personas.FirstOrDefaultAsync(p => p.CodPersona == transaccion.Representante);
                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada?.EstadoEntrada ?? 0);

                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", rmTransaccion.TipoOperacion);

                        //var entradaBoleta = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == rmTransaccion.NumeroEntrada);
                        //var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == rmTransaccion.IdTransaccion);
                        //var marcasSenal = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(b=>b.NumeroEntrada== entradaBoleta.NumeroEntrada);
                        //var nroBoletaMarca = marcasSenal?.NroBoleta ?? "0";
                        //var boletamarca = await _dbContext.RmBoletasMarcas.FirstOrDefaultAsync(b => b.NroBoleta == Convert.ToInt64(nroBoletaMarca));
                        InformeCustom inscripcion = new()
                        {
                            NumeroEntrada = nEntrada,
                            EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0,
                            NroEntradaTrans = transaccion?.NumeroEntrada??0,
                            NroBoleta = mesaEntrada?.NroBoleta ?? "",
                            IdTransaccion = transaccion?.IdTransaccion ?? 0,
                            NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                            NomTitular = mesaEntrada?.NomTitular ?? "",
                            NroDocumentoTitular = mesaEntrada?.NroDocumentoTitular ?? "",
                            FechaAlta = transaccion?.FechaAlta,
                            Comentario = rmTransaccion.Comentario
                        };
                        return View("Index", inscripcion);
                    }
                    else
                    {
                        if (estadoRegistrador.CodigoEstado != mesaEntrada.EstadoEntrada)
                        {
                            return Json(new { Success = false, ErrorMessage = "Error, debe recepcionar el trabajo" });
                        }
                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion","24");
                        //var nroBoletaMarca = mesaEntrada?.NroBoleta ?? "0";
                        //var boletamarca = await _dbContext.RmBoletasMarcas.FirstOrDefaultAsync(b => b.NroBoleta == Convert.ToInt64(nroBoletaMarca));
                        InformeCustom inscripcion = new()
                        {
                            NumeroEntrada = mesaEntrada.NumeroEntrada,
                            EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0,
                            NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            MatriculaAutorizante = autorizante?.MatriculaRegistro??0,
                            NomTitular = mesaEntrada?.NomTitular??"",
                            NroDocumentoTitular =mesaEntrada?.NroDocumentoTitular??"",
                            NroBoleta = mesaEntrada?.NroBoleta??""
                        };
                        return View("Index", inscripcion);
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

        public async Task<IActionResult> AddInforme(decimal nEntrada)
        {
            try
            {
                var mesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                var nuevaTrans = await _dbContext.RmTransacciones.OrderByDescending(o => o.FechaAlta).FirstOrDefaultAsync(t => t.NumeroEntrada == nEntrada);

                if (mesaEntrada != null)
                {
                    var transaccion = await _dbContext.RmTransacciones.Where(t => t.NroBoleta != null).FirstOrDefaultAsync(p => p.NroBoleta == mesaEntrada.NroBoleta);
                    var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(p => p.IdAutorizante == mesaEntrada.IdAutorizante);

                    if (transaccion != null)
                    {
                        //var nomRepresentante = await _dbContext.Personas.FirstOrDefaultAsync(p => p.CodPersona == transaccion.Representante);

                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", nuevaTrans?.EstadoTransaccion ?? "");
                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion");
                        var tiposOper = await _dbContext.RmTiposOperaciones.FirstOrDefaultAsync(p => p.TipoOperacion == Convert.ToInt32(nuevaTrans.TipoOperacion));
                        var descrOperacion = tiposOper?.DescripTipoOperacion ?? "";
                        var entradaBoleta = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == transaccion.NumeroEntrada);
                        var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);

                        InformeCustom inscripcion = new()
                        {
                            NumeroEntrada = nEntrada,
                            EstadoEntrada = Convert.ToInt64(nuevaTrans?.EstadoTransaccion),
                            NroEntradaTrans = transaccion.NumeroEntrada,
                            TipoOperacion = descrOperacion,
                            NroBoleta = transaccion?.NroBoleta ?? "",
                            IdTransaccion = transaccion?.IdTransaccion ?? 0,
                            NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                            NomTitular = mesaEntrada?.NomTitular ?? "",
                            NroDocumentoTitular = mesaEntrada?.NroDocumentoTitular ?? "",
                            FechaAlta = transaccion?.FechaAlta,
                            Comentario = nuevaTrans?.Comentario ?? "",
                            Observacion = nuevaTrans?.Observacion ?? ""
                        };
                        return View("Index", inscripcion);
                    }
                    else
                    {
                        var transacciones = await _dbContext.RmTransacciones.FirstOrDefaultAsync(t => t.NumeroEntrada == mesaEntrada.NumeroEntrada);
                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion");
                        InformeCustom inscripcion = new()
                        {
                            NumeroEntrada = mesaEntrada.NumeroEntrada,
                            EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0,
                            NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                            NomTitular = mesaEntrada?.NomTitular ?? "",
                            NroDocumentoTitular = mesaEntrada?.NroDocumentoTitular ?? "",
                            NroBoleta = mesaEntrada?.NroBoleta ?? "",
                            Comentario = transacciones?.Comentario ?? ""
                        };
                        return View("Index", inscripcion);
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
        public async Task<IActionResult> GetTitular(string codPersona)
        {
            try
            {
                var mesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NroDocumentoTitular == codPersona);

                if (mesaEntrada != null)
                {
                    var NroBoleta = mesaEntrada?.NroBoleta??"";
                    var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b=>b.Descripcion == NroBoleta);
                    var nroBoletaMarca = boletaMarca?.NroBoleta ?? 0;

                    var transaccion = await _dbContext.RmTransacciones.Where(t => t.NroBoleta != null).FirstOrDefaultAsync(p => p.NroBoleta == nroBoletaMarca.ToString());
                    //var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(p => p.IdAutorizante == mesaEntrada.IdAutorizante);

                    if (transaccion != null)
                    {
                        //var nomRepresentante = await _dbContext.Personas.FirstOrDefaultAsync(p => p.CodPersona == transaccion.Representante);

                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada?.EstadoEntrada ?? 0);
                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", "20");

                        var entradaBoleta = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == transaccion.NumeroEntrada);
                        var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);
                        //var nroBoletaMarca = entradaBoleta?.NroBoleta??"0";
                        //var boletamarca = await _dbContext.RmBoletasMarcas.FirstOrDefaultAsync(b => b.NroBoleta == Convert.ToInt64(nroBoletaMarca));
                        
                        InformeCustom inscripcion = new()
                        {
                            //EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0,
                            NroEntradaTrans = transaccion.NumeroEntrada,
                            NroBoleta = mesaEntrada?.NroBoleta ?? "",
                            IdTransaccion = transaccion?.IdTransaccion ?? 0,
                            //NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            //MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                            NomTitular = mesaEntrada?.NomTitular ?? "",
                            NroDocumentoTitular = mesaEntrada?.NroDocumentoTitular ?? "",
                            FechaAlta = transaccion?.FechaAlta
                        };
                        return View("Index", inscripcion);
                    }
                    else
                    {
                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", "20");
                        InformeCustom inscripcion = new()
                        {
                            //NumeroEntrada = mesaEntrada.NumeroEntrada,
                            //EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0,
                            //NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            //MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                            NomTitular = mesaEntrada?.NomTitular ?? "",
                            NroDocumentoTitular = mesaEntrada?.NroDocumentoTitular ?? "",
                            NroBoleta = mesaEntrada?.NroBoleta ?? ""
                        };
                        return View("Index", inscripcion);
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
        public async Task<JsonResult> CargarDistrito(decimal idTransaccion)
        {
            IQueryable<RmDistrito> queryDistritos = _dbContext.RmDistritos.AsQueryable();
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
                     DescripcionEstable = mov.Descripcion
                 }
             );
            var observacions = await queryEstableDistrito.ToArrayAsync();

            return Json(observacions);
        }





        public async Task<JsonResult> CargarOperaciones(string codPersona)
        {

            IQueryable<RmTitularesMarca> queryTitularMarca = _dbContext.RmTitularesMarcas.AsQueryable();
            IQueryable<RmTransaccione> queryTransaccion = _dbContext.RmTransacciones.AsQueryable();
            IQueryable<RmTiposOperacione> queryTipoOperacion = _dbContext.RmTiposOperaciones.AsQueryable();
            IQueryable<RmMesaEntradum> queryEntrada = _dbContext.RmMesaEntrada.AsQueryable();
            IQueryable<RmDistrito> queryDistrito = _dbContext.RmDistritos.AsQueryable();
            IQueryable<RmMarcasXEstab> queryMarcasEstab = _dbContext.RmMarcasXEstabs.AsQueryable();
            IQueryable<RmBoletasMarca> queryBoletaMarca = _dbContext.RmBoletasMarcas.AsQueryable();


            //Para obtener transaccion 
            var queryTitularTrans = queryTitularMarca.Where(t => t.IdTitular == codPersona ).AsQueryable().Join(queryTransaccion,
                titu => titu.IdTransaccion,
                trans => trans.IdTransaccion,
                (titu, trans)=>
                new Operaciones { NroEntrada= trans.NumeroEntrada, TipoOperacion = trans.TipoOperacion, NroBoleta = trans.NroBoleta } );


            var primer = await queryTitularTrans.ToArrayAsync();

            //Para obtener NroBoleta
            var queryBoleta = queryTitularTrans.AsQueryable().Join(queryBoletaMarca,
                trans => trans.NroBoleta,
                bol=> bol.NroBoleta.ToString(),
                (trans, bol)=> new Operaciones { NroEntrada = trans.NroEntrada, TipoOperacion = trans.TipoOperacion, NroBoleta = bol.Descripcion, FechaEntrada = trans.FechaEntrada });


            //Para obtener transaccion 
            var queryTitularEntrada = queryBoleta.AsQueryable().Join(queryEntrada,
                titu => titu.NroEntrada,
                trans => trans.NumeroEntrada,
                (titu, trans) =>
                new Operaciones { NroEntrada = trans.NumeroEntrada, TipoOperacion = titu.TipoOperacion, NroBoleta = trans.NroBoleta, FechaEntrada = trans.FechaEntrada });


            var segundo = await queryTitularEntrada.ToArrayAsync();


            //Para obtener Tipo Operacion 
            var quryFinal = queryTitularEntrada.AsQueryable().Join(queryTipoOperacion,
                trans => trans.TipoOperacion,
                tipo => tipo.TipoOperacion.ToString(),
                (trans, tipo)=>
                new Operaciones { NroEntrada = trans.NroEntrada, TipoOperacion = tipo.DescripTipoOperacion, NroBoleta = trans.NroBoleta , FechaEntrada = trans.FechaEntrada }
                ).GroupBy(op => op.NroEntrada)
                .Select(group => group.First());

            var titularesMarcas = await quryFinal.ToArrayAsync();

            return Json(titularesMarcas);
        }
        public async Task<JsonResult> CargarOperacionesBoleta(string nroBoleta)
        {

            IQueryable<RmTitularesMarca> queryTitularMarca = _dbContext.RmTitularesMarcas.AsQueryable();
            IQueryable<RmTransaccione> queryTransaccion = _dbContext.RmTransacciones.AsQueryable();
            IQueryable<RmTiposOperacione> queryTipoOperacion = _dbContext.RmTiposOperaciones.AsQueryable();
            IQueryable<RmBoletasMarca> queryBoleta = _dbContext.RmBoletasMarcas.AsQueryable();
            IQueryable<RmMesaEntradum> queryEntrada = _dbContext.RmMesaEntrada.AsQueryable();
            IQueryable<RmMarcasXEstab> queryMarcasEstab = _dbContext.RmMarcasXEstabs.AsQueryable();

            var transaccion = _dbContext.RmBoletasMarcas.FirstOrDefault(t=>t.Descripcion==nroBoleta);
            var boletaTrans = transaccion?.NroBoleta;
            //Para obtener transaccion 
            var queryTitularEntrada = queryTransaccion.Where(t=>t.NroBoleta == boletaTrans.ToString()).AsQueryable().Join(queryEntrada,
                titu => titu.NumeroEntrada,
                trans => trans.NumeroEntrada,
                (titu, trans) =>
                new Operaciones { NroEntrada = trans.NumeroEntrada, TipoOperacion = titu.TipoOperacion, NroBoleta = titu.NroBoleta, FechaEntrada = trans.FechaEntrada });


            var segundo = await queryTitularEntrada.ToArrayAsync();


            queryTitularEntrada = queryTitularEntrada.AsQueryable().Join(queryBoleta, 
                en => en.NroBoleta,
                op=> op.NroBoleta.ToString(),
                (en, op)=> new Operaciones{ NroEntrada = en.NroEntrada, TipoOperacion = en.TipoOperacion, NroBoleta= op.Descripcion, FechaEntrada = en.FechaEntrada} );



            //Para obtener Tipo Operacion 
            var quryFinal = queryTitularEntrada.AsQueryable().Join(queryTipoOperacion,
                trans => trans.TipoOperacion,
                tipo => tipo.TipoOperacion.ToString(),
                (trans, tipo) =>
                new Operaciones { NroEntrada = trans.NroEntrada, TipoOperacion = tipo.DescripTipoOperacion, NroBoleta = trans.NroBoleta, FechaEntrada = trans.FechaEntrada }
                ).GroupBy(op => op.NroEntrada)
                .Select(group => group.First());

            var titularesMarcas = await quryFinal.ToArrayAsync();

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
                    decimal? nroEntrada = null;
                    var enrevision = await _dbContext.RmEstadosEntrada.FirstOrDefaultAsync(e => e.DescripEstado == "En Revisión");
                    var EntradaRevision = _dbContext.RmMesaEntrada.Where(m => m.EstadoEntrada == enrevision.CodigoEstado).FirstOrDefault(m => m.NumeroEntrada == id);
                    if (codGrupo == "ADMIN")
                    {
                        nroEntrada = id;
                    }
                    else
                    {
                        nroEntrada = EntradaRevision?.NumeroEntrada ?? 0;
                    }
                    var transaccion = await _dbContext.RmTransacciones.OrderByDescending(o => o.FechaAlta).FirstOrDefaultAsync(p => p.NumeroEntrada == nroEntrada);
                    if (transaccion != null)
                    {
                        if (nuevoEstado != "Aprobado/Registrador")
                        { //Eliminar el registro cargado de esa transaccion
                            if (nuevoEstado == "Nota Negativa/Registrador") //si es nota negativa se agrega en la tabla
                            {
                                var notasNegativas = _dbContext.RmNotasNegativas.FirstOrDefault(n => n.IdEntrada == id);
                                if (notasNegativas == null)
                                {
                                    RmNotasNegativa notaNegativa = new()
                                    {
                                        IdEntrada = id,
                                        FechaAlta = DateTime.Now,
                                        DescripNotaNegativa = comentario,
                                        IdUsuario = User.Identity.Name
                                    };
                                    await _dbContext.AddAsync(notaNegativa);
                                }
                                else
                                {
                                    notasNegativas.DescripNotaNegativa = comentario;
                                    notasNegativas.IdUsuario = User.Identity.Name;
                                    notasNegativas.FechaAlta = DateTime.Now;
                                    _dbContext.SaveChanges();
                                }

                            }
                            transaccion.NroBoleta = null;
                            transaccion.Comentario = null;
                            //transaccion.TipoOperacion = ;se mantiene
                            transaccion.Representante = null;
                            transaccion.FormaConcurre = null;
                            transaccion.FechaActoJuridico = null;
                            transaccion.IdAutorizante = null;
                            transaccion.EstadoTransaccion = estadoEntrada.CodigoEstado.ToString();
                            if (comentario != null)
                            {
                                transaccion.Observacion = comentario;
                                //transaccion.TipoOperacion = tipoOperacion;
                            }
                            await _dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            if (codGrupo != "ADMIN")
                            {
                                transaccion.EstadoTransaccion = estadoEntrada.CodigoEstado.ToString();
                            }
                            if (comentario != null)
                            {
                                transaccion.Observacion = comentario;
                                //transaccion.TipoOperacion = tipoOperacion;
                            }

                            _dbContext.Update(transaccion);

                        }


                    }
                    else
                    {
                        if (nuevoEstado == "Nota Negativa/Registrador") //si es nota negativa se agrega en la tabla
                        {
                            var notasNegativas = _dbContext.RmNotasNegativas.FirstOrDefault(n => n.IdEntrada == id);
                            if (notasNegativas == null)
                            {
                                RmNotasNegativa notaNegativa = new()
                                {
                                    IdEntrada = id,
                                    FechaAlta = DateTime.Now,
                                    DescripNotaNegativa = comentario,
                                    IdUsuario = User.Identity.Name
                                };
                                await _dbContext.AddAsync(notaNegativa);
                            }
                            else
                            {
                                notasNegativas.DescripNotaNegativa = comentario;
                                notasNegativas.IdUsuario = User.Identity.Name;
                                notasNegativas.FechaAlta = DateTime.Now;
                                _dbContext.SaveChanges();
                            }

                        }
                        RmTransaccione bdTransaccion = new()
                        {
                            NumeroEntrada = id,
                            FechaAlta = DateTime.Now,
                            Observacion = comentario,
                            IdUsuario = User.Identity.Name,
                            EstadoTransaccion = estadoEntrada.CodigoEstado.ToString(),
                            TipoOperacion = tipoOperacion
                        };
                        await _dbContext.AddAsync(bdTransaccion);

                    }

                    if (codGrupo != "ADMIN")
                    {
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
                    }
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
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMINFORM", "Create", "Informe" })]

        public async Task<IActionResult> Create(InformeCustom transaccion)
        {
            var rmMesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(t => t.NumeroEntrada == transaccion.NumeroEntrada);
            var rmTransaccion = await _dbContext.RmTransacciones.OrderByDescending(t=>t.FechaAlta).FirstOrDefaultAsync(t => t.NumeroEntrada == transaccion.NumeroEntrada );
           
            if (rmTransaccion != null)
            {
                var nroBoleta = transaccion?.NroBoleta??"";
                var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b=>b.Descripcion== nroBoleta);
                var NroBolmarca = boletaMarca?.NroBoleta ?? 0;
                //var IdAutorizante = rmMesaEntrada?.IdAutorizante ?? 0;
                //var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(a => a.IdAutorizante == IdAutorizante);
                //var desAuto = autorizante?.DescripAutorizante;
                //var interviniente = _dbContext.RmIntervinientes.FirstOrDefault(i=>i.Nombre == desAuto);
                var idAuto = transaccion?.NombreAutorizante ?? "";
                var autorizantes = _dbContext.RmAutorizantes.FirstOrDefault(a => a.DescripAutorizante == idAuto);
                //var interviniente = await _dbContext.RmIntervinientes.FirstOrDefaultAsync(a => a.Nombre == descAuto);
                if (autorizantes == null && idAuto != "")
                {
                    RmAutorizante rmAutorizante = new()
                    {
                        DescripAutorizante = idAuto
                    };
                    _dbContext.Add(rmAutorizante);
                    await _dbContext.SaveChangesAsync();
                    autorizantes = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(a => a.DescripAutorizante == idAuto);
                }
                if (rmMesaEntrada != null)
                {
                    rmTransaccion.NumeroEntrada = transaccion.NumeroEntrada;
                    rmTransaccion.Comentario = transaccion.Comentario;
                    rmTransaccion.TipoOperacion = transaccion.TipoOperacion;
                    rmTransaccion.Representante = transaccion.Representante;
                    rmTransaccion.IdAutorizante = autorizantes?.IdAutorizante;
                    rmTransaccion.EstadoTransaccion = rmMesaEntrada.EstadoEntrada.ToString();
                    rmTransaccion.NroBoleta = NroBolmarca.ToString();
                    rmTransaccion.NombreRepresentante = transaccion.NombreRepresentante;
                    //rmTransaccion.IdUsuario = User.Identity.Name;
                    rmTransaccion.FechaAlta = DateTime.Now;

                    _dbContext.Update(rmTransaccion);
                    await _dbContext.SaveChangesAsync();
                }
                return RedirectToAction("Index");

            }

            return View();
        }

        public IActionResult GenerarPdf(InformeCustom informe)
        {
            var mesaSalida = GetTituloData(informe);
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

            var transacion = _dbContext.RmTransacciones.OrderByDescending(o => o.FechaAlta).FirstOrDefault(t => t.NumeroEntrada == informe.NumeroEntrada);
            var estadoObservado = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Observado/Registrador");
            var estadoNotaNegativa = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Nota Negativa/Registrador");
            var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m => m.NumeroEntrada == informe.NumeroEntrada);
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

                    if (mesaEntrada.EstadoEntrada == estadoObservado.CodigoEstado)
                    {
                        mesaSalida.Comentario = transacion.Observacion;
                        viewHtml = RenderViewToString("ObservadoPDF", mesaSalida);
                    }
                    else if (mesaEntrada.EstadoEntrada == estadoNotaNegativa.CodigoEstado)
                    {
                        mesaSalida.Comentario = transacion.Observacion;
                        viewHtml = RenderViewToString("NotaNegativaPDF", mesaSalida);
                    }
                    else
                    {
                        if (mesaSalida.IdAutorizante == 0 && mesaSalida.NombreAutorizante == "")
                        {
                            // Renderizar la vista Razor a una cadena HTML
                            viewHtml = RenderViewToString("TicketPDF2", mesaSalida);
                        }
                        else
                        {
                            // Renderizar la vista Razor a una cadena HTML
                            viewHtml = RenderViewToString("TicketPDF", mesaSalida);
                        }
                    }
                }
            }
            else
            {
                if (mesaEntrada.EstadoEntrada == estadoObservado.CodigoEstado)
                {
                    mesaSalida.Comentario = transacion.Observacion;
                    viewHtml = RenderViewToString("ObservadoPDF", mesaSalida);
                }
                else if (mesaEntrada.EstadoEntrada == estadoNotaNegativa.CodigoEstado)
                {
                    mesaSalida.Comentario = transacion.Observacion;
                    viewHtml = RenderViewToString("NotaNegativaPDF", mesaSalida);
                }
                else
                {
                    if (mesaSalida.IdAutorizante == 0 && mesaSalida.NombreAutorizante == "")
                    {
                        // Renderizar la vista Razor a una cadena HTML
                        viewHtml = RenderViewToString("TicketPDF2", mesaSalida);
                    }
                    else
                    {
                        // Renderizar la vista Razor a una cadena HTML
                        viewHtml = RenderViewToString("TicketPDF", mesaSalida);
                    }
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

        private DatosTituloCustom GetTituloData(InformeCustom informe)
        {
            var mesaEntrada = _dbContext.RmMesaEntrada.SingleOrDefault(p => p.NumeroEntrada == informe.NumeroEntrada);
            var transaccion = _dbContext.RmTransacciones.FirstOrDefault(p => p.NumeroEntrada == informe.NumeroEntrada);
            var dataEstablecimiento = _dbContext.RmMarcasXEstabs.FirstOrDefault(p => p.IdMarca == mesaEntrada.NumeroEntrada);
            var boletaSenal = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NumeroEntrada == mesaEntrada.NumeroEntrada);
            var autorizante = _dbContext.RmAutorizantes.FirstOrDefault(a => a.IdAutorizante == transaccion.IdAutorizante);

            //var result = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NumeroEntrada.Equals(nEntrada));
            //string imagenMarcaPath = result?.MarcaNombre;
            //string imagenSenhalPath = result?.SenalNombre;

            //string imagenMarcaDataUri = ConvertImageToDataUri(imagenMarcaPath);
            //string imagenSenhalDataUri = ConvertImageToDataUri(imagenSenhalPath);

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
                //ImagenMarca = imagenMarcaDataUri,
                //ImagenSenhal = imagenSenhalDataUri,
                NroBoleta = mesaEntrada?.NroBoleta ?? "",
                NomTitular = mesaEntrada?.NomTitular ?? "",
                NroDocumentoTitular = mesaEntrada?.NroDocumentoTitular ?? "",
                Distritos = GetDistritos(),
                DistritoEstable = dataEstablecimiento?.CodDistrito ?? "",
                MatriculaAutorizante = informe?.MatriculaAutorizante ?? 0,
                NombreAutorizante = informe?.NombreAutorizante ?? "",
                Operaciones = GetOperaciones(),
                TipoOperacion = transaccion?.TipoOperacion ?? "",
                MatriculaRegistro = GetMatriculaRegistro(),
                Autorizantes = GetAutorizante(),
                IdAutorizante = mesaEntrada?.IdAutorizante ?? 0,
                FechaActoJuridico = DateOnly.FromDateTime(transaccion?.FechaActoJuridico ?? DateTime.MinValue),
                NroBoletaSenal = boletaSenal?.NroBoleta ?? "",
                Oficinas = GetOficinasRegistrales(),
                CodigoOficina = mesaEntrada?.CodigoOficina ?? 0,
                Comentario = informe?.Comentario ?? ""
            };
            if (mesaEntrada.Reingreso == "S")
            {
                tituloData.FechaReingreso = mesaEntrada.FechaEntrada;
                tituloData.FechaEntrada = mesaEntrada.FechaAntReingreso;
            }
            else
            {
                tituloData.FechaEntrada = mesaEntrada.FechaEntrada;
            }
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
