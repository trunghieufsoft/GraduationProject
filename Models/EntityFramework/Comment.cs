using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFramework
{
    public class Comment : Auditable
    {
        public Comment()
        {
            Replies = new List<Reply>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        public string ComID { get; set; }

        [Required]
        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        [Required]
        [StringLength(50)]
        public string UserID { get; set; }

        public User User { get; set; }

        public ICollection<Reply> Replies { get; set; }
    }
}