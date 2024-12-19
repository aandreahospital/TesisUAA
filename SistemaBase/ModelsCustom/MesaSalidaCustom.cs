using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.ModelsCustom
{
    public class MesaSalidaCustom
    {
        [Display(Name ="Numero Entrada")]
        public decimal NumeroEntrada { get; set; }
        [Display(Name ="Fecha Salida")]
        public DateTime? FechaSalida { get; set; }
        [Display(Name ="Nro Formulario")]
        public decimal? NroFormulario { get; set; }
        [Display(Name ="Oficina")]
        public decimal? CodigoOficina { get; set; }
        [Display(Name = "Oficina")]
        public decimal? CodOficinaRetiro { get; set; }
        [Display(Name ="Tipo Solicitud")]
        public decimal? TipoSolicitud { get; set; }
        [Display(Name ="Estado Entrada")]
        public decimal? EstadoEntrada { get; set; }
        [Display(Name ="Nombre Titular")]
        public string? NomTitular { get; set; }
        [Display(Name ="Retirado Por:")]
        public string? NombreRetirador { get; set; }
        [Display(Name ="Tipo Documento")]
        public decimal? TipoDocumentoRetirador { get; set; }
        [Display(Name ="Nro Documento")]
        public string? NroDocumentoRetirador { get; set; }
        public string? ImageDataUri { get; set; }

        public List<SelectListItem>? Oficinas { get; set; }
        public List<SelectListItem>? TiposSolicitud { get; set; }
        public List<SelectListItem>? EstadosEntrada { get; set; }
        public List<SelectListItem>? TiposDocumentos { get; set; }

    }
}
