using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    [Table("TBL_SHIFTDETAILS")]
    public class ShiftDetailModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int SHIFT_ID { get; set; }

        public int TYPE { get; set; }

        public int WORK_GROUP { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime START_DATE { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime END_DATE { get; set; }
    }
}
