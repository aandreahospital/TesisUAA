using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmCertificacione
    {
        public decimal? NroEntrada { get; set; }
        public decimal? IdAutorizante { get; set; }
        public DateTime? FechaCertificacion { get; set; }
        public DateTime? FechaCaducidad { get; set; }
        public string? EstadoCertificacion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public decimal? NroBoleta { get; set; }
        public string? Observacion { get; set; }
        public string? ObservacionSup { get; set; }
        public string? EstadoTransaccion { get; set; }
        public string? TipoOperacion { get; set; }
        public string? IdUsuario { get; set; }
        public decimal? EstadoRegistral { get; set; }
        public string? UsuarioSup { get; set; }
        public decimal? RepId { get; set; }
        public string? Entregado { get; set; }
        public string? IdBeneficiario { get; set; }
        public decimal? IdMarca { get; set; }
        public string? NombreEscribano { get; set; }
        public decimal? NroBoletaSenal { get; set; }
        public Guid Rowid { get; set; }
        public decimal IdCertificacion { get; set; }
    }
}
