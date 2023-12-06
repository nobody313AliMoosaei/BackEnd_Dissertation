using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.OUTPUT
{
    public class DownloadOutModelDTO
    {
        public FileStream? FileStream { get; set; }
        public string? ContentType { get; set; } = "application/octet-stream";
        public string? FileDownloadName { get; set; }
    }
}
