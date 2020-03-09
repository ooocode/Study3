using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public static class EntityFrameworkException
    {
        /// <summary>
        /// 获取entity framework 错误
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetEntityFrameworkErrorMsg(this Exception ex)
        {
            var message = (ex.InnerException?.Message) ?? ex.Message;
            if (message.StartsWith("Duplicate"))
            {
                return "重复记录";
            }
            return message;
        }
    }
}
