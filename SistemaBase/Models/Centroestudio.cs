using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class CentroEstudio
    {
        //public CentroEstudio()
        //{
        //    DatosAcademicos = new HashSet<DatosAcademico>();
        //}

        public int IdCentroEstudio { get; set; }
        public string? Descripcion { get; set; }

        //public virtual ICollection<DatosAcademico> DatosAcademicos { get; set; }
    }
}
