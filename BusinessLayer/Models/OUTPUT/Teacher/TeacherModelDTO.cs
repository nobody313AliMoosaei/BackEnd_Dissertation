using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.OUTPUT.Teacher
{
    public class TeacherOutModelDTO:Models.INPUT.Teacher.TeacherInModelDTO
    {
        public long Id { get; set; }
    }
}
