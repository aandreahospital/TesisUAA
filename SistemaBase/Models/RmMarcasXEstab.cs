using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class RmMarcasXEstab
    {
        public decimal? IdMarca { get; set; }
        public string? CodEstable { get; set; }
        public decimal? NroInscripcion { get; set; }
        public string? CodEstablePj { get; set; }
        public string? CodDistrito { get; set; }
        public string? Descripcion { get; set; }
        public decimal? IdTransaccion { get; set; }
        public string? GpsH { get; set; }
        public string? GpsV { get; set; }
        public string? GpsSc { get; set; }
        public string? GpsS { get; set; }
        public string? GpsW { get; set; }
        public Guid Rowid { get; set; }
        public decimal IdMarEst { get; set; }
    }
}
