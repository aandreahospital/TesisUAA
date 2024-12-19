using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Sectore
    {

        public string CodEmpresa { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? IndGestion { get; set; }
        public Guid Rowid { get; set; }
        public string CodSector { get; set; } = null!;

        public virtual Empresa CodEmpresaNavigation { get; set; } = null!;
    }
}
