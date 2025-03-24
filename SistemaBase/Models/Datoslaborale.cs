using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class DatosLaborale
    {
        public int IdDatosLaborales { get; set; }
        public string CodUsuario { get; set; } = null!;
        public string? LugarTrabajo { get; set; }
        public string? Cargo { get; set; }
        public int? Antiguedad { get; set; }
        public string? Herramientas { get; set; }

        public string? Sector { get; set; }
        public string? UniversidadTrabajo { get; set; }
        public string? Estado { get; set; }

        public string? MateriaTrabajo { get; set; }

        public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
    }
}
