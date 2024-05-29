namespace SQMS.Models
{
    public class AspNetRole
    {
        public AspNetRole()
        {
            this.AspNetUsers = new HashSet<AspNetUser>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
