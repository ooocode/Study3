using Microsoft.Extensions.DependencyInjection;
using Study.Service._Configure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Study.Service
{
    /// <summary>
    /// 自动配置用户服务类
    /// </summary>
    public class ConfigCustomService
    {
        public ConfigCustomService(IServiceCollection services)
        {
            var ass = Assembly.GetExecutingAssembly();
            var types = ass.GetTypes();

            //选择出包含【AutoInjectAttribute】的接口
            var autoInjectInterfaces = types
                .Where(e => e.CustomAttributes.Any(attr => attr.AttributeType == typeof(AutoInjectAttribute))).ToList();

            foreach (Type inter in autoInjectInterfaces)
            {
                CustomAttributeTypedArgument autoInjectAttribute = inter.CustomAttributes
                    .FirstOrDefault().ConstructorArguments[0];
                var interfaceType = inter;
                var implType = (Type)autoInjectAttribute.Value;

                services.AddScoped(interfaceType, implType);
            }
        }
    }
}
