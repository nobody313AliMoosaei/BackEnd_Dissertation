using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.OUTPUT.Administrator
{
    public class UserModelDTO
    {
        public long UserId { get; set; }
        public string? FirsName { get; set; }
        public string? LastName { get; set; }
        public string? NationalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? CollegeName { get; set; }
        public long CollegeRef { get; set; }
        public List<string> TeachersName { get; set; } = new List<string>();
        public bool HasDissertation { get; set; }
        public bool Active { get; set; } = false;
        public string? Email { get; set; }
    }
}
