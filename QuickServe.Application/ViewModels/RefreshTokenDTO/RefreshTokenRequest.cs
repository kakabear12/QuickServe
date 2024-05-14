using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Application.ViewModels.RefreshTokenDTO
{
    public class RefreshTokenRequest
    {
       
        [Required(ErrorMessage = "Mã làm mới là bắt buộc")]
        public string RefreshToken { get; set; } = null!;

    }
}