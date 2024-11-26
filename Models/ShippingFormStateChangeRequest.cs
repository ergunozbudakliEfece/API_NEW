namespace SQL_API.Models
{
    public class ShippingFormStateChangeRequest
    {
        public int TYPE { get; set; }
        public string BELGE_NO { get; set; }
        public string SIPARIS_NO { get; set; }
        public string STOK_KODU { get; set; }
        public int GIRIS_DEPO { get; set; }
        public int CIKIS_DEPO { get; set; }
        public int USER_ID { get; set; }
        public bool STATE { get; set; }
    }
}
