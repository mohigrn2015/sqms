﻿using SQMS.Models.MetaModels;
using System.ComponentModel.DataAnnotations;

namespace SQMS.Models
{
    public class AspNetUserLogin
    {
        [Key]
        [StringLength(128)]
        public string LoginProvider { get; set; }

        [StringLength(128)]
        public string ProviderKey { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        public int branch_id { get; set; }

        public int counter_id { get; set; }

        public DateTime login_time { get; set; }

        public DateTime logout_time { get; set; }

        public DateTime idle_from { get; set; }

        public int is_idle { get; set; }

        public int logout_type_id { get; set; }
        public string logout_reason { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual tblLogoutType tblLogoutType { get; set; }
    }
}
