﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.ModelsCustom
{
    public partial class RecepcionDocumentoCustom
    {
        [Key]

        [DisplayName("Nro Entrada")]
        public int NroEntrada { get; set; }

        [DisplayName("Id Tipo Solicitud")]
        public decimal? TipoSolicitud { get; set; }

        [DisplayName("Descripcion Solicitud")]
        public string? DescSolicitud { get; set; }

        [DisplayName("Descripcion Oficina")]
        public string? DescOficina { get; set; }

        [DisplayName("Fecha/Hora Ingreso")]
        public DateTime? FechaEntrada { get; set; }
        
    }

    


}