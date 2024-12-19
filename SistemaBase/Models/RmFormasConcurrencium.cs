using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmFormasConcurrencium
    {
        public string CodFormaConcurrencia { get; set; } = null!;
        public string? DescFormaConcurrencia { get; set; }
        public Guid Rowid { get; set; }
    }
}
