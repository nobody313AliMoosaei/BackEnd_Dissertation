using Microsoft.AspNetCore.Identity;

namespace DataLayer.Entities
{
    public class Users : IdentityUser<long>
    {
        public Users()
        {
            Comments = new HashSet<Comments>();
            Dissertations = new HashSet<Dissertations>();
            Teachers = new HashSet<Teachers>();
        }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? College { get; set; }

        public string? NationalCode { get; set; }

        public bool Active { get; set; } = true;

        public long? CollegeRef { get; set; }

        public override string? Email { get; set; } = null;

        // -----------------     ---------------------------
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<Dissertations> Dissertations { get; set; }
        public virtual ICollection<Teachers> Teachers { get; set; }
        public virtual Baslookup? CollegeRefNavigation { get; set; }
    }
}
