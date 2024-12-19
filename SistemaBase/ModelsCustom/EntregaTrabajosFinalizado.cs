using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.ModelsCustom
{
    public class EntregaTrabajosFinalizado
    {
        [Key]

        [DisplayName("Nro Entrada")]
        public int NroEntrada { get; set; }

        [DisplayName("Descripcion Solicitud")]
        public string? DescSolicitud { get; set; }

        [DisplayName("Fecha Alta")]
        public DateTime? FechaAlta { get; set; }

        [DisplayName("Nombre Usuario")]
        public string? UserName { get; set; }
    }
}
