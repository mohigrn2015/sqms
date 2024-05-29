using SQMS.Utility;

namespace SQMS.Models.ViewModels
{
    public class VMTokenProgress
    {
        public string static_ip { get; set; }
        public string counter_no { get; set; }
        public string token_prefix { get; set; }
        public string token_no { get; set; }

        public string token_no_formated
        {
            get
            {
                if (token_no == ApplicationSetting.DisplayWhenEmptyToken)
                    return token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
                else
                    return token_prefix + token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
            }
        }
    }
}
