namespace SistemaBase.ModelsCustom
{
    public class DatosAcademicoCustom
    {
        public int IdDatosAcademicos { get; set; }
        public string CodUsuario { get; set; } = null!;
        public int IdCentroEstudio { get; set; }
        public int IdCarrera { get; set; }
        public string? AnhoInicio { get; set; }
        public string? AnhoFin { get; set; }
        public string? Estado { get; set; }
        public string? Carrera { get; set; }
        public string? CentroEstudio { get; set; }
    }
}
