using System.ComponentModel.DataAnnotations;

namespace Models.DataAccess
{
    public class ProductRequestDto : Auditable
    {
        [Display(Name = "Mã Sản phẩm")]
        public int ProdID { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được rỗng")]
        [Display(Name = "Tên sản phẩm")]
        public string ProdName { get; set; }

        [Display(Name = "Mã Code")]
        public string Code { get; set; }

        [Display(Name = "Ảnh sản phẩm")]
        [Required(ErrorMessage ="Ảnh sản phẩm không được rỗng")]
        public string ImageUrl { get; set; }

        [Display(Name = "Mô tả")]
        public string Decription { get; set; }

        [Required(ErrorMessage = "Giá không được rỗng")]
        [Display(Name = "Giá")]
        public int Cost { get; set; }

        [Display(Name = "Trạng thái")]
        public bool isActive { get; set; }

        [Display(Name = "Danh mục sản phẩm")]
        public byte CateID { get; set; }
    }
}
