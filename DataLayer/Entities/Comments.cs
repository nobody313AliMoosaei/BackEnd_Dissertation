using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Comments
    {
        [Key]
        public ulong Comment_Id { get; set; }

        public string? Description { get; set; }

        public DateTime Insert_DateTime { get; set; }


        // ----------------------     -------------------------
        public Users? Uesr { get; set; }
        public List<Comments>? Comments_Replay { get; set; }

    }
}
