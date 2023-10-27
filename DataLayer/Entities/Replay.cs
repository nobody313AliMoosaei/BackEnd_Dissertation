using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public partial class Replay
    {
        public long Id { get; set; }
        public long? CommentRef { get; set; }
        public long? ReplayId { get; set; }

        public virtual Comments? CommentRefNavigation { get; set; }
        public virtual Comments? ReplayNavigation { get; set; }
    }
}
