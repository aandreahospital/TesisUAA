using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;

namespace SistemaBase.Controllers
{
    public class GenerarBoletaController : Controller
    {
        private readonly DbvinDbContext _dbContext;

        public GenerarBoletaController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        //FILTRO QUE AUTORIZA AL USUARIO PARA ACCEDER AL CONTROLADOR CON EL PERMISO
        [TypeFilter(typeof(AutorizarUsuarioFilter), Arguments = new object[] { "RMRTRAOP", "Index", "GenerarBoleta" })]

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgregarBoleta(DatosTituloCustom datos)
        {
            try
            {
                var mesaEntrada = await _dbContext.RmMesaEntrada.FirstOrDefaultAsync(o=>o.NumeroEntrada==datos.NumeroEntrada);
                if (mesaEntrada != null)
                {
                    string alfanumerico = "";
                    var marcasSenal = await _dbContext.RmMarcasSenales.FirstOrDefaultAsync(m => m.NumeroEntrada == datos.NumeroEntrada);
                    var nuevaBoleta = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.Asignado == null);
                    if (nuevaBoleta == null)
                    {
                        return Json(new { Success = false, ErrorMessage = "Ya no existen bolestas" });
                    }
                    if (marcasSenal == null)
                    {
                        var parametroMarca = await _dbContext.ParametrosGenerales.FirstOrDefaultAsync(m => m.Parametro == "DIR_IMGMARCA");
                        var parametroSenal = await _dbContext.ParametrosGenerales.FirstOrDefaultAsync(m => m.Parametro == "DIR_IMGSENAL");
                        var marcaSenale = _dbContext.RmMarcasSenales.Max(m => m.IdMarca) + 1;
                        var newBol = nuevaBoleta?.NroBoleta??0;
                        alfanumerico = nuevaBoleta?.Descripcion ?? "";
                        RmMarcasSenale marcasSenales = new()
                        {
                            NumeroEntrada = datos.NumeroEntrada,
                            MarcaNombre = parametroMarca.Valor + datos.NumeroEntrada + ".bmp",
                            SenalNombre = parametroSenal.Valor + datos.NumeroEntrada + ".bmp",
                            IdMarca = marcaSenale,
                            NroBoleta = newBol.ToString()
                        };

                        nuevaBoleta.Asignado = "S";

                        _dbContext.Update(nuevaBoleta);
                        _dbContext.Add(marcasSenales);
                        _dbContext.SaveChanges();
                       
                    }
                    else
                    {
                        var nroBoleta = marcasSenal?.NroBoleta??"0";
                        if (nroBoleta!="0")
                        {
                            var boletaMarca = _dbContext.RmBoletasMarcas.FirstOrDefault(b => b.NroBoleta == Convert.ToInt64(nroBoleta));
                            alfanumerico = boletaMarca?.Descripcion ?? "";
                        }
                        else
                        {
                          
                            var newBol = nuevaBoleta?.NroBoleta ?? 0;
                            alfanumerico = nuevaBoleta?.Descripcion ?? "";
                            nuevaBoleta.Asignado = "S";
                            marcasSenal.NroBoleta = newBol.ToString();
                            _dbContext.Update(marcasSenal);
                            _dbContext.Update(nuevaBoleta);
                           
                        }
                       
                    }

                    if (alfanumerico != "")
                    {
                        return Json(new { Success = true, nroboleta = alfanumerico });
                    }
                    else
                    {
                        return Json(new { Success = false, ErrorMessage = "No se genero alfanumerico" });
                    }
                   // return Ok();
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "No existe numero de entrada" });
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar/actualizar : " + ex.Message);
                return BadRequest("Error al agregar/actualizar : " + ex.Message);
            }
        }



    }
}
