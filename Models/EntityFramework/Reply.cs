using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFramework
{
    public class Reply : Auditable
    {
        [Key]
        [Column(Order = 0, TypeName = "varchar")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(10)]
        public string ComID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RepNo { get; set; }

        [Required]
        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        [Required]
        [StringLength(50)]
        public string UserID { get; set; }

        public Comment Comment { get; set; }

        public User User { get; set; }
    }
}