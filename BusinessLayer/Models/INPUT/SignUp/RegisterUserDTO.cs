using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.INPUT.SignUp
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "وارد کردن کد ملی یا شماره پرسنلی به عنوان نام کاربری اجباری است")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "وارد کردن رایانامه اجباری است")]
        [EmailAddress(ErrorMessage = "فرمت وارد کردن ایمیل کاربر درست نیست")]
        public string? Email { get; set; }

        [Required]
        public string? NationalCode { get; set; }
    }
}
