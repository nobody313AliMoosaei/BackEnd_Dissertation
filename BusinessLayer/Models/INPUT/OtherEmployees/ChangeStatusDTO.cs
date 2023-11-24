using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.INPUT.OtherEmployees
{
    public class ChangeStatusDTO
    {
        [Required(ErrorMessage = "وارد کردن شناسه پایان نامه اجباری است")]
        public long? DissertationId { get; set; }
        
        [Required(ErrorMessage = "وارد کردن شناسه وضعیت اجباری است")]
        public long? StatusId { get; set; }
    }
}
