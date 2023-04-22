namespace Dissertation_Project.Model.DTO.Fillter
{
    public class User_Fillter_AllData_DTO
    {
        public ulong User_ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? College { get; set; }
        public string? NationalCode { get; set; }
        public List<DataLayer.Entities.Users>? Teacher { set; get; } = new List<DataLayer.Entities.Users>();
    }
}
