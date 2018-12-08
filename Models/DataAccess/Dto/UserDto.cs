using System.ComponentModel.DataAnnotations;

namespace Models.DataAccess
{
    public class UserDto
    {
        [Required(ErrorMessage = "Tên đăng nhập không được rỗng")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được rỗng")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}