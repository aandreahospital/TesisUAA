using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Datosacademico
    {
        public int Iddatosacademicos { get; set; }
        public string CodUsuario { get; set; } = null!;
        public int Idcentroestudio { get; set; }
        public int Idcarrera { get; set; }
        public DateTime? Fechainicio { get; set; }
        public DateTime? Fechafin { get; set; }
        public string? Estado { get; set; }
        public Guid Rowid { get; set; }

        public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
    }
}
