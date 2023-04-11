using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dissertation_Project.Model.DTO.INPUT.SignUp
{
    public class RegisterUserDTO
    {
        //[Required(ErrorMessage ="نام را وارد کنید")]
        //public string?  FirstName { get; set; }

        //[Required(ErrorMessage ="نام خانوادگی را وارد کنید")]
        //public string? LastName { get; set; }

        //[Required(ErrorMessage ="نام دانشکده مربوطه نمی تواند خالی باشد")]
        //public string? College { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //public string? Password { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[Compare(nameof(Password))]
        //public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage ="وارد کردن کد ملی یا شماره پرسنلی به عنوان نام کاربری اجباری است")]
        public string? UserName { get; set; }

        [Required(ErrorMessage ="وارد کردن رایانامه اجباری است")]
        [EmailAddress(ErrorMessage ="فرمت وارد کردن ایمیل کاربر درست نیست")]
        public string? Email { get; set; }

        [Required]
        public string? NationalCode { get; set; }
    }
}
