using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Empresa
    {
        public Empresa()
        {
            Calendarios = new HashSet<Calendario>();
            SubtiposTrans = new HashSet<SubtiposTran>();
            Sucursales = new HashSet<Sucursale>();
            TiposTrans = new HashSet<TiposTran>();
            Usuarios = new HashSet<Usuario>();
            UsuariosBkps = new HashSet<UsuariosBkp>();
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
        public virtual ICollection<Calendario> Calendarios { get; set; }
        public virtual ICollection<SubtiposTran> SubtiposTrans { get; set; }
        public virtual ICollection<Sucursale> Sucursales { get; set; }
        public virtual ICollection<TiposTran> TiposTrans { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<UsuariosBkp> UsuariosBkps { get; set; }
    }
}
