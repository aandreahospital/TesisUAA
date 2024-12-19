using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmLevantamiento
    {
        public decimal? IdMedida { get; set; }
        public decimal? IdMarca { get; set; }
        public decimal? NroEntrada { get; set; }
        public decimal? NroDocumentoLevanta { get; set; }
        public DateTime? FechaActoJuridico { get; set; }
        public decimal? IdAutorizante { get; set; }
        public decimal? MontoLevantamiento { get; set; }
        public string? IdUsuario { get; set; }
        public DateTime? FechaAlta { get; set; }
        public decimal? TipoMoneda { get; set; }
        public string? EstadoTransaccion { get; set; }
        public string? Observacion { get; set; }
        public string? Entregado { get; set; }
        public string? UsuarioSup { get; set; }
        public string? ObservacionSup { get; set; }
        public string? NroBoleta { get; set; }
        public string? TipoOperacion { get; set; }
        public decimal? RepId { get; set; }
        public decimal? EstadoRegistral { get; set; }
        public decimal? NroOficio { get; set; }
        public string? TotalParcial { get; set; }
        public Guid Rowid { get; set; }
        public decimal IdLevantamiento { get; set; }

        public virtual Usuario? IdUsuarioNavigation { get; set; }
    }
}
