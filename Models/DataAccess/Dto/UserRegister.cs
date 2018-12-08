using System.ComponentModel.DataAnnotations;

namespace Models.DataAccess.Dto
{
    public class UserRegister
    {
        [Required(ErrorMessage = "Tên tài khoản không được rỗng")]
        [Display(Name = "Tên tài khoản")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được rỗng")]
        [Display(Name = "Mật khẩu")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,15}$", ErrorMessage = "Mật khẩu quá yếu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được rỗng")]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        [Display(Name = "Xác nhận Mật khẩu")]
        
        public string ConfirmPassword { get; set; }

        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }


        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}