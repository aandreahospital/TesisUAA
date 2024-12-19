using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class UsuariosCustodio
    {
        public string CodEmpresa { get; set; } = null!;
        public string CodUsuario { get; set; } = null!;
        public string CodCustodio { get; set; } = null!;
        public Guid Rowid { get; set; }
    }
}
