using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class AccesosGrupo
    {
        public string CodGrupo { get; set; } = null!;
        public string CodModulo { get; set; } = null!;
        public string NomForma { get; set; } = null!;
        public string? PuedeInsertar { get; set; }
        public string? PuedeBorrar { get; set; }
        public string? PuedeActualizar { get; set; }
        public string? PuedeConsultar { get; set; }
        public string? ItemMenu { get; set; }
        public Guid Rowid { get; set; }

        public virtual GruposUsuario CodGrupoNavigation { get; set; } = null!;
        public virtual Modulo CodModuloNavigation { get; set; } = null!;
    }
}
