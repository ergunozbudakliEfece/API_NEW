using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    public class AttendanceUpdateRequest
    {
        public int INCKEY { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime DATE { get; set; }
        public int UPD_USER_ID { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime UPD_DATE { get; set; } = DateTime.Now;
    }
}
