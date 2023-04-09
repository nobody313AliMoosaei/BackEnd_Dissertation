using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Logs
    {
        [Key]
        public string Id { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? Ip { get; set; }
        public string? Url { get; set; }
        public string? Method { get; set; }
        public string? Level { get; set; }
        public string? Client { get; set; }
        public string? System { get; set; }
        public string? Message { get; set; }
        public Logs()
        {
            Id= Guid.NewGuid().ToString();
        }
    }
}
