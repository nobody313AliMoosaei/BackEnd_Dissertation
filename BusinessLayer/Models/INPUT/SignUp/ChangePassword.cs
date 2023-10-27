using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.INPUT.SignUp
{
    public class ChangePassword_UserDTO
    {
        [Required(ErrorMessage = "ارسال شناسه کاربر اجباری است")]
        public long? User_Id { get; set; }
        [Required(ErrorMessage = "ارسال شناسه تغییر برای کاربر اجباری است")]
        public string? Token { get; set; }

        [Required]
        public string? Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
    }
}
