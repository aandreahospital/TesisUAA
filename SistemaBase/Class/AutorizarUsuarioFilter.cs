using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SistemaBase.Models;
using System;
using System.Linq;
using System.Security.Claims;

public class AutorizarUsuarioFilter : IAuthorizationFilter
{
    private readonly string modulo;
    private readonly string operacion;
    private readonly string controlador;

    private readonly ILogger<AutorizarUsuarioFilter> logger;
    private readonly DbvinDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string? accion;
    private DbvinDbContext dbContext;
    private IHttpContextAccessor httpContextAccessor;

    public AutorizarUsuarioFilter(string? controlador, string? accion, ILogger<AutorizarUsuarioFilter> logger, DbvinDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        this.controlador = controlador;
        this.accion = accion;
        this.logger = logger;
        this.dbContext = dbContext;
        this.httpContextAccessor = httpContextAccessor;
    }

    public AutorizarUsuarioFilter(
        string modulo,
        string operacion,
        string controlador,
        ILogger<AutorizarUsuarioFilter> logger,
        DbvinDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        this.modulo = modulo;
        this.operacion = operacion;
        this.controlador = controlador;

        this.logger = logger;
        this._dbContext = context;
        this._httpContextAccessor = httpContextAccessor;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

        if (userId != null)
        {
            var usu = _dbContext.Usuarios.FirstOrDefault(e => e.CodUsuario == userId );

            if (usu != null && UsuarioTienePermiso(usu.CodGrupo))
            {
                // El usuario tiene permiso, permitir el acceso
                return;
            }
        }

        // El usuario no tiene permiso, redirigir o devolver un resultado de autorización
        context.Result = new ForbidResult();
    }

    private bool UsuarioTienePermiso(string codGrupo)
    {
        var registroModulo = _dbContext.AccesosGrupos.Where(m => m.NomForma == modulo /*&& m.PuedeConsultar =="S" || m.NomForma == modulo && m.PuedeInsertar == "S" || m.NomForma == modulo && m.PuedeActualizar == "S" || m.NomForma == modulo && m.PuedeBorrar == "S"*/ ).ToList();

        foreach (var items in registroModulo)
        {
            if (items.CodGrupo == codGrupo)
            {
                return true;
            }
        }

        logger.LogWarning("El usuario no tiene permisos para acceder a {Modulo}", modulo);
        return false;
    }
}
