using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    public class AttendanceConditionRequest
    {
        public string USER_ID { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime DATE { get; set; }
        public bool UPDATE { get; set; }
    }
}
