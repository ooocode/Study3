using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Services
{
    public class ErrorMessages
    {
        public const string NotFound = "不存在资源";

        /// <summary>
        /// 解析Id失败
        /// </summary>
        public const string ParseIdFaild = "解析Id失败";
    }

    public class Result
    {
        public Result()
        {
            Succeeded = false;
        }


        public string ErrorMessage { get; set; }

        public string SucceedMessage { get; set; }

        public bool Succeeded { get;  set; }
    }


    public class Result<T>
    {
        public Result()
        {
            Succeeded = false;
        }

        public T Data  { get; set; }


        public string ErrorMessage { get; set; }

        public string SucceedMessage { get; set; }

        public bool Succeeded { get; set; }
    }
}
