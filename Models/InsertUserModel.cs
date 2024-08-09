namespace SQL_API.Models
{
    public class InsertUserModel
    {
        
        public string USER_NAME { get; set; }
        public string USER_FIRSTNAME { get; set; }
        public string USER_LASTNAME { get; set; }
        public bool ACTIVE { get; set; }
        public string USER_MAIL { get; set; }
        public string USER_TYPE { get; set; }
        public int INS_USER_ID { get; set; }
        public int? ROLE_ID { get; set; }
    }
}
