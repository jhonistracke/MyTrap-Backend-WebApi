using System;

namespace MyTrap.Framework.Utils
{
    public class DateUtils
    {
        public static string DATE_FORMAT_YYYY_MM_DD_HH_MM_SS = "yyyy-MM-dd HH:mm:ss";

        public static string DateToString(DateTime date, string format = "")
        {
            if (string.IsNullOrEmpty(format))
            {
                return date.ToString(DATE_FORMAT_YYYY_MM_DD_HH_MM_SS);
            }
            else
            {
                return date.ToString(format);
            }
        }
    }
}