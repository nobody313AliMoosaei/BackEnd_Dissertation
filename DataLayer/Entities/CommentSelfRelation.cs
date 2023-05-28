using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class CommentSelfRelation
    {
        [Key]
        public int Id { get; set; }


        // -----------------------  Navigations  -------------------------
        // یک کامنت می تواند چندین پاسخ داشته باشد
        // پس پرنت کامنت اصلی است و چایلد ها کامنت های پاسخ است
        public Comments? Parent { get; set; }
        public Comments? Child { get; set; }
    }
}
