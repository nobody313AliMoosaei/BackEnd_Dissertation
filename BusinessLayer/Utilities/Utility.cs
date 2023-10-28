using Microsoft.IdentityModel.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Utilities
{
    public static class Utility
    {
        private static System.Globalization.PersianCalendar PersianCalender { get; set; } = new System.Globalization.PersianCalendar();



        public static int Val32(this string? value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            int num;
            int.TryParse(value, out num);
            return num;
        }
        public static long Val64(this string? value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            long num;
            long.TryParse(value, out num);
            return num;
        }
        public static double Val72(this string? value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            double num;
            double.TryParse(value, out num);
            return num;
        }
        public static bool IsNullOrEmpty(this string? value)
        {
            try
            {
                if (value == null || string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }
        public static DateTime ToPersianDateTime(this DateTime value)
        {
            try
            {
                return new DateTime(PersianCalender.GetYear(value), PersianCalender.GetMonth(value), PersianCalender.GetDayOfMonth(value)
                    , PersianCalender.GetHour(value), PersianCalender.GetMinute(value), PersianCalender.GetSecond(value));
            }
            catch
            {
                return DateTime.Now;
            }
        }
        public static DateTime ToMiladi(this DateTime value)
        {
            try
            {
                return PersianCalender.ToDateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
            }
            catch
            {
                return DateTime.Now;
            }
        }
       
        public enum Level_log
        {
            Emergency = 1,
            Alert = 2,
            Critical = 3,
            Error = 4,
            Warning = 5,
            Notice = 6,
            Informational = 7,
            Debug = 8
        }

    }
}
