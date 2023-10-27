using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.INPUT.Log
{
    public class InsertLogDTO
    {
        public DateTime? Date { get; set; }
        public string? Ip { get; set; }
        public string? Url { get; set; }
        public string? Level { get; set; }
        public string? Client { get; set; }
        public string? Message { get; set; }
    }
}
