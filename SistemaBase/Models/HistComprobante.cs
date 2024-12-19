using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class HistComprobante
    {
        public string? CodEmpresa { get; set; }
        public DateTime? FecEnvio { get; set; }
        public string? TipComprobante { get; set; }
        public string? SerComprobante { get; set; }
        public decimal? NroComprobante { get; set; }
        public decimal? Copia { get; set; }
        public string? CodAreaEnt { get; set; }
        public string? CodUsuarioEnt { get; set; }
        public string? CodSucursalSal { get; set; }
        public string? CodAreaSal { get; set; }
        public string? CodUsuarioSal { get; set; }
        public string? Recibido { get; set; }
        public DateTime? FecRecibido { get; set; }
        public string? Enviado { get; set; }
        public DateTime? FecEnviado { get; set; }
        public string? Observacion { get; set; }
        public string? CodAreaEnv { get; set; }
        public decimal? Paso { get; set; }
        public Guid Rowid { get; set; }
    }
}
