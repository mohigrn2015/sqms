using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SQMS.Models
{
    public class LogReqModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("USER_NAME")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("USER_PASSWORD")]
        public string UserPassword { get; set; }

        [Column("FRANCHISE")]
        public int Franchise { get; set; }

        [MaxLength(255)]
        public string SecurityToken { get; set; }

        [MaxLength(255)]
        [Column("USER_TYPE")]
        public string UserType { get; set; }

        [MaxLength(255)]
        [Column("LOGIN_NAME")]
        public string LoginName { get; set; }
    }
}
