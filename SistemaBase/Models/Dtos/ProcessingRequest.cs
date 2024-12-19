namespace SistemaBase.Models.Dtos
{
    public class ProcessingRequest
    {
        public int smoothing_level { get; set; }
        public int blob_threshold { get; set; }
        public int grid_thickness { get; set; }
        public int background_type { get; set; }
        public int image_size { get; set; }
        public string image_format { get; set; }
        //public string user_id { get; set; }
        public string token { get; set; }
    }
}
