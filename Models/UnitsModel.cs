using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    [Table("TBL_WORKUNIT")]
    public sealed class UnitsModel
    {
        [Key]
        public int UNIT_ID { get; set; }
        public string UNIT_NAME_TR { get; set; }
        public string UNIT_NAME_EN { get; set; }
        public int? INS_USER_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? INS_DATE { get; set; }
        public int? UPD_USER_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UPD_DATE { get; set; }

    }
}
