using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class SrsImg
    {
        public decimal ImageId { get; set; }
        public string? Name { get; set; }
        public byte[]? XmlData { get; set; }
        public Guid Rowid { get; set; }
    }
}
