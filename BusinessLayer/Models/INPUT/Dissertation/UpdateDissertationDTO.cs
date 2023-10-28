using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.INPUT.Dissertation
{
    public class UpdateDissertationDTO
    {
        public long? StudentId { get; set; }
        public long? Teacher1 { get; set; }
        public long? Teacher2 { get; set; }
        public long? Teacher3 { get; set; }
        public string? College { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }

        public long? Dissertation_Id { get; set; }
        public string? TitlePersian { get; set; }
        public string? TitleEnglish { get; set; }
        public string? TermNumber { get; set; }
        public string? Abstract { get; set; }
        public List<string>? KeyWords { get; set; } = new List<string>();

    }
}
