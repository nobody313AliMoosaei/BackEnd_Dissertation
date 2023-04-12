using System.ComponentModel.DataAnnotations;

namespace Dissertation_Project.Model.DTO.INPUT.Pre_Registration
{
    public class Personal_Info_DTO
    {
        [Required(ErrorMessage ="وارد کردن نام کوچک اجباری است")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage ="وارد کردن نام خانوادگی کاربر اجباری است")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "انتخاب دانشکده اجباری است")]
        public string? College { get; set; }

        [Required(ErrorMessage = "انتخاب استاد راهنمای اول اجباری است")]
        public ulong Teacher_1 { get; set; }
      
        public ulong Teacher_2 { get; set; }
      
        public ulong Teacher_3 { get; set; }
    }
}
