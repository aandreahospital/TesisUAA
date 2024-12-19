namespace SistemaBase.Models.Dtos
{
    public class RetrieveRequest
    {
        public int limit { get; set; }
        public int sensitivity { get; set; }
        //public string user_id { get; set; }
        public string token { get; set; }
        //public int cont { get; set; }
    }
}
