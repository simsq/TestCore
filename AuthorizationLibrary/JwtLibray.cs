using AuthorizationLibrary.Dto;
using Newtonsoft.Json;

namespace AuthorizationLibrary
{
    public class JwtLibray
    {
        private static string key = "abcdefghijkmlopquABCDEFGHIJKLMN";

        public string DecodeToken(string token)
        {
            return JsonWebToken.Decode(token, key);
        }

        /// <summary>
        /// get token
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public string GetToken(AuthorizationInput input)
        {
            //校验用户信息是否正确，如果正确则返回签名信息
            var sigin = JsonWebToken.Encode(input, key, JwtHashAlgorithm.HS384);
            return sigin;
        }

        /// <summary>
        /// verify token info
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public object VerifyToken(string token)
        {
            //获取用户信息
            var userNameAndPwd = JsonWebToken.Decode(token, key);
            //数据查询用户信息是否正确

            return userNameAndPwd;
        }

        /// <summary>
        /// 获取授权信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AuthorizationOutput GetAuthorizationInfo(string token)
        {
            var authorizationInfo = JsonWebToken.Decode(token, key);
            var authorizeInfo = JsonConvert.DeserializeObject<AuthorizationOutput>(authorizationInfo);
            return authorizeInfo;
        }
    }
}
