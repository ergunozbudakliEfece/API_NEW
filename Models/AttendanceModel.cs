using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    public class AttendanceModel
    {
        [Key]
        public int INCKEY { get; set; }

        public int USER_ID { get; set; }
        public string CHECK_IN_OUT { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime DATE {  get; set; }
        public int? INS_USER_ID { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime? INS_DATE { get; set; }
    }
}
