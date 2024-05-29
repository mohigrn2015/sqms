using System.ComponentModel.DataAnnotations;

namespace SQMS.Models
{
    public class SignalRBroadcastLog
    {
        [Key]
        public long broadcast_log_id { get; set; }

        public int branch_id { get; set; }

        public string counter_no { get; set; }

        public string token_no { get; set; }

        public int is_token_added { get; set; }

        public int is_token_called { get; set; }

        public int is_playlist_changed { get; set; }

        public int is_footer_changed { get; set; }

        public DateTime broadcast_time { get; set; }

    }
}
