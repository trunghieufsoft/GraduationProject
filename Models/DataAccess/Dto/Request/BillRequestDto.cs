using System.ComponentModel.DataAnnotations;

namespace Models.DataAccess
{
    public class BillRequestDto : Auditable
    {
        [Display(Name = "Mã Hóa đơn")]
        public string BillID { get; set; }


        [Display(Name = "Tên khách hàng")]
        public string CustomerName { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        public string DeliveryAddress { get; set; }

        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Tổng thanh toán")]
        public int TotalPrice { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        [Display(Name = "Mã Khách hàng")]
        public string UserID { get; set; }

        public bool Status { get; set; }
    }
}
