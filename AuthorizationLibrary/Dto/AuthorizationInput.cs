using System.Collections.Generic;

namespace AuthorizationLibrary.Dto
{
    public class AuthorizationInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 角色列表，可以用于记录该用户的角色
        /// </summary>
        public List<string> MenuList { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
    }
}
