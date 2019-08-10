using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Dto
{
    public class Utility
    {
        public static string NewGuid()
        {
            return $"{DateTime.Now.Ticks.ToString()}_{Guid.NewGuid().ToString("N")}";
        }
    }
}
