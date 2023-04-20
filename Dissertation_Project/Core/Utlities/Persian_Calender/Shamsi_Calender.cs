namespace Dissertation_Project.Core.Utlities.Persian_Calender
{
    public static class Shamsi_Calender
    {
        private static System.Globalization.PersianCalendar PersianCalender = new System.Globalization.PersianCalendar();
        public static int GetYear()
        {
            return PersianCalender.GetYear(DateTime.Now);
        }
        public static int GetMonth()
        {
            return PersianCalender.GetMonth(DateTime.Now);
        }
        public static int GetDay()
        {
            return PersianCalender.GetDayOfMonth(DateTime.Now);
        }

        public static string GetDate_Shamsi()
        {
            return $"{GetYear()}/{GetMonth()}/{GetDay()}";
        }
    }
}
