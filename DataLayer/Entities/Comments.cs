using System.ComponentModel.DataAnnotations;


namespace DataLayer.Entities
{
    public class Comments
    {
        [Key]
        public ulong Comment_Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime Insert_DateTime { get; set; }


        // ----------------------  Navigations   -------------------------
        
        public Users? Sender { get; set; }
        
        public IList<Comment_User>? Receivers { get; set; }

        public IList<Comments>? Replay_Comment { get; set; }

        public Comments()
        {
            Receivers = new List<Comment_User>();
            Replay_Comment= new List<Comments>();
        }
    }
}