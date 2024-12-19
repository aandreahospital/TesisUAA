using System.ComponentModel.DataAnnotations;

namespace SistemaBase.ModelsCustom
{
    public class Online
    {
        [Display(Name = "Numero Entrada")]
        public decimal? NumeroEntrada { get; set; }
        public int? NroEntrada { get; set; }

        [Display(Name = "Total Bruto")]
        public double? TotalBruto { get; set; }

        [Display(Name = "Exoneración")]
        public string? Exoneracion { get; set; }


        [Display(Name = "Monto Liquidación")]
        public decimal? MontoLiquidacion { get; set; }

        [Display(Name = "Codigo Oficina")]
        public decimal CodigoOficina { get; set; }

        [Display(Name = "Número Liquidación")]
        public string? NumeroLiquidacion { get; set; }

        public decimal TipoSolicitud { get; set; }
        public string? DescTipoSolicitud { get; set; }

        public string? DescOficina { get; set; }
        public string? NombreAutorizante { get; set; }
        public decimal? MatriculaAutorizante { get; set; }

        [Display(Name = "Titular Propietario:")]
        public string NomTitular { get; set; } = null!;

        public string? IdTipoDocumentoTitular { get; set; }

        [Display(Name = "Número Documento Titular:")]
        public string? NroDocumentoTitular { get; set; }

        [Display(Name = "Nombre Presentador:")]
        public string? NombrePresentador { get; set; }

        [Display(Name = "Tipo Documento Presentador:")]
        public decimal TipoDocumentoPresentador { get; set; }

        [Display(Name = "Nro Documento Presentador:")]
        public string NroDocumentoPresentador { get; set; } = null!;

        [Display(Name = "Nro Boleta:")]
        public string? NroBoleta { get; set; }

        [Display(Name = "Titular es Presentador:")]
        public string? EsPresentador { get; set; }
        //public string? ImageDataUri { get; set; }
        public string? Barcode { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? FechaEntrada { get; set; }

        //public decimal? EstadoEntrada { get; set; }
        //public string? Comentario { get; set; }

        [Required(ErrorMessage = "Selecciona un archivo PDF.")]
        [Display(Name = "Liquidacion PDF")]
        public byte[]? ArchivoPDF { get; set; }
        [Display(Name = "Anexo PDF")]
        public byte[]? AnexoPDF { get; set; }

    }
}
