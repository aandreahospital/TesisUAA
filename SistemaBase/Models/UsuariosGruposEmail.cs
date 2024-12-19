using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class UsuariosGruposEmail
    {
        public string? CodGrupo { get; set; }
        public string? CodUsuario { get; set; }
        public Guid Rowid { get; set; }
    }
}
