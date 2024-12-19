using System.ComponentModel.DataAnnotations;

namespace SistemaBase.ModelsCustom
{
    public partial class Diseño
    {
        [Display(Name ="Numero Entrada")]
        public int NroEntrada { get; set; }

        [Display(Name ="Tipo Solicitud")]
        public string? TipoSolicitud { get; set; } = null;
        [Display(Name ="Fecha/Hora Ingreso")]
        public DateTime? FechaIngreso { get; set; }
        public string? Recibido { get; set; }
        public string? Desasignado { get; set; }
        public string? UsuarioAsignado { get; set; }


        public string StrNroEntrada { get; set; }

    }
}
