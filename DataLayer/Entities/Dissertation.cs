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

        public string? Date { get; set; }
        public string? Time { get; set; }

        public Dissertations()
        {
            ConfirmationsDissertations = new List<ConfirmationsDissertations>();
            Comments = new List<Comments>();
            Persian_KeyWords = new List<KeyWord>();
            English_KeyWords = new List<KeyWord>();
            Time = DateTime.Now.ToLongTimeString();
            Status_Dissertation = Tools.Status_Dissertation.During;
        }

        // ---------------------    -------------------------
        public List<Comments>? Comments { get; set; }
        public Users? Student{ get; set; }
        public List<KeyWord>? Persian_KeyWords { get; set; }
        public List<KeyWord>? English_KeyWords { get; set; }
        public List<ConfirmationsDissertations>? ConfirmationsDissertations { get; set; }
        public Tools.Status_Dissertation Status_Dissertation { get; set; }

    }
}
