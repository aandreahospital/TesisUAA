using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmMesaEntradaDup
    {
        public decimal NumeroEntrada { get; set; }
        public DateTime? FechaEntrada { get; set; }
        public decimal? CodigoOficina { get; set; }
        public decimal? TipoSolicitud { get; set; }
        public decimal? NroFormulario { get; set; }
        public decimal? EstadoEntrada { get; set; }
        public string? NombrePresentador { get; set; }
        public decimal? TipoDocumentoPresentador { get; set; }
        public string? NroDocumentoPresentador { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string? NombreRetirador { get; set; }
        public decimal? TipoDocumentoRetirador { get; set; }
        public string? NroDocumentoRetirador { get; set; }
        public string? UsuarioEntrada { get; set; }
        public string? UsuarioSalida { get; set; }
        public string? NomTitular { get; set; }
        public decimal? IdAutorizante { get; set; }
        public decimal? NroEntradaOriginal { get; set; }
        public string? Reingreso { get; set; }
        public string? NombreReingresante { get; set; }
        public string? DocReingresante { get; set; }
        public decimal? TipoDocReingresante { get; set; }
        public string? NroDocumentoTitular { get; set; }
        public string? TipoDocumentoTitular { get; set; }
        public string? NroBoleta { get; set; }
        public string? Impreso { get; set; }
        public string? NroOficio { get; set; }
        public string? TipoDocumento { get; set; }
        [ScaffoldColumn(false)]
        public decimal NumeroLiquidacion { get; set; }
    }
}
