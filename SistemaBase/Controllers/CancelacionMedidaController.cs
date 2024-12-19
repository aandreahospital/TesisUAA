using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using SistemaBase.ModelsCustom;


namespace SistemaBase.Controllers
{
    [Authorize]
    public class CancelacionMedidaController : Controller
    {
        private readonly DbvinDbContext _dbContext;
        public CancelacionMedidaController(DbvinDbContext context)
        {
            _dbContext = context;
        }
        // GET: CancelacionMedidaController

        public async Task<IActionResult> Index()
        {
            var idLevantamiento = _dbContext.RmLevantamientos.Max(m => m.IdLevantamiento) + 1;
            ViewBag.AutorizanteM = new SelectList(_dbContext.RmAutorizantes, "IdAutorizante", "DescripAutorizante");

            ViewBag.TipoMoneda = new SelectList(_dbContext.RmTiposMonedas, "TipoMoneda", "DescripTipoMoneda");
            ViewBag.AutorizanteL = new SelectList(_dbContext.RmAutorizantes, "IdAutorizante", "DescripAutorizante");


            CancelacionMedidaCustom CancelacionMedidas = new CancelacionMedidaCustom();
            CancelacionMedidas.IdLevantamiento = idLevantamiento;


            return View(await Task.FromResult(CancelacionMedidas));
        }


        // POST: CancelacionMedidaController/Create
        [HttpPost]
        public async Task<IActionResult> Create(RmLevantamiento rmLevantamiento)
        {
            try
            {
                _dbContext.Add(rmLevantamiento);
                await _dbContext.SaveChangesAsync();
                return Ok("Datos guardados correctamente");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar el levantamiento: " + ex.Message);
                return BadRequest("Error al agregar el levantamiento: " + ex.Message);


            }

        }
    }
}
