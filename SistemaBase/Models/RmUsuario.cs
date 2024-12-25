using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class RmUsuario
    {
        public decimal IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public decimal? CodigoOficina { get; set; }
        public DateTime? FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public decimal? IdEstado { get; set; }
        public decimal? IdTipoUsuario { get; set; }
        public string? Username { get; set; }
        public string? PasswordUsuario { get; set; }
        public string? UsuarioBaseDatos { get; set; }
        public string? PasswordBaseDatos { get; set; }
        public string? Email { get; set; }
        public Guid Rowid { get; set; }
    }
}
