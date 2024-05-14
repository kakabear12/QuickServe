using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServe.Application.ViewModels.UserDTO
{
    public class ProfileUserDTO
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Address { get; set; }
    }
}
