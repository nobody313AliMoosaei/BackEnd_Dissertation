using Microsoft.AspNetCore.Identity;

namespace DataLayer.Entities
{
    public class Users:IdentityUser<ulong>
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        
        public string? College { get; set; }

        public string? NationalCode { get; set; }


        // -----------------     ---------------------------
        public List<User_User_Relation>? Teachers { get; set; }

        public Users()
        {
            Teachers= new List<User_User_Relation>();
        }
    }
}
