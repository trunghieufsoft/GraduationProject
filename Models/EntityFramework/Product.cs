using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFramework
{
    public class Product : Auditable
    {
        public Product()
        {
            Orders = new List<Order>();
            Ratings = new List<Rating>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProdID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [StringLength(200)]
        public string ProdName { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(230)]
        public string Code { get; set; }

        [Column(TypeName = "image")]
        public byte[] Image { get; set; }

        [Column(TypeName = "nvarchar")]
        public string ImageUrl { get; set; }

        [Column(TypeName = "ntext")]
        public string Decription { get; set; }

        [Required]
        public int Cost { get; set; }
        
        public bool isActive { get; set; }

        public byte CateID { get; set; }

        public Category Category { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<Rating> Ratings { get; set; }
    }
}