using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Comment_User
    {
        public int Id { get; set; }

        // ------------------  Navigations  -----------------------

        public ulong? User_Id { get; set; }

        [ForeignKey(nameof(User_Id))]
        public Users? User { get; set; }
    }
}
