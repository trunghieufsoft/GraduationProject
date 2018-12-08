using System.ComponentModel.DataAnnotations;

namespace Models.DataAccess
{
    public class GrantRequestDto : Auditable
    {
        [Display(Name = "Mã quyền")]
        public byte GrantID { get; set; }

        [Display(Name="Tên quyền")]
        [Required(ErrorMessage ="Tên phân quyền không được rỗng")]
        public string GrantName { get; set; }
        
        public bool isActive { get; set; }
    }
}
