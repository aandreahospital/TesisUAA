using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmParametrosNotificado
    {
        public string Parametro { get; set; } = null!;
        public string? Valor { get; set; }
        public string? CodModulo { get; set; }
        public string? Operacion { get; set; }
        public decimal? NroOperacion { get; set; }
        public string? OldParametro { get; set; }
        public string? OldCodModulo { get; set; }
        public string? OldValor { get; set; }
        public string? Notificado { get; set; }
        public string? OldNotificado { get; set; }
    }
}
