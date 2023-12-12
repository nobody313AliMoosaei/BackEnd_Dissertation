using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.OUTPUT.General
{
    public class CommentOutPutModelDTO
    {
        public long Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? InsertDateTime { get; set; }

        public string? User_FullName { get; set; }

        public string? User_UserName { get; set; }
    }
}
