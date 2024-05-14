using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Application.ViewModels.UserDTO
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "Tên người dùng là bắt buộc")]
        [MaxLength(40, ErrorMessage = "Tên không vượt quá 40 kí tự.")]
        [MinLength(6, ErrorMessage = "Tên không ngắn hơn 6 kí tự.")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Tên không vượt quá 50 kí tự.")]
        [MinLength(6, ErrorMessage = "Tên không ngắn hơn 6 kí tự.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Tên là bắt buộc")]
        [MaxLength(100, ErrorMessage = "Tên không vượt quá 100 kí tự.")]
        [MinLength(4, ErrorMessage = "Tên không ngắn hơn 4 kí tự.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Giới tính là bắt buộc")]
        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Định dạng số điện thoại không hợp lệ")]
        [RegularExpression(@"^(84|0[3|5|7|8|9])+([0-9]{8})\b$", ErrorMessage = "Định dạng số điện thoại không hợp lệ")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        public DateTime Birthday { get; set; }
    }
}
