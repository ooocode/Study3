using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility
{
    public static class DateTimeFormat
    {
        /// <summary>
        /// 到本地字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToLocalString(DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }


        /// <summary>
        /// 计算过去的时间,几天前,几小时前,分,秒
        /// </summary>
        /// <param name="dt"></param>
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
                return
                "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return
                "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return
                "1周前";
            }
            else if (span.TotalDays > 1)
            {
                return
                string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return
                string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return
                string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return
                string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return
                "1秒前";
            }
        }
    }
}
