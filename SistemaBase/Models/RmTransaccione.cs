using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SistemaBase.Models
{
    public partial class RmTransaccione
    {
        [JsonPropertyName("IdTransaccion")]
        public Int32 IdTransaccion { get; set; }
        [JsonPropertyName("IdMarca")]
        public decimal? IdMarca { get; set; }
        [JsonPropertyName("NumeroEntrada")]
        public decimal NumeroEntrada { get; set; }
        [JsonPropertyName("TipoOperacion")]
        public string? TipoOperacion { get; set; }
        [JsonPropertyName("FecActoJuridico")]
        public DateTime? FecActoJuridico { get; set; }
        [JsonPropertyName("FecAlta")]
        public DateTime? FecAlta { get; set; }
        [JsonPropertyName("Representante")]
        public string? Representante { get; set; }
        [JsonPropertyName("TipoMoneda")]
        public string? TipoMoneda { get; set; }
        [JsonPropertyName("MontoOperacion")]
        public decimal? MontoOperacion { get; set; }
        [JsonPropertyName("Semejanza")]
        public string? Semejanza { get; set; }
        [JsonPropertyName("FormaConcurrente")]
        public string? FormaConcurrente { get; set; }
        [JsonPropertyName("Imagen")]
        public byte[]? Imagen { get; set; }
        [JsonPropertyName("UsuarioSup")]
        public string? UsuarioSup { get; set; }
        [JsonPropertyName("EstadoTransaccion")]
        public string? EstadoTransaccion { get; set; }
        [JsonPropertyName("FormaConcurre")]
        public string? FormaConcurre { get; set; }
        [JsonPropertyName("FechaAlta")]
        public DateTime? FechaAlta { get; set; }
        [JsonPropertyName("FechaActoJuridico")]
        public DateTime? FechaActoJuridico { get; set; }
        [JsonPropertyName("IdUsuario")]
        public string? IdUsuario { get; set; }
        [JsonPropertyName("Observacion")]
        public string? Observacion { get; set; }
        [JsonPropertyName("IdAutorizante")]
        public decimal? IdAutorizante { get; set; }
        [JsonPropertyName("Serie")]
        public string? Serie { get; set; }
        [JsonPropertyName("NroBoleta")]
        public string? NroBoleta { get; set; }
        [JsonPropertyName("IdEscribanoJuez")]
        public string? IdEscribanoJuez { get; set; }
        [JsonPropertyName("NroEscrituraDsAi")]
        public string? NroEscrituraDsAi { get; set; }
        [JsonPropertyName("FechaEscrituraDsAi")]
        public DateTime? FechaEscrituraDsAi { get; set; }
        [JsonPropertyName("Entregado")]
        public string? Entregado { get; set; }
        [JsonPropertyName("RepId")]
        public decimal? RepId { get; set; }
        [JsonPropertyName("ObservacionSup")]
        public string? ObservacionSup { get; set; }
        [JsonPropertyName("IdTranfe")]
        public string? IdTranfe { get; set; }
        [JsonPropertyName("EstadoRegistral")]
        public decimal? EstadoRegistral { get; set; }
        [JsonPropertyName("NroOficio")]
        public decimal? NroOficio { get; set; }
        [JsonPropertyName("FirmanteRuego")]
        public string? FirmanteRuego { get; set; }
        [JsonPropertyName("NombreRepresentante")]
        public string? NombreRepresentante { get; set; }
        [JsonPropertyName("Comentario")]
        public string? Comentario { get; set; }
        [JsonPropertyName("Asiento")]
        public decimal? Asiento { get; set; }
        [JsonPropertyName("NroBolMarcaAnt")]
        public string? NroBolMarcaAnt { get; set; }
        [JsonPropertyName("NroBolSenhalAnt")]
        public string? NroBolSenhalAnt { get; set; }
        [JsonPropertyName("EntregadoDis")]
        public string? EntregadoDis { get; set; }
        [JsonPropertyName("IdUsuarioRecDis")]
        public string? IdUsuarioRecDis { get; set; }
        [JsonPropertyName("FechaRecDis")]
        public DateTime? FechaRecDis { get; set; }
        [JsonPropertyName("CodEstadoCivilTitular")]
        public string? CodEstadoCivilTitular { get; set; }
        [JsonPropertyName("CodProfesionTitular")]
        public string? CodProfesionTitular { get; set; }
        [JsonPropertyName("DireccionTitular")]
        public string? DireccionTitular { get; set; }
        [JsonPropertyName("CodEstadoCivilApoderado")]
        public string? CodEstadoCivilApoderado { get; set; }
        [JsonPropertyName("CodProfesionApoderado")]
        public string? CodProfesionApoderado { get; set; }
        [JsonPropertyName("DireccionApoderado")]
        public string? DireccionApoderado { get; set; }
        [JsonPropertyName("Rowid")]
        [ScaffoldColumn(false)]
        public Guid? Rowid { get; set; }

        //[JsonPropertyName("IdAutorizanteNavigation")]
        //public virtual RmInterviniente? IdAutorizanteNavigation { get; set; }
    }
}