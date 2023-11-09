using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public partial class Baslookup
    {
        public long Id { get; set; }

        public int? Code { get; set; }

        public string? Type { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }


        public virtual ICollection<Users> Users { get; } = new List<Users>();
    }

}
