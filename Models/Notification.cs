using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    [Table("TBL_NOTIFICATION")]
    public class Notification
    {
        public byte TYPE { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NOTIFICATION_ID { get; set; }
        public int SENDER_ID { get; set; }
        public string SUBJECT {  get; set; }
        public string NOTIFICATION_BODY { get; set; }
        public byte IMPORTANCE { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? INS_DATE { get; set; }
    }
}
