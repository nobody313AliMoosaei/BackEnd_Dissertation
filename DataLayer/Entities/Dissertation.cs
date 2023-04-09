using System.ComponentModel.DataAnnotations;


namespace DataLayer.Entities
{
    public class Dissertations
    {
        [Key]
        public ulong Dissertation_Id { get; set; }
        public string? Title_Persian { get; set; }
        public string? Title_English { get; set; }
        public string? Term_Number { get; set; }

        // اجازه تغییر دادن پایان نامه
        public bool Allow_Edit { get; set; } = true;

        // چکیده - مقدمه
        public string? Abstract { get; set; }

        // فایل اصلی پایان نامه
        public string? Dissertation_FileName { get; set; }
        public string? Dissertation_FileAddress { get; set; }

        // فایل نهایی : صورت جلسه
        public string? Proceedings_FileName { get; set; }
        public string? Proceedings_FileAddress { get; set; }

        public DateTime Insert_DateTime { get; set; }

        public Dissertations()
        {
            ConfirmationsDissertations = new List<ConfirmationsDissertations>();
        }

        // ---------------------    -------------------------
        public List<Comments>? Comments { get; set; }
        public Users? Student{ get; set; }
        public List<KeyWord>? KeyWords { get; set; }
        public List<ConfirmationsDissertations>? ConfirmationsDissertations { get; set; }
    }
}
