using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    [Table("TBL_SHIFTS")]
    public class ShiftModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string NAME { get; set; }

        public int INS_USER_ID { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime INS_DATE { get; set; }
    }
}
