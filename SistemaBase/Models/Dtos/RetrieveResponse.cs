using System.Text.Json.Serialization;

namespace SistemaBase.Models.Dtos
{
    public class RetrieveResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public Content content { get; set; }
    }
    public class Result
    {
        [JsonPropertyName("class-id")]
        public string class_id { get; set; }
        public double score { get; set; }
        public double? probability { get; set; }
        public double? likelihood { get; set; }
        public double? similarity { get; set; }
        public double? coincidence { get; set; }
        public double? overlap { get; set; }
    }

    public class Content
    {
        public List<Result> results { get; set; }
    }
}
