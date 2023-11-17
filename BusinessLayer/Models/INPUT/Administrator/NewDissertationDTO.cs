using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.INPUT.Administrator
{
    public class NewDissertationDTO
    {
        public string? Title_Persian { get; set; }

        public string? Title_English { get; set; }

        public string? Term_Number { get; set; }

        public string? Abstract { get; set; }

        public List<string>? KeyWords { get; set; } = new List<string>();
    }
}
