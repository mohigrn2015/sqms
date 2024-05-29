namespace SQMS.Models
{
    public class AspNetUser
    {
        public AspNetUser()
        {
            this.tblBranchUsers = new HashSet<tblBranchUser>();
            this.tblTokenQueues = new HashSet<tblTokenQueue>();
            this.AspNetRoles = new HashSet<AspNetRole>();
            this.tblDailyBreaks = new HashSet<tblDailyBreak>();
        }

        public string Id { get; set; }
        public string Hometown { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<tblBranchUser> tblBranchUsers { get; set; }
        public virtual ICollection<tblTokenQueue> tblTokenQueues { get; set; }
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
        public virtual ICollection<tblDailyBreak> tblDailyBreaks { get; set; }
    }
}
