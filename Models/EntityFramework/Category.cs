using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFramework
{
    public class Category : Auditable
    {
        public Category()
        {
            Products = new List<Product>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte CateID { get; set; }

        
        [Column(TypeName = "varchar")]
        [StringLength(150)]
        public string CodeName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [StringLength(150)]
        public string CateName { get; set; }

        public bool isActive { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}