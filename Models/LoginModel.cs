using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SQL_API.Models
{
    public class LoginModel
    {
        [Key]
        public int LOG_ID { set; get; }
        public int USER_ID { set; get; }
        [Column(TypeName = "datetime")]
        public DateTime? LOGIN_DATE { set; get; }
        [Column(TypeName = "datetime")]
        public DateTime? LOGOUT_DATE { set; get; }
        [Column(TypeName = "datetime")]
        public DateTime? LAST_ACTIVITY_DATE { set; get; }
        public int LAST_ACTIVITY { set; get; }
        public string PLATFORM { set; get; }
    }
}
