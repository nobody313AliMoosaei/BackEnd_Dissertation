using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dissertation_Project.Model.DTO.INPUT.SignUp
{
    public class LoginUserDTO
    {
        [Required(ErrorMessage ="وارد کردن نام کاربری اجباری است")]
        public string? UserName { get; set; }

        [Required(ErrorMessage ="وارد کردن رمز اجباری است")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool IsRememberMe { get; set; }
    }
}
