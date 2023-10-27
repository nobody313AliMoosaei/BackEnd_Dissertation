using System.ComponentModel.DataAnnotations;


namespace DataLayer.Entities
{
    public class Comments
    {

        public Comments()
        {
            ReplayCommentRefNavigations = new HashSet<Replay>();
            ReplayReplayNavigations = new HashSet<Replay>();
        }
        [Key]
        public long CommentId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? InsertDateTime { get; set; }
        public long? UserRef { get; set; }
        public long? DissertationRef { get; set; }

        public virtual Dissertations? DissertationRefNavigation { get; set; }
        public virtual Users? UserRefNavigation { get; set; }
        public virtual ICollection<Replay> ReplayCommentRefNavigations { get; set; }
        public virtual ICollection<Replay> ReplayReplayNavigations { get; set; }
    }

}