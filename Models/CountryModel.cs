using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    [Table("TBL_COUNTRIES")]
    public sealed class CountryModel
    {   
        public int ID { get; set; }
        public string COUNTRY { get; set; }
        public string ISO { get; set; }
        public string CALLING_CODE { get; set; }
    }
}
