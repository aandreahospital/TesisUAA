using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class OeClCoejecutor
    {
        public decimal CoeCodigo { get; set; }
        public decimal? PreCodigo { get; set; }
        public string CoeDescripcion { get; set; } = null!;
        public Guid Rowid { get; set; }
        public virtual PreMaPrestamo? PreCodigoNavigation { get; set; }
    }
}
