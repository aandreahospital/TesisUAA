using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Datoslaborale
    {
        public int Iddatoslaborales { get; set; }
        public string CodUsuario { get; set; } = null!;
        public string? Lugartrabajo { get; set; }
        public string? Universidadtrabajo { get; set; }
        public string? Materia { get; set; }
        public string? Sector { get; set; }
        public int? CargoIdcargo { get; set; }
        public string? Direccionlaboral { get; set; }
        public string? Herramientas { get; set; }
        public string? Antiguedad { get; set; }
        public string? Estado { get; set; }
        public Guid Rowid { get; set; }
        public string? Cargo { get; set; }

        public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
    }
}
