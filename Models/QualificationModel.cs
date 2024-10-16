using System.ComponentModel.DataAnnotations;

namespace SQL_API.Models
{
    public class QualificationModel
    {
        [Key]
        public int QUALIFICATION_ID { get; set; }
        public string QUALIFICATION_NAME { get; set; }
        public string QUALIFICATION_NAME_EN { get; set; }
    }
}
