using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.INPUT.Administrator
{
    public class EditUserDTO
    {
        public long UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? NationalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public long? CollegeRef { get; set; }
        public long? Teacher1_Ref { get; set; }
        public long? Teacher2_Ref { get; set; }
        public long? Teacher3_Ref { get; set; }
    }
}
