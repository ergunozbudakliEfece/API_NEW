using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    [Table("TBL_SHIPPINGFORMS")]
    public class ShippingForm
    {
        [Key]
        public int INCKEY { get; set; }
        public int TYPE { get; set; }
        public bool ACTIVE { get; set; }
        public string BELGE_NO { get; set; }
        public string? SIPARIS_NO { get; set; }
        public string IRS_NO { get; set; }
        public string SERI_NO { get; set; }
        public string STOK_KODU { get; set; }
        public decimal MIKTAR1 { get; set; }
        public string OLCU_BR1 { get; set; }
        public decimal MIKTAR2 { get; set; }
        public string? OLCU_BR2 { get; set; }
        public string? ACIK1 { get; set; }
        public string? ACIK2 { get; set; }
        public string? ACIK3 { get; set; }
        public string? SERI_NO_3 { get; set; }
        public string? SERI_NO_4 { get; set; }
        public string? ACIKLAMA_4 { get; set; }
        public string? ACIKLAMA_5 { get; set; }
        public int INS_USER_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? INS_DATE { get; set; }
        public int? UPD_USER_ID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UPD_DATE { get; set; }
        public string? EXP_1 { get; set; }
        public string? EXP_2 { get; set; }
        public string? EXP_3 { get; set; }
        public short GIRIS_DEPO { get; set; }
        public short CIKIS_DEPO { get; set; }
        public string? PLAKA { get; set; }
        public string? SOFOR { get; set; }
    }
}
