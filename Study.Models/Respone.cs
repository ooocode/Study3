using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Dto
{
    public class Respone<T>
    {
        public T Data { get; set; }

        public string ErrorMessage { get; set; }


        public bool IsOk { get; set; } = false;


        public DateTime Time { get; } = DateTime.Now;
    }


    public class Respone
    {
        public string ErrorMessage { get; set; }

        public bool IsOk { get; set; } = false;

        public DateTime Time { get; } = DateTime.Now;
    }
}
