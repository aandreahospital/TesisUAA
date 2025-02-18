using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Carrera
    {
        public Carrera()
        {
            DatosAcademicos = new HashSet<DatosAcademico>();
        }

        public int IdCarrera { get; set; }
        public string? Descripcion { get; set; }

        public virtual ICollection<DatosAcademico> DatosAcademicos { get; set; }
    }
}
