namespace SistemaBase.Models.Dtos
{
    public class UploadResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public ContentModel content { get; set; }
    }
    
    public class ContentModel
    {
        public string token { get; set; }
        public long expire { get; set; }
    }
}
