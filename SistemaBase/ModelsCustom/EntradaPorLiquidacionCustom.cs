﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaBase.ModelsCustom
{
    public partial class EntradaPorLiquidacionCustom
    {
        [Display(Name = "Numero Entrada")]
        public decimal? NumeroEntrada { get; set; }

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

        public decimal IdAutorizante { get; set; }
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
        public string? ImageDataUri { get; set; }
        public string? Barcode { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? FechaEntrada { get; set; }

        public decimal? EstadoEntrada { get; set; }
        public string? Comentario { get; set; }
        public string? UsuarioEntrada { get; set; }



        public List<SelectListItem>? Oficinas { get; set; }

        public List<SelectListItem>? TiposSolicitud { get; set; }

        public List<SelectListItem>? Autorizantes { get; set; }


        public List<SelectListItem>? TipoDocumentoTitular { get; set; }

        public List<SelectListItem>? DocumentoPresentador { get; set; }


    }
}