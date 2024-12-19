using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Memito
    {
        public decimal? OtNumero { get; set; }
        public string? Memo1 { get; set; }
        public string? Memo2 { get; set; }
        public Guid Rowid { get; set; }
    }
}
