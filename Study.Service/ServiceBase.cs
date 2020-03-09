using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study.Service
{
    public class ServiceBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ServiceBase(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 当前用户id
        /// </summary>
        public string CurUserId
        {
            get
            {
                return httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(e => e.Type == "sub")?.Value;
            }
        }
    }
}
