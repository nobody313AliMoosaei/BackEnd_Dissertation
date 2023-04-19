using System.ComponentModel.DataAnnotations;

namespace Dissertation_Project.Model.DTO.INPUT.Teacher_Data
{
    public class Add_Teacher_DTO
    {
        [Required(ErrorMessage ="وارد کردن نام اجباری است")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "وارد کردن نام خانوادگی اجباری است")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "وارد کردن کد ملی اجباری است")]
        public string? NationalCode { get; set; }

        [Required(ErrorMessage = "وارد کردن ایمیل کاربر اجباری است")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "وارد کردن نام کاربری اجباری است")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "وارد کردن دانشکده مرتبط اجباری است")]
        public string? College { get; set; }
    }
}
