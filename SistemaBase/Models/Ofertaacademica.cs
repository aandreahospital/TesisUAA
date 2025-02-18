using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class OfertaAcademica
    {
        public int IdOfertaAcademica { get; set; }
        public string CodUsuario { get; set; } = null!;
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string? Estado { get; set; }

        public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
    }
}
