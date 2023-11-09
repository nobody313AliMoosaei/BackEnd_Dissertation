using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class ErrorsVM
    {
        public string? Title { get; set; } = "توجه";
        public string? Message { get; set; }
        public long Key { get; set; }
        public bool IsValid { get; set; } = false;
        public List<string?> ErrorList { get; set; } = new List<string?>();
    }
}
