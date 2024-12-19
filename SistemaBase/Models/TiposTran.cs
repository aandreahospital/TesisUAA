using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class TiposTran
    {
        public TiposTran()
        {
            SubtiposTrans = new HashSet<SubtiposTran>();
        }
        public string CodEmpresa { get; set; } = null!;
        public string CodModulo { get; set; } = null!;
        public decimal TipoTrans { get; set; }
        public string? Contabiliza { get; set; }
        public string? Estado { get; set; }
        public string? ImprimeComprob { get; set; }
        public string? CostoXServic { get; set; }
        public string? Descripcion { get; set; }
        public Guid Rowid { get; set; }
        public virtual Empresa CodEmpresaNavigation { get; set; } = null!;
        public virtual Modulo CodModuloNavigation { get; set; } = null!;
        public virtual ICollection<SubtiposTran> SubtiposTrans { get; set; }
    }
}
