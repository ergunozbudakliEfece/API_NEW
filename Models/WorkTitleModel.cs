using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SQL_API.Models
{
   
        [Table("TBL_WORKTITLE")]
        public sealed class WorkTitleModel
        {
            [Key]
            public int TITLE_ID { get; set; }
            public string TITLE_NAME_TR { get; set; }
            public string TITLE_NAME_EN { get; set; }
            public int? INS_USER_ID { get; set; }
            [Column(TypeName = "datetime")]
            public DateTime? INS_DATE { get; set; }
            public int? UPD_USER_ID { get; set; }
            [Column(TypeName = "datetime")]
            public DateTime? UPD_DATE { get; set; }

        }
  
}
