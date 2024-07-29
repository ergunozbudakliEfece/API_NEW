namespace SQL_API.Models
{
    public class StartWorkDayModel
    {
        public int USER_ID { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime? MEVCUT_IS_ILK_TARIH { get; set; }
    }
}
