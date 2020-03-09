using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Service._Configure
{
    /// <summary>
    /// 自动注入属性
    /// </summary>
    public class AutoInjectAttribute : Attribute
    {
        /// <summary>
        /// 实现的接口类
        /// </summary>
        public Type ImplClass  { get; set; }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="implementedInterface">实现该接口的类</param>
        public AutoInjectAttribute(Type implClass)
        {
            this.ImplClass = implClass;
        }
    }
}
