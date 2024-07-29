using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    public class BirthDayModel
    {
        public int USER_ID { get; set; }
        public bool ACTIVE { get; set; }
        public string? USER_FIRSTNAME { get; set; }
        public string? USER_LASTNAME { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DOGUM_TARIHI { get; set; }

    }
}
