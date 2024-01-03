using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.INPUT.SignUp
{
    public class UpdateUserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? NationalCode { get; set; }
        public string? UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? EmailAddress { get; set; }
        
        public long? CollegeRef { get; set; }
        public long? Teacher_1 { get; set; }
        public long? Teacher_2 { get; set; }
        public long? Teacher_3 { get; set; }
    }
}
