using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using static SistemaBase.ModelsCustom.TransaccionCustom;
using static SistemaBase.ModelsCustom.DatosTituloCustom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Html2pdf;
using iText.Commons.Actions.Contexts;
using ZXing;
using ZXing.Windows.Compatibility;
using ZXing.Common;
using System.Drawing;
using System.Drawing.Imaging;
using static SistemaBase.ModelsCustom.CertificadoCustom;
using Microsoft.AspNetCore.Authorization;

namespace SistemaBase.Controllers
{
    [Authorize]

    public class TransaccionCustomController : Controller
    {
        private readonly DbvinDbContext _dbContext;

        public TransaccionCustomController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMTRAOPE", "Index", "TransaccionCustom" })]

        public IActionResult Index()
        {
            try
            {
                ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion","3");
                //ViewBag.Autorizante = new SelectList(_dbContext.RmAutorizantes, "IdAutorizante", "DescripAutorizante");
                

                return View();


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
                var mesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                var estadoRegistrador = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Registrador");

                if (mesaEntrada == null)
                {
                    return Json(new { Success = false, ErrorMessage = "El número de entrada no existe." });
                }
                var tipoSolicitud = mesaEntrada?.TipoSolicitud ?? 0;
                var listaConceptos = new List<decimal> { 2, 25, 18, 8, 7, 3 };
                if (!listaConceptos.Contains(tipoSolicitud))
                {
                    return Json(new { Success = false, ErrorMessage = "Error, el tipo de solicitud no corresponde" });
                }
                if (mesaEntrada != null)
                {
                    var transaccione = await _dbContext.RmTransacciones.OrderByDescending(o=>o.FechaAlta).FirstOrDefaultAsync(t=>t.NumeroEntrada== nEntrada);
                    if (mesaEntrada.EstadoEntrada == estadoRegistrador.CodigoEstado)
                    {
                        transaccione = null;
                    }
                    if (transaccione != null)
                    {
                        var estadoRetiradoObs = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Retirado/Observado");
                        var estadoRetiradoNN = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Retirado/Nota negativa");
                        var estadoRetiradoAprob = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Retirado/Aprobado");
                        var rmReingreso = _dbContext.RmReingresos.FirstOrDefault(p => p.NroEntrada == nEntrada);
                        var estadoRevision = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "En Revisión");
                        ////aqui
                        var administrador = _dbContext.Usuarios.FirstOrDefault(u => u.CodUsuario == User.Identity.Name);
                        var codGrupo = administrador?.CodGrupo ?? "";
                        if (codGrupo != "ADMIN")
                        {
                            if (mesaEntrada.Reingreso != "S" && estadoRevision.CodigoEstado != mesaEntrada.EstadoEntrada)
                            {
                                return Json(new { Success = false, ErrorMessage = "Error, el trabajo ya se encuentra resgistrado" });
                            }
                            if ((mesaEntrada.Reingreso == "S" && estadoRetiradoAprob.CodigoEstado.ToString() != transaccione.EstadoTransaccion) &&
                                (mesaEntrada.Reingreso == "S" && estadoRetiradoObs.CodigoEstado.ToString() != transaccione.EstadoTransaccion) &&
                                (mesaEntrada.Reingreso == "S" && estadoRetiradoNN.CodigoEstado.ToString() != transaccione.EstadoTransaccion))
                            {
                                return Json(new { Success = false, ErrorMessage = "Error, el trabajo ya se encuentra resgistrado" });
                            }
                        }

                        var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(p => p.IdAutorizante == transaccione.IdAutorizante);
                        var nroBoleta = mesaEntrada?.NroBoleta ?? "0";
                        var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.Descripcion == nroBoleta);
                        var nroBolImagen = boletaMarca?.NroBoleta??0;
                        var transBoleta = _dbContext.RmMarcasSenales.FirstOrDefault(t=>t.NroBoleta== nroBolImagen.ToString());
                        var nroEntradaImg = transBoleta?.NumeroEntrada??0;
                        //var rmMarcasSenale = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(p => p.NumeroEntrada == TransaccionEntrada.NumeroEntrada);
                        var transaccionEntrada = _dbContext.RmTransacciones.OrderByDescending(o=>o.FechaAlta).FirstOrDefault(t => t.NumeroEntrada == nroEntradaImg);
                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", transaccione.TipoOperacion);
                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada?.EstadoEntrada ?? 0);
                        var titularAdjudicado = _dbContext.RmTitularesMarcas.FirstOrDefault(t=>t.IdTransaccion== transaccione.IdTransaccion);
                        var titularAd = titularAdjudicado?.IdPropietario??"0";
                        var NomTitu = _dbContext.Personas.FirstOrDefault(t=>t.CodPersona== titularAd);

                        var IdEscribano = transaccione?.IdEscribanoJuez;
                        var nomEscribano = _dbContext.Personas.FirstOrDefault(t=>t.CodPersona== IdEscribano);
                        TransaccionCustom inscripcion = new()
                        {
                            NumeroEntrada = nEntrada,
                            //NroEntradaTrans = transaccionEntrada?.IdTransaccion,
                            IdTransaccionPropietario = transaccione.IdTransaccion,
                            IdPropietario = titularAdjudicado?.IdPropietario??"",
                            FechaRegistro = titularAdjudicado?.FechaRegistro,
                            DescTitular = NomTitu?.Nombre??"",
                            EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0,
                            NroBoleta = mesaEntrada?.NroBoleta??"",
                            IdTransaccion = transaccionEntrada?.IdTransaccion,
                            NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                            Representante = transaccione?.Representante??"",
                            NombreRepresentante = transaccione?.NombreRepresentante??"",
                            FechaActoJuridico = transaccione?.FechaActoJuridico,
                            IdEscribanoJuez = transaccione?.IdEscribanoJuez,
                            NomEscribano = nomEscribano?.Nombre??"",
                            Comentario = transaccione?.Comentario
                        };

                        return View("Index", inscripcion);
                    }
                    else
                    {
                        //var estadoRegistrador = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Registrador");

                        if (estadoRegistrador.CodigoEstado != mesaEntrada.EstadoEntrada)
                        {
                            return Json(new { Success = false, ErrorMessage = "Error, debe recepcionar el trabajo" });
                        }
                        var nroBoleta = mesaEntrada?.NroBoleta ?? "0";
                        var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.Descripcion == nroBoleta);
                        var nroBolImagen = boletaMarca?.NroBoleta ?? 0;
                        var marcasSenale = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(p => p.NroBoleta == nroBolImagen.ToString());
                        var nroEntradaMarca = marcasSenale?.NumeroEntrada??0;
                        var transaccion = await _dbContext.RmTransacciones.OrderByDescending(o=>o.FechaAlta).FirstOrDefaultAsync(t=>t.NumeroEntrada == nroEntradaMarca);
                        if (transaccion != null)
                        {
                            var nomRepresentante = await _dbContext.Personas.FirstOrDefaultAsync(p => p.CodPersona == transaccion.Representante);
                            //var tipoSolicitud = await _dbContext.RmTipoSolicituds.FirstOrDefaultAsync(t=>t.TipoSolicitud == mesaEntrada.TipoSolicitud);
                            //var tipoOperacion = await _dbContext.RmTiposOperaciones.FirstOrDefaultAsync(o=>o.DescripTipoOperacion == tipoSolicitud.DescripSolicitud);
                            var rmMarcasSenale = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(p => p.NumeroEntrada == transaccion.NumeroEntrada);
                            //Que muestre como primer dato la descripcion CERTIFICADO
                            ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", "3");
                            ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada?.EstadoEntrada ?? 0);
                            var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);
                            
                            // cargar para edicion
                            TransaccionCustom inscripcion = new()
                            {
                                NumeroEntrada = nEntrada,
                                //NroEntradaTrans = transaccion.IdTransaccion,
                                EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0,
                                NroBoleta = mesaEntrada?.NroBoleta??"",
                                IdTransaccion = transaccion?.IdTransaccion??0,
                                //IdEscribanoJuez = transaccion?.IdEscribanoJuez??"",
                                //FechaActoJuridico = transaccion?.FechaActoJuridico,
                                //NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                                //MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                                //Serie = transaccion?.Serie ?? "",
                                //FormaConcurre = transaccion?.FormaConcurre ?? ""
                                //Representante = mesaEntrada?.NroDocumentoPresentador ?? "",
                                //NombreRepresentante = mesaEntrada?.NombrePresentador ?? ""
                            };
                            return View("Index", inscripcion);
                        }
                        else
                        {
                            // cargar nuevo
                            //var nroBoleta = mesaEntrada?.NroBoleta ?? "0";
                            //var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.Descripcion == nroBoleta);
                            TransaccionCustom inscripcion = new();
                            ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada?.EstadoEntrada ?? 0);
                            ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion");
                            inscripcion.NumeroEntrada = nEntrada;
                            inscripcion.EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0;
                            //inscripcion.NombreAutorizante = autorizante?.DescripAutorizante ?? "";
                            //inscripcion.MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0;
                            inscripcion.NroBoleta = mesaEntrada?.NroBoleta ?? "";


                            return View("Index", inscripcion);
                            //return View("Index");
                        }

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


        public async Task<IActionResult> AddTransferencia(decimal nEntrada)
        {
            try
            {
                var mesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

                if (mesaEntrada != null)
                {
                    var transaccion = await _dbContext.RmTransacciones.OrderByDescending(p => p.FechaAlta).FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

                    if (transaccion != null)
                    {
                        var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(p => p.IdAutorizante == transaccion.IdAutorizante);
                        var nroBoleta = mesaEntrada?.NroBoleta ?? "0";
                        var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.Descripcion == nroBoleta);
                        var nroBolImagen = boletaMarca?.NroBoleta ?? 0;
                        var transBoleta = _dbContext.RmMarcasSenales.FirstOrDefault(t => t.NroBoleta == nroBolImagen.ToString());
                        var nroEntradaImg = transBoleta?.NumeroEntrada ?? 0;
                        //var rmMarcasSenale = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(p => p.NumeroEntrada == TransaccionEntrada.NumeroEntrada);
                        var transaccionEntrada = _dbContext.RmTransacciones.OrderByDescending(o => o.FechaAlta).FirstOrDefault(t => t.NumeroEntrada == nroEntradaImg);
                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", transaccion.TipoOperacion);
                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada?.EstadoEntrada ?? 0);
                        var titularAdjudicado = _dbContext.RmTitularesMarcas.FirstOrDefault(t => t.IdTransaccion == transaccion.IdTransaccion);
                        var titularAd = titularAdjudicado?.IdPropietario ?? "0";
                        var NomTitu = _dbContext.Personas.FirstOrDefault(t => t.CodPersona == titularAd);

                        var IdEscribano = transaccion?.IdEscribanoJuez;
                        var nomEscribano = _dbContext.Personas.FirstOrDefault(t => t.CodPersona == IdEscribano);
                        TransaccionCustom inscripcion = new()
                        {
                            NumeroEntrada = nEntrada,
                            //NroEntradaTrans = transaccionEntrada?.IdTransaccion,
                            IdTransaccionPropietario = transaccion.IdTransaccion,
                            IdPropietario = titularAdjudicado?.IdPropietario ?? "",
                            FechaRegistro = titularAdjudicado?.FechaRegistro,
                            DescTitular = NomTitu?.Nombre ?? "",
                            EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0,
                            NroBoleta = mesaEntrada?.NroBoleta ?? "",
                            IdTransaccion = transaccionEntrada?.IdTransaccion,
                            NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                            Representante = transaccion?.Representante ?? "",
                            NombreRepresentante = transaccion?.NombreRepresentante ?? "",
                            FechaActoJuridico = transaccion?.FechaActoJuridico,
                            IdEscribanoJuez = transaccion?.IdEscribanoJuez,
                            NomEscribano = nomEscribano?.Nombre ?? "",
                            Comentario = transaccion?.Comentario
                        };

                        return View("Index", inscripcion);
                    }
                    else
                    {
                        // cargar nuevo

                        TransaccionCustom inscripcion = new();
                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada?.EstadoEntrada ?? 0);
                        inscripcion.NumeroEntrada = nEntrada;
                        inscripcion.EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0;


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
        public IActionResult GetImagenMarca(string nroEntrada)
        {
            if (nroEntrada != null)
            {
                decimal nEntrada = Decimal.Parse(nroEntrada);
                var result = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NumeroEntrada.Equals(nEntrada));

                if (result != null)
                {
                    try
                    {
                        string imagePath = result?.MarcaNombre ?? ""; // Ruta a la imagen en tu proyecto
                        string imageDataUri;
                        byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                        imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);


                        string imagePath2 = result?.SenalNombre ?? ""; // Ruta a la imagen en tu proyecto
                        string imageDataUri2;
                        byte[] imageBytes2 = System.IO.File.ReadAllBytes(imagePath2);
                        imageDataUri2 = "data:image/png;base64," + Convert.ToBase64String(imageBytes2);

                        return Json(new { Success = true, srcmarca = imageDataUri, srcsenhal = imageDataUri2 });
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
        public ActionResult AddTitular()
        {
            return View("AddTitular");
            //return View("AddTitular", new PersonaTitular());

        }

        public async Task<JsonResult> CargarHistorico(int idTransaccion)
        {

            IQueryable<RmTitularesMarca> queryTitularMarca = _dbContext.RmTitularesMarcas.AsQueryable();
            IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();

            //Para obtener el nombre del usuario
            var queryTitularPersona = queryTitularMarca.Where(t => t.IdTransaccion == idTransaccion).AsQueryable().Join(queryPersona,
                  propietario => propietario.IdPropietario,
                  people => people.CodPersona,
                  (propietario, people) =>
                      new HistoricoPropietarios { IdPropietario = propietario.IdPropietario, Nombre = people.Nombre, FechaRegistro= propietario.FechaRegistro, IdTransaccion = propietario.IdTransaccion});
          
            var titularesMarcas = await queryTitularPersona.ToArrayAsync();

            return Json(titularesMarcas);
        }

        public async Task<JsonResult> CargarTitulares(int idTransaccion)
        {

            IQueryable<RmTitularesMarca> queryTitularMarca = _dbContext.RmTitularesMarcas.AsQueryable();
            IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();

            var primerTitu = _dbContext.RmTitularesMarcas.FirstOrDefault(t=>t.IdTransaccion== idTransaccion);
            var codPrimer = primerTitu?.IdPropietario??"0";
            //Para obtener el nombre del usuario
            var queryTitularPersona = queryTitularMarca.Where(t => t.IdTransaccion == idTransaccion && t.IdPropietario!= codPrimer).AsQueryable().Join(queryPersona,
                  propietario => propietario.IdPropietario,
                  people => people.CodPersona,
                  (propietario, people) =>
                      new TitularesMarcas { IdPropietario = propietario.IdPropietario, Nombre = people.Nombre});
           
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
                                RmNotasNegativa notaNegativa = new()
                                {
                                    IdEntrada = id,
                                    FechaAlta = DateTime.Now,
                                    DescripNotaNegativa = comentario,
                                    IdUsuario = User.Identity.Name
                                };
                                await _dbContext.AddAsync(notaNegativa);

                            }
                            var rmTitularesMarca = _dbContext.RmTitularesMarcas.FirstOrDefault(t => t.IdTransaccion == transaccion.IdTransaccion);
                            if (rmTitularesMarca != null)
                            {
                                // Obtén todos los titulares de la transacción actual
                                var titularesExistentes = await _dbContext.RmTitularesMarcas
                                    .Where(p => p.IdTransaccion == transaccion.IdTransaccion)
                                    .ToListAsync();
                                // Elimina todos los titulares existentes
                                _dbContext.RemoveRange(titularesExistentes);

                                // Guarda los cambios en la base de datos para aplicar las eliminaciones
                                await _dbContext.SaveChangesAsync();
                            }
                            var rmMarcasxEstable = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(t => t.IdTransaccion == transaccion.IdTransaccion);
                            if (rmMarcasxEstable != null)
                            {
                                _dbContext.Remove(rmMarcasxEstable);
                                await _dbContext.SaveChangesAsync();
                            }

                            //transaccion.NumeroEntrada = inscripcion.NroEntrada;se mantiene
                            transaccion.Asiento = null;
                            transaccion.NroBoleta = null;
                            transaccion.Comentario = null;
                            //transaccion.TipoOperacion = ;se mantiene
                            transaccion.Representante = null;
                            transaccion.FormaConcurre = null;
                            transaccion.FechaActoJuridico = null;
                            transaccion.IdAutorizante = null;
                            //transaccion.EstadoTransaccion = rmMesaEntrada.EstadoEntrada.ToString();se mantiene
                            transaccion.Serie = null;
                            transaccion.FirmanteRuego = null;
                            transaccion.NombreRepresentante = null;
                            transaccion.CodEstadoCivilTitular = null;
                            transaccion.DireccionTitular = null;
                            transaccion.CodProfesionApoderado = null;
                            transaccion.DireccionApoderado = null;

                            if (codGrupo != "ADMIN")
                            {
                                transaccion.EstadoTransaccion = estadoEntrada.CodigoEstado.ToString();
                            }
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
                        RmTransaccione bdTransaccion = new()
                        {
                            NumeroEntrada = id,
                            FechaAlta = DateTime.Now,
                            Observacion = comentario,
                            EstadoTransaccion = estadoEntrada.CodigoEstado.ToString(),
                            TipoOperacion = tipoOperacion,
                            IdUsuario = User.Identity.Name
                    };
                        await _dbContext.AddAsync(bdTransaccion);

                       
                    }
                    if (codGrupo != "ADMIN")
                    {
                        mesaEntradum.EstadoEntrada = estadoEntrada.CodigoEstado;
                        _dbContext.Update(mesaEntradum);
                   
                   

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

        public async Task<IActionResult> Create([FromBody] TransaccionCustom inscripcion)
        {
            try
            {
                var rmMesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(t => t.NumeroEntrada == inscripcion.NumeroEntrada);
                var rmTransaccion = await _dbContext.RmTransacciones.OrderByDescending(t=>t.FechaAlta).FirstOrDefaultAsync(t => t.NumeroEntrada == inscripcion.NumeroEntrada);
                //var rmMarcasxEstable = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(t => t.IdTransaccion == rmTransaccion.IdTransaccion);
                //var rmTitulares = await _dbContext.RmTitularesMarcas.FirstOrDefaultAsync(t => t.IdTransaccion == inscripcion.IdTransaccion);
                var boletaMarca = await _dbContext.RmBoletasMarcas.FirstOrDefaultAsync(b=>b.Descripcion== inscripcion.NroBoleta);
                var nroBoleta = boletaMarca?.NroBoleta??0;
                var marcasSenal = _dbContext.RmMarcasSenales.FirstOrDefault(m=>m.NroBoleta== nroBoleta.ToString());
                var idAuto = inscripcion?.NombreAutorizante ?? "";
                var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(a => a.DescripAutorizante == idAuto);
                if (autorizante == null && idAuto != "")
                {
                    RmAutorizante rmAutorizante = new()
                    {
                        DescripAutorizante = idAuto
                    };
                    _dbContext.Add(rmAutorizante);
                    await _dbContext.SaveChangesAsync();
                    autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(a => a.DescripAutorizante == idAuto);
                }
                if (rmTransaccion != null)
                {
                    if (rmMesaEntrada != null)
                    {
                        
                        rmTransaccion.IdMarca = marcasSenal?.IdMarca??0;
                        rmTransaccion.NumeroEntrada = inscripcion.NumeroEntrada;
                        rmTransaccion.Comentario = inscripcion.Comentario;
                        rmTransaccion.TipoOperacion = inscripcion.TipoOperacion;
                        rmTransaccion.NroBoleta = boletaMarca?.NroBoleta.ToString();
                        rmTransaccion.IdEscribanoJuez = inscripcion.IdEscribanoJuez;
                        rmTransaccion.Representante = inscripcion.Representante;
                        rmTransaccion.FormaConcurre = inscripcion.FormaConcurre;
                        rmTransaccion.FechaActoJuridico = inscripcion.FechaActoJuridico;
                        rmTransaccion.IdAutorizante = autorizante?.IdAutorizante;
                        var administrador = _dbContext.Usuarios.FirstOrDefault(u => u.CodUsuario == User.Identity.Name);
                        var codGrupo = administrador?.CodGrupo ?? "";
                        if (codGrupo != "ADMIN")
                        {
                            rmTransaccion.EstadoTransaccion = rmMesaEntrada.EstadoEntrada.ToString();
                        }
                        
                        rmTransaccion.Serie = inscripcion.Serie;
                        rmTransaccion.NombreRepresentante = inscripcion.NombreRepresentante;
                        //rmTransaccion.IdUsuario = User.Identity.Name;
                        //rmTransaccion.FechaAlta = DateTime.Now;

                        if (inscripcion.IdPropietario != null)
                        {
                            var persona = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == inscripcion.IdPropietario);
                            rmTransaccion.CodEstadoCivilTitular = persona?.CodEstadoCivil;
                            rmTransaccion.CodProfesionTitular = persona?.Profesion;
                            var direccion = _dbContext.DirecPersonas.FirstOrDefault(d => d.CodPersona == inscripcion.IdPropietario);
                            rmTransaccion.DireccionTitular = direccion?.Detalle;
                        }
                        if (inscripcion.Representante != null)
                        {
                            var persona = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == inscripcion.Representante);
                            rmTransaccion.CodEstadoCivilApoderado = persona?.CodEstadoCivil;
                            rmTransaccion.CodProfesionApoderado = persona?.Profesion;
                            var direccion = _dbContext.DirecPersonas.FirstOrDefault(d => d.CodPersona == inscripcion.Representante);
                            rmTransaccion.DireccionApoderado = direccion?.Detalle;
                        }

                        _dbContext.Update(rmTransaccion);

                        var rmTitularesMarca = _dbContext.RmTitularesMarcas.FirstOrDefault(t => t.IdTransaccion == rmTransaccion.IdTransaccion);
                        if (rmTitularesMarca != null)
                        {
                            // Obtén todos los titulares de la transacción actual
                            var titularesExistentes = await _dbContext.RmTitularesMarcas
                                .Where(p => p.IdTransaccion == rmTransaccion.IdTransaccion)
                                .ToListAsync();
                            // Elimina todos los titulares existentes
                            _dbContext.RemoveRange(titularesExistentes);

                            // Guarda los cambios en la base de datos para aplicar las eliminaciones
                            await _dbContext.SaveChangesAsync();
                        }

                        if (inscripcion.IdPropietario != null)
                        {
                             rmTitularesMarca = await _dbContext.RmTitularesMarcas.Where(p => p.IdTransaccion == rmTransaccion.IdTransaccion && p.IdPropietario == inscripcion.IdPropietario).FirstOrDefaultAsync();
                            if (rmTitularesMarca == null)
                            {
                                RmTitularesMarca titulares = new()
                                {
                                    IdTransaccion = rmTransaccion.IdTransaccion,
                                    IdPropietario = inscripcion.IdPropietario,
                                    IdTitular = inscripcion.IdPropietario,
                                    FechaRegistro = rmTransaccion.FechaAlta,
                                    CodUsuario = rmTransaccion.IdUsuario
                                };
                                await _dbContext.AddAsync(titulares);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                        

                        //List<RmTitularesMarca> titularesMarca = new();
                        foreach (var item in inscripcion.Titulares)
                        {
                            var rmTitularesMarcas = await _dbContext.RmTitularesMarcas.Where(p => p.IdTransaccion == rmTransaccion.IdTransaccion).FirstOrDefaultAsync(p => p.IdPropietario == item.IdPropietario);
                            if (rmTitularesMarcas == null)
                            {
                                RmTitularesMarca titulares = new()
                                {
                                    IdTransaccion = rmTransaccion.IdTransaccion,
                                    IdPropietario = item.IdPropietario,
                                    IdTitular = item.IdPropietario,
                                    FechaRegistro = rmTransaccion.FechaAlta,
                                    CodUsuario = rmTransaccion.IdUsuario
                                };
                                await _dbContext.AddAsync(titulares);
                                await _dbContext.SaveChangesAsync();
                            }
                        }

                        

                        await _dbContext.SaveChangesAsync();
                    }


                }
            }
            catch
            {
                // Manejo de errores
                return View("Error");
            }
            return Ok();
        }

        public async Task<IActionResult> DetailsTransaccion([FromBody] TransaccionCustom inscripcion)
        {
            try
            {
                var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m => m.NumeroEntrada == inscripcion.NumeroEntrada);

                var NroBoletaEntda = mesaEntrada?.NroBoleta ?? "";
                var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.Descripcion == NroBoletaEntda);
                var nroBolImg = boletaMarca?.NroBoleta ?? 0;
                var result = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NroBoleta == nroBolImg.ToString());
                var EntradaResul = result?.NumeroEntrada ?? 0;
                var dataRepresentante = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == inscripcion.Representante);
                var transaccionAnt = _dbContext.RmTransacciones.OrderByDescending(o => o.FechaAlta).Where(t => t.NumeroEntrada == EntradaResul).FirstOrDefault();
               // var transaccion = _dbContext.RmTransacciones.OrderByDescending(o => o.FechaAlta).Where(t => t.NumeroEntrada == inscripcion.NumeroEntrada).FirstOrDefault();

              //  var titularMarca = _dbContext.RmTitularesMarcas.FirstOrDefault(t => t.IdTransaccion == transaccion.IdTransaccion);
                var codTitular = inscripcion?.IdPropietario ?? "";
                var dataTitular = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == codTitular);
                var dataEstablecimiento = _dbContext.RmMarcasXEstabs.FirstOrDefault(p => p.IdTransaccion == transaccionAnt.IdTransaccion);
                var ciudadTitu = _dbContext.DirecPersonas.FirstOrDefault(p => p.CodPersona == codTitular);
                var ciudadRep = _dbContext.DirecPersonas.FirstOrDefault(p => p.CodPersona == inscripcion.Representante);
                var fecReingreso = _dbContext.RmReingresos.FirstOrDefault(p => p.NroEntrada == inscripcion.NumeroEntrada);

                //var autorizante = _dbContext.RmAutorizantes.FirstOrDefault(p => p.MatriculaRegistro == inscripcion.MatriculaAutorizante);
                string imagenMarcaPath = result?.MarcaNombre ?? "";
                string imagenSenhalPath = result?.SenalNombre ?? "";

                var nroBoleta = inscripcion?.NroBoleta ?? "";
                string imagenMarcaDataUri = ConvertImageToDataUri(imagenMarcaPath);
                string imagenSenhalDataUri = ConvertImageToDataUri(imagenSenhalPath);
                var persona = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == codTitular);
                DatosTituloCustom tituloData = new()
                {
                    NumeroEntrada = inscripcion.NumeroEntrada,
                    //FechaEntrada = mesaEntrada?.FechaEntrada,
                    FechaActual = GetFechaActual(),
                    ImagenMarca = imagenMarcaDataUri,
                    ImagenSenhal = imagenSenhalDataUri,
                    NroBoleta = inscripcion.NroBoleta,
                    Barcode = GetCodigoBarra(nroBoleta),
                    Oficinas = GetOficinasRegistrales(),
                    CodigoOficina = mesaEntrada?.CodigoOficina ?? 0,
                    NomTitular = persona?.Nombre ?? "",
                    NroDocumentoTitular = codTitular,
                    DireccionTitular = ciudadTitu?.Detalle ?? "",
                    CiudadesTitu = GetCiudades(),
                    CiudadTitular = ciudadTitu?.CodCiudad ?? "",
                    Nacionalidades = GetNacionalidad(),
                    CodPais = dataTitular?.CodPais ?? "",
                    Profesiones = GetProfesion(),
                    CodProfesion = dataTitular?.Profesion ?? "",
                    FecNacimiento = dataTitular?.FecNacimiento ?? DateTime.MinValue,
                    Edad = CalcularEdad(dataTitular?.FecNacimiento ?? DateTime.MinValue), // Calcula la edad aquí y usa DateTime.MinValue como valor predeterminado si FecNacimiento es nulo
                    EstadoCivil = GetEstadoCivil(),
                    CodEstadoCivil = dataTitular?.CodEstadoCivil ?? "",
                    Distritos = GetDistritos(),
                    DistritoEstable = dataEstablecimiento?.CodDistrito ?? "",
                    DepartamentoEstable = dataEstablecimiento?.Descripcion ?? "",
                    NombreRepresentante = inscripcion?.NombreRepresentante ?? "",
                    Representante = inscripcion?.Representante ?? "",
                    NacionalidadesRep = GetNacionalidad(),
                    CodPaisRep = dataRepresentante?.CodPais ?? "",
                    EstadoCivilRep = GetEstadoCivil(),
                    CodEstadoCivilRep = dataRepresentante?.CodEstadoCivil ?? "",
                    ProfesionesRep = GetProfesion(),
                    CodProfesionRep = dataRepresentante?.Profesion ?? "",
                    DirecRep = ciudadRep?.Detalle ?? "",
                    CiudadesRep = GetCiudades(),
                    CiudadRep = ciudadRep?.CodCiudad ?? "",
                    CodCiudadAuto = GetCodCiudad(),
                    DescripCiudadAuto = GetCiudades(),
                    NombreAutorizante = inscripcion?.NombreAutorizante ?? "",
                    MatriculaAutorizante = inscripcion?.MatriculaAutorizante ?? 0,
                    FechaActoJuridico = DateOnly.FromDateTime(inscripcion?.FechaActoJuridico ?? DateTime.MinValue),
                    //FechaReingreso = fecReingreso?.FechaReingreso,
                    Operaciones = GetOperaciones(),
                    TipoOperacion = inscripcion?.TipoOperacion ?? ""
                };
                tituloData.Comentario = inscripcion?.Comentario;
                if (mesaEntrada.Reingreso == "S")
                {
                    tituloData.FechaReingreso = mesaEntrada.FechaEntrada;
                    tituloData.FechaEntrada = mesaEntrada.FechaAntReingreso;
                }
                else
                {
                    tituloData.FechaEntrada = mesaEntrada.FechaEntrada;
                }
                //var nroBoleta = inscripcion?.NroBoleta ?? "";
                if (nroBoleta != "")
                {

                    tituloData.Barcode = GetCodigoBarra(nroBoleta);
                }
                else
                {
                    tituloData.Barcode = "";
                }

                List<TitularesCarga> titularesConvertidos = new List<TitularesCarga>();

                foreach (var titular in inscripcion.Titulares)
                {
                    // Aquí conviertes cada objeto TitularMarcaViewModel a un objeto TitularesCarga
                    TitularesCarga titularCarga = new TitularesCarga
                    {
                        IdPropietario = titular.IdPropietario,
                        Nombre = titular.Nombre // Ajusta esto según las propiedades de la clase TitularesCarga
                                                // Agrega otras propiedades si es necesario
                    };

                    // Agrega el objeto convertido a la nueva lista
                    titularesConvertidos.Add(titularCarga);
                }

                // Asigna la nueva lista convertida a tituloData.Titulares
                tituloData.Titulares = titularesConvertidos;

                // Verifica si la lista es nula y crea una nueva si es necesario (esto puede no ser necesario dependiendo de la lógica de tu aplicación)
                if (tituloData.Titulares == null)
                {
                    tituloData.Titulares = new List<TitularesCarga>();
                }



                return View("DetailsTransaccion", tituloData);
            }
            catch (Exception ex)
            {

                return View("Error");

            }
            return Ok();
        }


        [HttpPost]
        public IActionResult GenerarPdf(TransaccionCustom inscripcion)
        {
            var mesaSalida = GetTituloData(inscripcion);
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
            var transacion = _dbContext.RmTransacciones.OrderByDescending(o => o.FechaAlta).FirstOrDefault(t => t.NumeroEntrada == inscripcion.NumeroEntrada);
            var estadoObservado = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Observado/Registrador");
            var estadoNotaNegativa = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Nota Negativa/Registrador");
            var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m => m.NumeroEntrada == inscripcion.NumeroEntrada);
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
                        // Renderizar la vista Razor a una cadena HTML
                        viewHtml = RenderViewToString("TicketPDF", mesaSalida);
                    }
                }

            }
            else {
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

        [HttpPost]
        private DatosTituloCustom GetTituloData(TransaccionCustom inscripcion)
        {

            var estadoObservado = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Observado/Registrador");
            var estadoNotaNegativa = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Nota Negativa/Registrador");
            var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m => m.NumeroEntrada == inscripcion.NumeroEntrada);
            if (mesaEntrada.EstadoEntrada == estadoObservado.CodigoEstado)
            {

                DatosTituloCustom tituloData = new()
                {
                    NumeroEntrada = inscripcion.NumeroEntrada,
                    //FechaEntrada = mesaEntrada.FechaEntrada,
                    FechaActual = GetFechaActual(),
                    Oficinas = GetOficinasRegistrales(),
                    CodigoOficina = mesaEntrada?.CodigoOficina ?? 0,
                    Operaciones = GetOperaciones(),
                    TipoOperacion = inscripcion?.TipoOperacion ?? ""
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


            else if (mesaEntrada.EstadoEntrada == estadoNotaNegativa.CodigoEstado)
            {

                DatosTituloCustom tituloData = new()
                {
                    NumeroEntrada = inscripcion.NumeroEntrada,
                    //FechaEntrada = mesaEntrada.FechaEntrada,
                    FechaActual = GetFechaActual(),
                    Oficinas = GetOficinasRegistrales(),
                    CodigoOficina = mesaEntrada?.CodigoOficina ?? 0,
                    Operaciones = GetOperaciones(),
                    TipoOperacion = inscripcion?.TipoOperacion ?? ""
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
            else
            {
                //var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(p => p.NumeroEntrada == inscripcion.NumeroEntrada);

                var NroBoletaEntda = mesaEntrada?.NroBoleta ?? "";
                var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.Descripcion == NroBoletaEntda);
                var nroBolImg = boletaMarca?.NroBoleta ?? 0;
                var result = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NroBoleta == nroBolImg.ToString());
                var EntradaResul = result?.NumeroEntrada ?? 0;
                var dataRepresentante = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == inscripcion.Representante);
                var transaccionAnt = _dbContext.RmTransacciones.OrderByDescending(o=>o.FechaAlta).Where(t => t.NumeroEntrada == EntradaResul).FirstOrDefault();
                var transaccion = _dbContext.RmTransacciones.OrderByDescending(o=>o.FechaAlta).Where(t => t.NumeroEntrada == inscripcion.NumeroEntrada).FirstOrDefault();
                
                var titularMarca = _dbContext.RmTitularesMarcas.FirstOrDefault(t => t.IdTransaccion == transaccion.IdTransaccion);
                var codTitular = titularMarca?.IdTitular ?? "";
                var dataTitular = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == codTitular);
                var dataEstablecimiento = _dbContext.RmMarcasXEstabs.FirstOrDefault(p => p.IdTransaccion == transaccionAnt.IdTransaccion);
                var ciudadTitu = _dbContext.DirecPersonas.FirstOrDefault(p => p.CodPersona == codTitular);
                var ciudadRep = _dbContext.DirecPersonas.FirstOrDefault(p => p.CodPersona == inscripcion.Representante);
                var fecReingreso = _dbContext.RmReingresos.FirstOrDefault(p => p.NroEntrada == inscripcion.NumeroEntrada);

                //var autorizante = _dbContext.RmAutorizantes.FirstOrDefault(p => p.MatriculaRegistro == inscripcion.MatriculaAutorizante);
                string imagenMarcaPath = result?.MarcaNombre ?? "";
                string imagenSenhalPath = result?.SenalNombre ?? "";
               
                var nroBoleta = inscripcion?.NroBoleta ?? "";
                string imagenMarcaDataUri = ConvertImageToDataUri(imagenMarcaPath);
                string imagenSenhalDataUri = ConvertImageToDataUri(imagenSenhalPath);
                var persona = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == codTitular);
                DatosTituloCustom tituloData = new()
                {
                    NumeroEntrada = inscripcion.NumeroEntrada,
                    //FechaEntrada = mesaEntrada?.FechaEntrada,
                    FechaActual = GetFechaActual(),
                    ImagenMarca = imagenMarcaDataUri,
                    ImagenSenhal = imagenSenhalDataUri,
                    NroBoleta = inscripcion.NroBoleta,
                    Barcode = GetCodigoBarra(nroBoleta),
                    Oficinas = GetOficinasRegistrales(),
                    CodigoOficina = mesaEntrada?.CodigoOficina ?? 0,
                    NomTitular = persona?.Nombre ?? "",
                    NroDocumentoTitular = codTitular,
                    DireccionTitular = transaccion?.DireccionTitular ?? "",
                    CiudadesTitu = GetCiudades(),
                    CiudadTitular = ciudadTitu?.CodCiudad ?? "",
                    Nacionalidades = GetNacionalidad(),
                    CodPais = dataTitular?.CodPais ?? "",
                    Profesiones = GetProfesion(),
                    CodProfesion = transaccion?.CodProfesionTitular ?? "",
                    FecNacimiento = dataTitular?.FecNacimiento ?? DateTime.MinValue,
                    Edad = CalcularEdad(dataTitular?.FecNacimiento ?? DateTime.MinValue), // Calcula la edad aquí y usa DateTime.MinValue como valor predeterminado si FecNacimiento es nulo
                    EstadoCivil = GetEstadoCivil(),
                    CodEstadoCivil = transaccion?.CodEstadoCivilTitular ?? "",
                    Distritos = GetDistritos(),
                    DistritoEstable = dataEstablecimiento?.CodDistrito ?? "",
                    DepartamentoEstable = dataEstablecimiento?.Descripcion ?? "",
                    NombreRepresentante = inscripcion?.NombreRepresentante ?? "",
                    Representante = inscripcion?.Representante ?? "",
                    NacionalidadesRep = GetNacionalidad(),
                    CodPaisRep = dataRepresentante?.CodPais ?? "",
                    EstadoCivilRep = GetEstadoCivil(),
                    CodEstadoCivilRep = transaccion?.CodEstadoCivilApoderado ?? "",
                    ProfesionesRep = GetProfesion(),
                    CodProfesionRep = transaccion?.CodProfesionApoderado ?? "",
                    DirecRep = transaccion?.DireccionApoderado ?? "",
                    CiudadesRep = GetCiudades(),
                    CiudadRep = ciudadRep?.CodCiudad ?? "",
                    CodCiudadAuto = GetCodCiudad(),
                    DescripCiudadAuto = GetCiudades(),
                    NombreAutorizante = inscripcion?.NombreAutorizante ?? "",
                    MatriculaAutorizante = inscripcion?.MatriculaAutorizante ?? 0,
                    FechaActoJuridico = DateOnly.FromDateTime(inscripcion?.FechaActoJuridico ?? DateTime.MinValue),
                    //FechaReingreso = fecReingreso?.FechaReingreso,
                    Operaciones = GetOperaciones(),
                    TipoOperacion = inscripcion?.TipoOperacion ?? ""
                };
                tituloData.Comentario = inscripcion?.Comentario;
                if (mesaEntrada.Reingreso == "S")
                {
                    tituloData.FechaReingreso = mesaEntrada.FechaEntrada;
                    tituloData.FechaEntrada = mesaEntrada.FechaAntReingreso;
                }
                else
                {
                    tituloData.FechaEntrada = mesaEntrada.FechaEntrada;
                }
                //var nroBoleta = inscripcion?.NroBoleta ?? "";
                if (nroBoleta != "")
                {

                    tituloData.Barcode = GetCodigoBarra(nroBoleta);
                }
                else
                {
                    tituloData.Barcode = "";
                }

                //Debe ser la nueva transaccion y no incluir al titular adjudicado
                var query = _dbContext.RmTitularesMarcas.Where(t => t.IdTransaccion == transaccion.IdTransaccion && t.IdTitular != inscripcion.IdPropietario).AsQueryable();

                var queryFinal = query.AsQueryable().Join(_dbContext.Personas,
                    titu => titu.IdTitular,
                    nom => nom.CodPersona,
                    (titu, nom) => new TitularesCarga { IdPropietario = titu.IdTitular, Nombre = nom.Nombre });
                List<TitularesCarga> titulares = null;
                titulares = queryFinal.ToList();

                tituloData.Titulares = titulares;
                if (tituloData.Titulares == null)
                {
                    tituloData.Titulares = new List<TitularesCarga>();
                }


                return tituloData;

            }

        }



        private string GetCodigoBarra(string nroBoleta)
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.CODE_128; // Puedes cambiar el formato según tus necesidades
            barcodeWriter.Options = new ZXing.Common.EncodingOptions
            {
                Width = 275,
                Height = 50,
                PureBarcode = true
            };

            var barcodeBitmap = barcodeWriter.Write(nroBoleta);

            using (var ms = new MemoryStream())
            {
                barcodeBitmap.Save(ms, ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return "data:image/png;base64," + base64String;
            }
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
                    Value = o.DescripAutorizante.ToString()
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

        private List<SelectListItem> GetNacionalidad()
        {
            using var dbContext = new DbvinDbContext();

            var tiposSolicitud = dbContext.Paises
                .Select(t => new SelectListItem
                {
                    Text = $"{t.Nacionalidad}",
                    Value = t.CodPais.ToString()
                })
                .ToList();

            return tiposSolicitud;
        }

        private List<SelectListItem> GetProfesion()
        {
            using var dbContext = new DbvinDbContext();
            var estadosEntrada = dbContext.Profesiones
            .Select(d => new SelectListItem
            {
                Text = $"{d.Descripcion}",
                Value = d.CodProfesion.ToString()
            })
            .ToList();

            return estadosEntrada;
        }

        private List<SelectListItem> GetEstadoCivil()
        {
            using var dbContext = new DbvinDbContext();
            var estadosEntrada = dbContext.EstadosCiviles
            .Select(d => new SelectListItem
            {
                Text = $"{d.Descripcion}",
                Value = d.CodEstadoCivil.ToString()
            })
            .ToList();

            return estadosEntrada;
        }


        // Este método calcula la edad a partir de la fecha de nacimiento
        private int CalcularEdad(DateTime fechaNacimiento)
        {
            DateTime fechaActual = DateTime.Now;
            int edad = fechaActual.Year - fechaNacimiento.Year;

            // Ajustar la edad si aún no ha tenido su cumpleaños este año
            if (fechaNacimiento > fechaActual.AddYears(-edad))
            {
                edad--;
            }

            return edad;
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

        public async Task<JsonResult> CargarTituImpresion(int idTransaccion)
        {

            IQueryable<RmTitularesMarca> queryTitularMarca = _dbContext.RmTitularesMarcas.AsQueryable();
            IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();

            //Para obtener el nombre del usuario
            var queryTitularPersona = queryTitularMarca.Where(t => t.IdTransaccion == idTransaccion).AsQueryable().Join(queryPersona,
                  propietario => propietario.IdTitular,
                  people => people.CodPersona,
                  (propietario, people) =>
                      new TitularesImpresion { IdPropietario = propietario.IdTitular, Nombre = people.Nombre });

            var titularesMarcas = await queryTitularPersona.ToArrayAsync();

            return Json(titularesMarcas);
        }
        public async Task<JsonResult> CargarDistrito(string nroBoleta)
        {
            IQueryable<RmDistrito> queryDistritos = _dbContext.RmDistritos.AsQueryable();
            IQueryable<RmMarcasXEstab> queryTransaccion = _dbContext.RmMarcasXEstabs.AsQueryable();
            IQueryable<RmTransaccione> queryMovimientos = _dbContext.RmTransacciones.AsQueryable();

            var transacion = _dbContext.RmTransacciones.OrderBy(p => p.FechaAlta).FirstOrDefault(t => t.NroBoleta == nroBoleta);

            var queryEstableDistrito = queryTransaccion
                 .Where(m => m.IdTransaccion == transacion.IdTransaccion)
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

        public async Task<JsonResult> CargarPropietario(string nroBoleta)
        {

            IQueryable<RmTitularesMarca> queryTitularMarca = _dbContext.RmTitularesMarcas.AsQueryable();
            IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();
            IQueryable<RmTiposPropiedad> queryTipoPropiedad = _dbContext.RmTiposPropiedads.AsQueryable();

            var transacion = _dbContext.RmTransacciones.OrderBy(p => p.FechaAlta).FirstOrDefault(t => t.NroBoleta == nroBoleta);

            //Para obtener el nombre del usuario
            var queryTitularPersona = queryTitularMarca.Where(t => t.IdTransaccion == transacion.IdTransaccion).AsQueryable().Join(queryPersona,
                  propietario => propietario.IdPropietario.ToString(),
                  people => people.CodPersona,
                  (propietario, people) =>
                      new PersonaDireccion { IdPropietario = propietario.IdPropietario.ToString(), Nombre = people.Nombre, TipoPropiedad = propietario.IdTipoPropiedad.ToString() });
            queryTitularPersona = queryTitularPersona.AsQueryable().Join(queryTipoPropiedad,
                persona => persona.TipoPropiedad,
                tipo => tipo.IdTipoPropiedad,
                (persona, tipo) =>
                    new PersonaDireccion { IdPropietario = persona.IdPropietario, Nombre = persona.Nombre, Descripcion = tipo.DescripcionTipoPropiedad, TipoPropiedad = persona.TipoPropiedad });

            var titularesMarcas = await queryTitularPersona.ToArrayAsync();

            return Json(titularesMarcas);
        }

    }
}
