using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SistemaBase.Models;
using System.Security.Claims;
//using PJAuthenticationService;
using SistemaBase.ModelsCustom;

namespace SistemaBase.Controllers
{
    public class LoginController : Controller
    {
        private readonly DbvinDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController(DbvinDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool login(string usuario, string pass)
        {
            try
            {
                var login = _context.Usuarios.Where(x => x.CodUsuario == usuario && x.Clave == pass)?.FirstOrDefault();
                if (usuario == null)
                {
                    ViewData["MENSAJE"] = "No tienes credenciales correctas";
                    return false;
                }
                else
                {
                    var persona = _context.Personas.FirstOrDefault(p => p.CodPersona == login.CodPersona);
                    //DEBEMOS CREAR UNA IDENTIDAD (name y role)
                    //Y UN PRINCIPAL
                    //DICHA IDENTIDAD DEBEMOS COMBINARLA CON LA COOKIE DE 
                    //AUTENTIFICACION
                    ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                    //TODO USUARIO PUEDE CONTENER UNA SERIE DE CARACTERISTICAS
                    //LLAMADA CLAIMS.  DICHAS CARACTERISTICAS PODEMOS ALMACENARLAS
                    //DENTRO DE USER PARA UTILIZARLAS A LO LARGO DE LA APP
                    Claim claimUserName = new Claim(ClaimTypes.Name, login.CodUsuario);
                    Claim claimRole = new Claim(ClaimTypes.Role, login.CodGrupo);
                    Claim claimIdUsuario = new Claim("IdUsuario", login.CodUsuario.ToString());
                    Claim claimIdPersona = new Claim("IdPersona", login.CodPersona);
                    Claim claimNombreUser = new Claim("NombreUsuario", persona.Nombre);

                    //Claim claimUserName = new Claim(ClaimTypes.Name, login.CodUsuario);

                    identity.AddClaim(claimUserName);
                    identity.AddClaim(claimRole);
                    identity.AddClaim(claimIdUsuario);
                    identity.AddClaim(claimIdPersona);
                    identity.AddClaim(claimNombreUser);

                    ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.Now.AddMinutes(45)
                    });

                    //var persona = _context.Personas.FirstOrDefault(p=>p.CodPersona == login.CodPersona);
                    //var nombrePersona = persona?.Nombre??"";
                    return login != null;
                }


            }
            catch
            {
                return false;
            }
        }


        //[HttpPost]
        //public async Task<bool> Loginconws(string usuario, string pass)
        //{
        //    //prueba de conexion con el ws 
   
        //    var ws = new PJAuthenticationService.AutenticacionSoapClient(AutenticacionSoapClient.EndpointConfiguration.AutenticacionSoap);
        //    var aut = new PJAuthenticationService.cControlAutenticacion();
        //    aut = await ws.AutenticarAsync(usuario.Trim(), pass.Trim(), 71, 168);

            
        //    try
        //       {
        //        /*&& x.Clave == pass*/
        //        Usuario login = null;
              
        //        if (aut.ResultadoLogin == true )
        //        {
        //            var loginINV = _context.Usuarios.Where(x => x.CodPersona == usuario)?.FirstOrDefault(x=>x.Estado=="A");
        //            login = new()
        //            {
        //                CodUsuario = loginINV.CodUsuario,
        //                CodGrupo = loginINV.CodGrupo,
        //                CodPersona = loginINV.CodPersona
        //            };

        //        }
               
        //        if (usuario == null || login == null)
        //        {
        //            ViewData["MENSAJE"] = "No tienes credenciales correctas";
        //            return false;
        //        }
        //        else
        //        {

        //            var persona = _context.Personas.FirstOrDefault(e => e.CodPersona == login.CodPersona);
        //            var accesoGrupo = _context.AccesosGrupos.Where(p => p.CodGrupo==login.CodGrupo).ToList();
        //            // Serializar la lista a JSON
        //            var serializedAccesoGrupo = JsonConvert.SerializeObject(accesoGrupo);

        //            // Crear una claim con la lista serializada y codificarla
        //            var claimsAcceso = new List<Claim>
        //            {
        //                new Claim("AccesosGrupo", serializedAccesoGrupo)
        //            };
        //            //DEBEMOS CREAR UNA IDENTIDAD (name y role)
        //            //Y UN PRINCIPAL
        //            //DICHA IDENTIDAD DEBEMOS COMBINARLA CON LA COOKIE DE 
        //            //AUTENTIFICACION
        //            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
        //            //TODO USUARIO PUEDE CONTENER UNA SERIE DE CARACTERISTICAS
        //            //LLAMADA CLAIMS.  DICHAS CARACTERISTICAS PODEMOS ALMACENARLAS
        //            //DENTRO DE USER PARA UTILIZARLAS A LO LARGO DE LA APP
        //            Claim claimUserName = new Claim(ClaimTypes.Name, login.CodUsuario);
        //            Claim claimRole = new Claim(ClaimTypes.Role, login.CodGrupo);
        //            Claim claimIdUsuario = new Claim("IdUsuario", login.CodUsuario.ToString());
        //            Claim claimIdPersona = new Claim("IdPersona", login.CodPersona);
        //            Claim claimNombreUser = new Claim("NombreUsuario", persona.Nombre);


        //            identity.AddClaim(claimUserName);
        //            identity.AddClaim(claimRole);
        //            identity.AddClaim(claimIdUsuario);
        //            identity.AddClaim(claimIdPersona);
        //            identity.AddClaim(claimNombreUser);

        //            ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
        //             HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
        //            {
        //                ExpiresUtc = DateTime.Now.AddMinutes(45)
        //            });

        //            return login != null;
        //        }
                

        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public IActionResult ErrorAcceso()
        {
            ViewData["MENSAJE"] = "Error de acceso";
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
