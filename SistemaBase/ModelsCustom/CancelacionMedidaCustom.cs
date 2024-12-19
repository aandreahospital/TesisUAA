using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaBase.ModelsCustom
{
    public partial class CancelacionMedidaCustom
    {
        public decimal IdLevantamiento { get; set; }
        public DateTime? FechaAlta { get; set; }
        public string? IdUsuario { get; set; }
        public decimal? IdMarca { get; set; }
        public decimal? NroBoletaSenal { get; set; }
        public DateTime? FechaOperacion { get; set; }
        public decimal? NroOficio { get; set; }
        public string? Libro { get; set; }
        public string? Folio { get; set; }
        public string? Acreedor { get; set; }
        public string? Deudor { get; set; }
        public decimal? MontoPrenda { get; set; }
        public decimal? IdAutorizanteM { get; set; }
        public decimal? IdAutorizanteL { get; set; }
        public decimal? TipoMoneda { get; set; }
        public decimal? NroEntrada { get; set; }
        public decimal? NroBoleta { get; set; }
        public DateTime? FechaActoJuridico { get; set; }
        public string? TotalParcial { get; set; }
        public decimal? MontoLevantamiento { get; set; }
        public decimal? IdMedida { get; set; }
        public decimal? NroDocLevantamiento { get; set; }
        public List<SelectListItem>? Levantamientos { get; set; }
        //public List<SelectListItem>? Usuarios { get; set; }
        public List<SelectListItem>? Autorizantes { get; set; }

    }
}
