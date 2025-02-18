using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class OfertaLaboral
    {
        public int IdOfertaLaboral { get; set; }
        public string CodUsuario { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string? Estado { get; set; }
        public string? Contacto { get; set; }

        public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
    }
}
