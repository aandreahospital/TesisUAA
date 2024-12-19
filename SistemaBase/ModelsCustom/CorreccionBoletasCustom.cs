using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaBase.ModelsCustom
{
    public class CorreccionBoletasCustom
    {

        [Key]
        [Display(Name = "Nro Entrada")]
        public decimal NroEntrada { get; set; }

        [Display(Name = "Tipo De la Solicitud")]
        public decimal? Tipo_Solicitud { get; set; }
        public string? DescSolicitud { get; set; }

        [Display(Name = "Fecha Alta")]
        public DateTime? FechaAlta { get; set; }

        [Display(Name = "Estado")]
        public string? DescEstado { get; set; }

        [Display(Name = "Observacion")]
        public string? Observacion { get; set; }

        [Display(Name = "Nombre Operador")]
        public string? NombreOperador { get; set; }
    }
}

