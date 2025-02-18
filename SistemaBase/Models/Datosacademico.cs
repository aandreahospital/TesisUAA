using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class DatosAcademico
    {
        public int IdDatosAcademicos { get; set; }
        public string CodUsuario { get; set; } = null!;
        public int IdCentroEstudio { get; set; }
        public int IdCarrera { get; set; }
        public string? AnhoInicio { get; set; }
        public string? AnhoFin { get; set; }
        public string? Estado { get; set; }
        //public string? Carrera { get; set; }
        //public string? CentroEstudio { get; set; }

        //public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
        //public virtual Carrera IdCarreraNavigation { get; set; } = null!;
        //public virtual CentroEstudio IdCentroEstudioNavigation { get; set; } = null!;
    }
}
