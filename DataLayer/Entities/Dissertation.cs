using System.ComponentModel.DataAnnotations;


namespace DataLayer.Entities
{
    public class Dissertations
    {
        public Dissertations()
        {
            Comments = new HashSet<Comments>();
            KeyWords = new HashSet<KeyWord>();
        }

        public long DissertationId { get; set; }
        public string? TitlePersian { get; set; }
        public string? TitleEnglish { get; set; }
        public string? TermNumber { get; set; }
        public bool? AllowEdit { get; set; }
        public string? Abstract { get; set; }
        public string? DissertationFileName { get; set; }
        public string? DissertationFileAddress { get; set; }
        public string? ProceedingsFileName { get; set; }
        public string? ProceedingsFileAddress { get; set; }
        public DateTime? DateTime { get; set; }
        public long? StudentId { get; set; } = 0;
        public int? StatusDissertation { get; set; }

        public virtual Users? Student { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<KeyWord> KeyWords { get; set; }
    }
}
