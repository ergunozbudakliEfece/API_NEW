using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    public class NotificationUserDetail
    {
        public int USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string USER_FIRSTNAME { get; set; }
        public string USER_LASTNAME { get; set; }
        public bool RECEIVER_READ { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? READ_TIME { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime INS_DATE { get; set; }
    }
}
