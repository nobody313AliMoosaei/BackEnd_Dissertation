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
        
        [Required(ErrorMessage ="وارد کردن کد تاییدیه اجباری است")]
        public DataLayer.Tools.Dissertation_Confirmations Code_Dissertation_Confirmation { get; set; }
        
        public string? PersianName { get; set; }
    }
}
