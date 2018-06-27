using AuthorizationLibrary;
using AuthorizationLibrary.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            var jwt = new JwtLibray();
            var roles = new List<string>
            {
                "用户管理",
                "财管管理"
            };

            var result = jwt.GetToken(new AuthorizationInput
            {
                IsAdmin = false,
                PassWord = "123456",
                MenuList = roles
            });

            Console.WriteLine("加密后的token字符串" + result);
            var menuList = jwt.GetAuthorizationInfo(result);
            Console.WriteLine("获取菜单权限" + JsonConvert.SerializeObject(menuList));
            var userInfo = jwt.VerifyToken(result);
            Console.WriteLine("获取用户信息" + JsonConvert.SerializeObject(userInfo));
            Console.ReadLine();


        }
    }
}
