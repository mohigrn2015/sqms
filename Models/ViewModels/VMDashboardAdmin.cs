namespace SQMS.Models.ViewModels
{
    public class VMDashboardAdmin
    {
        public string branch_name { get; set; }
        public int tokens { get; set; }
        public int services { get; set; }
    }

    public class VMDashboardBranchAdminCounters
    {
        public string counter_no { get; set; }
        public int tokens { get; set; }
    }

    public class VMDashboardBranchAdminServicesTokens
    {
        public int service_id { get; set; }
        public string service_name { get; set; }
        public int tokens { get; set; }
    }

    public class VMDashboardBranchAdminServicesWaitings
    {
        public int service_id { get; set; }
        public string service_name { get; set; }
        public int tokens { get; set; }
    }
}
