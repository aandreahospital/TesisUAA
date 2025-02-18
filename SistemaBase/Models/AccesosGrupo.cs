using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class AccesosGrupo
    {
        public string CodGrupo { get; set; } = null!;
        public string CodModulo { get; set; } = null!;
        public string NomForma { get; set; } = null!;

        public virtual Forma Forma { get; set; } = null!;
    }
}
