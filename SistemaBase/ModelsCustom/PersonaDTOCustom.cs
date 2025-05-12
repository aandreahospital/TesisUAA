namespace SistemaBase.ModelsCustom
{
    public class PersonaDTOCustom
    {
        public string? CodPersona { get; set; }
        public string? Nombre { get; set; }
        public string? Sexo { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Email { get; set; }
        public string? Clave { get; set; }
        public string? CodGrupo { get; set; }
        public string? Carrera { get; set; }

    }
    public class ProcesarDatosRequest
    {
        public List<PersonaDTOCustom>? Personas { get; set; }
        public decimal Carrera { get; set; }
    }

}
