using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models.OUTPUT.Dissertation
{
    public class DissertationModelOutPut
    {
        public long DissertationId { get; set; }
        public string? TitlePersian { get; set; }
        public string? TitleEnglish { get; set; }
        public string? TermNumber { get; set; }
        public string? Abstract { get; set; }
        public string? DissertationFileAddress { get; set; }
        public string? ProceedingsFileAddress { get; set; }
        public string? DateStr { get; set; }
        public string? TimeStr { get; set; }
        public long? StudentId { get; set; }
        public int? StatusDissertation { get; set; }
        public string? DisplayStatusDissertation { get; set; }
    }
}
