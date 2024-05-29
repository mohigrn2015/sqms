﻿using SQMS.Models.MetaModels;

namespace SQMS.Models
{
    public partial class tblServiceDetail
    {
        public long service_id { get; set; }
        public long token_id { get; set; }
        public long customer_id { get; set; }
        public string issues { get; set; }
        public string solutions { get; set; }
        public System.DateTime service_datetime { get; set; }
        public System.DateTime start_time { get; set; }
        public System.DateTime end_time { get; set; }
        public int service_sub_type_id { get; set; }

        public virtual tblCustomer tblCustomer { get; set; }
        public virtual tblTokenQueue tblTokenQueue { get; set; }
        public virtual tblServiceSubType tblServiceSubType { get; set; }
    }
}
