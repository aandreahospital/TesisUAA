using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmInforme
    {
        //public RmInforme()
        //{
        //    RmInformeDets = new HashSet<RmInformeDet>();
        //}
        public decimal IdInforme { get; set; }
        public decimal? NroEntrada { get; set; }
        public decimal? IdAutorizante { get; set; }
        public DateTime? FechaCertificacion { get; set; }
        public DateTime? FechaCaducidad { get; set; }
        public string? EstadoInforme { get; set; }
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
        public virtual RmMesaEntradum? NroEntradaNavigation { get; set; }
        public virtual ICollection<RmInformeDet>? RmInformeDets { get; set; }
    }
}
