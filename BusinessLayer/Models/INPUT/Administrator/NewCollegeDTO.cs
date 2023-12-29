using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.INPUT.Administrator
{
    public class NewCollegeDTO
    {
        public int Code { get; set; }
        public string Type { get { return DataLayer.Tools.BASLookupType.CollegesUni.ToString(); } }
        public string? Title { get; set; }
        public string? Dsr { get; set; }
    }
}
