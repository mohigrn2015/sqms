namespace SQMS.Models.ViewModels
{
    public class VMDashboardBranchToken
    {
        public string service_name { get; set; }
        public int served { get; set; }
        public int serving { get; set; }
        public int missing { get; set; }
        public int waiting { get; set; }
        public int total
        {
            get
            {
                return served + serving + missing + waiting;
            }
        }

    }
}
