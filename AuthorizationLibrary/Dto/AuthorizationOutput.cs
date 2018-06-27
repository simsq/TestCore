using System.Collections.Generic;

namespace AuthorizationLibrary.Dto
{
    public class AuthorizationOutput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 菜单列表 
        /// </summary>
        public List<string> MenuList { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
