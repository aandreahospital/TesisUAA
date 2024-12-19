using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace SistemaBase.ModelsCustom
{
    public partial class EntradaPorOficioCustom
    {
        [Display(Name = "Numero Entrada")]
        public decimal? NumeroEntrada { get; set; }
        [Display(Name = "Codigo Oficina")]
        public decimal CodigoOficina { get; set; }
        [Display(Name = "Id Profesional")]
        public decimal TipoDocumento { get; set; }
        public decimal IdAutorizante { get; set; }
        public decimal TipoSolicitud { get; set; }
        [Display(Name = "Titular Propietario:")]
        public string NomTitular { get; set; } = null!;
        public string? TipoDocumentoTitular { get; set; }
        public string? NroDocumentoTitular { get; set; }

        [Display(Name = "Numero Boleta:")]
        public string NroBoleta { get; set; } = null!;
        [Display(Name = "Nombre Presentador:")]
        public string NombrePresentador { get; set; } = null!;
        [Display(Name = "Tipo Documento Presentador:")]
        public decimal TipoDocumentoPresentador { get; set; } 
        [Display(Name = "Nro Documento Presentador:")]
        public string NroDocumentoPresentador { get; set; } = null!;
        [Display(Name = "Nro Señal:")]
        public string NroSenal { get; set; } = null!;
        [Display(Name = "Nro de Oficina:")]
        public string? NroOficio { get; set; } = null!;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? FechaEntrada { get; set; }
        public string? EsPresentador { get; set; }
        public string? UsuarioEntrada { get; set; }
        public decimal? EstadoEntrada { get; set; }
        public string? ImageDataUri { get; set; }
        public string? Barcode { get; set; }
        public decimal? MatriculaAutorizante { get; set; }
        public string? NombreAutorizante { get; set; }
        public string? Comentario { get; set; }

        public List<SelectListItem>? Oficinas { get; set; }
        public List<SelectListItem> ?TiposSolicitud { get; set; }
        public List<SelectListItem> ?Autorizantes { get; set; }
        public List<SelectListItem>? DocumentoTitular { get; set; }
        public List<SelectListItem>? DocumentoPresentador { get; set; }
    }
    
    public class DatosModeloTest
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
