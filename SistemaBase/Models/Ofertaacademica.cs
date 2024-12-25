using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Ofertaacademica
    {
        public int Idofertaacademica { get; set; }
        public string CodUsuario { get; set; } = null!;
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public DateTime? Fechacierre { get; set; }
        public string? Estado { get; set; }
        public Guid Rowid { get; set; }

        public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
    }
}
