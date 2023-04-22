using System.ComponentModel.DataAnnotations;

namespace Dissertation_Project.Model.DTO.INPUT.Pre_Registration
{
    public class AllData_DTO
    {
        [Required(ErrorMessage = "وارد کردن عنوان به فارسی اجباری است")]
        public string? Title_Persian { get; set; }

        [Required(ErrorMessage = "وارد کردن عنوان به انگلیسی اجباری است")]
        public string? Title_English { get; set; }

        [Required(ErrorMessage = "شماره ترم اجباری است")]
        public string? Term_Number { get; set; }

        [Required(ErrorMessage = "وارد کردن چکیده اجباری است")]
        public string? Abstract { get; set; }

        public List<string>? KeyWords_Persian { get; set; } = new List<string>();
        public List<string>? KeyWords_English { get; set; } = new List<string>();

        [Required(ErrorMessage = "وارد کردن نام کوچک اجباری است")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "وارد کردن نام خانوادگی کاربر اجباری است")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "انتخاب دانشکده اجباری است")]
        public string? College { get; set; }

        [Required(ErrorMessage = "انتخاب استاد راهنمای اول اجباری است")]
        public ulong Teacher_1 { get; set; }

        public ulong Teacher_2 { get; set; }

        public ulong Teacher_3 { get; set; }
    }
}
