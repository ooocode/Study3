using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Utility;

namespace Study.Database.Models
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public class EntityBase
    {
        [Key]
        public long Id  { get; private set; }

        public EntityBase()
        {
            Id = GuidEx.NewGuid();
        }
    }
}
