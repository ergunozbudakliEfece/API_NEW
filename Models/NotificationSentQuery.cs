using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SQL_API.Models
{
    public class NotificationSentQuery
    {
        public byte TYPE { get; set; }
        public int NOTIFICATION_ID { get; set; }
        public int SENDER_ID { get; set; }
        public string SUBJECT { get; set; }
        public string NOTIFICATION_BODY { get; set; }
        public string SENDER_NAME { get; set; }
        public byte IMPORTANCE { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? INS_DATE { get; set; }
    }
}
