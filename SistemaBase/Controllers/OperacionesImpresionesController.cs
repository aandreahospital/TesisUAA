using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;

namespace SistemaBase.Controllers
{
    [Authorize]
    public class OperacionesImpresionesController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public OperacionesImpresionesController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        // GET: OperacionesImpresionesController
        public async Task<IActionResult> Index()
        {
            var operacionImpresion = (from me in _dbContext.RmMesaEntrada
                                      join ts in _dbContext.RmTipoSolicituds on me.TipoSolicitud equals ts.TipoSolicitud
                                      join ee in _dbContext.RmEstadosEntrada on me.EstadoEntrada equals ee.CodigoEstado
                                      join t in _dbContext.RmTransacciones on me.NumeroEntrada equals t.NumeroEntrada
                                      join u in _dbContext.Usuarios on t.IdUsuario equals u.CodUsuario
                                      join p in _dbContext.Personas on u.CodPersona equals p.CodPersona
                                      orderby me.NumeroEntrada ascending
                                      select new OperacionesImpresion()
                                      {
                                          NroEntrada = me.NumeroEntrada,
                                          TipoSolicitud = ts.TipoSolicitud,
                                          DescSolicitud = ts.DescripSolicitud,
                                          FechaAlta = t.FechaAlta,
                                          DescEstado = ee.DescripEstado,
                                          Observacion = t.ObservacionSup,
                                          NombreOperador = p.Nombre
                                      });
            return View(await operacionImpresion.AsNoTracking().ToListAsync());
        }

    }
}
