using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.ModelsCustom
{
    public class Direccion
    {
        [Key]

        [DisplayName("Nro Entrada")]
        public int NroEntrada { get; set; }

        [DisplayName("Descripcion Solicitud")]
        public string? DescSolicitud { get; set; }

        [DisplayName("Fecha/Hora")]
        public DateTime? FechaAlta { get; set; }
        [DisplayName("Oficina")]
        public string? Oficina { get; set; }


    }
}
