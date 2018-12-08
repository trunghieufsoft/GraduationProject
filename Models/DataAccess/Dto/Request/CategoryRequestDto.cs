using System.ComponentModel.DataAnnotations;

namespace Models.DataAccess
{
    public class CategoryRequestDto : Auditable
    {
        [Display(Name = "Mã Danh mục")]
        public byte CateID { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được rỗng")]
        [Display(Name = "Tên Danh mục")]
        public string CateName { get; set; }

        
        public string CodeName { get; set; }

        [Display(Name = "Trạng thái")]
        public bool isActive { get; set; }
    }
}
