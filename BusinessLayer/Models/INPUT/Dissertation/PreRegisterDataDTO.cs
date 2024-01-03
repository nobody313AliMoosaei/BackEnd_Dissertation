using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.INPUT.Dissertation
{
    public class PreRegisterDataDTO
    {
        [Required(ErrorMessage = "وارد کردن عنوان به فارسی اجباری است")]
        public string? Title_Persian { get; set; }

        //[Required(ErrorMessage = "وارد کردن عنوان به انگلیسی اجباری است")]
        public string? Title_English { get; set; }

        //[Required(ErrorMessage = "شماره ترم اجباری است")]
        public string? Term_Number { get; set; }

        //[Required(ErrorMessage = "وارد کردن چکیده اجباری است")]
        public string? Abstract { get; set; }

        [Required(ErrorMessage = "وارد کردن نام کوچک اجباری است")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "وارد کردن نام خانوادگی کاربر اجباری است")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "انتخاب دانشکده اجباری است")]
        public long? CollegeRef { get; set; }

        [Required(ErrorMessage = "انتخاب استاد راهنمای اول اجباری است")]
        public long? Teacher_1 { get; set; }

        public long? Teacher_2 { get; set; }

        public long? Teacher_3 { get; set; }

        public List<string>? KeyWords { get; set; } = new List<string>();
    }
}
