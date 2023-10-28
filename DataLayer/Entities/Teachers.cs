using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public partial class Teachers
    {
        public long Id { get; set; }
        public long? TeacherId { get; set; }
        public long? StudentId { get; set; }

        public virtual Users? StudentNavigation { get; set; }
        public virtual Users? TeacherNavigation { get; set; }
    }
}
