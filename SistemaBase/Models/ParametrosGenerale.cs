using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SistemaBase.Models
{
    public partial class ParametrosGenerale
    {
        [Display(Name = "Parámetro")]
        public string Parametro { get; set; } = null!;
        [Display(Name ="Módulo")]
        public string CodModulo { get; set; } = null!;
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
        public string? Valor { get; set; }
        [ScaffoldColumn(false)]
        public string? Notificado { get; set; }
        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
        public virtual Modulo CodModuloNavigation { get; set; } = null!;
    }
}
