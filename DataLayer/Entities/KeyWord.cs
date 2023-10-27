using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public partial class KeyWord
    {
        public long Id { get; set; }
        public string? Word { get; set; }
        public long? DissertationRef { get; set; }

        public virtual Dissertations? DissertationRefNavigation { get; set; }
    }
}
