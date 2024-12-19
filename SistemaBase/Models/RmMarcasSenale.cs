using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaBase.Models
{
    public partial class RmMarcasSenale
    {
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]

        public decimal IdMarca { get; set; }
        public string? NroBoleta { get; set; }
        public decimal? EstadoMarca { get; set; }
        public string? CodigoEstablecimiento { get; set; }
        public decimal? CantidadGanado { get; set; }
        public decimal? TipoGanado { get; set; }
        public string? IdUsuario { get; set; }
        public DateTime? FechaAlta { get; set; }
        public decimal? EstadoProceso { get; set; }
        public string? MarcaNombre { get; set; }
        public string? SenalNombre { get; set; }
        public string? MarcaTipo { get; set; }
        public string? Semejantea { get; set; }
        public string? CodigoSenal { get; set; }
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]

        public decimal? NumeroEntrada { get; set; }
        public decimal? NumeroBoletaAnterior { get; set; }
        public DateTime? FechaBoletaAnterior { get; set; }
        public string? ObsMarca { get; set; }
        public string? ObsSenal { get; set; }
        public DateTime? FechaAltaAnterior { get; set; }
        public decimal? NroBoletaAnterior { get; set; }
        public string? IdSrsImg { get; set; }
        [ScaffoldColumn(false)]
        public Guid Rowid { get; set; }
    }
}
