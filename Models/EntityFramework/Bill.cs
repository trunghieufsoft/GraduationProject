using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFramework
{
    public class Bill : Auditable
    {
        public Bill()
        {
            Orders = new List<Order>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "varchar")]
        [StringLength(10)]
        public string BillID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [StringLength(250)]
        public string CustomerName { get; set; }

        [Required]
        [Column(TypeName = "ntext")]
        public string DeliveryAddress { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(11)]
        public string Phone { get; set; }

        public int TotalPrice { get; set; }

        [Column(TypeName = "ntext")]
        public string Note { get; set; }

        public bool Status { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        public User User { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}