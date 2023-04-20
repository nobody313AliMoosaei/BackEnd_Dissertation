using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using System.ComponentModel.DataAnnotations;

namespace Dissertation_Project.Model.DTO.INPUT.Pre_Registration
{
    public class Upload_Dissertation_DTO
    {
        public IFormFile? Proceedings_File { get; set; }
        public string? FileName1 { get; set; }

        public IFormFile? Dissertation_File{ get; set; }
        public string? FileName2 { get; set; }
    }
}
