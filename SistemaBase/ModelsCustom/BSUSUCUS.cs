using System.ComponentModel.DataAnnotations;
namespace SistemaBase.ModelsCustom
{
    public class BSUSUCUS
    {
        [Key]
        [Display(Name = "Usuario")]
        public string? CodigoUsuario { get; set; }
        [Display(Name = "Persona")]
        public string? CodigoPersona { get; set; }
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }
        [Display(Name = "Custodio")]
        public string? CodigoCustodio { get; set; }
        [Display(Name = "Nombre Custodio")]
        public string? DescripcionCustodio { get; set; }
    }
}
