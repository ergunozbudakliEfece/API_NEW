using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    [Table("TBL_CITIES")]
    public sealed class CityModel
    {
        public int ID { get; set; }
        public string CITYNAME { get; set; }
    }
}
