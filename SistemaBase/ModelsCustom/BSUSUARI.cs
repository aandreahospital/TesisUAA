using System.ComponentModel.DataAnnotations;
namespace SistemaBase.ModelsCustom
{
    public class BSUSUARI
    {
        [Key]
        public string CodUsuario { get; set; } = null!;
        public string Usuario { get; set; } = null!;
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string? Clave { get; set; }
        [Display(Name = "Grupo")]
        public string? CodGrupo { get; set; }
        [Display(Name = "Descripción")]
        public string? DescripcionGrupo { get; set; }
        [Display(Name = "Sucursal")]
        public string? DescripcionSucursal { get; set; }
        [Display(Name = "Custodio")]
        public string? CodCustodio { get; set; }
        [Display(Name = "Stock")]
        public bool? AutorizaStock { get; set; }
        [Display(Name = "CtaCte")]
        public bool? AutorizaCtacte { get; set; }
        public bool? Estado { get; set; }
    }
}
