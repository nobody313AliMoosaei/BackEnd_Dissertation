using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dissertation_Project.Model.DTO.INPUT.Letter_Part
{
    public class New_LetterDTO
    {
        [Required(ErrorMessage = "عنوان نامه ارسال نشده است")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "توضیحات الزامی است")]
        public string? Description { get; set; }

        [Required(ErrorMessage ="ارسال شناسه نقش الزامی است")]
        public string? RoleReceiver { get; set; }
    }
}
