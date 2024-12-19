using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.ModelsCustom
{
    public class ReingresoEntradaCustom
    {
        public decimal? NroEntrada { get; set; }
        public string? Observacion { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string? UsuarioSalida { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? FechaReingreso { get; set; }
        public string? UsuarioReingreso { get; set; }
        public string? UsuarioInscriptor { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string? NombreReingresante { get; set; }
        public string? DocReingresante { get; set; }
        public string? TipoDocReingresante { get; set; }
        public decimal IdReingreso { get; set; }
        public decimal? MontoLiquidacion { get; set; }
        public decimal CodigoOficina { get; set; }
        public string? NumeroLiquidacion { get; set; }
        public decimal TipoSolicitud { get; set; }
        public string? NombrePresentador { get; set; }
        public decimal TipoDocumentoPresentador { get; set; }
        public string NroDocumentoPresentador { get; set; } = null!;
        public string? NroBoleta { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? FechaEntrada { get; set; }
        public string? ImageDataUri { get; set; }
        public string? Barcode { get; set; }
        public string? NombreAutorizante { get; set; }
        public string? Comentario { get; set; }
        public List<SelectListItem>? Oficinas { get; set; }

        public List<SelectListItem>? TiposSolicitud { get; set; }

        public List<SelectListItem>? TipoDocumentoReingresante { get; set; }
        public List<SelectListItem>? DocumentoPresentador { get; set; }

    }
}
