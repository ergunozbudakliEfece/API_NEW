using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SQL_API.Models
{
    [Table("TBL_LINK")]
    public class Link
    {
        [Key]
        public int ID { get; set; }
        public int TYPE { get; set; }
        public bool SITUATION { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CREATE_DATE { get; set; }
        public int DURATION { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EXPIRE_DATE { get; set; }
        public string TOKEN { get; set; }
    }
}
