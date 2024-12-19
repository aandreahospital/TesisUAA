using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class UsuariosBkp
    {
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
        public Guid Rowid { get; set; }
        public virtual Empresa? CodEmpresaNavigation { get; set; }
    }
}
