namespace SQL_API.Models
{
    public sealed class LinkCreateRequest
    {
        public int TYPE { get; set; }
        public int DURATION { get; set; }
        public string TOKEN { get; set; }
    }
}
