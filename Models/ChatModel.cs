using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SQL_API.Models
{
    [Table("TBL_CHAT")]
    public class ChatModel
    {
        [Key]
        public int ID { get; set; }
        public string? CONNECTION_ID { get; set; }
        public string? SENDER_ID { get; set; }
        public string? RECEIVER_ID { get; set; }
        public bool RECEIVER_READ { get; set; }
        public string? CHAT { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DATE { get; set; }
        public string? SHOWID { get; set; }
    }
}
