using DataLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace Dissertation_Project.Model.DTO.INPUT.Pre_Registration
{
    public class Dissertation_Info
    {
        [Required(ErrorMessage ="وارد کردن عنوان به فارسی اجباری است")]
        public string? Title_Persian { get; set; }

        [Required(ErrorMessage = "وارد کردن عنوان به انگلیسی اجباری است")]
        public string? Title_English { get; set; }

        [Required(ErrorMessage = "شماره ترم اجباری است")]
        public string? Term_Number { get; set; }
        
        [Required(ErrorMessage = "وارد کردن چکیده اجباری است")]
        public string? Abstract { get; set; }

        public List<string>? KeyWords { get; set; }=new List<string>();
        
    }
}
