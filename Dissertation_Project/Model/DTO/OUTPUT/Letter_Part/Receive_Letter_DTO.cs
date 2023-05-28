namespace Dissertation_Project.Model.DTO.OUTPUT.Letter_Part
{
    public class Receive_Letter_DTO
    {
        public ulong Comment_Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<Receiver_Name_DTO>? Receiver_Name { get; set; }
    }

    public class Receiver_Name_DTO
    {
        public string? FirstName { get; set; }
        public string? Lastname { get; set; }
    }
}
