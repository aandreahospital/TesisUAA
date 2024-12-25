using System;
using System.Collections.Generic;

namespace SistemaBase.Models
{
    public partial class Persona
    {
        public string CodPersona { get; set; } = null!;
        public string? CodPerFisica { get; set; }
        public string? CodPerJuridica { get; set; }
        public string? EsFisica { get; set; }
        public string? Nombre { get; set; }
        public string? NombFantasia { get; set; }
        public string? Sexo { get; set; }
        public DateTime? FecNacimiento { get; set; }
        public string? Profesion { get; set; }
        public string? Conyugue { get; set; }
        public string? CodSector { get; set; }
        public string? DirecElectronica { get; set; }
        public string? EsMalDeudor { get; set; }
        public string? NivelEstudios { get; set; }
        public decimal? TotalIngresos { get; set; }
        public string? CodPais { get; set; }
        public DateTime? FecAlta { get; set; }
        public string? AltaPor { get; set; }
        public DateTime? FecActualizacion { get; set; }
        public string? ActualizadoPor { get; set; }
        public string? TipoSociedad { get; set; }
        public string? Lucrativa { get; set; }
        public string? Estatal { get; set; }
        public string? PaginaWeb { get; set; }
        public string? CodEstadoCivil { get; set; }
        public string? NroRegistroProf { get; set; }
        public string? NroRegistroSenacsa { get; set; }
        public string? EsFuncionarioSenacsa { get; set; }
        public string? EsVacunador { get; set; }
        public string? EsFiscalizador { get; set; }
        public string? EsVeterinario { get; set; }
        public string? EsPropietario { get; set; }
        public string? CodIdent { get; set; }
        public string? CodPropietarioOld { get; set; }
        public string? EsCoordinador { get; set; }
        public Guid Rowid { get; set; }
        public string? Email { get; set; }
    }
}
