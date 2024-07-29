using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    [Table("TBL_USERTYPES")]
    public class TypeModel
    {
        [Key]
        public string USER_TYPE { get; set; }
        public string? TYPE_NAME { get; set; }
        public int? INS_USER_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? INS_DATE { get; set; }
        public int? UPD_USER_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UPD_DATE { get; set; }
    }
}
