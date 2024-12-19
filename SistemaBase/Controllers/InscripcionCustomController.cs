using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Ini;
using Org.BouncyCastle.Utilities;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using System.Data;
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

using static SistemaBase.ModelsCustom.InscripcionCustom;
using static SistemaBase.Models.DbIdentificacionesContext;
using static SistemaBase.ModelsCustom.TransaccionCustom;
using static SistemaBase.ModelsCustom.DatosTituloCustom;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using BarcodeStandard;
using Microsoft.AspNetCore.Http;
using SistemaBase.Models.Dtos;
using BitMiracle.LibTiff.Classic;
using Microsoft.AspNetCore.Authorization;
using SkiaSharp;

namespace SistemaBase.Controllers
{
    [Authorize]

    public class InscripcionCustomController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        private readonly DbIdentificacionesContext _dbIdentContext;

        public InscripcionCustomController(DbvinDbContext context, DbIdentificacionesContext idenContext)
        {
            _dbContext = context;
            _dbIdentContext = idenContext;
        }
        // GET: InscripcionCustomController

        //public ActionResult Index()
        //{
        //    return View();

        //}

        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMINSCR", "Index", "InscripcionCustom" })]

        public IActionResult Index()
        {

            try
            {
                ViewBag.Operacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", "6");
                //ViewBag.Autorizante = new SelectList(_dbContext.RmAutorizantes, "IdAutorizante", "DescripAutorizante");
                ViewBag.Distrito = new SelectList(_dbContext.RmDistritos.OrderByDescending(o=>o.DescripDistrito), "CodigoDistrito", "DescripDistrito");

                return View();
                

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrió un error: " + ex.Message);

            }
        }

        public ActionResult AddTitular()
        {
            ViewBag.TipoPropiedad = new SelectList(_dbContext.RmTiposPropiedads, "IdTipoPropiedad", "DescripcionTipoPropiedad");
            return View("AddTitular");
            //return View("AddTitular", new PersonaTitular());

        }

        //public async Task<ActionResult> EditTitular(string idPersona)
        //{
        //    var titular =  _dbContext.RmTitularesMarcas.FirstOrDefault(t=>t.IdTitular.ToString() == idPersona);
        //    ViewBag.TipoPropiedad = new SelectList(_dbContext.RmTiposPropiedads, "IdTipoPropiedad", "DescripcionTipoPropiedad", titular.IdTipoPropiedad);

        //    return View("AddTitular", titular);
        //    //return View("AddTitular", new PersonaTitular());

        //}

        public async Task<ActionResult> AddPersonaIden(string idPersona)
        {
            //idPersona = "80137914-8";
            ViewBag.TipoDocumento = new SelectList(_dbContext.Identificaciones, "CodIdent", "Descripcion");
            var identPersona = (from ip in _dbContext.IdentPersonas
                                orderby ip.Rowid descending
                                where ip.CodPersona == idPersona
                                select new IdentPersona
                                {
                                    CodPersona = idPersona,
                                    CodIdent = ip.CodIdent,
                                    Numero = ip.Numero,
                                    FecVencimiento = ip.FecVencimiento,
                                    Rowid = ip.Rowid
                                });
            return View("AddPersonaIden", await identPersona.AsNoTracking().ToListAsync());

            //IdentPersona? identPersona = _dbContext.IdentPersonas.AsQueryable().OrderBy(p => p.Rowid).FirstOrDefault(p => p.CodPersona == idPersona);

            //return View("AddPersonaIden", identPersona ?? new IdentPersona());
            //return View("AddPersonaIden");

        }
        [HttpPost]
        public async Task<IActionResult> DeleteDocPersona(string CodIdent, string CodPersona)
        {
            try
            {
                var cantidad = _dbContext.IdentPersonas.Where(i=>i.CodPersona == CodPersona);
                var count = cantidad.Count();
                if (count > 1)
                {
                    var existingPersona = await _dbContext.IdentPersonas.Where(me => me.CodPersona == CodPersona).FirstOrDefaultAsync(me => me.CodIdent == CodIdent);
                    if (existingPersona != null)
                    {
                        _dbContext.IdentPersonas.Remove(existingPersona);

                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                    else
                {
                    return NotFound();
                }


            return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar/actualizar : " + ex.Message);
                return BadRequest("Error al agregar/actualizar : " + ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DocumentosPersona(IdentPersona identPersona)
        {
            try
            {
                var existingPersona = await _dbContext.IdentPersonas.Where(me => me.Rowid == identPersona.Rowid || me.CodPersona == identPersona.CodPersona).FirstOrDefaultAsync(me => me.CodPersona == identPersona.CodPersona);
                if (existingPersona != null)
                {
                    if (existingPersona.Rowid == identPersona.Rowid)
                    {
                        //Se actualiza los datos
                        //existingPersona.CodIdent = identPersona.CodIdent;
                        existingPersona.Numero = identPersona.Numero;
                        existingPersona.FecVencimiento = identPersona.FecVencimiento;

                        _dbContext.Update(existingPersona);
                    }
                    else
                    {
                        _dbContext.Add(identPersona);
                    }

                }
                else
                {
                    var Persona = await _dbContext.Personas.FirstOrDefaultAsync(me => me.CodPersona == identPersona.CodPersona);
                    if (Persona != null)
                    {
                        _dbContext.Add(identPersona);
                    }

                }

                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar/actualizar : " + ex.Message);
                return BadRequest("Error al agregar/actualizar : " + ex.Message);
            }
        }

        public async Task<ActionResult> EditTelPersona(string idPersona)
        {
            //idPersona = "80137914-8";
            var identPersona = (from ip in _dbContext.TelefPersonas
                                orderby ip.Rowid descending
                                where ip.CodPersona == idPersona
                                select new TelefPersona
                                {
                                    CodPersona = idPersona,
                                    CodigoArea = ip.CodigoArea,
                                    NumTelefono = ip.NumTelefono,
                                    TipTelefono = ip.TipTelefono,
                                    TelUbicacion = ip.TelUbicacion,
                                    Interno = ip.Interno,
                                    Nota = ip.Nota,
                                    PorDefecto = ip.PorDefecto,
                                    Rowid = ip.Rowid
                                });
            return View("AddPersonaTel", await identPersona.AsNoTracking().ToListAsync());
            //TelefPersona? telPersona = _dbContext.TelefPersonas.AsQueryable().OrderBy(p => p.Rowid).FirstOrDefault(p => p.CodPersona == idPersona);

            //return View("AddPersonaTel", telPersona ?? new TelefPersona());
        }


        [HttpPost]
        public async Task<IActionResult> TelefonosPersona(TelefPersona telefPersona)
        {
            try
            {
                var existingTelPersona = await _dbContext.TelefPersonas.Where(p => p.Rowid == telefPersona.Rowid || p.CodPersona == telefPersona.CodPersona).FirstOrDefaultAsync(me => me.CodPersona == telefPersona.CodPersona);
                if (existingTelPersona != null)
                {
                    if (existingTelPersona.Rowid == telefPersona.Rowid)
                    {
                        //Se actualiza los datos
                        existingTelPersona.CodigoArea = telefPersona.CodigoArea;
                        existingTelPersona.NumTelefono = telefPersona.NumTelefono;
                        existingTelPersona.TipTelefono = telefPersona.TipTelefono;
                        existingTelPersona.TelUbicacion = telefPersona.TelUbicacion;
                        existingTelPersona.Interno = telefPersona.Interno;
                        existingTelPersona.Nota = telefPersona.Nota;
                        existingTelPersona.PorDefecto = telefPersona.PorDefecto;

                        _dbContext.Update(existingTelPersona);
                    }
                    else
                    {
                        _dbContext.Add(telefPersona);
                    }

                }
                else
                {
                    var Persona = await _dbContext.Personas.FirstOrDefaultAsync(me => me.CodPersona == telefPersona.CodPersona);
                    if (Persona != null)
                    {
                        _dbContext.Add(telefPersona);
                    }

                }

                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar/actualizar : " + ex.Message);
                return BadRequest("Error al agregar/actualizar : " + ex.Message);
            }
        }

        public ActionResult AddAutorizante(string nomAuto, decimal matricula)
        {
            if (nomAuto!=null && nomAuto!="")
            {

                RmAutorizante? rmAutorizante = _dbContext.RmAutorizantes.AsQueryable().FirstOrDefault(p => p.DescripAutorizante.Contains(nomAuto));
                //if (rmAutorizante == null)
                //{
                //    RmAutorizante newAutorizante = new()
                //    {
                //        DescripAutorizante = nomAuto,
                //        MatriculaRegistro = matricula
                //    };
                //    _dbContext.Add(newAutorizante);
                //    _dbContext.SaveChanges();
                //    rmAutorizante = _dbContext.RmAutorizantes.AsQueryable().FirstOrDefault(p => p.DescripAutorizante == nomAuto);
                //}
                ViewBag.CodCiudad = new SelectList(_dbContext.Ciudades.OrderBy(o => o.Descripcion), "CodCiudad", "Descripcion", rmAutorizante?.CodCiudad ?? "");

                RmAutorizante autorizante = new()
                {
                    //IdAutorizante = rmAutorizante.IdAutorizante,
                    DescripAutorizante = rmAutorizante?.DescripAutorizante ?? nomAuto,
                    CodCiudad = rmAutorizante?.CodCiudad ?? "",
                    MatriculaRegistro = rmAutorizante?.MatriculaRegistro ?? matricula
                };


                return View("AddAutorizante", autorizante);
            }
            else
            {
                return NotFound();
            }
          
           
        }


        [HttpPost]
        public async Task<IActionResult> GuardarAutorizante(RmAutorizante autorizante)
        {
            try
            {
                var existingDireccion = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(me => me.DescripAutorizante == autorizante.DescripAutorizante);
                if (existingDireccion != null)
                {
                    //Se actualiza los datos
                    existingDireccion.MatriculaRegistro = autorizante.MatriculaRegistro;
                    existingDireccion.CodCiudad = autorizante.CodCiudad;
                  
                    _dbContext.Update(existingDireccion);
                }
                else
                {
                    _dbContext.Add(autorizante);
                }

                await _dbContext.SaveChangesAsync();

                if (autorizante != null)
                {
                    return Json(new { Success = true, matricula = autorizante.MatriculaRegistro, nomAutorizante = autorizante.DescripAutorizante });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Error" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar/actualizar : " + ex.Message);
                return BadRequest("Error al agregar/actualizar : " + ex.Message);
            }
        }




        public ActionResult EditPersona(string idPersona)
        {

            Persona? propietario = _dbContext.Personas.AsQueryable().FirstOrDefault(p => p.CodPersona == idPersona);
            Persona persona = new()
            {
                CodPersona = idPersona.ToString(),
                Nombre = propietario?.Nombre??"",
                FecNacimiento = propietario?.FecNacimiento,
                Conyugue = propietario?.Conyugue ?? "",
                Sexo = propietario?.Sexo ?? "",
                EsFisica = propietario?.EsFisica ?? ""
            };

            ViewBag.Profesion = new SelectList(_dbContext.Profesiones, "CodProfesion", "Descripcion", propietario?.Profesion ?? "");
            ViewBag.CodPais = new SelectList(_dbContext.Paises, "CodPais", "Nacionalidad", propietario?.CodPais ?? "");
            ViewBag.EstadoCivil = new SelectList(_dbContext.EstadosCiviles, "CodEstadoCivil", "Descripcion", propietario?.CodEstadoCivil ?? "");
            return View("EditPersona", persona);
            //idPersona = "80137914-8";
            //string nombres = "";
            //string apellidos = "";
            //DateTime fechaNacimiento = DateTime.MinValue;
            //if (idPersona.Any(c => !char.IsDigit(c)))
            //{
            //    var parametroNroDocumento1 = new SqlParameter("@CONDICION", SqlDbType.VarChar)
            //    {
            //        Value = idPersona
            //    };
            //    var results = _dbIdentContext.ConsultaPorRUC
            //        .FromSqlRaw("EXEC BuscarRUC @CONDICION", parametroNroDocumento1)
            //        .ToList();
            //    var result = results.SingleOrDefault();
            //    nombres = result?.nombre ?? "";

            //    Persona? propietario = _dbContext.Personas.AsQueryable().FirstOrDefault(p => p.CodPersona == idPersona);
            //    Persona persona = new()
            //    {
            //        CodPersona = idPersona.ToString(),
            //        Nombre = nombres,
            //        FecNacimiento = fechaNacimiento,
            //        Conyugue = propietario?.Conyugue ?? "",
            //        Sexo = propietario?.Sexo ?? "",
            //        EsFisica = propietario?.EsFisica??""
            //    };

            //    ViewBag.Profesion = new SelectList(_dbContext.Profesiones, "CodProfesion", "Descripcion", propietario?.Profesion ?? "");
            //    ViewBag.CodPais = new SelectList(_dbContext.Paises, "CodPais", "Nacionalidad", propietario?.CodPais ?? "");
            //    ViewBag.EstadoCivil = new SelectList(_dbContext.EstadosCiviles, "CodEstadoCivil", "Descripcion", propietario?.CodEstadoCivil ?? "");
            //    return View("EditPersona", persona);
            //}
            //else
            //{
            //    var parametroNroDocumento = new SqlParameter("@nro_documento", SqlDbType.VarChar)
            //    {
            //        Value = idPersona
            //    };
            //    var results2 = _dbIdentContext.ConsultaPorCedula
            //    .FromSqlRaw("EXEC ConsultaPorCedula @nro_documento", parametroNroDocumento)
            //    .ToList();

            //    var result2 = results2.SingleOrDefault();
            //    nombres = result2?.Nombres ?? "";
            //    apellidos = result2?.Apellidos ?? "";
            //    fechaNacimiento = result2?.FechaNacimiento ?? DateTime.MinValue;

            //    Persona? propietario = _dbContext.Personas.AsQueryable().FirstOrDefault(p => p.CodPersona == idPersona);
            //    Persona persona = new()
            //    {
            //        CodPersona = idPersona.ToString(),
            //        Nombre = nombres.Trim() + " " + apellidos.Trim(),
            //        FecNacimiento = fechaNacimiento,
            //        Conyugue = propietario?.Conyugue ?? "",
            //        Sexo = propietario?.Sexo ?? "",
            //        EsFisica = propietario?.EsFisica??""
            //    };

            //    ViewBag.Profesion = new SelectList(_dbContext.Profesiones, "CodProfesion", "Descripcion", propietario?.Profesion ?? "");
            //    ViewBag.CodPais = new SelectList(_dbContext.Paises, "CodPais", "Nacionalidad", propietario?.CodPais ?? "");
            //    ViewBag.EstadoCivil = new SelectList(_dbContext.EstadosCiviles, "CodEstadoCivil", "Descripcion", propietario?.CodEstadoCivil ?? "");
            //    return View("EditPersona", persona);
            //}
           
          
        }

        [HttpPost]
        public async Task<IActionResult> DatosPersona(Persona persona)
        {
            try
            {
                var existingPersona = await _dbContext.Personas.FirstOrDefaultAsync(me => me.CodPersona == persona.CodPersona);
                if (existingPersona != null)
                {
                    //Se actualiza los datos
                    existingPersona.Nombre = persona.Nombre;
                    existingPersona.CodEstadoCivil = persona.CodEstadoCivil;
                    existingPersona.Profesion = persona.Profesion;
                    existingPersona.Sexo = persona.Sexo;
                    existingPersona.FecNacimiento = persona.FecNacimiento;
                    existingPersona.Conyugue = persona.Conyugue;
                    existingPersona.CodPais = persona.CodPais;
                    existingPersona.EsFisica = persona.EsFisica;
                    //await _dbContext.Update(existingPersona);
                }
                else
                {
                     _dbContext.Add(persona);
                }

                 _dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar/actualizar : " + ex.Message);
                return BadRequest("Error al agregar/actualizar : " + ex.Message);
            }
        }



        public ActionResult EditDirPersona(string idPersona)
        {
            DirecPersona? direccion = _dbContext.DirecPersonas.AsQueryable().FirstOrDefault(p => p.CodPersona == idPersona);
            ViewData["CodPais"] = new SelectList(_dbContext.Paises, "CodPais", "Descripcion", direccion?.CodPais ?? "");
            ViewData["CodProvincia"] = new SelectList(_dbContext.Provincias, "CodProvincia", "Descripcion", direccion?.CodProvincia ?? "");
            ViewData["CodCiudad"] = new SelectList(_dbContext.Ciudades, "CodCiudad", "Descripcion", direccion?.CodCiudad ?? "");
            ViewData["CodBarrio"] = new SelectList(_dbContext.Barrios, "CodBarrio", "Descripcion", direccion?.CodBarrio ?? "");
            ViewData["Show"] = true;

            return View("EditDirPersona", direccion ?? new DirecPersona());
        }



        [HttpPost]
        public async Task<IActionResult> DireccionPersona(DirecPersona direccion)
        {
            try
            {
                var existingDireccion = await _dbContext.DirecPersonas.FirstOrDefaultAsync(me => me.CodPersona == direccion.CodPersona);
                if (existingDireccion != null)
                {
                    //Se actualiza los datos
                    existingDireccion.TipDireccion = direccion.TipDireccion;
                    existingDireccion.CodBarrio = direccion.CodBarrio;
                    existingDireccion.CodPais = direccion.CodPais;
                    existingDireccion.CodCiudad = direccion.CodCiudad;
                    existingDireccion.CodProvincia = direccion.CodProvincia;
                    existingDireccion.CasillaCorreo = direccion.CasillaCorreo;
                    existingDireccion.CodigoPostal = direccion.CodigoPostal;
                    existingDireccion.DescripcionRef = direccion.DescripcionRef;
                    existingDireccion.Detalle = direccion.Detalle;
                    _dbContext.Update(existingDireccion);
                }
                else
                {
                    var Persona = await _dbContext.Personas.FirstOrDefaultAsync(me => me.CodPersona == direccion.CodPersona);
                    if (Persona != null)
                    {
                        _dbContext.Add(direccion);
                    }
                }

                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar/actualizar : " + ex.Message);
                return BadRequest("Error al agregar/actualizar : " + ex.Message);
            }
        }




        public async Task<IActionResult> Get(decimal nEntrada)
        {
            try
            {
                ViewBag.Operacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", "6");
                //ViewBag.Autorizante = new SelectList(_dbContext.RmAutorizantes, "IdAutorizante", "DescripAutorizante");
                ViewBag.Distrito = new SelectList(_dbContext.RmDistritos, "CodigoDistrito", "DescripDistrito");
                
               
                var rmMesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);
               
                if (rmMesaEntrada == null)
                {
                    return Json(new { Success = false, ErrorMessage = "El número de entrada no existe." });
                }
                var tipoSolicitud = rmMesaEntrada?.TipoSolicitud??0;
                var listaConceptos = new List<decimal> { 1, 5, 4};
                if (!listaConceptos.Contains(tipoSolicitud))
                {
                    return Json(new { Success = false, ErrorMessage = "Error, el tipo de solicitud no corresponde" });
                }
                if (rmMesaEntrada != null)
                {
                    var estadoRegistrador = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Registrador");
                    var rmMarcasSenale = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

                    var rmTransacciones = await _dbContext.RmTransacciones.OrderByDescending(t=>t.FechaAlta).FirstOrDefaultAsync(t=>t.NumeroEntrada== nEntrada);
                    if (rmMesaEntrada.TipoSolicitud == 1 || rmMesaEntrada.TipoSolicitud==4)
                    {
                        if (rmMarcasSenale == null)
                        {
                            var parametroMarca = await _dbContext.ParametrosGenerales.FirstOrDefaultAsync(m => m.Parametro == "DIR_IMGMARCA");
                            var parametroSenal = await _dbContext.ParametrosGenerales.FirstOrDefaultAsync(m => m.Parametro == "DIR_IMGSENAL");
                            var marcaSenale = _dbContext.RmMarcasSenales.Max(m => m.IdMarca) + 1;
                            RmMarcasSenale marcasSenales = new()
                            {
                                NumeroEntrada = nEntrada,
                                MarcaNombre = parametroMarca.Valor + nEntrada + ".bmp",
                                SenalNombre = parametroSenal.Valor + nEntrada + ".bmp",
                                IdMarca = marcaSenale
                            };
                            _dbContext.Add(marcasSenales);
                            _dbContext.SaveChanges();
                        }
                      
                    }

                    if (rmMesaEntrada.EstadoEntrada == estadoRegistrador.CodigoEstado && rmMesaEntrada.TipoSolicitud==5)
                    {
                        rmTransacciones = null;
                    }
                    if (rmTransacciones!=null)
                    {
                        
                        var rmReingreso = _dbContext.RmReingresos.FirstOrDefault(p => p.NroEntrada == nEntrada);
                        var estadoRevision = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "En Revisión");
                        var estadoRetiradoObs = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Retirado/Observado");
                        var estadoRetiradoNN = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Retirado/Nota negativa");
                        var estadoRetiradoAprob = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Retirado/Aprobado");
                        var administrador = _dbContext.Usuarios.FirstOrDefault(u => u.CodUsuario == User.Identity.Name);
                        var codGrupo = administrador?.CodGrupo ?? "";
                        if (codGrupo!="ADMIN")
                        {
                            if (rmMesaEntrada.Reingreso != "S" && estadoRevision.CodigoEstado != rmMesaEntrada.EstadoEntrada)
                            {
                                return Json(new { Success = false, ErrorMessage = "Error, el trabajo ya se encuentra resgistrado" });
                            }

                            if ((rmMesaEntrada.Reingreso == "S" && estadoRetiradoAprob.CodigoEstado.ToString() != rmTransacciones.EstadoTransaccion) &&
                                     (rmMesaEntrada.Reingreso == "S" && estadoRetiradoObs.CodigoEstado.ToString() != rmTransacciones.EstadoTransaccion) &&
                                     (rmMesaEntrada.Reingreso == "S" && estadoRetiradoNN.CodigoEstado.ToString() != rmTransacciones.EstadoTransaccion))
                            {
                                return Json(new { Success = false, ErrorMessage = "Error, el trabajo ya se encuentra resgistrado" });
                            }
                        }
                        
                        //if (estadoRegistrador.CodigoEstado != rmMesaEntrada.EstadoEntrada)
                        //{
                        //    return Json(new { Success = false, ErrorMessage = "Error, debe recepcionar el trabajo" });
                        //}
                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", rmMesaEntrada?.EstadoEntrada ?? 0);
                        var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == rmTransacciones.IdTransaccion);
                        var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(p => p.IdAutorizante == rmTransacciones.IdAutorizante);
                        //var marcasSenales = _dbContext.RmMarcasSenales.FirstOrDefault(m => m.NumeroEntrada == nEntrada);
                        var boletaMS = rmMarcasSenale?.NroBoleta??"0";
                        var nroBoleta = rmTransacciones?.NroBoleta ?? boletaMS;
                        var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.NroBoleta == Convert.ToInt64(nroBoleta));
                        var DescBoleta = boletaMarca?.Descripcion ?? "";
                        var nroBoleMarca = boletaMarca?.NroBoleta ?? 0;



                        InscripcionCustom inscripcion = new()
                        {
                            NroEntrada = nEntrada,
                            IdTransaccion = rmTransacciones?.IdTransaccion ?? 0,
                            EstadoEntrada = rmMesaEntrada?.EstadoEntrada ?? 0,
                            CantidadGanado = rmMarcasSenale?.CantidadGanado ?? 0,
                            NroBoleta = rmMesaEntrada?.NroBoleta?? DescBoleta,
                            NroBolMarcaAnt = rmTransacciones?.NroBolMarcaAnt ?? "",
                            NroBolSenhalAnt = rmTransacciones?.NroBolSenhalAnt ?? "",
                            TipoOperacion = rmTransacciones?.TipoOperacion ?? "",
                            Operacion = await GetTiposOperacionesAsync(),
                            FechaActoJuridico = rmTransacciones?.FechaActoJuridico,
                            NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                            //Autorizante = await GetAutorizanteAsync(),
                            Serie = rmTransacciones?.Serie??"",
                            FormaConcurre = rmTransacciones?.FormaConcurre??"",
                            Representante = rmTransacciones?.Representante??"",
                            NombreRepresentante = rmTransacciones?.NombreRepresentante??"",
                            FirmanteRuego = rmTransacciones?.FirmanteRuego??"",
                            CodDistrito = rmMarcasXEstab == null ? "" : rmMarcasXEstab.CodDistrito,
                            Departamento = rmMarcasXEstab?.Descripcion ?? "",
                            Distrito = await GetDistritosAsync(),
                            //Departamento = departamento?.Descripcion ?? "",
                            GpsH = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsH,
                            GpsS = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsS,
                            GpsSc = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsSc,
                            GpsV = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsV,
                            GpsW = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsW,
                            Comentario = rmTransacciones?.Comentario??""
                        };
                        return View("Index", inscripcion);
                    }
                    else
                    {
                        if (estadoRegistrador.CodigoEstado != rmMesaEntrada.EstadoEntrada)
                        {
                            return Json(new { Success = false, ErrorMessage = "Error, debe recepcionar el trabajo" });
                        }

                        var boletaMarca = await _dbContext.RmBoletasMarcas.Where(p => p.Descripcion == rmMesaEntrada.NroBoleta).FirstOrDefaultAsync();
                        if (boletaMarca != null)
                        {
                            var nroBoleta = boletaMarca?.NroBoleta ?? 0;
                            var marcaSenales = _dbContext.RmMarcasSenales.FirstOrDefault(m=>m.NroBoleta== nroBoleta.ToString());
                            var nroEntrada = marcaSenales?.NumeroEntrada;
                            var transaccion = _dbContext.RmTransacciones.OrderByDescending(o=>o.FechaAlta).FirstOrDefault(t=>t.NumeroEntrada== nroEntrada);
                            //var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.NroBoleta == Convert.ToInt64(nroBoleta));
                            ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", rmMesaEntrada?.EstadoEntrada ?? 0);
                            var idTransaccion = transaccion?.IdTransaccion??0;
                            var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == idTransaccion);

                            var idAutorizante = transaccion?.IdAutorizante??0;
                            var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(p => p.IdAutorizante == idAutorizante);
                            ViewBag.Operacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", "5");
                            // cargar para edicion
                            InscripcionCustom inscripcion = new()
                            {
                                NroEntrada = nEntrada,
                                IdTransaccion = transaccion?.IdTransaccion,
                                EstadoEntrada = rmMesaEntrada?.EstadoEntrada ?? 0,
                                CantidadGanado = rmMarcasSenale?.CantidadGanado ?? 0,
                                NroBoleta = rmMesaEntrada?.NroBoleta ?? "",
                                NroBolMarcaAnt = transaccion?.NroBolMarcaAnt ?? "",
                                NroBolSenhalAnt = transaccion?.NroBolSenhalAnt ?? "",
                                //TipoOperacion = transaccion?.TipoOperacion ?? "",
                                Operacion = await GetTiposOperacionesAsync(),
                                FechaActoJuridico = transaccion?.FechaActoJuridico,
                                NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                                MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                                //Autorizante = await GetAutorizanteAsync(),
                                Serie = transaccion?.Serie,
                                FormaConcurre = transaccion?.FormaConcurre,
                                Representante = transaccion?.Representante,
                                FirmanteRuego = transaccion?.FirmanteRuego,
                                CodDistrito = rmMarcasXEstab == null ? "" : rmMarcasXEstab.CodDistrito,
                                Departamento = rmMarcasXEstab?.Descripcion ?? "",
                                Distrito = await GetDistritosAsync(),
                                //Departamento = departamento?.Descripcion ?? "",
                                GpsH = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsH,
                                GpsS = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsS,
                                GpsSc = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsSc,
                                GpsV = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsV,
                                GpsW = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsW
                            };
                            return View("Index", inscripcion);
                        }
                        else
                        {
                            // cargar nuevo

                            //var tipoOperacion = await _dbContext.RmTiposOperaciones.FirstOrDefaultAsync(p => p.DescripTipoOperacion == "INSCRIPCION");
                            //var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(a=>a.IdAutorizante == rmMesaEntrada.IdAutorizante);
                            InscripcionCustom inscripcion = new();
                            ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", rmMesaEntrada?.EstadoEntrada ?? 0);
                            inscripcion.NroEntrada = rmMesaEntrada?.NumeroEntrada ?? 0;
                            //inscripcion.Representante = rmMesaEntrada?.NroDocumentoPresentador??"";
                            //inscripcion.NombreRepresentante = rmMesaEntrada?.NombrePresentador??"";
                            //inscripcion.MatriculaAutorizante = autorizante.MatriculaRegistro;
                            //inscripcion.NombreAutorizante = autorizante.DescripAutorizante;
                            //inscripcion.TipoOperacion = tipoOperacion.TipoOperacion.ToString();
                            inscripcion.EstadoEntrada = rmMesaEntrada?.EstadoEntrada;
                            //inscripcion.CantidadGanado = rmMarcasSenale != null ? rmMarcasSenale.CantidadGanado ?? 0 : 0;

                            return View("Index", inscripcion);
                            //return View("Index");
                        }

                    }


                }
                return Ok();
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
                var rmMesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

                if (rmMesaEntrada != null)
                {
                    var rmMarcasSenale = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

                    var rmTransacciones = await _dbContext.RmTransacciones.OrderByDescending(t => t.FechaAlta).FirstOrDefaultAsync(t => t.NumeroEntrada == nEntrada);

                    if (rmTransacciones != null)
                    {


                        ViewBag.Operacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", rmTransacciones?.TipoOperacion ?? "");
                        ViewBag.Estado = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", rmMesaEntrada?.EstadoEntrada ?? 0);
                        var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(p => p.IdTransaccion == rmTransacciones.IdTransaccion);
                        var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(p => p.IdAutorizante == rmTransacciones.IdAutorizante);
                        ViewBag.Distrito = new SelectList(_dbContext.RmDistritos, "CodigoDistrito", "DescripDistrito", rmMarcasXEstab?.CodDistrito ?? "");
                        //var marcasSenales = _dbContext.RmMarcasSenales.FirstOrDefault(m => m.NumeroEntrada == nEntrada);
                        var boletaMS = rmMarcasSenale?.NroBoleta ?? "0";
                        var nroBoleta = rmTransacciones?.NroBoleta ?? boletaMS;
                        var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.NroBoleta == Convert.ToInt64(nroBoleta));
                        var DescBoleta = boletaMarca?.Descripcion ?? "";
                        var nroBoleMarca = boletaMarca?.NroBoleta ?? 0;
                        InscripcionCustom inscripcion = new()
                        {
                            NroEntrada = nEntrada,
                            IdTransaccion = rmTransacciones?.IdTransaccion ?? 0,
                            EstadoEntrada = rmMesaEntrada?.EstadoEntrada ?? 0,
                            CantidadGanado = rmMarcasSenale?.CantidadGanado ?? 0,
                            NroBoleta = rmMesaEntrada?.NroBoleta ?? DescBoleta,
                            NroBolMarcaAnt = rmTransacciones?.NroBolMarcaAnt ?? "",
                            NroBolSenhalAnt = rmTransacciones?.NroBolSenhalAnt ?? "",
                            TipoOperacion = rmTransacciones?.TipoOperacion ?? "",
                            FechaActoJuridico = rmTransacciones?.FechaActoJuridico,
                            NombreAutorizante = autorizante?.DescripAutorizante ?? "",
                            MatriculaAutorizante = autorizante?.MatriculaRegistro ?? 0,
                            Serie = rmTransacciones?.Serie ?? "",
                            FormaConcurre = rmTransacciones?.FormaConcurre ?? "",
                            Representante = rmTransacciones?.Representante ?? "",
                            NombreRepresentante = rmTransacciones?.NombreRepresentante ?? "",
                            FirmanteRuego = rmTransacciones?.FirmanteRuego ?? "",
                            CodDistrito = rmMarcasXEstab == null ? "" : rmMarcasXEstab.CodDistrito,
                            Departamento = rmMarcasXEstab?.Descripcion ?? "",
                            GpsH = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsH,
                            GpsS = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsS,
                            GpsSc = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsSc,
                            GpsV = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsV,
                            GpsW = rmMarcasXEstab == null ? "" : rmMarcasXEstab.GpsW,
                            Comentario = rmTransacciones?.Comentario ?? ""
                        };
                        return View("Index", inscripcion);

                    }

                    return Ok();

                }
                return Ok();

            }

            catch (Exception ex)
            {
                // Manejar el error de generación de PDF de alguna manera
                return BadRequest("Error al cargar la pagina " + ex.Message);
            }



        }
        public async Task<JsonResult> CargarTitulares(int idTransaccion)
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


        public async Task<IActionResult> Get2(decimal nEntrada)
        {

            var rmMarcasSenale = await _dbContext.RmMarcasSenales.SingleOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

            var transaccion = await _dbContext.RmTransacciones.SingleOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

            var rmMesaEntrada = await _dbContext.RmMesaEntrada.SingleOrDefaultAsync(p => p.NumeroEntrada == nEntrada);

            var rmMarcasXEstab = await _dbContext.RmMarcasXEstabs.SingleOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);

            var rmTitularesMarcas = await _dbContext.RmTitularesMarcas.SingleOrDefaultAsync(p => p.IdTransaccion == transaccion.IdTransaccion);

            if (rmMarcasSenale == null)
            {
                return NotFound();
            }
            if (rmMesaEntrada == null)
            {
                return NotFound();
            }
            if (transaccion == null)
            {
                ViewBag.EstadosEntrada = new SelectList(_dbContext.RmEstadosEntrada, "CodigoEstado", "DescripEstado", rmMesaEntrada.EstadoEntrada);

                InscripcionCustom inscripcion = new()
                {

                    NroEntrada = rmMesaEntrada.NumeroEntrada,
                    EstadoEntrada = rmMesaEntrada.EstadoEntrada,
                    CantidadGanado = rmMarcasSenale.CantidadGanado
                };
                return View("Index", inscripcion); ;
            }
            else
            {

                ViewBag.Operacion = new SelectList(_dbContext.RmTiposOperaciones, "TipoOperacion", "DescripTipoOperacion", transaccion.TipoOperacion);
                ViewBag.Autorizante = new SelectList(_dbContext.RmAutorizantes, "IdAutorizante", "DescripAutorizante", transaccion.IdAutorizante);
                ViewBag.Distrito = new SelectList(_dbContext.RmDistritos, "CodigoDistrito", "DescripDistrito", rmMarcasXEstab.CodDistrito);

                InscripcionCustom inscripcion = new()
                {
                    NroEntrada = transaccion.NumeroEntrada,
                    IdTransaccion = transaccion.IdTransaccion,
                    EstadoEntrada = rmMesaEntrada.EstadoEntrada,
                    CantidadGanado = rmMarcasSenale.CantidadGanado,
                    NroBolMarcaAnt = transaccion.NroBolMarcaAnt,
                    NroBolSenhalAnt = transaccion.NroBolSenhalAnt,
                    TipoOperacion = transaccion.TipoOperacion,
                    Operacion = await GetTiposOperacionesAsync(),
                    FechaActoJuridico = transaccion.FechaActoJuridico,
                    IdAutorizante = transaccion.IdAutorizante,
                    Autorizante = await GetAutorizanteAsync(),
                    Serie = transaccion.Serie,
                    FormaConcurre = transaccion.FormaConcurre,
                    Representante = transaccion.Representante,
                    FirmanteRuego = transaccion.FirmanteRuego,
                    CodDistrito = rmMarcasXEstab.CodDistrito,
                    Distrito = await GetDistritosAsync(),
                    GpsH = rmMarcasXEstab.GpsH,
                    GpsS = rmMarcasXEstab.GpsS,
                    GpsSc = rmMarcasXEstab.GpsSc,
                    GpsV = rmMarcasXEstab.GpsV,
                    GpsW = rmMarcasXEstab.GpsW

                };
                return View("Index", inscripcion);
            }

        }

        public IActionResult GetDepartamento(string CodDistrito)
        {
            if (CodDistrito!=null)
            {
                var distrito = _dbContext.RmDistritos.FirstOrDefault(p => p.CodigoDistrito.ToString() == CodDistrito);
                var codDepto = distrito?.CodigoDepto;
                var departamento = _dbContext.AvDepartamentos.FirstOrDefault(d => d.CodDepartamento == codDepto.ToString());

                if (departamento != null)
                {
                    return Json(new { Success = true, nombre = departamento.Descripcion });
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

        [HttpPost]
        public ActionResult AgregarTitular([FromBody] PersonaDireccion model)
        {
            if (model.IdPropietario != null)
            {
                var persona = _dbContext.Personas.FirstOrDefault(p=>p.CodPersona==model.IdPropietario);
           
                if (persona == null)
                {
                    Persona personas = new()
                    {
                        Nombre = model.Nombre,
                        //FecNacimiento = fechaNacimiento,
                        CodPersona = model.IdPropietario,
                        FecAlta = DateTime.Now
                    };
                    _dbContext.Add(personas);
                    DirecPersona direcPersona = new()
                    {
                        CodPersona = model.IdPropietario,
                        CodDireccion = "1"
                    };
                    _dbContext.Add(direcPersona);

                    TelefPersona telefPersona = new()
                    {
                        CodPersona = model.IdPropietario,
                        CodigoArea = "1",
                        NumTelefono = ""
                    };
                    _dbContext.Add(telefPersona);

                    if (model.IdPropietario.Any(c => !char.IsDigit(c)))
                    {

                        IdentPersona identPersona = new()
                        {
                            CodPersona = model.IdPropietario,
                            Numero = model.IdPropietario,
                            CodIdent = "RUC"
                        };
                    }
                    else
                    {
                        IdentPersona identPersona = new()
                        {
                            CodPersona = model.IdPropietario,
                            Numero = model.IdPropietario,
                            CodIdent = "CI"
                        };
                    }


                    _dbContext.SaveChanges();
                }
                else
                {
                    persona.Nombre = model.Nombre;
                    _dbContext.SaveChanges();
                }
            }
           
           
            // Devuelve una respuesta al cliente
            return Json(new { success = true, message = "Titular agregado correctamente" });
        }

        public IActionResult GetNombreActual(string codPersona)
        {
            if (codPersona != null)
            {
                    var result = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == codPersona);
                    
                    if (result != null)
                    {
                        return Json(new { Success = true, Nombre = result.Nombre });
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

        public IActionResult GetNombreByCodPersona(string codPersona)
        {
            //  var codigo = Int32.Parse(codPersona);
            //LLAMADA AL PROCEDIMIENTO ALMACENADO EN OTRA BASE DE DATOS.
            //SqlParameter parametro1 = new SqlParameter("@nro_documento", SqlDbType.VarChar);
            //parametro1.Value = codPersona; // Establecer el valor del parámetro de cadena
            //var parametro1 = new SqlParameter("@nro_documento", codPersona);
            //var results = _dbIdentContext.ConsultaPorCedula.FromSqlRaw("EXEC ConsultaPorCedula @nro_documento", parametro1.Value).ToList();
            if (codPersona!=null)
            {
                string nombres = "";
                string apellidos = "";
                string NombreApellido = "";

                ////PARA PRUEBAS FUERA DEL WS
                //var persona = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == codPersona);
                //if (persona != null)
                //{

                //    return Json(new { Success = true, Nombre = persona.Nombre });
                //}
                //else
                //{
                //    return Json(new { Success = false, ErrorMessage = "Error" });
                //}

                DateTime fechaNacimiento = DateTime.MinValue;
                if (codPersona.Any(c => !char.IsDigit(c)))
                {
                    var parametroNroDocumento1 = new SqlParameter("@CONDICION", SqlDbType.VarChar)
                    {
                        Value = codPersona
                    };
                    var results = _dbIdentContext.ConsultaPorRUC
                        .FromSqlRaw("EXEC BuscarRUC @CONDICION", parametroNroDocumento1)
                        .ToList();
                    var result = results.SingleOrDefault();
                    nombres = result?.nombre ?? "";

                    var persona = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == codPersona);
                  
                    if (persona == null)
                    {
                        Persona personas = new()
                        {
                            Nombre = nombres.Trim(),
                            //FecNacimiento = fechaNacimiento,
                            CodPersona = codPersona,
                            FecAlta = DateTime.Now
                        };
                        _dbContext.Add(personas);
                        _dbContext.SaveChanges();
                       
                    }
                    else
                    {
                        persona.FecNacimiento = fechaNacimiento;
                        persona.FecActualizacion = DateTime.Now;
                        _dbContext.Update(persona);
                        _dbContext.SaveChanges();
                    }
                    var direccion = _dbContext.DirecPersonas.FirstOrDefault(d => d.CodPersona == codPersona);
                    if (direccion == null)
                    {
                        DirecPersona direcPersona = new()
                        {
                            CodPersona = codPersona,
                            CodDireccion = "1"
                        };
                        _dbContext.Add(direcPersona);
                    }
                    var teelfono = _dbContext.TelefPersonas.FirstOrDefault(t => t.CodPersona == codPersona);
                    if (teelfono == null)
                    {
                        TelefPersona telefPersona = new()
                        {
                            CodPersona = codPersona,
                            CodigoArea = "1",
                            NumTelefono = ""
                        };
                        _dbContext.Add(telefPersona);
                    }
                    var identPe = _dbContext.IdentPersonas.FirstOrDefault(i => i.CodPersona == codPersona);
                    if (identPe == null)
                    {

                        IdentPersona identPersona = new()
                        {
                            CodPersona = codPersona,
                            Numero = codPersona,
                            CodIdent = "RUC"
                        };
                        _dbContext.Add(identPersona);
                    }
                    _dbContext.SaveChanges();

                    var newPersona = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == codPersona);

                    if (newPersona != null)
                    {
                        NombreApellido = newPersona?.Nombre ?? "";
                        return Json(new { Success = true, Nombre = NombreApellido });
                    }
                    else
                    {
                        return Json(new { Success = false, ErrorMessage = "Error" });
                    }
                }
                else
                {
                    var parametroNroDocumento = new SqlParameter("@nro_documento", SqlDbType.VarChar)
                    {
                        Value = codPersona
                    };
                    var results2 = _dbIdentContext.ConsultaPorCedula
                    .FromSqlRaw("EXEC ConsultaPorCedula @nro_documento", parametroNroDocumento)
                    .ToList();

                    var result2 = results2.SingleOrDefault();
                    nombres = result2?.Nombres ?? "";
                    apellidos = result2?.Apellidos ?? "";
                    fechaNacimiento = result2?.FechaNacimiento ?? DateTime.MinValue;

                    var persona = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == codPersona);
                
                    if (persona == null)
                    {
                        Persona personas = new()
                        {
                            Nombre = nombres.Trim() + " " + apellidos.Trim(),
                            FecNacimiento = fechaNacimiento,
                            CodPersona = codPersona,
                            FecAlta = DateTime.Now
                        };
                        _dbContext.Add(personas);
                        _dbContext.SaveChanges();
                        
                    }
                    else
                    {
                       
                        persona.FecNacimiento = fechaNacimiento;
                        //// persona.Nombre = nombres.Trim() + " " + apellidos.Trim();
                        persona.FecActualizacion = DateTime.Now;
                        _dbContext.Update(persona);
                        _dbContext.SaveChanges();
                    }
                    var direccion = _dbContext.DirecPersonas.FirstOrDefault(d => d.CodPersona == codPersona);
                    if (direccion == null)
                    {
                        DirecPersona direcPersona = new()
                        {
                            CodPersona = codPersona,
                            CodDireccion = "1"
                        };
                        _dbContext.Add(direcPersona);
                    }
                    var teelfono = _dbContext.TelefPersonas.FirstOrDefault(t => t.CodPersona == codPersona);
                    if (teelfono == null)
                    {
                        TelefPersona telefPersona = new()
                        {
                            CodPersona = codPersona,
                            CodigoArea = "1",
                            NumTelefono = ""
                        };
                        _dbContext.Add(telefPersona);
                    }
                    var identPe = _dbContext.IdentPersonas.FirstOrDefault(i => i.CodPersona == codPersona);
                    if (identPe == null)
                    {

                        IdentPersona identPersona = new()
                        {
                            CodPersona = codPersona,
                            Numero = codPersona,
                            CodIdent = "CI"
                        };
                        _dbContext.Add(identPersona);
                    }

                    _dbContext.SaveChanges();

                    var newPersona = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == codPersona);
                   
                    if (newPersona != null)
                    {
                        NombreApellido = newPersona?.Nombre??"";
                        return Json(new { Success = true, Nombre = NombreApellido });
                    }
                    else
                    {
                        return Json(new { Success = false, ErrorMessage = "Error" });
                    }
                }

            }
            else
            {
                return Json(new { Success = false, ErrorMessage = "Error" });
            }
                
    
        }

        public IActionResult GetImagenMarca(string nroEntrada)
        {
            if (nroEntrada!=null)
            {
                decimal nEntrada = Decimal.Parse(nroEntrada);
                var result = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NumeroEntrada.Equals(nEntrada));
                var parametroMarca =  _dbContext.ParametrosGenerales.FirstOrDefault(m => m.Parametro == "DIR_IMGMARCA");
                var parametroSenal =  _dbContext.ParametrosGenerales.FirstOrDefault(m => m.Parametro == "DIR_IMGSENAL");

                var marcaNA = _dbContext.ParametrosGenerales.FirstOrDefault(m => m.Parametro == "DIR_IMGMARCA_NA");
                var senalNA = _dbContext.ParametrosGenerales.FirstOrDefault(m => m.Parametro == "DIR_IMGSENAL_NA");

                if (result != null)
                {
                    try
                    {
                        string imagePath = result.MarcaNombre; // Ruta a la imagen en tu proyecto

                        if (result.MarcaNombre.Contains("No Aplicable")) {

                            result.MarcaNombre = parametroMarca.Valor + nroEntrada + ".bmp";
                            _dbContext.Update(result);
                            _dbContext.SaveChanges();
                            // En caso de que la imagen no se encuentre en la ruta, cambia la ruta a una alternativa
                            imagePath = result.MarcaNombre;
                        }

                        string imageDataUri;
                        if (System.IO.File.Exists(imagePath))
                        {
                            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                             imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                        }
                        else
                        {
                            result.MarcaNombre = marcaNA.Valor;
                            _dbContext.Update(result);
                            _dbContext.SaveChanges();
                            // En caso de que la imagen no se encuentre en la ruta, cambia la ruta a una alternativa
                            imagePath = result.MarcaNombre;
                            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                             imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                        }

                        string imagePath2 = result.SenalNombre; // Ruta a la imagen en tu proyecto
                        if (result.SenalNombre.Contains("No Aplicable"))
                        {
                            result.SenalNombre = parametroSenal.Valor + nroEntrada + ".bmp";
                            _dbContext.Update(result);
                            _dbContext.SaveChanges();
                            // En caso de que la imagen no se encuentre en la ruta, cambia la ruta a una alternativa
                            imagePath2 = result.SenalNombre;
                        }
                        string imageDataUri2;
                        if (System.IO.File.Exists(imagePath2))
                        {
                            byte[] imageBytes2 = System.IO.File.ReadAllBytes(imagePath2);
                             imageDataUri2 = "data:image/png;base64," + Convert.ToBase64String(imageBytes2);
                        }
                        else
                        {
                            result.SenalNombre = senalNA.Valor;
                            _dbContext.Update(result);
                            _dbContext.SaveChanges();
                            // En caso de que la imagen no se encuentre en la ruta, cambia la ruta a una alternativa
                            imagePath2 = result.SenalNombre;
                            byte[] imageBytes2 = System.IO.File.ReadAllBytes(imagePath2);
                             imageDataUri2 = "data:image/png;base64," + Convert.ToBase64String(imageBytes2);
                        }

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

        private async Task<List<SelectListItem>> GetTiposOperacionesAsync()
        {
            using var dbContext = new DbvinDbContext();

            var operacion = await dbContext.RmTiposOperaciones
            .Select(o => new SelectListItem
            {
                Text = $"{o.TipoOperacion}-{o.DescripTipoOperacion}",
                Value = o.TipoOperacion.ToString()
            })
            .ToListAsync();

            return operacion;
        }
        //private async Task<List<TitularMarcaViewModel>> GetTitularesMarcasAsync()
        //{
        //    using var dbContext = new DbvinDbContext();

        //    var operacion = await (from tm in dbContext.RmTitularesMarcas
        //                           join p in dbContext.Personas on tm.IdPropietario equals p.CodPersona
        //                           select new  TitularMarcaViewModel
        //                            {
        //                                IdPropietario = tm.IdPropietario,
        //                                Nombre = p.Nombre,
        //                                EsPropietario = tm.EsPropietario
        //                            }).ToListAsync();

        //    return operacion;
        //}

        private async Task<List<SelectListItem>> GetAutorizanteAsync()
        {
            using var dbContext = new DbvinDbContext();
            var autorizante = await dbContext.RmAutorizantes
            .Select(t => new SelectListItem
            {
                Text = $"{t.IdAutorizante}-{t.DescripAutorizante}",
                Value = t.IdAutorizante.ToString()
            })
            .ToListAsync();

            return autorizante;

        }
        private async Task<List<SelectListItem>> GetRepresentanteAsync()
        {
            using var dbContext = new DbvinDbContext();

            var representante = await (from tm in dbContext.RmTransacciones
                                       join p in dbContext.Personas on tm.Representante equals p.CodPersona
                                       select new SelectListItem()
                                       {
                                           Text = tm.Representante + " " + p.Nombre,
                                           Value = tm.Representante
                                       }).ToListAsync();

            return representante;
        }
        private async Task<List<SelectListItem>> GetDistritosAsync()
        {
            using var dbContext = new DbvinDbContext();
            var distritos = await dbContext.RmDistritos
            .Select(d => new SelectListItem
            {
                Text = $"{d.CodigoDistrito}-{d.DescripDistrito}",
                Value = d.CodigoDistrito.ToString()
            })
            .ToListAsync();

            return distritos;
        }


        [HttpPost]
        //[ValidateAntiForgeryToken] 
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMINSCR", "Edit", "InscripcionCustom" })]

        public async Task<IActionResult> Edit(decimal NumeroEntrada, InscripcionCustom inscripcion)
        {
            try
            {
                var updateTransaccion = await _dbContext.RmTransacciones.FindAsync(NumeroEntrada);

                var updateMarxEst = await _dbContext.RmMarcasXEstabs.FindAsync(updateTransaccion.IdTransaccion);

                var updateTitulares = await _dbContext.RmTitularesMarcas.FindAsync(updateTransaccion.IdTransaccion);

                var updateMesaEntrada = await _dbContext.RmMesaEntrada.FindAsync(NumeroEntrada);

                updateMesaEntrada.EstadoEntrada = 2;

                updateTransaccion.NroBolMarcaAnt = inscripcion.NroBolMarcaAnt;
                updateTransaccion.NroBolSenhalAnt = inscripcion.NroBolSenhalAnt;
                updateTransaccion.TipoOperacion = inscripcion.TipoOperacion;
                updateTransaccion.FechaActoJuridico = inscripcion.FechaActoJuridico;
                updateTransaccion.IdAutorizante = inscripcion.IdAutorizante;
                updateTransaccion.Serie = inscripcion.Serie;
                updateTransaccion.FormaConcurre = inscripcion.FormaConcurre;
                updateTransaccion.Representante = inscripcion.Representante;
                updateTransaccion.FirmanteRuego = inscripcion.FirmanteRuego;
                updateTransaccion.Comentario = inscripcion.Comentario;
                updateTransaccion.Asiento = inscripcion.Asiento;
                updateMarxEst.CodDistrito = inscripcion.CodDistrito;
                updateMarxEst.Descripcion = inscripcion.Descripcion;
                updateMarxEst.GpsS = inscripcion.GpsS;
                updateMarxEst.GpsV = inscripcion.GpsV;
                updateMarxEst.GpsSc = inscripcion.GpsSc;
                updateMarxEst.GpsW = inscripcion.GpsW;
                updateMarxEst.GpsH = inscripcion.GpsH;
                //updateTitulares.IdPropietario = inscripcion.IdPropietario;
                //updateTitulares.EsPropietario = inscripcion.EsPropietario;
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RmTransaccionExists(inscripcion.NroEntrada))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        //[ValidateAntiForgeryToken] 
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMINSCR", "Create", "InscripcionCustom" })]

        public async Task<IActionResult> Create([FromBody] InscripcionCustom inscripcion)
        {
            try
            {

                var rmMesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(t => t.NumeroEntrada == inscripcion.NroEntrada);
                var rmTransaccion = await _dbContext.RmTransacciones.OrderByDescending(t=>t.FechaAlta).FirstOrDefaultAsync(t => t.NumeroEntrada == inscripcion.NroEntrada);
               
                var idAuto = inscripcion?.NombreAutorizante??"";
                var autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(a => a.DescripAutorizante == idAuto);
                if (autorizante == null && idAuto !="")
                {
                    RmAutorizante rmAutorizante = new()
                    {
                        DescripAutorizante = idAuto
                    };
                    _dbContext.Add(rmAutorizante);
                    await _dbContext.SaveChangesAsync();
                    autorizante = await _dbContext.RmAutorizantes.FirstOrDefaultAsync(a => a.DescripAutorizante == idAuto);
                }
                var Boleta = inscripcion?.NroBoleta??"";
                var boletaMarca = await _dbContext.RmBoletasMarcas.FirstOrDefaultAsync(b => b.Descripcion == Boleta);
                var nroBoleta = boletaMarca?.NroBoleta ?? 0;
                if (rmTransaccion != null)
                {
                    if (rmMesaEntrada != null)
                    {
                        var rmMarcasSenale = _dbContext.RmMarcasSenales.FirstOrDefault(m => m.NumeroEntrada == rmTransaccion.NumeroEntrada);
                        //if (inscripcion.TipoOperacion=="6" || inscripcion.TipoOperacion=="15" )
                        //{
                            if (rmMarcasSenale != null)
                            {
                                if (rmMarcasSenale.NroBoleta==null)
                                {
                                    if (rmTransaccion.NroBoleta!=null )
                                    {
                                        rmMarcasSenale.NroBoleta = rmTransaccion.NroBoleta;
                                    }
                                    else
                                    {
                                        rmMarcasSenale.NroBoleta = nroBoleta.ToString();
                                    }
                                   
                                }
                                if (rmMarcasSenale.FechaAlta == null)
                                {
                                    rmMarcasSenale.FechaAlta = DateTime.Now;
                                }
                                if (rmMarcasSenale.IdUsuario == null)
                                {
                                    rmMarcasSenale.IdUsuario = User.Identity.Name;
                                }
                                _dbContext.Update(rmMarcasSenale);
                            }
                            rmTransaccion.IdMarca = rmMarcasSenale?.IdMarca;
                        //}
                        if (nroBoleta!=0)
                        {
                            rmTransaccion.NroBoleta = nroBoleta.ToString();
                        }
                           
                          
                            //rmTransaccion.NumeroEntrada = inscripcion.NroEntrada;
                            rmTransaccion.Asiento = inscripcion.Asiento;
                            rmTransaccion.Comentario = inscripcion.Comentario;
                            rmTransaccion.TipoOperacion = inscripcion.TipoOperacion;
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
                            rmTransaccion.FirmanteRuego = inscripcion.FirmanteRuego;
                            rmTransaccion.NombreRepresentante = inscripcion.NombreRepresentante;
                            if(inscripcion.Titulares != null)
                            {
                                var titulares = inscripcion.Titulares.FirstOrDefault();
                                var persona = _dbContext.Personas.FirstOrDefault(p=>p.CodPersona== titulares.IdPropietario);
                                rmTransaccion.CodEstadoCivilTitular = persona?.CodEstadoCivil;
                                rmTransaccion.CodProfesionTitular = persona?.Profesion;
                                var direccion = _dbContext.DirecPersonas.FirstOrDefault(d=>d.CodPersona== titulares.IdPropietario);
                                rmTransaccion.DireccionTitular = direccion?.Detalle??"";
                            }
                            if (inscripcion.Representante != null)
                            {
                                var persona = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == inscripcion.Representante);
                                rmTransaccion.CodProfesionApoderado = persona?.Profesion;
                                rmTransaccion.CodEstadoCivilApoderado = persona?.CodEstadoCivil;
                                var direccion = _dbContext.DirecPersonas.FirstOrDefault(d => d.CodPersona == inscripcion.Representante);
                                rmTransaccion.DireccionApoderado = direccion?.Detalle??"";
                            }
                            //rmTransaccion.CodEstadoCivilTitular = rmMesaEntrada?.NroDocumentoTitular;
                            _dbContext.Update(rmTransaccion);
                            _dbContext.SaveChanges();
                        ///Se guarda la inscripcion
                       
                        var rmMarcasxEstable = await _dbContext.RmMarcasXEstabs.FirstOrDefaultAsync(t => t.IdTransaccion == rmTransaccion.IdTransaccion);
                        if (rmMarcasxEstable == null)
                        {
                            RmMarcasXEstab ubicacion = new()
                            {
                                CodDistrito = inscripcion.CodDistrito,
                                IdTransaccion = rmTransaccion.IdTransaccion,
                                Descripcion = inscripcion.Departamento,
                                GpsH = inscripcion.GpsH,
                                GpsS = inscripcion.GpsS,
                                GpsSc = inscripcion.GpsSc,
                                GpsV = inscripcion.GpsV,
                                GpsW = inscripcion.GpsW,
                                IdMarca = rmMarcasSenale?.IdMarca

                            };

                            await _dbContext.AddAsync(ubicacion);
                        }
                        else
                        {
                            rmMarcasxEstable.CodDistrito = inscripcion.CodDistrito;
                            rmMarcasxEstable.IdTransaccion = rmTransaccion.IdTransaccion;
                            rmMarcasxEstable.Descripcion = inscripcion.Departamento;
                            rmMarcasxEstable.GpsH = inscripcion.GpsH;
                            rmMarcasxEstable.GpsS = inscripcion.GpsS;
                            rmMarcasxEstable.GpsSc = inscripcion.GpsSc;
                            rmMarcasxEstable.GpsV = inscripcion.GpsV;
                            rmMarcasxEstable.GpsW = inscripcion.GpsW;
                           await _dbContext.SaveChangesAsync();
                        }

                        var rmTitularesMarca = _dbContext.RmTitularesMarcas.FirstOrDefault(t=>t.IdTransaccion==rmTransaccion.IdTransaccion);
                        if (rmTitularesMarca!=null)
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
                        
                        //List<RmTitularesMarca> titularesMarca = new();
                        foreach (var item in inscripcion.Titulares)
                        {
                            rmTitularesMarca = _dbContext.RmTitularesMarcas.FirstOrDefault(t=> t.IdTransaccion == rmTransaccion.IdTransaccion && t.IdTitular== item.IdPropietario);

                            if (rmTitularesMarca == null)
                            {
                                var propietario = await _dbContext.RmTiposPropiedads.FirstOrDefaultAsync(p => p.DescripcionTipoPropiedad == item.EsPropietario);
                                RmTitularesMarca titulares = new()
                                {
                                    IdTransaccion = rmTransaccion.IdTransaccion,
                                    IdPropietario = item.IdPropietario,
                                    IdTitular = item.IdPropietario,
                                    IdTipoPropiedad = Convert.ToInt32(propietario?.IdTipoPropiedad),
                                    FechaRegistro = rmTransaccion.FechaAlta,
                                    CodUsuario = rmTransaccion.IdUsuario,
                                    IdMarca = rmMarcasSenale?.IdMarca
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

        private bool RmTransaccionExists(decimal id)
        {
            return (_dbContext.RmTransacciones?.Any(e => e.NumeroEntrada == id)).GetValueOrDefault();
        }


        [HttpPost]
        public async Task<IActionResult> CambiarEstado(decimal id, string nuevoEstado, string comentario, string nroBoleta, string tipoOperacion)
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
                    var EntradaRevision = _dbContext.RmMesaEntrada.Where(m=>m.EstadoEntrada== enrevision.CodigoEstado).FirstOrDefault(m=>m.NumeroEntrada== id);
                    if (codGrupo == "ADMIN")
                    {
                        nroEntrada = id;
                    }
                    else
                    {
                        nroEntrada = EntradaRevision?.NumeroEntrada ?? 0;
                    }
                    var transaccion = await _dbContext.RmTransacciones.OrderByDescending(o=>o.FechaAlta).FirstOrDefaultAsync(p => p.NumeroEntrada == nroEntrada);
                    if (transaccion != null)
                    {

                        var marcaBoleta = await _dbContext.RmBoletasMarcas.FirstOrDefaultAsync(m => m.Descripcion == nroBoleta);
                        if (nuevoEstado!= "Aprobado/Registrador")
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
                                    //notasNegativas.IdUsuario = User.Identity.Name;
                                    //notasNegativas.FechaAlta = DateTime.Now;
                                    _dbContext.SaveChanges();
                                }

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
                          
                            if (tipoOperacion == "INSCRIPCION" || tipoOperacion=="6" || tipoOperacion == "DUPLICADO" || tipoOperacion =="5" )
                            { //Volver a habilitar el nro de boleta
                                if (transaccion.NroBoleta != null)
                                {
                                    var rmMarcaBoleta = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.NroBoleta.ToString() == transaccion.NroBoleta);
                                    if (rmMarcaBoleta.Asignado == "S")
                                    {
                                        rmMarcaBoleta.Asignado = null;
                                        _dbContext.Update(rmMarcaBoleta);
                                        await _dbContext.SaveChangesAsync();
                                    }
                                }
                                mesaEntradum.NroBoleta = null;
                                _dbContext.Update(mesaEntradum);
                                await _dbContext.SaveChangesAsync();
                            }
                            //transaccion.NumeroEntrada = inscripcion.NroEntrada;se mantiene
                            transaccion.Asiento = null;
                            transaccion.NroBoleta =null;
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
                            var MarcaSenal = _dbContext.RmMarcasSenales.FirstOrDefault(m=>m.NumeroEntrada== id);
                            if (MarcaSenal!=null) {
                                MarcaSenal.NroBoleta = null;
                                _dbContext.Update(MarcaSenal);
                            }
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

                            if (transaccion.NroBoleta == null && (nroBoleta==null || nroBoleta == ""))
                            {
                                var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(p => p.Asignado == null);
                                nroBoleta = boletaMarca.NroBoleta.ToString();
                                boletaMarca.Asignado = "S";
                                _dbContext.Update(boletaMarca);
                                await _dbContext.SaveChangesAsync();

                                transaccion.NroBoleta = nroBoleta;
                            }
                           
                            if (codGrupo!="ADMIN")
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
                        if (nuevoEstado == "Aprobado/Registrador")
                        {
                            if (nroBoleta == "" || nroBoleta== null)
                            {
                                var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(p => p.Asignado == null);
                                nroBoleta = boletaMarca.NroBoleta.ToString();

                                boletaMarca.Asignado = "S";
                                _dbContext.Update(boletaMarca);
                                _dbContext.SaveChanges();

                            }
                            else
                            {
                                var boleta = _dbContext.RmBoletasMarcas.FirstOrDefault(b=>b.Descripcion== nroBoleta);
                                nroBoleta = boleta.NroBoleta.ToString();
                            }


                        }
                        RmTransaccione bdTransaccion = new()
                        {
                            NumeroEntrada = id,
                            FechaAlta = DateTime.Now,
                            Observacion = comentario,
                            EstadoTransaccion = estadoEntrada.CodigoEstado.ToString(),
                            TipoOperacion = tipoOperacion,
                            IdUsuario = User.Identity.Name
                        };
                        if (nroBoleta != null && nroBoleta!="")
                        {
                            bdTransaccion.NroBoleta = nroBoleta;

                        }
                        await _dbContext.AddAsync(bdTransaccion);
                       
                        await _dbContext.SaveChangesAsync();

                       
                        if (codGrupo != "ADMIN")
                        {
                            mesaEntradum.EstadoEntrada = estadoEntrada.CodigoEstado;
                            //if (tipoOperacion == "6" || tipoOperacion == "INSCRIPCION")
                            //{
                            //    mesaEntradum.NroBoleta = nroBoleta;
                            //}
                            _dbContext.Update(mesaEntradum);
                        }
                      

                    }
                    if (codGrupo != "ADMIN")
                    {
                        mesaEntradum.EstadoEntrada = estadoEntrada.CodigoEstado;
                        //if (tipoOperacion == "6" || tipoOperacion == "INSCRIPCION")
                        //{
                        //    mesaEntradum.NroBoleta = nroBoleta;
                        //}
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


        public IActionResult GetNroBoleta(decimal TipoSolicitud)
        {
            var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(p => p.Asignado == null );

            if (TipoSolicitud == 6)
            {
                return Json(new { Success = true, NroBoleta = boletaMarca.Descripcion });
            }
            else
            {
                return Json(new { Success = false, ErrorMessage = "Error" });
            }


        }
        // GET: RmTitularesMarca/
        // 
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(RmTitularesMarca rmTitularesMarca)
        //{
        //    _dbContext.Add(rmTitularesMarca);
        //    await _dbContext.SaveChangesAsync();
        //    return RedirectToAction("ResultTable");

        //}
        [HttpPost]
        public IActionResult GenerarPdf(InscripcionCustom inscripcion)
        {

           
           var  mesaSalida = GetTituloData(inscripcion);
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
            //var transacion = _dbContext.RmTransacciones.FirstOrDefault(t=>t.NumeroEntrada == inscripcion.NroEntrada);
            var transacion = _dbContext.RmTransacciones.OrderByDescending(o=>o.FechaAlta).FirstOrDefault(t => t.NumeroEntrada==inscripcion.NroEntrada);
            var estadoObservado = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Observado/Registrador");
            var estadoNotaNegativa = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Nota Negativa/Registrador");
            var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m => m.NumeroEntrada == inscripcion.NroEntrada);
            string viewHtml;
            var usuario = _dbContext.Usuarios.FirstOrDefault(u=>u.CodUsuario==User.Identity.Name);
            var codGrupo = usuario?.CodGrupo?? "";
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
                        mesaSalida.TipoOperacion = transacion.TipoOperacion;
                        viewHtml = RenderViewToString("ObservadoPDF", mesaSalida);
                    }
                    else if (mesaEntrada.EstadoEntrada == estadoNotaNegativa.CodigoEstado)
                    {
                        mesaSalida.Comentario = transacion.Observacion;
                        mesaSalida.TipoOperacion = transacion.TipoOperacion;
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
                if (mesaEntrada.EstadoEntrada == estadoObservado.CodigoEstado)
                {
                    mesaSalida.Comentario = transacion.Observacion;
                    mesaSalida.TipoOperacion = transacion.TipoOperacion;
                    viewHtml = RenderViewToString("ObservadoPDF", mesaSalida);
                }
                else if (mesaEntrada.EstadoEntrada == estadoNotaNegativa.CodigoEstado)
                {
                    mesaSalida.Comentario = transacion.Observacion;
                    mesaSalida.TipoOperacion = transacion.TipoOperacion;
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

            // Generar el nombre del archivo PDF con el formato deseado
            string fileName = $"TituloDePropiedad-Nro{inscripcion.NroEntrada}-{DateTime.Now:dd-MM-yyyy}.pdf";

            // Devolver el PDF como un archivo descargable con el nuevo nombre
            return File(pdfBytes, "application/pdf",fileName);
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
        private DatosTituloCustom GetTituloData(InscripcionCustom inscripcion)
        {
            try
            {


                var estadoObservado = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Observado/Registrador");
                var estadoNotaNegativa = _dbContext.RmEstadosEntrada.FirstOrDefault(e => e.DescripEstado == "Nota Negativa/Registrador");
                var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m => m.NumeroEntrada == inscripcion.NroEntrada);
                var transaccion = _dbContext.RmTransacciones.OrderByDescending(o => o.FechaAlta).Where(t => t.NumeroEntrada == inscripcion.NroEntrada).FirstOrDefault();

                string viewHtml;
                if (mesaEntrada.EstadoEntrada == estadoObservado.CodigoEstado)
                {

                    DatosTituloCustom tituloData = new()
                    {
                        NumeroEntrada = inscripcion.NroEntrada,
                        FechaEntrada = mesaEntrada.FechaEntrada,
                        FechaAlta = transaccion?.FechaAlta,
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
                        NumeroEntrada = inscripcion.NroEntrada,
                        FechaEntrada = mesaEntrada.FechaEntrada,
                        FechaAlta = transaccion?.FechaAlta,
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

                    //var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(p => p.NumeroEntrada == inscripcion.NroEntrada);
                  
                    var NroBoletaEntda = mesaEntrada?.NroBoleta ?? "";
                    
                    var nroBolImg = transaccion?.NroBoleta ?? "0";
                    var result = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NroBoleta == nroBolImg);
                    var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.NroBoleta == Convert.ToInt32( nroBolImg));
                    var bolDescripcion = boletaMarca?.Descripcion??"";
                    var EntradaResul = result?.NumeroEntrada ?? 0;
                    var transaccionAnt = _dbContext.RmTransacciones.Where(t => t.NumeroEntrada == EntradaResul).FirstOrDefault();
                    var dataRepresentante = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == inscripcion.Representante);
                    var titularMarca = _dbContext.RmTitularesMarcas.FirstOrDefault(t => t.IdTransaccion == transaccion.IdTransaccion);
                    var codTitular = titularMarca?.IdTitular ?? "";
                    var dataTitular = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == codTitular);
                    //var dataEstablecimiento = _dbContext.RmMarcasXEstabs.FirstOrDefault(p => p.IdTransaccion == inscripcion.IdTransaccion);
                    var ciudadTitu = _dbContext.DirecPersonas.FirstOrDefault(p => p.CodPersona == codTitular);
                    var ciudadRep = _dbContext.DirecPersonas.FirstOrDefault(p => p.CodPersona == inscripcion.Representante);
                    var fecReingreso = _dbContext.RmReingresos.FirstOrDefault(p => p.NroEntrada == inscripcion.NroEntrada);
                    //var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b=>b.Descripcion== inscripcion.NroBoleta);
                    //var paraImg = boletaMarca?.NroBoleta??0;
                    //var result = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NroBoleta== paraImg.ToString());
                    //var autorizante = _dbContext.RmAutorizantes.FirstOrDefault(p => p.MatriculaRegistro == inscripcion.MatriculaAutorizante);

                    string imagenSenhalPath = result?.SenalNombre ?? "";
                    string imagenMarcaPath = result?.MarcaNombre ?? "";

                    //if (inscripcion.TipoOperacion == "22")
                    //{
                    //    imagenSenhalPath = "\\\\172.30.8.18\\inventiva\\imagenes_senales\\No Aplicable.bmp";
                    //    inscripcion.TipoOperacion = "5";
                    //}
                    //if (inscripcion.TipoOperacion == "23")
                    //{
                    //    imagenMarcaPath = "\\\\172.30.8.18\\inventiva\\imagenes_marcas\\No Aplicable.bmp";
                    //    inscripcion.TipoOperacion = "5";
                    //}


                    string imagenSenhalDataUri = ConvertImageToDataUri(imagenSenhalPath);
                    string imagenMarcaDataUri = ConvertImageToDataUri(imagenMarcaPath);



                    var persona = _dbContext.Personas.FirstOrDefault(p=>p.CodPersona== codTitular.ToString());
                    DatosTituloCustom tituloData = new()
                    {
                        NumeroEntrada = inscripcion.NroEntrada,
                        //FechaEntrada = mesaEntrada.FechaEntrada,
                        FechaAlta = transaccion?.FechaAlta,
                        ImagenMarca = imagenMarcaDataUri,
                        ImagenSenhal = imagenSenhalDataUri,
                        NroBoleta = bolDescripcion,
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
                        DistritoEstable = inscripcion?.CodDistrito ?? "",
                        DepartamentoEstable = inscripcion?.Departamento ?? "",
                        NombreRepresentante = inscripcion?.NombreRepresentante ?? "",
                        Representante = inscripcion?.Representante ?? "",
                        NacionalidadesRep = GetNacionalidad(),
                        CodPaisRep = dataRepresentante?.CodPais ?? "",
                        EstadoCivilRep = GetEstadoCivil(),
                        CodEstadoCivilRep = transaccion?.CodEstadoCivilApoderado ?? "",
                        ProfesionesRep = GetProfesion(),
                        CodProfesionRep = transaccion?.CodProfesionApoderado ?? "",
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

                    if (mesaEntrada.Reingreso=="S")
                    {
                        tituloData.FechaReingreso = mesaEntrada.FechaEntrada;
                        tituloData.FechaEntrada = mesaEntrada.FechaAntReingreso;
                    }
                    else
                    {
                        tituloData.FechaEntrada = mesaEntrada.FechaEntrada;
                    }

                    var nroBoleta = inscripcion?.NroBoleta ?? "";
                    if (bolDescripcion != "")
                    {

                        tituloData.Barcode = GetCodigoBarra(bolDescripcion);
                    }
                    else
                    {
                        tituloData.Barcode = "";
                    }
                    tituloData.Comentario = inscripcion?.Comentario ?? "";
                   
                    //tituloData.Comentario = transaccion.Observacion;

                    //Debe ser la nueva transaccion y no incluir al titular que se ingresa por entrada
                    var query = _dbContext.RmTitularesMarcas.Where(t => t.IdTransaccion == transaccion.IdTransaccion).AsQueryable();
                    var queryFinal = query.Where(o=>o.IdTitular!= codTitular).AsQueryable().Join(_dbContext.Personas,
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
            catch (Exception ex)
            {

                throw new Exception("Ocurrió un error: " + ex.Message);

            }
            
        }

        [HttpPost]
        //[ValidateAntiForgeryToken] 
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMINSCR", "Create", "InscripcionCustom" })]

        public async Task<IActionResult> DetailsInscripcion([FromBody] InscripcionCustom inscripcion)
        {
            try
            {
              
                var mesaEntrada = _dbContext.RmMesaEntrada.FirstOrDefault(m => m.NumeroEntrada == inscripcion.NroEntrada);
               
                var nroBolImg = inscripcion.NroBoleta;
                string imagenSenhalPath =  "";
                string imagenMarcaPath =  "";
                if (nroBolImg =="")
                {
                    var nuevaBol = _dbContext.RmMarcasSenales.FirstOrDefault(m=>m.NumeroEntrada==inscripcion.NroEntrada);
                    imagenSenhalPath = nuevaBol?.SenalNombre ?? "";
                    imagenMarcaPath = nuevaBol?.MarcaNombre ?? "";
                    var nroBol = nuevaBol?.NroBoleta ?? "0";
                    var descBoleta = _dbContext.RmBoletasMarcas.FirstOrDefault(b=>b.NroBoleta == Convert.ToInt64(nroBol));
                    nroBolImg = descBoleta?.Descripcion??"";
                }
                else
                {
                    var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.Descripcion == nroBolImg);
                    var bolDescripcion = boletaMarca?.NroBoleta ?? 0;
                    var result = _dbContext.RmMarcasSenales.FirstOrDefault(p => p.NroBoleta == bolDescripcion.ToString());
                    imagenSenhalPath = result?.SenalNombre ?? "";
                    imagenMarcaPath = result?.MarcaNombre ?? "";
                }
                
            
               // var EntradaResul = result?.NumeroEntrada ?? 0;
               // var transaccionAnt = _dbContext.RmTransacciones.Where(t => t.NumeroEntrada == EntradaResul).FirstOrDefault();
                var dataRepresentante = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == inscripcion.Representante);
               // var titularMarca = _dbContext.RmTitularesMarcas.FirstOrDefault(t => t.IdTransaccion == transaccion.IdTransaccion);
                var codTitular = "";
                foreach (var item in inscripcion.Titulares)
                {
                     codTitular = item.IdPropietario;
                    break;
                }
                    
                var dataTitular = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == codTitular);
                var ciudadTitu = _dbContext.DirecPersonas.FirstOrDefault(p => p.CodPersona == codTitular);
                var ciudadRep = _dbContext.DirecPersonas.FirstOrDefault(p => p.CodPersona == inscripcion.Representante);
                var fecReingreso = _dbContext.RmReingresos.FirstOrDefault(p => p.NroEntrada == inscripcion.NroEntrada);



                string imagenSenhalDataUri = ConvertImageToDataUri(imagenSenhalPath);
                string imagenMarcaDataUri = ConvertImageToDataUri(imagenMarcaPath);



                var persona = _dbContext.Personas.FirstOrDefault(p => p.CodPersona == codTitular.ToString());
                DatosTituloCustom tituloData = new()
                {
                    NumeroEntrada = inscripcion.NroEntrada,
                    //FechaEntrada = mesaEntrada.FechaEntrada,
                   //FechaAlta = transaccion?.FechaAlta,
                    ImagenMarca = imagenMarcaDataUri,
                   ImagenSenhal = imagenSenhalDataUri,
                    NroBoleta = nroBolImg,
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
                    DistritoEstable = inscripcion?.CodDistrito ?? "",
                    DepartamentoEstable = inscripcion?.Departamento ?? "",
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

                if (mesaEntrada.Reingreso == "S")
                {
                    tituloData.FechaReingreso = mesaEntrada.FechaEntrada;
                    tituloData.FechaEntrada = mesaEntrada.FechaAntReingreso;
                }
                else
                {
                    tituloData.FechaEntrada = mesaEntrada.FechaEntrada;
                }

                var nroBoleta = inscripcion?.NroBoleta ?? "";
                if (nroBolImg != "")
                {

                    tituloData.Barcode = GetCodigoBarra(nroBolImg);
                }
                else
                {
                    tituloData.Barcode = "";
                }
                tituloData.Comentario = inscripcion?.Comentario ?? "";

                //tituloData.Comentario = transaccion.Observacion;

                //Debe ser la nueva transaccion y no incluir al titular que se ingresa por entrada
                //var query = _dbContext.RmTitularesMarcas.Where(t => t.IdTransaccion == transaccion.IdTransaccion).AsQueryable();
                //var queryFinal = query.Where(o => o.IdTitular != codTitular).AsQueryable().Join(_dbContext.Personas,
                //   titu => titu.IdTitular,
                //   nom => nom.CodPersona,
                //   (titu, nom) => new TitularesCarga { IdPropietario = titu.IdTitular, Nombre = nom.Nombre });
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
                    if (titular.IdPropietario!=codTitular)
                    {
                        // Agrega el objeto convertido a la nueva lista
                        titularesConvertidos.Add(titularCarga);
                    }
                   
                }

                // Asigna la nueva lista convertida a tituloData.Titulares
                tituloData.Titulares = titularesConvertidos;

                // Verifica si la lista es nula y crea una nueva si es necesario (esto puede no ser necesario dependiendo de la lógica de tu aplicación)
                if (tituloData.Titulares == null)
                {
                    tituloData.Titulares = new List<TitularesCarga>();
                }

                return View("DetailsInscripcion", tituloData);
            }
            catch (Exception ex)
            {

                return View("Error");

            }
            return Ok();
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

    }
}
