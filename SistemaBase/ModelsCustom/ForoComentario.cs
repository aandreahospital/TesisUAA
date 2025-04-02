using SistemaBase.Models;

namespace SistemaBase.ModelsCustom
{
    public class ForoComentario
    {
        public int IdComentario { get; set; }
        public string CodUsuario { get; set; } = null!;
        public int IdForoDebate { get; set; }
        public string? DescripcionCom { get; set; }
        public DateTime? FechaComentario { get; set; }
        public string? Titulo { get; set; }
        public byte[]? Adjunto { get; set; }
        public string? DescripcionForo { get; set; }
        public string? Estado { get; set; }

        public List<ComentarioCustom>? Comentarios;
    }

}
