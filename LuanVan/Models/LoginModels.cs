using System.ComponentModel.DataAnnotations;

namespace LuanVan.Models
{
    public class LoginModels
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nhập tài khoản!")]
        public string Username { set; get; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nhập mật khẩu!")]
        public string Password { set; get; }
    }
}
