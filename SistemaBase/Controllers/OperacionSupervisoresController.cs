using iText.Html2pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Html2pdf;
using iText.Commons.Actions.Contexts;
using Org.BouncyCastle.Asn1.Ocsp;

namespace SistemaBase.Controllers
{
    public class OperacionSupervisoresController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public OperacionSupervisoresController(DbvinDbContext context)
        {
            _dbContext = context;
        }

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMOPSUP", "Index", "OperacionSupervisores" })]

        public async Task<IActionResult> Index()
        {
            var queryFinal = (from me in _dbContext.RmMesaEntrada
                              join ts in _dbContext.RmTipoSolicituds on me.TipoSolicitud equals ts.TipoSolicitud
                              join t in _dbContext.RmTransacciones on me.NumeroEntrada equals t.NumeroEntrada
                              join e in _dbContext.RmEstadosEntrada on me.EstadoEntrada equals e.CodigoEstado
                              join au in _dbContext.RmAsignaciones on me.NumeroEntrada equals au.NroEntrada
                              join u in _dbContext.Usuarios on t.IdUsuario equals u.CodUsuario
                              join p in _dbContext.Personas on u.CodPersona equals p.CodPersona
                              where e.DescripEstado == "Recepcionado Supervisor"
                              orderby t.FechaAlta descending
                              select new
                              {
                                  NroEntrada = Convert.ToInt32(me.NumeroEntrada),
                                  DescSolicitud = ts.DescripSolicitud,
                                  FechaAlta = t.FechaAlta,
                                  DescEstado = e.DescripEstado,
                                  Observacion = t.ObservacionSup,
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
                                Observacion = result.Observacion,
                                NombreOperador = result.NombreOperador
                            });


            //var operacionSupervisor = (from me in _dbContext.RmMesaEntrada
            //                           join ts in _dbContext.RmTipoSolicituds on me.TipoSolicitud equals ts.TipoSolicitud
            //                           join t in _dbContext.RmTransacciones on me.NumeroEntrada equals t.NumeroEntrada
            //                           join e in _dbContext.RmEstadosEntrada on me.EstadoEntrada equals e.CodigoEstado
            //                           join au in _dbContext.RmAsignaciones on me.NumeroEntrada equals au.NroEntrada
            //                           join u in _dbContext.Usuarios on t.IdUsuario equals u.CodUsuario
            //                           join p in _dbContext.Personas on u.CodPersona equals p.CodPersona
            //                           where e.DescripEstado== "Recepcionado Supervisor" /*&& au.IdUsuarioAsignado == User.Identity.Name*/
            //                           //(e.DescripEstado == "Aprobado/JefeRegistral" || e.DescripEstado == "Observado/JefeRegistral" || e.DescripEstado == "Nota Negativa/JefeRegistral")
            //                           //(e.CodigoEstado == 1 || e.CodigoEstado == 2 || e.CodigoEstado == 3
            //                           //|| e.CodigoEstado == 4 || e.CodigoEstado == 7 || e.CodigoEstado == 8 || e.CodigoEstado == 16)
            //                           orderby t.FechaAlta descending
            //                           select new OperacionSupervisorCustom()
            //                           {
            //                               NroEntrada = Convert.ToInt32(me.NumeroEntrada),
            //                               DescSolicitud = ts.DescripSolicitud,
            //                               FechaAlta = t.FechaAlta,
            //                               DescEstado = e.DescripEstado,
            //                               Observacion = t.ObservacionSup,
            //                               NombreOperador = p.Nombre
            //                           });

            //var resul = operacionSupervisor.GroupBy(r => r.NroEntrada)
            //  .Select(g => g.OrderByDescending(r => r.FechaAlta).First());

            return View(queryFinal.ToList());
        }

        public async Task<IActionResult> AddInforme(decimal nEntrada)
        {
            try
            {
                var mesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);


                if (mesaEntrada != null)
                {
                    var titularMarca = await _dbContext.RmTitularesMarcas.FirstOrDefaultAsync(t => t.IdTitular.ToString() == mesaEntrada.NroDocumentoTitular);

                    var transaccion = await _dbContext.RmTransacciones.FirstOrDefaultAsync(p => p.IdTransaccion == titularMarca.IdTransaccion);

                    if (transaccion != null)
                    {
                        var nomRepresentante = await _dbContext.Personas.FirstOrDefaultAsync(p => p.CodPersona == transaccion.Representante);
                        var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(p => p.IdAutorizante == transaccion.IdAutorizante);

                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", transaccion?.EstadoTransaccion ?? "");
                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", transaccion.TipoOperacion);

                        var entradaBoleta = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == transaccion.NumeroEntrada);
                        var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);

                        InformeCustom inscripcion = new()
                        {
                            NumeroEntrada = transaccion.NumeroEntrada,
                            EstadoEntrada = Convert.ToInt64(transaccion?.EstadoTransaccion),
                            NroBoleta = transaccion?.NroBoleta ?? "",
                            IdTransaccion = transaccion?.IdTransaccion ?? 0,
                            TipoOperacion = transaccion?.TipoOperacion ?? "",
                            FechaActoJuridico = transaccion?.FechaActoJuridico,
                            NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                            FechaAlta = transaccion?.FechaAlta,
                            Serie = transaccion?.Serie ?? "",
                            FormaConcurre = transaccion?.FormaConcurre ?? "",
                            Representante = transaccion?.Representante ?? "",
                            NombreRepresentante = nomRepresentante?.Nombre ?? ""
                        };
                        return View("Informe", inscripcion);
                    }
                    else
                    {
                        InformeCustom inscripcion = new()
                        {
                            NumeroEntrada = mesaEntrada.NumeroEntrada,
                            EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0,
                            NroBoleta = mesaEntrada?.NroBoleta ?? "No Existe"
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

        public async Task<IActionResult> AddInscripcion(decimal nEntrada)
        {
            try
            {
                try
                {
                    var mesaentrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                    var transaccion = await _dbContext.RmTransacciones.OrderByDescending(o => o.FechaAlta).FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                    if (transaccion != null)
                    {
                        var rmMesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

                        var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);

                        var rmTitularesMarcas = await _dbContext.RmTitularesMarcas.FirstOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);
                        var rmMarcasSenale = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
                        ViewBag.Operacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", transaccion?.TipoOperacion ?? "");
                        var tiposOper = await _dbContext.RmTiposOperaciones.FirstOrDefaultAsync(p => p.TipoOperacion == Convert.ToInt32(transaccion.TipoOperacion));
                        var descrOperacion = tiposOper?.DescripTipoOperacion ?? "";

                        //ViewBag.Autorizante = new SelectList(_dbContext.RmAutorizantes, "IdAutorizante", "DescripAutorizante", transaccion?.IdAutorizante ?? 0);
                        ViewBag.Distrito = new SelectList(_dbContext.RmDistritos, "CodigoDistrito", "DescripDistrito", rmMarcasXEstab?.CodDistrito ?? "");

                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", transaccion?.EstadoTransaccion ?? "");
                        var nombreRepresentante = await _dbContext.Personas.Where(p => p.CodPersona == transaccion.Representante).FirstOrDefaultAsync();
                        var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(a => a.IdAutorizante == transaccion.IdAutorizante);
                        var codDistrito = rmMarcasXEstab?.CodDistrito ?? "";
                        var distrito = await _dbContext.RmDistritos.FirstOrDefaultAsync(d => d.CodigoDistrito.ToString() == codDistrito);
                        var codDepartamento = distrito?.CodigoDepto ?? 0;
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
                            DescBoleta = boletaMarca?.Descripcion ?? "";
                        }

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

                if (mesaEntrada != null)
                {
                    var transaccion = await _dbContext.RmTransacciones.OrderBy(t => t.FechaAlta).FirstOrDefaultAsync(p => p.NroBoleta == mesaEntrada.NroBoleta);
                    var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(p => p.IdAutorizante == mesaEntrada.IdAutorizante);

                    if (transaccion != null)
                    {
                        var nomRepresentante = await _dbContext.Personas.FirstOrDefaultAsync(p => p.CodPersona == transaccion.Representante);

                        var rmMarcasSenale = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(p => p.NumeroEntrada == transaccion.NumeroEntrada);
                        ViewBag.TipoOperacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", transaccion.TipoOperacion);
                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", transaccion?.EstadoTransaccion ?? "");
                        var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);

                        // cargar para edicion
                        CertificadoCustom inscripcion = new()
                        {
                            NumeroEntrada = mesaEntrada.NumeroEntrada,
                            NroEntradaTrans = transaccion.NumeroEntrada,
                            EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0,
                            NroBoleta = transaccion.NroBoleta,
                            IdTransaccion = transaccion.IdTransaccion,
                            CantidadGanado = rmMarcasSenale?.CantidadGanado ?? 0,
                            TipoOperacion = transaccion?.TipoOperacion ?? "",
                            FechaActoJuridico = transaccion?.FechaActoJuridico,
                            FechaAlta = transaccion?.FechaAlta,
                            NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                            Serie = transaccion?.Serie ?? "",
                            FormaConcurre = transaccion?.FormaConcurre ?? "",
                            Representante = transaccion?.Representante ?? "",
                            NombreRepresentante = nomRepresentante?.Nombre ?? ""
                        };
                        return View("Certificado", inscripcion);
                    }
                    else
                    {
                        // cargar nuevo

                        CertificadoCustom inscripcion = new();
                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", mesaEntrada?.EstadoEntrada ?? 0);
                        inscripcion.NumeroEntrada = nEntrada;
                        inscripcion.EstadoEntrada = mesaEntrada?.EstadoEntrada ?? 0;

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
                        var nomEscribano = await _dbContext.Personas.FirstOrDefaultAsync(p => p.CodPersona == transaccion.IdEscribanoJuez);
                        var tituAdjudicado = await _dbContext.RmTitularesMarcas.OrderBy(t => t.FechaRegistro).FirstOrDefaultAsync(t => t.IdTransaccion == transaccion.IdTransaccion);
                        string? idPropietario = tituAdjudicado?.IdPropietario ?? "";
                        var descTitu = await _dbContext.Personas.FirstOrDefaultAsync(t => t.CodPersona == idPropietario.ToString());
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
                            IdTransaccionPropietario = transaccion?.IdTransaccion ?? 0,
                            IdPropietario = tituAdjudicado?.IdPropietario ?? "",
                            DescTitular = descTitu?.Nombre ?? "",
                            FechaRegistro = tituAdjudicado?.FechaRegistro,
                            IdEscribanoJuez = transaccion?.IdEscribanoJuez,
                            NomEscribano = nomEscribano?.Nombre ?? "",
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
            var rmLevantamiento = await _dbContext.RmLevantamientos.SingleOrDefaultAsync(p => p.NroEntrada == nEntrada);
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



        [HttpPost]
        public async Task<IActionResult> EditarTransaccion(int id, string nuevoEstado, string comentario)
        {
            try
            {
                var mesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(t => t.NumeroEntrada == id);
                var estadoEntrada = await _dbContext.RmEstadosEntrada.FirstOrDefaultAsync(e => e.DescripEstado == nuevoEstado);
                var transaccion = await _dbContext.RmTransacciones.OrderByDescending(o => o.FechaAlta).FirstOrDefaultAsync(t => t.NumeroEntrada == id);

                if (mesaEntrada != null)
                {
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
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                // Manejo de errores
                return View("Error");
            }
            return View("Index");
        }
        private bool RmTransaccioneExists(decimal id)
        {
            return (_dbContext.RmTransacciones?.Any(e => e.IdTransaccion == id)).GetValueOrDefault();
        }

        public IActionResult GenerarPdf(decimal nEntrada)
        {
            var mesaSalida = GetTituloData(nEntrada);
            // Lee la imagen y conviértela en una cadena de datos URI codificada en base64
            string imagePath = "wwwroot/assets/img/PJ/PoderJudicial.png"; // Ruta a la imagen en tu proyecto
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

            // Pasa la cadena de datos URI a la vista a través del modelo de vista
            mesaSalida.ImageDataUri = imageDataUri;

            // Renderizar la vista Razor a una cadena HTML
            string viewHtml = RenderViewToString("TicketPDF", mesaSalida);

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

            // Generar el nombre del archivo PDF con el formato deseado
            string fileName = $"MesaDeSalida-Nro{nEntrada}-{DateTime.Now:dd-MM-yyyy}.pdf";

            // Devolver el PDF como un archivo descargable con el nuevo nombre
            return File(pdfBytes, "application/pdf", fileName);
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


        private DatosTituloCustom GetTituloData(decimal nEntrada)
        {
            var mesaEntrada = _dbContext.RmMesaEntrada.SingleOrDefault(p => p.NumeroEntrada == nEntrada);
            var transaccion = _dbContext.RmTransacciones.FirstOrDefault(p => p.NumeroEntrada == nEntrada);
            var dataRepresentante = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == transaccion.Representante);
            var dataTitular = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == mesaEntrada.NroDocumentoTitular);
            //var fecReingreso = _dbContext.RmReingresos.FirstOrDefault(p => p.NroEntrada == mesaEntrada.NumeroEntrada);

            if (mesaEntrada == null)
            {
                return null; // Manejar el caso cuando no se encuentra la entrada
            }


            DatosTituloCustom tituloData = new()
            {
                NumeroEntrada = mesaEntrada.NumeroEntrada,
                FechaActual = GetFechaActual(),
                NroBoleta = mesaEntrada.NroBoleta,
                Oficinas = GetOficinasRegistrales(),
                CodigoOficina = mesaEntrada.CodigoOficina,
                NomTitular = mesaEntrada.NomTitular,
                NroDocumentoTitular = mesaEntrada.NroDocumentoTitular,
                DireccionTitular = transaccion.DireccionTitular,
                Nacionalidades = GetNacionalidad(),
                CodPais = dataTitular.CodPais,
                Profesiones = GetProfesion(),
                CodProfesion = dataTitular.Profesion,
                FecNacimiento = dataTitular.FecNacimiento,
                Edad = CalcularEdad(dataTitular.FecNacimiento ?? DateTime.MinValue), // Calcula la edad aquí y usa DateTime.MinValue como valor predeterminado si FecNacimiento es nulo
                EstadoCivil = GetEstadoCivil(),
                CodEstadoCivil = dataTitular.CodEstadoCivil,
                NombreRepresentante = dataRepresentante.Nombre,
                Representante = transaccion.Representante,
                NacionalidadesRep = GetNacionalidad(),
                CodPaisRep = dataRepresentante.CodPais,
                EstadoCivilRep = GetEstadoCivil(),
                CodEstadoCivilRep = dataRepresentante.CodEstadoCivil,
                ProfesionesRep = GetProfesion(),
                CodProfesionRep = dataRepresentante.Profesion,
                MatriculaRegistro = GetMatriculaRegistro(),
                Autorizantes = GetAutorizante(),
                IdAutorizante = mesaEntrada.IdAutorizante,
                FechaActoJuridico = DateOnly.FromDateTime(transaccion?.FechaActoJuridico ?? DateTime.MinValue)
                //FechaReingreso=fecReingreso.FechaReingreso
            };

            return tituloData;
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
                    Text = $"{o.CodigoOficina}-{o.DescripOficina}",
                    Value = o.CodigoOficina.ToString()
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
                    Text = $"{t.CodPais}-{t.Nacionalidad}",
                    Value = t.CodPais.ToString()
                })
                .ToList();

            return tiposSolicitud;
        }

        private List<SelectListItem> GetMatriculaRegistro()
        {
            using var dbContext = new DbvinDbContext();

            var tiposDocumentos = dbContext.RmAutorizantes
                .Select(d => new SelectListItem
                {
                    Text = $"{d.IdAutorizante}-{d.MatriculaRegistro}",
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
                    Text = $"{d.IdProfesional}-{d.Nombre}",
                    Value = d.IdProfesional.ToString()
                })
                .ToList();

            return autorizante;
        }

        private List<SelectListItem> GetProfesion()
        {
            using var dbContext = new DbvinDbContext();
            var estadosEntrada = dbContext.Profesiones
            .Select(d => new SelectListItem
            {
                Text = $"{d.CodProfesion}-{d.Descripcion}",
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
                Text = $"{d.CodEstadoCivil}-{d.Descripcion}",
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


    }
}
