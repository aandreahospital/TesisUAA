using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class MesaEntradaViej
    {
        public decimal NumeroEntrada { get; set; }
        public DateTime? FechaEntrada { get; set; }
        public decimal CodigoOficina { get; set; }
        public decimal? NumeroLiquidacion { get; set; }
        public decimal TipoSolicitud { get; set; }
        public decimal? NroFormulario { get; set; }
        public decimal EstadoEntrada { get; set; }
        public string? NombrePresentador { get; set; }
        public decimal? TipoDocumentoPresentador { get; set; }
        public string? NroDocumentoPresentador { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string? NombreRetirador { get; set; }
        public decimal? TipoDocumentoRetirador { get; set; }
        public string? NroDocumentoRetirador { get; set; }
        public string? UsuarioEntrada { get; set; }
        public string? UsuarioSalida { get; set; }
    }
}
