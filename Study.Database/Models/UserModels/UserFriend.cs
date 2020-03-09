using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Database.Entity.UserEntities
{
    public class UserFriend
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Key]
        public string UserId  { get; set; }

        /// <summary>
        /// 朋友id
        /// </summary>
        [Key]
        public string FriendId { get; set; }
    }
}
