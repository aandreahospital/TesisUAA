using SistemaBase.Models;

namespace SistemaBase.ModelsCustom
{
    public class DatosAlumnoCustom
    {

        public string CodPersona { get; set; } = null!;
        public string? EsFisica { get; set; }
        public string? Nombre { get; set; }
        public string? Sexo { get; set; }
        public string? Email { get; set; }
        public string? DireccionParticular { get; set; }
        public string? SitioWeb { get; set; }
        public DateTime? FecNacimiento { get; set; }
        public string? Profesion { get; set; }
        public string? Conyugue { get; set; }
        public string? CodPais { get; set; }
        public string? CodEstadoCivil { get; set; }
        public string Tipo { get; set; } = null!;
        public string? Numero { get; set; }
        public string? Cargo { get; set; }
        public int Iddatosacademicos { get; set; }
        public string CodUsuario { get; set; } = null!;
        public int Idcentroestudio { get; set; }
        public int Idcarrera { get; set; }
        public DateTime? Fechainicio { get; set; }
        public DateTime? Fechafin { get; set; }
        public string? Estado { get; set; }
        public int Iddatoslaborales { get; set; }
        public string? Lugartrabajo { get; set; }
        public string? Universidadtrabajo { get; set; }
        public int? CargoIdcargo { get; set; }
        public string? Direccionlaboral { get; set; }
        public string? FotoPerfil { get; set; }
        public int? Antiguedad { get; set; }

        public List <DatosAcademicoCustom>? Educacion;
        

        public List<DatosLaborale>? ExperienciaLaboral;
    }
}
