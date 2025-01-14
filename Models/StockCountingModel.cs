namespace SQL_API.Models
{
    public class StockCountingModel
    {
        public int? ID { get; set; }
        public string? STOK_KODU { get; set; }
        public string? MIKTAR { get; set; }
        public string? MIKTAR2 { get; set; }
        public int? SAYIM_ID { get; set; }
        public int? KAYIT_KULLANICI_ID { get; set; }
        public int? GUNCELLEME_KULLANICI_ID { get; set; }

    }
}
