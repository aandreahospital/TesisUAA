using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class SrsTmpRe
    {
        public string TUid { get; set; } = null!;
        public decimal TPid { get; set; }
        public decimal TIid { get; set; }
        public decimal TSeq { get; set; }
        public decimal ImageId { get; set; }
        public decimal ImageSeq { get; set; }
        public decimal ResScore { get; set; }
    }
}
