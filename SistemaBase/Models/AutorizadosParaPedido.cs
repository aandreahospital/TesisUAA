using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class AutorizadosParaPedido
    {
        public string? CodPersona { get; set; }
        public string? Nombre { get; set; }
        public string? CedulaIdentidad { get; set; }
        public Guid Rowid { get; set; }
    }
}
