using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFramework
{
    public class User : Auditable
    {
        public User()
        {
            Bills = new List<Bill>();
            Comments = new List<Comment>();
            Ratings = new List<Rating>();
            Replies = new List<Reply>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string UserID { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(30)]
        public string Password { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [StringLength(250)]
        public string FullName { get; set; }

        [Required]
        [Column(TypeName = "ntext")]
        public string Address { get; set; }

        [Required]
        [StringLength(11)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        public byte GrantID { get; set; }

        public Grant Grant { get; set; }

        [Required]
        public bool isActive { get; set; }

        public ICollection<Bill> Bills { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Rating> Ratings { get; set; }

        public ICollection<Reply> Replies { get; set; }
    }
}