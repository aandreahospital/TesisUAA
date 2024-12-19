using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmImagenTemp
    {
        public string? IdUsuario { get; set; }
        public byte[]? MarcaBlob { get; set; }
        public byte[]? SenalBlob { get; set; }
        public string? Senal { get; set; }
        public string? Marca { get; set; }
        public string? MarcaSig { get; set; }
        public Guid Rowid { get; set; }
    }
}
