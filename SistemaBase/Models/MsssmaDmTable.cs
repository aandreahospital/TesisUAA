using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class MsssmaDmTable
    {
        public int ObjectId { get; set; }
        public int SchemaId { get; set; }
        public byte? Status { get; set; }
        public DateTime DmStartTime { get; set; }
    }
}
