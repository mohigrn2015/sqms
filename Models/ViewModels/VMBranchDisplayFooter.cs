namespace SQMS.Models.ViewModels
{
    public class VMBranchDisplayFooter
    {
        public int branch_display_footer_id { get; set; } = 0;
        public int display_footer_id { get; set; }  
        public string content_en { get; set; } = "";
        public string content_bn { get; set; } = "";
        public int branch_id { get; set; }
        public string branch_name { get; set; } = "";
    }
}
