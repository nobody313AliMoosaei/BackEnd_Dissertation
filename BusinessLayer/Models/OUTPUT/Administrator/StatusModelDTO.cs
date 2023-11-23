using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.OUTPUT.Administrator
{
    public class StatusModelDTO
    {
        public long Id { get; set; }
        public int Code { get; set; }
        public string? Title { get; set; }
    }
}
