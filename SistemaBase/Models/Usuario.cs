using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Cargos = new HashSet<Cargo>();
            CarreraUsuarios = new HashSet<CarreraUsuario>();
            Comentarios = new HashSet<Comentario>();
            Datosacademicos = new HashSet<Datosacademico>();
            Datoslaborales = new HashSet<Datoslaborale>();
            Forodebates = new HashSet<Forodebate>();
            Ofertaacademicas = new HashSet<Ofertaacademica>();
            Ofertalaborals = new HashSet<Ofertalaboral>();
        }

        public string CodUsuario { get; set; } = null!;
        public string? CodPersona { get; set; }
        public string? Clave { get; set; }
        public string? CodGrupo { get; set; }
        public string? CodEmpresa { get; set; }
        public string? CodSucursal { get; set; }
        public string? Estado { get; set; }
        public string? CodArea { get; set; }
        public string? CodCustodio { get; set; }
        public string? CodColorRegistro { get; set; }
        public string? AutorizaStock { get; set; }
        public string? AutorizaCtacte { get; set; }
        public string? EMail { get; set; }
        public DateTime? FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public decimal? IdTipoUsuario { get; set; }
        public decimal? CodOficina { get; set; }
        public Guid Rowid { get; set; }

        public virtual Sucursale? Cod { get; set; }
        public virtual Empresa? CodEmpresaNavigation { get; set; }
        public virtual GruposUsuario? CodGrupoNavigation { get; set; }
        public virtual ICollection<Cargo> Cargos { get; set; }
        public virtual ICollection<CarreraUsuario> CarreraUsuarios { get; set; }
        public virtual ICollection<Comentario> Comentarios { get; set; }
        public virtual ICollection<Datosacademico> Datosacademicos { get; set; }
        public virtual ICollection<Datoslaborale> Datoslaborales { get; set; }
        public virtual ICollection<Forodebate> Forodebates { get; set; }
        public virtual ICollection<Ofertaacademica> Ofertaacademicas { get; set; }
        public virtual ICollection<Ofertalaboral> Ofertalaborals { get; set; }
    }
}
