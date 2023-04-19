using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using System.ComponentModel.DataAnnotations;

namespace Dissertation_Project.Model.DTO.INPUT.Pre_Registration
{
    public class Upload_Dissertation_DTO
    {
        [Required(ErrorMessage ="فایل صورت جلسه ارسال نشده است")]
        public IFormFile? Proceedings_File { get; set; }

        [Required(ErrorMessage = "فایل پایان نامه ارسال نشده است")]
        public IFormFile? Dissertation_File{ get; set; }
    }
}
