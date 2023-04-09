using System.ComponentModel.DataAnnotations;

namespace Dissertation_Project.Model.DTO.INPUT.SignUp
{
    public class ChangePassword_UserDTO
    {
        [Required(ErrorMessage ="ارسال شناسه کاربر اجباری است")]
        public ulong? User_Id { get; set; }
        [Required(ErrorMessage ="ارسال شناسه تغییر برای کاربر اجباری است")]
        public string? Token { get; set; }

        [Required]
        public string? Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
    }
}
