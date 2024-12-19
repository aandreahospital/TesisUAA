namespace SistemaBase.ModelsCustom
{
    public partial class DatosPersona
    {
        public string CodPersona { get; set; } = null!;
        public string? EsFisica { get; set; }
        public string? Nombre { get; set; }
        public string? Sexo { get; set; }
        public DateTime? FecNacimiento { get; set; }
        public string? Profesion { get; set; }
        public string? Conyugue { get; set; }
        public string? CodPais { get; set; }
        public string? CodEstadoCivil { get; set; }
        public string Tipo { get; set; } = null!;
        public string? Numero { get; set; }
        


    }
}
