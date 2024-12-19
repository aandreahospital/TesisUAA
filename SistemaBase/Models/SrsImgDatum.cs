using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class SrsImgDatum
    {
        public decimal ImageId { get; set; }
        public decimal ImageSeq { get; set; }
        public string? ImageSig { get; set; }
        public string? ImageData { get; set; }
        public byte[]? RawData { get; set; }
    }
}
