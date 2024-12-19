using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class SrsTmpImgDatum
    {
        public string TUid { get; set; } = null!;
        public decimal TPid { get; set; }
        public decimal TIid { get; set; }
        public decimal TSeq { get; set; }
        public string? ImageSig { get; set; }
        public string? ImageData { get; set; }
        public byte[]? RawData { get; set; }
    }
}
