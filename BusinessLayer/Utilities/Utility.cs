using Microsoft.IdentityModel.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public static bool IsValidNationalCode(this string? NationalCode)
        {
            try
            {
                if (NationalCode.IsNullOrEmpty())
                    return false;

                char[] chArray = NationalCode.ToCharArray();
                int[] numArray = new int[chArray.Length];
                for (int i = 0; i < chArray.Length; i++)
                {
                    numArray[i] = (int)char.GetNumericValue(chArray[i]);
                }
                int num2 = numArray[9];
                switch (NationalCode)
                {
                    case "0000000000":
                    case "1111111111":
                    case "22222222222":
                    case "33333333333":
                    case "4444444444":
                    case "5555555555":
                    case "6666666666":
                    case "7777777777":
                    case "8888888888":
                    case "9999999999":
                        return false;
                }
                int num3 = ((((((((numArray[0] * 10) + (numArray[1] * 9)) + (numArray[2] * 8)) + (numArray[3] * 7)) + (numArray[4] * 6)) + (numArray[5] * 5)) + (numArray[6] * 4)) + (numArray[7] * 3)) + (numArray[8] * 2);
                int num4 = num3 - ((num3 / 11) * 11);
                if ((((num4 == 0) && (num2 == num4)) || ((num4 == 1) && (num2 == 1))) || ((num4 > 1) && (num2 == Math.Abs((int)(num4 - 11)))))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            ParameterExpression param = expr1.Parameters[0];
            if (ReferenceEquals(param, expr2.Parameters[0]))
                return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, expr2.Body), param);
            
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body,Expression.Invoke(expr2, param)), param);
        }

        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            ParameterExpression param = expr1.Parameters[0];
            if (ReferenceEquals(param, expr2.Parameters[0]))
                return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, expr2.Body), param);

            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, Expression.Invoke(expr2, param)), param);
        }


    }
}
