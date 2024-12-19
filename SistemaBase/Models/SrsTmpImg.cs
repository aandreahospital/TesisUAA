using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class SrsTmpImg
    {
        public string TUid { get; set; } = null!;
        public decimal TPid { get; set; }
        public decimal TIid { get; set; }
        public byte[]? XmlData { get; set; }
    }
}
