using Microsoft.AspNetCore.Mvc;
using SistemaBase.Models;
using Microsoft.EntityFrameworkCore;
using SistemaBase.ModelsCustom;
using Microsoft.AspNetCore.Mvc.Rendering;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Html2pdf;
using iText.Commons.Actions.Contexts;
using Scryber.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Org.BouncyCastle.Crypto.Tls;
using static SistemaBase.ModelsCustom.TransaccionCustom;
using static SistemaBase.ModelsCustom.Reportes;
using static SistemaBase.ModelsCustom.InformeCustom;

namespace SistemaBase.Controllers
{
    public class OperacionSupervisorCustomController : Controller
    {

        private readonly DbvinDbContext _dbContext;
        public OperacionSupervisorCustomController(DbvinDbContext context)
        {
            _dbContext = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMJEFREG", "Index", "OperacionSupervisorCustom" })]
        public async Task<IActionResult> Index()
        {

            var queryFinal = (from me in _dbContext.RmMesaEntrada
                              join ts in _dbContext.RmTipoSolicituds on me.TipoSolicitud equals ts.TipoSolicitud
                              join t in _dbContext.RmTransacciones on me.NumeroEntrada equals t.NumeroEntrada
                              join e in _dbContext.RmEstadosEntrada on me.EstadoEntrada equals e.CodigoEstado
                              join u in _dbContext.Usuarios on t.IdUsuario equals u.CodUsuario
                              join p in _dbContext.Personas on u.CodPersona equals p.CodPersona
                              where (e.DescripEstado == "Aprobado/Registrador" || e.DescripEstado == "Observado/Registrador" || e.DescripEstado == "Nota Negativa/Registrador")
                              orderby t.FechaAlta descending
                              select new
                              {
                                  NroEntrada = Convert.ToInt32(me.NumeroEntrada),
                                  DescSolicitud = ts.DescripSolicitud,
                                  FechaAlta = t.FechaAlta,
                                  DescEstado = e.DescripEstado,
                                  Observacion = t.Observacion,
                                  NombreOperador = p.Nombre
                              }).AsEnumerable()
                             .GroupBy(result => result.NroEntrada)
                             .Select(group => group.OrderByDescending(g => g.FechaAlta).First())
                             .Select(result => new OperacionSupervisorCustom()
                             {
                                 NroEntrada = result.NroEntrada,
                                 DescSolicitud = result.DescSolicitud,
                                 FechaAlta = result.FechaAlta,
                                 DescEstado = result.DescEstado,
                                 Observacion =result.Observacion,
                                 NombreOperador = result.NombreOperador
                             });


            //var operacionSupervisor = (from me in _dbContext.RmMesaEntrada
            //                           join ts in _dbContext.RmTipoSolicituds on me.TipoSolicitud equals ts.TipoSolicitud
            //                           join t in _dbContext.RmTransacciones on me.NumeroEntrada equals t.NumeroEntrada
            //                           join e in _dbContext.RmEstadosEntrada on t.EstadoTransaccion equals e.CodigoEstado.ToString()
            //                           join u in _dbContext.Usuarios on t.IdUsuario equals u.CodUsuario
            //                           join p in _dbContext.Personas on u.CodPersona equals p.CodPersona
            //                           where (e.DescripEstado == "Aprobado/Registrador" || e.DescripEstado == "Observado/Registrador" || e.DescripEstado == "Nota Negativa/Registrador")
            //                           //(e.CodigoEstado == 1 || e.CodigoEstado == 2 || e.CodigoEstado == 3
            //                           //|| e.CodigoEstado == 4 || e.CodigoEstado == 7 || e.CodigoEstado == 8 || e.CodigoEstado == 16)
            //                           orderby t.FechaAlta descending
            //                           select new OperacionSupervisorCustom()
            //                           {
            //                               NroEntrada = Convert.ToInt32(me.NumeroEntrada),
            //                               DescSolicitud = ts.DescripSolicitud,
            //                               FechaAlta = t.FechaAlta,
            //                               DescEstado = e.DescripEstado,
            //                               Observacion = t.Observacion,
            //                               NombreOperador = p.Nombre
            //                           });

            //var resul = operacionSupervisor.GroupBy(r => r.NroEntrada)
            //  .Select(g => g.OrderByDescending(r => r.FechaAlta).First());

            return View( queryFinal.ToList());
        }


        public async Task<IActionResult> AddInscripcion(decimal nEntrada)
        {
            try
            {
                var mesaentrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p=>p.NumeroEntrada== nEntrada);
                var transaccion = await _dbContext.RmTransacciones.OrderByDescending(o=>o.FechaAlta).FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                if (transaccion !=null)
                {
                    var rmMesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

                    var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);

                    var rmTitularesMarcas = await _dbContext.RmTitularesMarcas.FirstOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);
                    var rmMarcasSenale = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                    ViewBag.Operacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", transaccion?.TipoOperacion ?? "");
                    var tiposOper = await _dbContext.RmTiposOperaciones.FirstOrDefaultAsync(p => p.TipoOperacion == Convert.ToInt32(transaccion.TipoOperacion) );
                    var descrOperacion = tiposOper?.DescripTipoOperacion??"";

                    //ViewBag.Autorizante = new SelectList(_dbContext.RmAutorizantes, "IdAutorizante", "DescripAutorizante", transaccion?.IdAutorizante ?? 0);
                    ViewBag.Distrito = new SelectList(_dbContext.RmDistritos, "CodigoDistrito", "DescripDistrito", rmMarcasXEstab?.CodDistrito ?? "");

                    ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", transaccion?.EstadoTransaccion ?? "");
                    var nombreRepresentante = await _dbContext.Personas.Where(p => p.CodPersona == transaccion.Representante).FirstOrDefaultAsync();
                    var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(a => a.IdAutorizante == transaccion.IdAutorizante);
                    var codDistrito = rmMarcasXEstab?.CodDistrito??"";
                    var distrito = await _dbContext.RmDistritos.FirstOrDefaultAsync(d => d.CodigoDistrito.ToString() == codDistrito);
                    var codDepartamento = distrito?.CodigoDepto??0;
                    var departamento = await _dbContext.AvDepartamentos.FirstOrDefaultAsync(d => d.CodDepartamento == codDepartamento.ToString());
                    var nombreFirmante = await _dbContext.Personas.Where(p => p.CodPersona == transaccion.FirmanteRuego).FirstOrDefaultAsync();
                    var nroBoleta = transaccion?.NroBoleta ?? "0";
               
                    string DescBoleta = null;
                    if (nroBoleta == "0")
                    {
                        var marcasSenales = _dbContext.RmMarcasSenales.FirstOrDefault(m => m.NumeroEntrada == nEntrada);
                        var boleta = marcasSenales?.NroBoleta ?? "0";
                        var boletaMarcaEn = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.NroBoleta == Convert.ToInt64(boleta));
                        DescBoleta = boletaMarcaEn?.Descripcion ?? "";
                    }
                    else
                    {
                        var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.NroBoleta == Convert.ToInt64(nroBoleta));
                        DescBoleta = boletaMarca?.Descripcion??"";
                    }


                    //var nroBoleta = marcasSenales?.NroBoleta ?? "0";


                    InscripcionCustom inscripcion = new()
                    {
                        NroEntrada = transaccion?.NumeroEntrada ?? 0,
                        IdTransaccion = transaccion?.IdTransaccion ?? 0,
                        EstadoEntrada = Convert.ToInt64(transaccion?.EstadoTransaccion),
                        CantidadGanado = rmMarcasSenale?.CantidadGanado ?? 0,
                        NroBoleta = DescBoleta,
                        NroBolMarcaAnt = transaccion?.NroBolMarcaAnt ?? "",
                        NroBolSenhalAnt = transaccion?.NroBolSenhalAnt ?? "",
                        TipoOperacion = descrOperacion,
                        FechaActoJuridico = transaccion?.FechaActoJuridico ?? null,
                        MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                        NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                        Serie = transaccion?.Serie ?? "",
                        FormaConcurre = transaccion?.FormaConcurre ?? "",
                        Representante = transaccion?.Representante ?? "",
                        NombreRepresentante = nombreRepresentante?.Nombre ?? "",
                        FirmanteRuego = transaccion?.FirmanteRuego ?? "",
                        NombreFirmanteRuego = nombreFirmante?.Nombre ?? "",
                        CodDistrito = rmMarcasXEstab?.CodDistrito ?? "",
                        Departamento = departamento?.Descripcion ?? "",
                        GpsH = rmMarcasXEstab?.GpsH ?? "",
                        GpsS = rmMarcasXEstab?.GpsS ?? "",
                        GpsSc = rmMarcasXEstab?.GpsSc ?? "",
                        GpsV = rmMarcasXEstab?.GpsV ?? "",
                        GpsW = rmMarcasXEstab?.GpsW ?? "",
                        Comentario = transaccion?.Comentario ?? "",
                        Asiento = transaccion?.Asiento ?? 0,
                        Observacion = transaccion?.Observacion ?? ""
                    };

                    return View("Inscripcion", inscripcion);
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
                        return View("Certificado", inscripcion);
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


                        return View("Certificado", inscripcion);
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
       
        public async Task<IActionResult> AddPrenda(decimal nEntrada)
        {
            try
            {
                // Busca la entrada en la base de datos en función de su número de entrada.
                var mesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

                // Cargar información sobre los distritos para mostrar en la vista.

                //Si se encuentra la mesa de entrada:
                //Si también se encuentra la prenda, crea un objeto "PrendaCustom" con los detalles y muestra la vista "Index" con estos detalles.
                //Si no se encuentra la prenda, muestra la vista "Index" sin detalles de la prenda.
                if (mesaEntrada != null)
                {
                    var rmTransaccion = await _dbContext.RmTransacciones.OrderByDescending(o=>o.FechaAlta).FirstOrDefaultAsync(t => t.NumeroEntrada == mesaEntrada.NumeroEntrada);
                    //Busca información sobre la prenda asociada a la mesa de entrada.
                    var rmPrenda = await _dbContext.RmMedidasPrendas.FirstOrDefaultAsync(p => p.NroEntrada == mesaEntrada.NumeroEntrada);

                    if (rmPrenda != null)
                    {
                        //Obtiene detalles del distrito y el departamento asociados a la prenda.
                        var codDistrito = rmPrenda?.CodDistrito??"";
                        var distrito = await _dbContext.RmDistritos.FirstOrDefaultAsync(d => d.CodigoDistrito.ToString() == codDistrito);
                        var codDepto = distrito?.CodigoDepto ?? 0;

                        var departamento = await _dbContext.AvDepartamentos.FirstOrDefaultAsync(d => d.CodDepartamento == codDepto.ToString());

                        //var codAcreedor = rmPrenda?.Acreedor??"";
                        //var acreedor = await _dbContext.Personas.FirstOrDefaultAsync(p=>p.CodPersona== codAcreedor);
                        //var codDeudor = rmPrenda?.Deudor ?? "";
                        //var deudor = await _dbContext.Personas.FirstOrDefaultAsync(p=>p.CodPersona == codDeudor);

                        ViewBag.Distrito = new SelectList(_dbContext.RmDistritos, "CodigoDistrito", "DescripDistrito", rmPrenda?.CodDistrito ?? "");

                        PrendaCustom prenda = new()
                        {
                            Libro = rmPrenda?.Libro,
                            Folio = rmPrenda?.Folio,
                            NroEntrada = rmPrenda?.NroEntrada,
                            FechaInscripcion = rmPrenda?.FechaInscripcion,
                            FechaOperacion = rmPrenda?.FechaOperacion,
                            Acreedor = rmPrenda?.Acreedor,
                            Deudor = rmPrenda?.Deudor,
                            FechaVencimiento = rmPrenda?.FechaVencimiento,
                            MontoPrenda = rmPrenda?.MontoPrenda,
                            MontoDeJusticia = rmPrenda?.MontoDeJusticia,
                            NroBoleta = rmPrenda?.NroBoleta,
                            NroBoletaSenal = rmPrenda?.NroBoletaSenal,
                            CodDistrito = rmPrenda?.CodDistrito,
                            Departamento = departamento?.Descripcion ?? "",
                            //Distrito = await GetDistritosAsync(),
                            Instrumento = rmPrenda?.Instrumento,
                            Comentario = rmTransaccion?.Comentario
                        };
                        return View("Prenda", prenda);
                    }
                    else
                    {
                        PrendaCustom prenda = new()
                        {
                            NroEntrada = mesaEntrada.NumeroEntrada,
                            NroBoleta = mesaEntrada?.NroBoleta??""
                            //Distrito = await GetDistritosAsync(),
                            //Instrumento = rmPrenda.Instrumento
                        };
                        return View("Prenda", prenda);
                    }

                    //return View("Index");
                }
                //Si la mesa de entrada no se encuentra, devuelve un resultado "NotFound".

                else
                {
                    return NotFound();
                }


            }
            //Captura y lanza excepciones en caso de errores.

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
                    var transaccion = await _dbContext.RmTransacciones.OrderByDescending(o => o.FechaAlta).FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

                    if (transaccion != null)
                    {
                        var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(p => p.IdAutorizante == transaccion.IdAutorizante);

                        var nomRepresentante = await _dbContext.Personas.FirstOrDefaultAsync(p => p.CodPersona == transaccion.Representante);
                        var rmMarcasSenale = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(p => p.NumeroEntrada == transaccion.NumeroEntrada);
                        
                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", transaccion.TipoOperacion);
                        var tiposOper = await _dbContext.RmTiposOperaciones.FirstOrDefaultAsync(p => p.TipoOperacion == Convert.ToInt32(transaccion.TipoOperacion));
                        var descrOperacion = tiposOper?.DescripTipoOperacion ?? "";

                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", transaccion?.EstadoTransaccion ?? "");
                        var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);
                        var nomEscribano = await _dbContext.Personas.FirstOrDefaultAsync(p=>p.CodPersona== transaccion.IdEscribanoJuez);
                        var tituAdjudicado = await _dbContext.RmTitularesMarcas.OrderBy(t=>t.IdTm).FirstOrDefaultAsync(t=>t.IdTransaccion==transaccion.IdTransaccion);
                        string? idPropietario = tituAdjudicado?.IdPropietario??"";
                        var descTitu = await _dbContext.Personas.FirstOrDefaultAsync(t=>t.CodPersona== idPropietario.ToString());
                        var boletaMarca = await _dbContext.RmBoletasMarcas.FirstOrDefaultAsync(b => b.Descripcion == mesaEntrada.NroBoleta);
                        var nroBoletaMarca = boletaMarca?.NroBoleta ?? 0;
                        var marcasSenal = _dbContext.RmMarcasSenales.FirstOrDefault(t => t.NroBoleta == nroBoletaMarca.ToString());
                        var nroEntradaIm = marcasSenal?.NumeroEntrada ?? 0;
                        //var transaccion = await _dbContext.RmTransacciones.FirstOrDefaultAsync(p => p.NumeroEntrada == nroEntradaIm);
                        var transaccionAnte = await _dbContext.RmTransacciones.OrderByDescending(t => t.FechaAlta).FirstOrDefaultAsync(p => p.NumeroEntrada == nroEntradaIm);

                        // cargar para edicion
                        TransaccionCustom inscripcion = new()
                        {
                            NumeroEntrada = nEntrada,
                            NroEntradaTrans = transaccionAnte?.IdTransaccion,
                            EstadoEntrada = Convert.ToInt64(transaccion?.EstadoTransaccion),
                            TipoOperacion = descrOperacion,
                            NroBoleta = mesaEntrada.NroBoleta,
                            IdTransaccion = transaccionAnte?.IdTransaccion,
                            IdTransaccionPropietario = transaccion?.IdTransaccion??0,
                            IdPropietario = tituAdjudicado?.IdPropietario ?? "",
                            DescTitular = descTitu?.Nombre??"",
                            FechaRegistro = tituAdjudicado?.FechaRegistro,
                            IdEscribanoJuez = transaccion?.IdEscribanoJuez,
                            NomEscribano = nomEscribano?.Nombre??"",
                            FechaActoJuridico = transaccion?.FechaActoJuridico,
                            NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                            Serie = transaccion?.Serie ?? "",
                            FormaConcurre = transaccion?.FormaConcurre ?? "",
                            Representante = transaccion?.Representante ?? "",
                            NombreRepresentante = transaccion?.NombreRepresentante ?? "",
                            Comentario = transaccion?.Comentario ?? "",
                            Observacion = transaccion?.Observacion ?? ""
                        };
                        return View("Transferencia", inscripcion);
                    }
                    else
                    {
                        // cargar nuevo

                        TransaccionCustom inscripcion = new();
                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada?.EstadoEntrada ?? 0);
                        inscripcion.NumeroEntrada = nEntrada;
                        inscripcion.EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0;


                        return View("Transferencia", inscripcion);
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

        public async Task<IActionResult> AddLevantamiento(decimal nEntrada)
        {
            try
            {

                var rmLevantamiento = await _dbContext.RmLevantamientos.FirstOrDefaultAsync(p => p.NroEntrada == nEntrada);
                if (rmLevantamiento == null)
                {
                    // No se encontró ningún registro con el número de entrada especificado.
                    ViewData["ErrorMessage"] = "No se encontró ningún registro con el número de entrada especificado.";
                }
                else
                {
                    ViewData["IdUsuario"] = new SelectList(_dbContext.Usuarios, "CodUsuario", "CodUsuario", rmLevantamiento.IdUsuario ?? "");
                    ViewData["TipoMoneda"] = new SelectList(_dbContext.RmTiposMonedas, "TipoMoneda", "DescripTipoMoneda", rmLevantamiento.TipoMoneda ?? 0);
                    ViewData["IdAutorizante"] = new SelectList(_dbContext.RmAutorizantes, "IdAutorizante", "DescripAutorizante", rmLevantamiento.IdAutorizante ?? 0);

                }
                return View("Levantamiento", rmLevantamiento);
            }
            catch (Exception ex)
            {
                // Manejar el error de generación de PDF de alguna manera
                return BadRequest("Error al cargar la pagina " + ex.Message);
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
                            Comentario = nuevaTrans?.Comentario??"",
                            Observacion = nuevaTrans?.Observacion ?? ""
                        };
                        return View("Informe", inscripcion);
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
                        return View("Informe", inscripcion);
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

        public async Task<IActionResult> AddDuplicado(decimal nEntrada)
        {
            try
            {
                var transaccion = await _dbContext.RmTransacciones.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                if (transaccion != null)
                {
                    var rmMesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

                    var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);

                    var rmTitularesMarcas = await _dbContext.RmTitularesMarcas.FirstOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);
                    var rmMarcasSenale = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                    ViewBag.Operacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", transaccion?.TipoOperacion ?? "");
                    //ViewBag.Autorizante = new SelectList(_dbContext.RmAutorizantes, "IdAutorizante", "DescripAutorizante", transaccion?.IdAutorizante ?? 0);
                    ViewBag.Distrito = new SelectList(_dbContext.RmDistritos, "CodigoDistrito", "DescripDistrito", rmMarcasXEstab?.CodDistrito ?? "");

                    ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", rmMesaEntrada?.EstadoEntrada ?? 0);
                    var nombreRepresentante = await _dbContext.Personas.Where(p => p.CodPersona == transaccion.Representante).FirstOrDefaultAsync();
                    var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(a => a.IdAutorizante == rmMesaEntrada.IdAutorizante);
                    var codDistrito = rmMarcasXEstab?.CodDistrito ?? "";
                    var distrito = await _dbContext.RmDistritos.FirstOrDefaultAsync(d => d.CodigoDistrito.ToString() == codDistrito);
                    var codDepartamento = distrito?.CodigoDepto ?? 0;
                    var departamento = await _dbContext.AvDepartamentos.FirstOrDefaultAsync(d => d.CodDepartamento == codDepartamento.ToString());
                    var nombreFirmante = await _dbContext.Personas.Where(p => p.CodPersona == transaccion.FirmanteRuego).FirstOrDefaultAsync();

                    InscripcionCustom inscripcion = new()
                    {
                        NroEntrada = transaccion?.NumeroEntrada ?? 0,
                        IdTransaccion = transaccion?.IdTransaccion ?? 0,
                        EstadoEntrada = rmMesaEntrada?.EstadoEntrada ?? 0,
                        CantidadGanado = rmMarcasSenale?.CantidadGanado ?? 0,
                        NroBoleta = transaccion?.NroBoleta ?? "",
                        NroBolMarcaAnt = transaccion?.NroBolMarcaAnt ?? "",
                        NroBolSenhalAnt = transaccion?.NroBolSenhalAnt ?? "",
                        TipoOperacion = transaccion?.TipoOperacion ?? "",
                        FechaActoJuridico = transaccion?.FechaActoJuridico ?? null,
                        MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                        NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                        Serie = transaccion?.Serie ?? "",
                        FormaConcurre = transaccion?.FormaConcurre ?? "",
                        Representante = transaccion?.Representante ?? "",
                        NombreRepresentante = nombreRepresentante?.Nombre ?? "",
                        FirmanteRuego = transaccion?.FirmanteRuego ?? "",
                        NombreFirmanteRuego = nombreFirmante?.Nombre ?? "",
                        CodDistrito = rmMarcasXEstab?.CodDistrito ?? "",
                        Departamento = departamento?.Descripcion ?? "",
                        GpsH = rmMarcasXEstab?.GpsH ?? "",
                        GpsS = rmMarcasXEstab?.GpsS ?? "",
                        GpsSc = rmMarcasXEstab?.GpsSc ?? "",
                        GpsV = rmMarcasXEstab?.GpsV ?? "",
                        GpsW = rmMarcasXEstab?.GpsW ?? "",
                        Comentario = transaccion?.Comentario ?? "",
                        Asiento = transaccion?.Asiento ?? 0,
                        Observacion = transaccion?.Observacion ?? ""
                    };

                    return View("Duplicado", inscripcion);
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

        public async Task<JsonResult> CargarDatosPersona(string idPersona)
        {
            IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();
            IQueryable<DirecPersona> queryDirPersona = _dbContext.DirecPersonas.AsQueryable();
            IQueryable<EstadosCivile> queryEstadosCiviles = _dbContext.EstadosCiviles.AsQueryable();
            IQueryable<Paise> queryPaises = _dbContext.Paises.AsQueryable();
            IQueryable<Provincia> queryProvincias = _dbContext.Provincias.AsQueryable();
            IQueryable<Ciudade> queryCiudades = _dbContext.Ciudades.AsQueryable();
            IQueryable<Barrio> queryBarrios = _dbContext.Barrios.AsQueryable();

            var persona = await _dbContext.Personas.FirstOrDefaultAsync(p => p.CodPersona == idPersona);
            string estadoCiv = "";
            string nacionalidad = "";
            string pais = "";
            string dpto = "";
            string ciudad = "";
            string barrio = "";
            string profesion = "";

            if (persona?.CodEstadoCivil != null)
            {
                var estado = await _dbContext.EstadosCiviles.FirstOrDefaultAsync(p => p.CodEstadoCivil == persona.CodEstadoCivil);
                estadoCiv = estado != null ? estado.Descripcion : "";
            }
            if (persona?.Profesion != null)
            {
                var profesionS = await _dbContext.Profesiones.FirstOrDefaultAsync(p => p.CodProfesion == persona.Profesion);
                profesion = profesionS != null ? profesionS.Descripcion : "";
            }

            if (persona?.CodPais != null)
            {
                var esPais = await _dbContext.Paises.FirstOrDefaultAsync(p => p.CodPais == persona.CodPais);
                pais = esPais != null ? esPais.Descripcion : "";
                nacionalidad = esPais != null ? esPais.Nacionalidad : "";

            }

            var personaDir = await _dbContext.DirecPersonas.FirstOrDefaultAsync(p => p.CodPersona == idPersona);

            if (personaDir?.CodProvincia != null)
            {
                var esProv = await _dbContext.Provincias.FirstOrDefaultAsync(p => p.CodProvincia == personaDir.CodProvincia);
                dpto = esProv != null ? esProv.Descripcion : "";
            }
            if (personaDir?.CodCiudad != null)
            {
                var esCiudad = await _dbContext.Ciudades.FirstOrDefaultAsync(p => p.CodCiudad == personaDir.CodCiudad);
                ciudad = esCiudad != null ? esCiudad.Descripcion : "";
            }
            if (personaDir?.CodBarrio != null)
            {
                var esBarrio = await _dbContext.Barrios.FirstOrDefaultAsync(p => p.CodBarrio == personaDir.CodBarrio);
                barrio = esBarrio != null ? esBarrio.Descripcion : "";
            }

            var datos = new
            {
                CodPersona = persona.CodPersona,
                Nombre = persona.Nombre,
                FechaNacimiento = persona.FecNacimiento,
                Sexo = persona.Sexo,
                EstadoCivil = estadoCiv,
                Direccion = personaDir.Detalle,
                Nacionalidad = nacionalidad,
                Pais = pais,
                Provincia = dpto,
                Ciudad = ciudad,
                Barrio = barrio,
                Profesion = profesion
            };



            return Json(datos);
        }

        [HttpPost]
        public async Task<IActionResult> EditarTransaccion(int id, string nuevoEstado, string comentario)
        {
            try
            {
                var mesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(t => t.NumeroEntrada == id);

                if (mesaEntrada != null)
                {
                    var transaccion = await _dbContext.RmTransacciones.OrderByDescending(o=>o.FechaAlta).FirstOrDefaultAsync(t => t.NumeroEntrada == id);

                    var estadoEntrada = await _dbContext.RmEstadosEntrada.FirstOrDefaultAsync(e => e.DescripEstado == nuevoEstado);
                    mesaEntrada.EstadoEntrada = estadoEntrada.CodigoEstado;
                    _dbContext.Update(mesaEntrada);

                     RmMovimientosDoc movimientos = new()
                    {
                        NroEntrada = id,
                        CodUsuario = User.Identity.Name, //Debe ser usuario logueado
                        FechaOperacion = DateTime.Now,
                        CodOperacion = "09", //Parametro para cambio de estado 
                        NroMovimientoRef = id.ToString(),
                        EstadoEntrada = estadoEntrada.CodigoEstado
                    };
                    await _dbContext.AddAsync(movimientos);
                    await _dbContext.SaveChangesAsync();
                    if (transaccion == null)
                    {
                        // Manejo si la transacción no se encuentra
                        return NotFound();
                    }
                    else
                    {
                        transaccion.EstadoTransaccion = estadoEntrada.CodigoEstado.ToString();
                        transaccion.UsuarioSup = User.Identity.Name;
                        if (comentario == "")
                        {
                            transaccion.ObservacionSup = transaccion.ObservacionSup;
                        }
                        else
                        {
                            transaccion.ObservacionSup = comentario;
                        }

                        _dbContext.Update(transaccion);
                        await _dbContext.SaveChangesAsync();
                    }
                    return View("Index");
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return BadRequest("Error al cargar la pagina " + ex.Message);

            }

        }
        private bool RmTransaccioneExists(decimal id)
        {
            return (_dbContext.RmTransacciones?.Any(e => e.IdTransaccion == id)).GetValueOrDefault();
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

        //public IList<OperacionSupervisorCustom> YourCustomSearchFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        //{
        //    var searchBy = (model.Search != null) ? model.Search.Value : null;
        //    var take = model.Length;
        //    var skip = model.Start;

        //    string sortBy = "";
        //    bool sortDir = true;

        //    var result = GetDataFromDbase(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
        //    if (result == null)
        //    {
        //        return new List<OperacionSupervisorCustom>();
        //    }
        //    return result;
        //}

        //public List<OperacionSupervisorCustom> GetDataFromDbase(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        //{

        //    if (String.IsNullOrEmpty(searchBy))
        //    {
        //        sortBy = "NroEntrada";
        //        sortDir = true;
        //    }
        //    List<decimal> codigosPermitidos = new List<decimal> { 1, 2, 3, 4, 7, 8, 16 };
        //    IQueryable<RmMesaEntradum> queryMesaEntrada = _dbContext.RmMesaEntrada.AsQueryable();
        //    IQueryable<RmTipoSolicitud> queryTipoSolicitud = _dbContext.RmTipoSolicituds.AsQueryable();
        //    IQueryable<RmTransaccione> queryTransaccion = _dbContext.RmTransacciones.AsQueryable();
        //    IQueryable<RmEstadosEntradum> queryEstadoEntrada = _dbContext.RmEstadosEntrada.Where(e => codigosPermitidos.Contains(e.CodigoEstado)).AsQueryable();
        //    IQueryable<Usuario> queryUsuario = _dbContext.Usuarios.AsQueryable();
        //    IQueryable<Persona> queryPersona = _dbContext.Personas.AsQueryable();

        //    //Para obtener transaccion 
        //    var queryEntradaXTransaccion = queryMesaEntrada.AsQueryable().Join(queryTransaccion,
        //        nroEntrada => nroEntrada.NumeroEntrada,
        //        transaccion => transaccion.NumeroEntrada,
        //        (nroEntrada, transaccion) =>
        //        new {
        //            NroEntrada = nroEntrada.NumeroEntrada,
        //            TipoSolicitud = nroEntrada.TipoSolicitud,
        //            FechaAlta = transaccion.FechaAlta,
        //            CodEstado = transaccion.EstadoTransaccion,
        //            Observacion = transaccion.Observacion,
        //            Usuario = transaccion.IdUsuario
        //        });

        //    //Para Obetener tipo solicitud 
        //    var queryEntradaTipoSol = queryEntradaXTransaccion.AsQueryable().Join(queryTipoSolicitud,
        //        nroEntrada => nroEntrada.TipoSolicitud,
        //        tipoSolicitud => tipoSolicitud.TipoSolicitud,
        //        (nroEntrada, tipoSolicitud) => new {
        //            NroEntrada = nroEntrada.NroEntrada,
        //            TipoSolicitud = nroEntrada.TipoSolicitud,
        //            DescSolicitud = tipoSolicitud.DescripSolicitud,
        //            FechaAlta = nroEntrada.FechaAlta,
        //            CodEstado = nroEntrada.CodEstado,
        //            Observacion = nroEntrada.Observacion,
        //            Usuario = nroEntrada.Usuario
        //             });


        //    //Para obtener estado 
        //    var queryEstadoTransa = queryEntradaTipoSol.AsQueryable().Join(queryEstadoEntrada,
        //        transaccion => transaccion.CodEstado,
        //        estado => estado.CodigoEstado.ToString(),
        //        (transaccion, estado) => new {
        //            NroEntrada = transaccion.NroEntrada,
        //            TipoSolicitud = transaccion.TipoSolicitud,
        //            DescSolicitud = transaccion.DescSolicitud,
        //            FechaAlta = transaccion.FechaAlta,
        //            Observacion = transaccion.Observacion,
        //            Usuario = transaccion.Usuario,
        //            DescEstado = estado.DescripEstado

        //        });
        //    //Para obtener el nombre del operador
        //    var queryFinal = queryEstadoTransa.AsQueryable().Join(queryPersona,
        //          transaccion => transaccion.Usuario,
        //          people => people.CodPersona,
        //          (transaccion, people) =>
        //              new OperacionSupervisorCustom
        //              {
        //                  NroEntrada = transaccion.NroEntrada,
        //                  TipoSolicitud = transaccion.TipoSolicitud,
        //                  DescSolicitud = transaccion.DescSolicitud,
        //                  FechaAlta = transaccion.FechaAlta,
        //                  DescEstado = transaccion.DescEstado,
        //                  Observacion = transaccion.Observacion,
        //                  NombreOperador = people.Nombre

        //              });

        //    if (!string.IsNullOrEmpty(searchBy))
        //    {
        //        queryFinal = queryFinal.AsQueryable().Where(item =>
        //                item.NroEntrada.ToString().Contains(searchBy) 
        //            );
        //    }

        //    var result = queryFinal
        //                   .Select(m => new OperacionSupervisorCustom
        //                   {
        //                       NroEntrada = m.NroEntrada,
        //                       TipoSolicitud = m.TipoSolicitud,
        //                       DescSolicitud = m.DescSolicitud,
        //                       FechaAlta = m.FechaAlta,
        //                       DescEstado = m.DescEstado,
        //                       Observacion = m.Observacion,
        //                       NombreOperador = m.NombreOperador
        //                   })
        //                   .Skip(skip)
        //                  .Take(take)
        //                  .ToList();

        //    filteredResultsCount = queryFinal.Count();
        //    totalResultsCount = queryFinal.Count();

        //    return result;
        //}


        // GET: OperacionSupervisorController/Details/5
        public async Task<IActionResult> Details(string NroEntrada, string DescSolicitud, DateTime FechaAlta, string DescEstado, string Observacion, string NombreOperador)
        {
            var operacionSuper = await _dbContext.RmMesaEntrada
            .FindAsync(NroEntrada, DescSolicitud, FechaAlta, DescEstado, Observacion, NombreOperador);
            if (operacionSuper == null)
            {
                return NotFound();
            }
            return View(operacionSuper);
        }
        // GET: OperacionSupervisorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OperacionSupervisorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMJEFREG", "Create", "OperacionSupervisorCustom" })]

        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OperacionSupervisorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OperacionSupervisorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMJEFREG", "Edit", "OperacionSupervisorCustom" })]

        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult GenerarPdf(string codPersona)
        {
            // Obtiene los datos filtrados según los parámetros del modelo
            var datosReporte = GetReporteData(codPersona);

            // Lee la imagen y conviértela en una cadena de datos URI codificada en base64
            string imagePath = "wwwroot/assets/img/PJ/PoderJudicial.png"; // Ruta a la imagen en tu proyecto
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

            // Pasa la cadena de datos URI a la vista a través del modelo de vista
            datosReporte.ImageDataUri = imageDataUri;

            // Renderiza la vista Razor a una cadena HTML
            string viewHtml = RenderViewToString("ReportePDF", datosReporte);

            if (string.IsNullOrWhiteSpace(viewHtml))
            {
                return BadRequest("La vista HTML está vacía o nula.");
            }

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
            Response.Headers["Content-Disposition"] = "inline; filename=Reporte-de-Entrada.pdf";

            // Devolver el PDF como un archivo descargable con el nuevo nombre
            return File(pdfBytes, "application/pdf");
        }

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
        private InformeCustom GetReporteData(string codPersona)
        {
            IQueryable<RmTitularesMarca> queryTitularMarca = _dbContext.RmTitularesMarcas.AsQueryable();
            IQueryable<RmTransaccione> queryTransaccion = _dbContext.RmTransacciones.AsQueryable();
            IQueryable<RmTiposOperacione> queryTipoOperacion = _dbContext.RmTiposOperaciones.AsQueryable();
            IQueryable<RmMesaEntradum> queryEntrada = _dbContext.RmMesaEntrada.AsQueryable();
            IQueryable<RmDistrito> queryDistrito = _dbContext.RmDistritos.AsQueryable();
            IQueryable<RmMarcasXEstab> queryMarcasEstab = _dbContext.RmMarcasXEstabs.AsQueryable();


            //Para obtener transaccion 
            var queryTitularTrans = queryTitularMarca.Where(t => t.IdTitular == codPersona).AsQueryable().Join(queryTransaccion,
                titu => titu.IdTransaccion,
                trans => trans.IdTransaccion,
                (titu, trans) =>
                new Operaciones { NroEntrada = trans.NumeroEntrada, TipoOperacion = trans.TipoOperacion, NroBoleta = trans.NroBoleta });


            var primer =  queryTitularTrans.ToArray();

            //Para obtener transaccion 
            var queryTitularEntrada = queryTitularTrans.AsQueryable().Join(queryEntrada,
                titu => titu.NroEntrada,
                trans => trans.NumeroEntrada,
                (titu, trans) =>
                new Operaciones { NroEntrada = trans.NumeroEntrada, TipoOperacion = titu.TipoOperacion, NroBoleta = trans.NroBoleta, FechaEntrada = trans.FechaEntrada });


            var segundo = queryTitularEntrada.ToArray();


            //Para obtener Tipo Operacion 
            var quryFinal = queryTitularEntrada.AsQueryable().Join(queryTipoOperacion,
                trans => trans.TipoOperacion,
                tipo => tipo.TipoOperacion.ToString(),
                (trans, tipo) =>
                new Operaciones { NroEntrada = trans.NroEntrada, TipoOperacion = tipo.DescripTipoOperacion, NroBoleta = trans.NroBoleta, FechaEntrada = trans.FechaEntrada }
                ).GroupBy(op => op.NroEntrada)
                .Select(group => group.First());

            var titularesMarcas =  quryFinal.ToList();

            InformeCustom tituloData = new();
            tituloData.ListaOperaciones = titularesMarcas;
            tituloData.TotalOperaciones = titularesMarcas.Count();
            //tituloData.FechaDesde = parametros.FechaDesde;
            //tituloData.FechaHasta = parametros.FechaHasta;
            tituloData.FechaActual = DateTime.Now;
            //tituloData.Usuario = User.Identity.Name;

            return tituloData;
        }

    }
}
