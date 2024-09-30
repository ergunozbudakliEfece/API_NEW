using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    public class AttendanceCreateRequest
    {
        public string USER_ID { get; set; }
        public string CHECK_IN_OUT { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime DATE { get; set; }
        public int INS_USER_ID { get; set; }
    }
}
