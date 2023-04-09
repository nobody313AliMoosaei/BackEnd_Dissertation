using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Confirmations
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="وارد کردن نام اجباری است")]
        public string? Name { get; set; }
        
        [Required(ErrorMessage = "پر کردن نرمالسازی نام اجباری است")]
        public string? NormalizeName { get; set; }

        public string? PersianName { get; set; }
        
        public string? Description { get; set; }
    }
}
