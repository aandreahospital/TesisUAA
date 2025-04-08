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
using Org.BouncyCastle.Crypto.Generators;
using System.Text.RegularExpressions;

namespace SistemaBase.Controllers
{
    public class LoginController : Controller
    {
        private readonly Models.UAADbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController(Models.UAADbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult PrimerLogin(string usuario, string pass)
        {
            try
            {
                var persona = _context.Personas.FirstOrDefault(x => x.CodPersona == usuario);
                if (persona == null)
                {
                    return Json(new { success = false, message = "No tienes credenciales correctas" });
                }

                var usuarioExistente = _context.Usuarios.FirstOrDefault(x => x.CodUsuario == usuario && x.Clave == usuario);
                if (usuarioExistente != null)
                {
                    return Json(new { success = false, redirect = Url.Action("ActualizarPass", "Login", new { user = usuario }) });
                }

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false, message = "Error en el servidor" });
            }
        }


        public IActionResult ActualizarPass(string user)
        {
            // Obtener el usuario autenticado
            if (User.Identity.Name != null)
            {
                user = User.Identity.Name;
            }
            var usuario = user;
            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Login"); // Si no está autenticado, redirigir
            }

            // Pasar el usuario a la vista
            ViewBag.Usuario = usuario;
            return View();
        }


        [HttpPost]
        public JsonResult Login(string usuario, string pass)
        {
            try
            {
                string hashedPassword = HashPassword(pass);

                var login = _context.Usuarios.FirstOrDefault(x => x.CodUsuario == usuario && x.Clave == hashedPassword);
                if (login == null)
                {
                    return Json(new { success = false, message = "No tienes credenciales correctas" });
                }

                

                var persona = _context.Personas.FirstOrDefault(p => p.CodPersona == login.CodPersona);

                // Crear identidad y autenticación
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaims(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login.CodUsuario),
                    new Claim(ClaimTypes.Role, login.CodGrupo),
                    new Claim("IdUsuario", login.CodUsuario.ToString()),
                    new Claim("IdPersona", login.CodPersona),
                    new Claim("NombreUsuario", persona?.Nombre ?? "")
                });

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddMinutes(45)
                });

                return Json(new { success = true, redirect = "/Bienvenido" });
            }
            catch
            {
                return Json(new { success = false, message = "Error en el servidor" });
            }
        }



        [HttpPost]
        public async Task<IActionResult> ActualizarPassword(string usuario, string pass, string confirmarPass)
        {
            if (string.IsNullOrEmpty(pass) || pass != confirmarPass)
            {
                TempData["Error"] = "Las contraseñas no coinciden.";
                return RedirectToAction("ActualizarPassword");
            }
           
            try
            {
                    var persona = await _context.Personas.FirstOrDefaultAsync(u => u.CodPersona == usuario);
                if (persona == null)
                {
                    TempData["Error"] = "Datos incorrectos.";
                    return RedirectToAction("Login");
                }

                string hashedPassword = HashPassword(pass);

                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.CodPersona == usuario);
                    if (user == null)
                    {
                        Usuario addUsuario = new()
                        {
                            CodUsuario = usuario,
                            CodPersona = usuario,
                            Clave = hashedPassword,
                            FecCreacion = DateTime.Now,
                            CodGrupo ="USER"

                        };
                        _context.Add(addUsuario);

                    }
                    else
                    {
                        user.Clave = hashedPassword;
                        _context.Update(user);
                    }
                    _context.SaveChanges();
                    //await _context.SaveChangesAsync();

                    
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrió un error al actualizar la contraseña.";
                return RedirectToAction("Login");
            }
        }

        // Método para validar la contraseña
        private bool EsPasswordValido(string password)
        {
            // Expresión regular para validar:
            // - Máximo 8 caracteres
            // - Al menos una letra mayúscula
            // - Al menos un carácter especial
            string pattern = @"^(?=.*[A-Z])(?=.*[\W_]).{1,8}$";
            return Regex.IsMatch(password, pattern);
        }


        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }



        [HttpPost]
        public bool loginsinws(string usuario, string pass)
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
