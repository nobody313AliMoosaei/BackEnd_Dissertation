using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class ConfirmationsDissertations
    {
        [Key]
        public int Id { get; set; }

        public bool IsConfirm { get; set; } = false;

        public string? Date { get; set; }
        public string? Time { get; set; } = DateTime.Now.ToLongTimeString();

        // Contructor
        public ConfirmationsDissertations()
        {
            Date = "";
            Time = "";
            
            System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();
            Date = persianCalendar.GetYear(DateTime.Now).ToString() +
                persianCalendar.GetMonth(DateTime.Now)+
                persianCalendar.GetDayOfMonth(DateTime.Now);

            Time = DateTime.Now.ToLongTimeString();

            Confirmation = new Confirmations(); 
        }

        // ---------------------
        public Confirmations? Confirmation { get; set; }
        public Users? User { get; set; }

    }
}
