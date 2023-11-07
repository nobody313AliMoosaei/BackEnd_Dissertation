using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.OUTPUT.Teacher
{
    public class TeacherModelDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? NationalCode { get; set; }
        public string? UserName { get; set; }
        public string? College { get; set; }
        public int MyProperty { get; set; }
    }
}
