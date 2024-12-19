using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class MsssmaDmNckeyColumn
    {
        public int ObjectId { get; set; }
        public string KeyName { get; set; } = null!;
        public string KeyType { get; set; } = null!;
        public int KeyColumnId { get; set; }
        public int? IndexColumnId { get; set; }
        public bool? IsDisabled { get; set; }
        public int? ReferencedObjectId { get; set; }
        public byte? DeleteReferentialAction { get; set; }
        public byte? UpdateReferentialAction { get; set; }
        public bool? IsNotForReplication { get; set; }
        public bool? IsNotTrusted { get; set; }
        public int? ReferencedColumnId { get; set; }
    }
}
