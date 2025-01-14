namespace SQL_API.Models
{
    public class PermitModel
    {
        public string? BASLANGIC_TARIHI { get; set; }
        public string? BITIS_TARIHI { get; set; }
        public string? ISE_DONUS { get; set; }
        public int USER_ID { get; set; }
        public int? SUBS_ID { get; set; }
        public int? PERMIT_TYPE { get; set; }
        public int? OFFDAY { get; set; }
        public string? EXP { get; set; }
    }
}
