using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Forma
    {
        public string CodModulo { get; set; } = null!;
        public string NomForma { get; set; } = null!;
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public Guid Rowid { get; set; }
        public decimal Id { get; set; }

        public virtual Modulo CodModuloNavigation { get; set; } = null!;
    }
}
