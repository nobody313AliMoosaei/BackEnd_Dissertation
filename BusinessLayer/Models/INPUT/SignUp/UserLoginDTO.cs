using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.INPUT.SignUp
{
    public class LoginUserDTO
    {
        [Required(ErrorMessage = "وارد کردن نام کاربری اجباری است")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "وارد کردن رمز اجباری است")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool IsRememberMe { get; set; }
    }
}
