﻿using SQMS.Models.MetaModels;

namespace SQMS.Models
{
    public partial class tblDailyBreak
    {
        public long daily_break_id { get; set; }
        public int break_type_id { get; set; }
        public string user_id { get; set; }
        public int counter_id { get; set; }
        public Nullable<System.DateTime> start_time { get; set; }
        public Nullable<System.DateTime> end_time { get; set; }
        public string remarks { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual tblBreakType tblBreakType { get; set; }
        public virtual tblCounter tblCounter { get; set; }
    }
}