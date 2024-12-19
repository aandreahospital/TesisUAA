using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Inscripcion
    {
        public double IdAlumno { get; set; }
        public double IdMateria { get; set; }
        public Guid Rowid { get; set; }
        public virtual Materium IdMateriaNavigation { get; set; } = null!;
    }
}
