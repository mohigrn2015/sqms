using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SQMS.Models
{
    public class tblNotification
    {
        public int notification_id { get; set; }
        [Display(Name = "Sent Now")]
        public bool sent_now { get; set; }
        [Required]
        public DateTime notification_date_time { get; set; }
        [Required]
        [Display(Name = "Message")]
        [StringLength(302)]
        public string message { get; set; }
        public bool have_attachment { get; set; }
        public DateTime? created_date { get; set; }
        public string created_by { get; set; }
        public DateTime? updated_date { get; set; }
        public string updated_by { get; set; }
        [Required]
        [NotMapped]
        public string SelectedUserId { get; set; }
        [NotMapped]
        public string NotificationDate { get; set; }
        [NotMapped]
        public string NotificationTime { get; set; }
        [NotMapped]
        public byte[] NotificationFile { get; set; }
        [NotMapped]
        public string sender { get; set; }
        [NotMapped]
        public bool is_seen { get; set; }
        [NotMapped]
        public bool is_PostBack { get; set; }
    }
}
