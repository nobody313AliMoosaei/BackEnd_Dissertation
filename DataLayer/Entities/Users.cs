using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Users:IdentityUser<ulong>
    {
        [Required(ErrorMessage ="وارد کردن نام کوچک اجباری است")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage ="وارد کردن نام خانوادگی اجباری است")]
        public string? LastName { get; set; }
        
        public string? College { get; set; }

        public string? NationalCode { get; set; }


        // -----------------     ---------------------------
        public List<Users>? Teachers { get; set; }
    }
}
