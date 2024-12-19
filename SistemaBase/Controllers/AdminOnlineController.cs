using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;

namespace SistemaBase.Controllers
{
    public class AdminOnlineController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public AdminOnlineController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "BSONLINE", "Index", "AdminOnline" })]
        public async Task<IActionResult> Index()
        {
            var recepcionDis = (from me in _dbContext.RmMesaEntrada
                                join ee in _dbContext.RmEstadosEntrada on me.EstadoEntrada equals ee.CodigoEstado
                                join ts in _dbContext.RmTipoSolicituds on me.TipoSolicitud equals ts.TipoSolicitud
                                join of in _dbContext.RmOficinasRegistrales on me.CodigoOficina equals of.CodigoOficina
                                where me.UsuarioEntrada == "USERONLINE"
                                orderby me.FechaEntrada descending
                                select new Online()
                                {
                                    NroEntrada = Convert.ToInt32(me.NumeroEntrada),
                                    DescTipoSolicitud = ts.DescripSolicitud,
                                    DescOficina = of.DescripOficina,
                                    FechaEntrada = me.FechaEntrada,
                                    ArchivoPDF = me.ArchivoPDF,
                                    AnexoPDF = me.AnexoPDF
                                });
            return View(await recepcionDis.AsNoTracking().ToListAsync());
        }

        public IActionResult AbrirPDF(int numeroEntrada)
        {
            var mesaEntrada = _dbContext.RmMesaEntrada.SingleOrDefault(me => me.NumeroEntrada == numeroEntrada);

            if (mesaEntrada != null && mesaEntrada.ArchivoPDF != null && mesaEntrada.ArchivoPDF.Length > 0)
            {
                // Retorna el archivo PDF como FileResult
                return File(mesaEntrada.ArchivoPDF, "application/pdf");
            }

            return NotFound();
        }
        public IActionResult AbrirAnexoPDF(int numeroEntrada)
        {
            var mesaEntrada = _dbContext.RmMesaEntrada.SingleOrDefault(me => me.NumeroEntrada == numeroEntrada);

            if (mesaEntrada != null && mesaEntrada.AnexoPDF != null && mesaEntrada.AnexoPDF.Length > 0)
            {
                // Retorna el archivo PDF como FileResult
                return File(mesaEntrada.AnexoPDF, "application/pdf");
            }

            return NotFound();
        }

    }
}
