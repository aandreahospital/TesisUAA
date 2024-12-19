using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;
using System.Security.Claims;
using static SistemaBase.ModelsCustom.Reportes;

namespace SistemaBase.Controllers
{
    public class EnvioEntradaRegionalController : Controller
    {
        private readonly DbvinDbContext _context;

        public EnvioEntradaRegionalController(DbvinDbContext context)
        {
            _context = context;
        }

        ////FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMENSECC", "Index", "EnvioEntradaRegional" })]
        
        public async Task<IActionResult> Index()
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(p => p.CodUsuario == User.Identity.Name);

            if (usuario.CodGrupo == "ADMIN" || usuario.CodGrupo == "EASDR" || usuario.CodGrupo == "JEFDIV")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina

                                         //Condicion para que filtre por Documentos con titulo diferentes asuncion
                                         where ( ee.DescripEstado == "Mesa entrada" || ee.DescripEstado== "Ingresado") &&  or.DescripOficina != "Asunción" 
                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });

                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUSP" || usuario.CodGrupo == "JSP")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "San Pedro" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());

            }

            else if (usuario.CodGrupo == "USUNE" || usuario.CodGrupo == "JUSUNE")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Ñeembucú" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUEN" || usuario.CodGrupo == "JEN")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Encarnación" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUPA" || usuario.CodGrupo == "JUSUPA")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Paraguarí" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUCP" || usuario.CodGrupo == "JCP")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado") && or.DescripOficina == "Concepción"

                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUCO" || usuario.CodGrupo == "JCO")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Cnel. Oviedo" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUAP" || usuario.CodGrupo == "JAP")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Alto Parana" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUMN" || usuario.CodGrupo == "JMN")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Misiones" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUPH" || usuario.CodGrupo == "JPH")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Pdte. Hayes" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUCD" || usuario.CodGrupo == "JCD")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Canindeyú" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUBR" || usuario.CodGrupo == "JBR")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Boqueron" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUPJC" || usuario.CodGrupo == "JPJC")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Amambay" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUGA" || usuario.CodGrupo == "JGA")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Guaira" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUCA" || usuario.CodGrupo == "JCA")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Caazapa" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUHO" || usuario.CodGrupo == "JHO")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Horqueta" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUCI" || usuario.CodGrupo == "JCI")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Cordillera" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            else if (usuario.CodGrupo == "USUFO" || usuario.CodGrupo == "JFO")
            {
                var recepcionInterior = (from rme in _context.RmMesaEntrada
                                         join ts in _context.RmTipoSolicituds on rme.TipoSolicitud equals ts.TipoSolicitud
                                         join ee in _context.RmEstadosEntrada on rme.EstadoEntrada equals ee.CodigoEstado
                                         join or in _context.RmOficinasRegistrales on rme.CodigoOficina equals or.CodigoOficina
                                         where or.DescripOficina == "Fuerte Olimpo" && (ee.DescripEstado == "Mesa entrada" || ee.DescripEstado == "Ingresado")
                                         //ee.DescripEstado == "Aprobado/JefeSup/Firma" || ee.DescripEstado == "Aprobado/JefeRegistral/Firma"
                                         //&& (ts.DescripSolicitud != "CERTIFICADO DE CONDICION DE DOMINIO" || ts.DescripSolicitud != "INFORME")
                                         //|| ee.DescripEstado == "Observado/JefeRegistral" || ee.DescripEstado == "Nota Negativa/JefeRegistral"
                                         //|| ee.DescripEstado == "Observado/Supervisor" || ee.DescripEstado == "Nota Negativa/Supervisor"
                                         //|| ee.DescripEstado == "Observado/Finalizado" || ee.DescripEstado == "Nota Negativa/Finalizado"

                                         orderby rme.FechaEntrada descending
                                         select new Direccion
                                         {
                                             NroEntrada = Convert.ToInt32(rme.NumeroEntrada),
                                             DescSolicitud = ts.DescripSolicitud,
                                             FechaAlta = rme.FechaEntrada,
                                             Oficina = or.DescripOficina

                                         });
                return View(await recepcionInterior.AsNoTracking().ToListAsync());
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado([FromBody] List<decimal> selectedNroEntradas)
        {

            if (selectedNroEntradas == null || selectedNroEntradas.Count == 0)
            {
                return Json(new { Success = false, ErrorMessage = "Debe seleccionar un trabajo." });
            }

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
                    var estadoEntrada = await _context.RmEstadosEntrada.FirstOrDefaultAsync(p => p.DescripEstado == "Enviado a Div Regional");

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


        [HttpPost]
        public async Task<IActionResult> GenerarPdf([FromBody] List<decimal> selectedNroEntradas)
        {
            try
            {
                decimal codOficina = 0;
                var tipoSolicitud = _context.RmTipoSolicituds.AsQueryable();
                var mesaEntrada = _context.RmMesaEntrada.AsQueryable();
                //Se utiliza codigo para Operaciones
                var transaccion = _context.RmTransacciones.AsQueryable();
                // Procesar la lista de números decimales y obtener los datos necesarios de la base de datos
                var listaProcesada = new List<ListadoJefes>();
                foreach (decimal numero in selectedNroEntradas)
                {
                    var queryTransa = mesaEntrada.Where(m => m.NumeroEntrada == numero).AsQueryable();

                    var queryTipo = queryTransa.AsQueryable().Join(tipoSolicitud,
                       en => en.TipoSolicitud,
                       tipo => tipo.TipoSolicitud,
                       (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada,
                           TipoSolicitud = tipo.DescripSolicitud,
                           FechaEntrada = en.FechaEntrada,
                           NomTitular = en.NomTitular, 
                           NomOperador = en.UsuarioEntrada });

                    var segundo = queryTipo.ToList();


                    var queryUsu = queryTipo.AsQueryable().Join(_context.Usuarios,
                      en => en.NomOperador,
                      tipo => tipo.CodUsuario,
                      (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, TipoSolicitud = en.TipoSolicitud, FechaEntrada = en.FechaEntrada, NomTitular = en.NomTitular, NomOperador = tipo.CodPersona });

                    var cuarto = queryUsu.ToList();

                    var queryFinal = queryUsu.OrderByDescending(u => u.FechaEntrada).AsQueryable().Join(_context.Personas,
                    en => en.NomOperador,
                    tipo => tipo.CodPersona,
                    (en, tipo) => new ListadoJefes { NumeroEntrada = en.NumeroEntrada, TipoSolicitud = en.TipoSolicitud, FechaEntrada = en.FechaEntrada, NomTitular = en.NomTitular, NomOperador = tipo.Nombre });


                    listaProcesada.AddRange(queryFinal);

                    var entrada = _context.RmMesaEntrada.FirstOrDefault(m => m.NumeroEntrada == numero);
                    var oficina = _context.RmOficinasRegistrales.FirstOrDefault(o => o.CodigoOficina == entrada.CodigoOficina);
                    codOficina = oficina?.CodigoOficina ?? 0;
                }
                ViewBag.Oficina = new SelectList(_context.RmOficinasRegistrales, "CodigoOficina", "DescripOficina");
                // Crear un modelo de datos que contiene la lista procesada
                Reportes tituloData = new Reportes
                {
                    ListadoParaJefes = listaProcesada,
                    FechaActual = DateTime.Now,
                    Usuario = User.Identity.Name,
                    TotalIngresado = listaProcesada.Count,
                    CodigoOficina = codOficina
                };

                // Leer la imagen y convertirla en una cadena de datos URI codificada en base64
                string imagePath = "wwwroot/assets/img/PJ/PoderJudicial.png"; // Ruta a la imagen en tu proyecto
                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                string imageDataUri = "data:image/png;base64," + Convert.ToBase64String(imageBytes);

                // Pasar la cadena de datos URI a la vista a través del modelo de vista
                tituloData.ImageDataUri = imageDataUri;

                // Renderizar la vista Razor a una cadena HTML
                string viewHtml = await RenderViewToStringAsync("ReportePDF", tituloData);

                if (string.IsNullOrWhiteSpace(viewHtml))
                {
                    return BadRequest("La vista HTML está vacía o nula.");
                }

                // Crear un documento PDF utilizando iText7
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfDocument pdfDoc = new PdfDocument(new PdfWriter(memoryStream));
                    Document document = new Document(pdfDoc);

                    // Agregar el contenido HTML convertido al documento PDF
                    HtmlConverter.ConvertToPdf(viewHtml, pdfDoc, new ConverterProperties());

                    document.Close();

                    // Convertir el MemoryStream a un arreglo de bytes
                    byte[] pdfBytes = memoryStream.ToArray();

                    // Configurar el encabezado Content-Disposition
                    Response.Headers["Content-Disposition"] = "inline; filename=Reporte-de-Entrada.pdf";

                    // Devolver el PDF como un archivo descargable con el nuevo nombre
                    return File(pdfBytes, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que pueda ocurrir
                return BadRequest($"Error al generar el PDF: {ex.Message}");
            }
        }

        private async Task<string> RenderViewToStringAsync(string viewName, object model)
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

                await viewResult.View.RenderAsync(viewContext); // Utilizar await para renderizar de manera asincrónica
                return sw.GetStringBuilder().ToString();
            }
        }

    }
}
