using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFramework
{
    public class Rating : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        public string RatID { get; set; }

        [Required]
        public int ProdID { get; set; }

        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        [Required]
        public byte Level { get; set; }

        [Required]
        [StringLength(50)]
        public string UserID { get; set; }
        
        public User User { get; set; }

        public Product Product { get; set; }
    }
}