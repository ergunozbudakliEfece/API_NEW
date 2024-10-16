using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    [Table("TBL_SETTEDPRICES")]
    public sealed class SettedPriceModel
    {
        [Key]
        public string STOK_KODU { get; set; }
        public decimal FIYAT { get; set; }
    }
}
