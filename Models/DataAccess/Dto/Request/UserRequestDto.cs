using System.ComponentModel.DataAnnotations;

namespace Models.DataAccess
{
    public class UserRequestDto: Auditable
    {
        [Required(ErrorMessage ="Tên tài khoản không được rỗng")]
        [Display(Name="Tên tài khoản")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được rỗng")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Tên người dùng không được rỗng")]
        [Display(Name = "Tên người dùng")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được rỗng")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được rỗng")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }
        
        [Required(ErrorMessage = "Email không được rỗng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mã quyền không được rỗng")]
        [Display(Name = "Mã Quyền")]
        public byte GrantID { get; set; }

        [Display(Name = "Trạng thái")]
        public bool isActive { get; set; }

        [Display(Name = "Quyền")]
        public string GrantName { get; set; }
    }
}
