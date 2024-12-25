using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Empresa
    {
        public Empresa()
        {
            Sucursales = new HashSet<Sucursale>();
            Usuarios = new HashSet<Usuario>();
        }

        public string CodEmpresa { get; set; } = null!;
        public string? CodMonedaOrigen { get; set; }
        public string? CodPerJuridica { get; set; }
        public string? TituloReportes { get; set; }
        public string? Descripcion { get; set; }
        public string? UsaCalendario { get; set; }
        public string? RucEmpresa { get; set; }
        public string? Direccion { get; set; }
        public string? Actividad { get; set; }
        public string? NroPatronal { get; set; }
        public string? Localidad { get; set; }
        public string? Departamento { get; set; }
        public Guid Rowid { get; set; }

        public virtual ICollection<Sucursale> Sucursales { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
