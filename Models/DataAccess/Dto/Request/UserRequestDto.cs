using System.ComponentModel.DataAnnotations;

namespace Models.DataAccess
{
    public class UserRequestDto: Auditable
    {
        [Required(ErrorMessage ="Tên tài khoản không được rỗng")]
        [Display(Name="Tên tài khoản")]
        public string UserID { get; set; }

        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Tên người dùng")]
        public string FullName { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        
        public string Email { get; set; }

        [Display(Name = "Mã Quyền")]
        public byte GrantID { get; set; }

        [Display(Name = "Trạng thái")]
        public bool isActive { get; set; }

        [Display(Name = "Quyền")]
        public string GrantName { get; set; }
    }
}
