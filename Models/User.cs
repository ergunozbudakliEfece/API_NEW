﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQL_API.Models
{
    [Table("TBL_USERDATA")]
    public sealed class User
    {
        [Key]
        public int USER_ID { get; set; }
        public string? USER_NAME { get; set; }
        public string? USER_PASSWORD { get; set; }
        public string? USER_FIRSTNAME { get; set; }
        public string? USER_LASTNAME { get; set; }
        public bool ACTIVE { get; set; }
        public bool LOGIN_ACTIVE { get; set; }
        public string? USER_MAIL { get; set; }
        public char USER_TYPE { get; set; }
        public int LOGIN_LIMIT { get; set; }
        public int INS_USER_ID { get; set; }
        public int? ROLE_ID { get; set; }
    }
}
