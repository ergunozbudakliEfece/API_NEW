using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    [Table("TBL_NOTIFICATIONTARGET")]
    public class NotificationTarget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int INCKEY { get; set; }
        public int NOTIFICATION_ID { get; set; }
        public int RECEIVER_ID { get; set; }
        public bool RECEIVER_READ { get; set; }
        public bool RECEIVER_DELETE { get; set; }
    }
}