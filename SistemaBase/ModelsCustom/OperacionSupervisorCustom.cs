using System;
using System.Collections.Generic;
using SistemaBase.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Text.Json.Serialization;

namespace SistemaBase.ModelsCustom
{
    public class OperacionSupervisorCustom
    {
        [Display(Name ="Nro Entrada")]
        public int NroEntrada { get; set; }

        [Display(Name = "Tipo De Solicitud")]
        public decimal? TipoSolicitud { get; set; }

        [Display(Name = "Descripcion De Solicitud")]
        public string? DescSolicitud { get; set; }

        [Display(Name = "Fecha Alta")]
        public DateTime? FechaAlta { get; set; }

        [Display(Name = "Estado")]
        public string? DescEstado { get; set; }

        [Display(Name = "Observacion")]
        public string? Observacion { get; set; }

        [Display(Name = "Nombre Operador")]
        public string? NombreOperador { get; set; }
        [Display(Name = "Nombre Titular")]
        public string? NombreTitular { get; set; }
        public string? ImageDataUri { get; set; }

    }
}
