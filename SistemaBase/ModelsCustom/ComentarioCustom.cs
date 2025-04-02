namespace SistemaBase.ModelsCustom
{
    public class ComentarioCustom
    {
        public int IdComentario { get; set; }
        public string CodUsuario { get; set; } = null!;
        public int IdForoDebate { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaComentario { get; set; }

        public string? Nombre { get; set; }



    }
}
