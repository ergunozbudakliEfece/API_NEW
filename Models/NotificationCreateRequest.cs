namespace SQL_API.Models
{
    public class NotificationCreateRequest
    {
        public List<int> Users { get; set; }
        public Notification Notification { get; set; }
    }
}
