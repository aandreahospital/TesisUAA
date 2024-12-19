using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class PreMaPrestamo
    {
        public PreMaPrestamo()
        {
            OeClCoejecutors = new HashSet<OeClCoejecutor>();
        }
        public decimal PreCodigo { get; set; }
        public Guid Rowid { get; set; }
        public virtual ICollection<OeClCoejecutor> OeClCoejecutors { get; set; }
    }
}
