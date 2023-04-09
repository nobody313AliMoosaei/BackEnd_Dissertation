using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class KeyWord
    {
        [Key]
        public ulong KeyWord_Id { get; set; }
        public string? Word_Persion { get; set; }
        public string? Word_English { get; set; }
    }
}
