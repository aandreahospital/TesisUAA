using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class SrsTmpRaw
    {
        public string TUid { get; set; } = null!;
        public decimal TPid { get; set; }
        public decimal TIid { get; set; }
        public byte[]? TData { get; set; }
    }
}
