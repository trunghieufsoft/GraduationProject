using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFramework
{
    public class Order : Auditable
    {
        [Key]
        [Column(Order = 0, TypeName = "varchar")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(10)]
        public string BillID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProdID { get; set; }

        [Required]
        public int Count { get; set; }

        public Product Product { get; set; }

        public Bill Bill { get; set; }
    }
}