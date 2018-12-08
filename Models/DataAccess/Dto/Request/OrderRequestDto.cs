using System.ComponentModel.DataAnnotations;

namespace Models.DataAccess
{
    public class OrderRequestDto : Auditable
    {
        [Display(Name = "Mã Hóa đơn")]
        public string BillID { get; set; }

        [Display(Name = "Mã Sản phẩm")]
        public int ProdID { get; set; }

        [Display(Name = "Số lượng")]
        public int Count { get; set; }

        public ProductRequestDto Product{ get; set; }
    }
}
