using System.ComponentModel.DataAnnotations;

namespace SQL_API.Models
{
    public class SectorModel
    {
        [Key]
        public int SECTOR_ID { get; set; }
        public string SECTOR_NAME { get; set; }
    }
}
