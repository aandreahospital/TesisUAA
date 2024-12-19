using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace SistemaBase.Models
{
    public partial class AccesosGrupo
    {
        [Display(Name = "Grupo:")]
        public string CodGrupo { get; set; } = null!;
        [Display(Name = "Modulo:")]
        public string CodModulo { get; set; } = null!;
        [Display(Name = "Formulario:")]
        public string NomForma { get; set; } = null!;
        [Display(Name = "Puede Insertar:")]
        public string? PuedeInsertar { get; set; }
        [Display(Name = "Puede Borrar:")]
        public string? PuedeBorrar { get; set; }
        [Display(Name = "Puede Actualizar:")]
        public string? PuedeActualizar { get; set; }
        [Display(Name = "Puede Consultar:")]
        public string? PuedeConsultar { get; set; }
        [ScaffoldColumn(false)]
        public string? ItemMenu { get; set; }
        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
        [ScaffoldColumn(false)]
        public virtual GruposUsuario CodGrupoNavigation { get; set; } = null!;
        [ScaffoldColumn(false)]
        public virtual Modulo CodModuloNavigation { get; set; } = null!;

    }
}
