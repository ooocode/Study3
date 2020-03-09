using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    /// <summary>
    /// 日期时间
    /// </summary>
    public static class DateTimeFormat
    {
        /// <summary>
        /// 转成二十四小时制的时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToLocalString(DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }


        /// <summary>
        /// 计算目标时间过去的时间,几天前,几小时前,几分,几秒...
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static string DateStringFromNow(DateTime dateTime)
        {
            TimeSpan span = DateTime.Now - dateTime;
            if (span.TotalDays > 60)
            {
                return dateTime.ToString("yyyy-MM-dd");
            }
            else if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return "1周前";
            }
            else if (span.TotalDays > 1)
            {
                return $"{(int)Math.Floor(span.TotalDays)}天前";
            }
            else if (span.TotalHours > 1)
            {
                return $"{(int)Math.Floor(span.TotalHours)}小时前";
            }
            else if (span.TotalMinutes > 1)
            {
                return $"{ (int)Math.Floor(span.TotalMinutes)}分钟前";
            }
            else if (span.TotalSeconds >= 1)
            {
                return $"{(int)Math.Floor(span.TotalSeconds)}秒前";
            }
            else
            {
                return "1秒前";
            }
        }


        /// <summary>
        /// 计算过去的时间,几天前,几小时前,几分,几秒...
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateStringFromNow(DateTimeOffset dateTime)
        {
            return DateStringFromNow(dateTime.AddHours(8).DateTime);
        }
    }
}
