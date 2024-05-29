﻿using SQMS.Utility;

namespace SQMS.Models.ViewModels
{
    public class VMNextToken
    {
        public string static_ip { get; set; }

        public int display_next { get; set; }
        public string token_prefix { get; set; }
        public long token_no { get; set; }
        public string token_no_formated
        {
            get
            {
                return token_prefix + token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
            }
        }
    }
}
