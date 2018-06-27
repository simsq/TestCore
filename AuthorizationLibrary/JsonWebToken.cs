using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AuthorizationLibrary
{
   public class JsonWebToken
    {
        private static Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>> HashAlgorithms;

        static JsonWebToken()
        {
            HashAlgorithms = new Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>>
            {
                { JwtHashAlgorithm.RS256, (key, value) => { using (var sha = new HMACSHA256(key)) { return sha.ComputeHash(value); } } },
                { JwtHashAlgorithm.HS384, (key, value) => { using (var sha = new HMACSHA384(key)) { return sha.ComputeHash(value); } } },
                { JwtHashAlgorithm.HS512, (key, value) => { using (var sha = new HMACSHA512(key)) { return sha.ComputeHash(value); } } }
            };
        }

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="payload">载体</param>
        /// <param name="key">盐</param>
        /// <param name="algorithm">加密算法</param>
        /// <returns></returns>
        public static string Encode(object payload, string key, JwtHashAlgorithm algorithm)
        {
            return Encode(payload, Encoding.UTF8.GetBytes(key), algorithm);
        }

        /// <summary>
        /// JWT 
        /// </summary>
        /// <param name="payload">载体（有效信息）</param>
        /// <param name="keyBytes">盐</param>
        /// <param name="algorithm">加密算法</param>
        /// <returns></returns>
        public static string Encode(object payload, byte[] keyBytes, JwtHashAlgorithm algorithm)
        {
            var segments = new List<string>();

            ///jWT head 默认有2部分构成
            var header = new { alg = algorithm.ToString(), typ = "JWT" };

            byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));

            byte[] payloadBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload, Formatting.None));

            // JWT有三部分组成 分别是 head.payload.signature 

            //1.将头部信息编码构成JWT的第一部分
            segments.Add(headerBytes.Base64UrlEncode());

            //2.将payload信息编码构成JWT的第二部分
            segments.Add(payloadBytes.Base64UrlEncode());

            //3.把base64后的头部信息跟base64后的paylod用.连接成字符串用于构成jwt的第三部分
            var stringToSign = string.Join(".", segments.ToArray());
            var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

            //4. 根据head中的加密方式再加上盐（key）进行加密获得 jwt的第三部分 signature
            byte[] signature = HashAlgorithms[algorithm](keyBytes, bytesToSign);
            segments.Add(signature.Base64UrlEncode());

            //5.把这三部分用 . 拼接成字符串 就是最终的jwt信息了
            return string.Join(".", segments.ToArray());
        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="token">token字符串</param>
        /// <param name="key">盐</param>
        /// <returns></returns>

        public static string Decode(string token, string key)
        {
            return Decode(token, key, true);
        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="token"></param>
        /// <param name="key"></param>
        /// <param name="verify"></param>
        /// <returns></returns>
        public static string Decode(string token, string key, bool verify)
        {
            var parts = token.Split('.');
            var header = parts[0];
            var payload = parts[1];
            byte[] crypto = parts[2].Base64UrlDecode();

            var headerJson = Encoding.UTF8.GetString(header.Base64UrlDecode());
            var headerData = JObject.Parse(headerJson);
            var payloadJson = Encoding.UTF8.GetString(payload.Base64UrlDecode());
            var payloadData = JObject.Parse(payloadJson);

            if (verify)
            {
                var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
                var keyBytes = Encoding.UTF8.GetBytes(key);
                var algorithm = (string)headerData["alg"];

                var signature = HashAlgorithms[GetHashAlgorithm(algorithm)](keyBytes, bytesToSign);
                var decodedCrypto = Convert.ToBase64String(crypto);
                var decodedSignature = Convert.ToBase64String(signature);

                if (decodedCrypto != decodedSignature)
                {
                    throw new ApplicationException(string.Format("Invalid signature. Expected {0} got {1}", decodedCrypto, decodedSignature));
                }
            }

            return payloadData.ToString();
        }


        /// <summary>
        /// 获取散列算法
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public static JwtHashAlgorithm GetHashAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case "RS256": return JwtHashAlgorithm.RS256;
                case "HS384": return JwtHashAlgorithm.HS384;
                case "HS512": return JwtHashAlgorithm.HS512;
                default: throw new InvalidOperationException("Algorithm not supported.");
            }
        }
    }
}
