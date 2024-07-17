using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    public class NotificationModel
    {
        public byte TYPE { get; set; }
        [Column("NOTIFICATION_ID")]
        public int NOTIFICATION_ID { get; set; }
        public int RECEIVER_ID { get; set; }
        public bool RECEIVER_READ { get; set; }
        public bool RECEIVER_DELETE { get; set; }
        public int SENDER_ID { get; set; }
        public string? SENDER_NAME { get; set; }
        public string? SUBJECT { get; set; }
        public string? NOTIFICATION_BODY { get; set; }
        public byte IMPORTANCE { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? INS_DATE { get; set; }
    }
}
