using System.ComponentModel.DataAnnotations;

namespace SQL_API.Models
{
    public sealed class PasswordUpdateRequest
    {
        public int USER_ID { get; set; }
        public string USER_PASSWORD { get; set; }
        public string USER_MAIL { get; set; }
        public string TOKEN { get; set; }
    }
}
