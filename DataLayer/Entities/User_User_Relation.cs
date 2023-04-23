using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class User_User_Relation
    {
        [Key]
        public ulong Id { get; set; }

        public ulong Teacher_Id { get; set; }
    }
}
