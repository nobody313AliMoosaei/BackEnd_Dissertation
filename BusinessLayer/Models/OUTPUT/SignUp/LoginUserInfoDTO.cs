using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.OUTPUT.SignUp
{
    public class LoginUserInfoDTO
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }

        public ErrorsVM? Errors { get; set; } = new ErrorsVM();
    }
}
