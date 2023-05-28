using Microsoft.AspNetCore.Identity;

namespace Dissertation_Project.Model.DTO.OUTPUT.Letter_Part
{
    public class Send_Letter_DTO
    {
        public ulong? Comment_Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Sender_Name { get; set; }
    }
}
