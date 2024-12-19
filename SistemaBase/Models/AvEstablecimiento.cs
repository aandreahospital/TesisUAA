using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class AvEstablecimiento
    {
        public string CodEstable { get; set; } = null!;
        public string? CodPropietario { get; set; }
        public string? Descripcion { get; set; }
        public string? Telefono { get; set; }
        public string? Domicilio { get; set; }
        public string? CodDepartamento { get; set; }
        public string? CodZona { get; set; }
        public string? CodRegional { get; set; }
        public string? CodDistrito { get; set; }
        public string? CodLocalidad { get; set; }
        public string? CodTipoEstab { get; set; }
        public string? CodFinalidad { get; set; }
        public string? PistaAterrisaje { get; set; }
        public string? Banio { get; set; }
        public string? Brete { get; set; }
        public string? Embarcadero { get; set; }
        public string? Cepo { get; set; }
        public string? Galpon { get; set; }
        public string? Corral { get; set; }
        public string? FrecuenciaRadio { get; set; }
        public decimal? NroPotero { get; set; }
        public decimal? PasturaNatural { get; set; }
        public decimal? PasturaCultivada { get; set; }
        public decimal? PasturaMonte { get; set; }
        public decimal? PasturaOtro { get; set; }
        public decimal? TotalHectarea { get; set; }
        public string? LinderoNorte { get; set; }
        public string? LinderoSur { get; set; }
        public string? LinderoEste { get; set; }
        public string? LinderoOeste { get; set; }
        public string? GpsV { get; set; }
        public string? GpsS { get; set; }
        public string? GpsW { get; set; }
        public string? GpsH { get; set; }
        public string? GpsSc { get; set; }
        public string? CodEstableOld { get; set; }
        public string? UsuarioUltMod { get; set; }
        public DateTime? FecUltMod { get; set; }
        public string? CodUsuarioAlta { get; set; }
        public DateTime? FechaAlta { get; set; }
        public string CodEstablePj { get; set; } = null!;
        public Guid Rowid { get; set; }
    }
}
