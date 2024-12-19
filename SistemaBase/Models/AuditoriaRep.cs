using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class AuditoriaRep
    {
        public string? CodEmpresa { get; set; }
        public string? NomReporte { get; set; }
        public DateTime? Fecha { get; set; }
        public string? CodUsuario { get; set; }
        public string? Comentario { get; set; }
        public string? Maquina { get; set; }
        public Guid Rowid { get; set; }
    }
}
