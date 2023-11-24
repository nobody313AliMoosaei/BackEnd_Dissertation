using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.INPUT
{
    public class CommentInputDTO
    {
        // long UserId, long DissertationId, string Title, string Dsr, long CommentId = 0

        public long UserId { get; set; }
        public long DissertationId { get; set; }
        public string Title { get; set; }
        public string Dsr { get; set; }
        public long CommentId { get; set; } = 0;

    }
}
