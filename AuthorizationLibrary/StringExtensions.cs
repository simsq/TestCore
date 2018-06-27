using System;

namespace AuthorizationLibrary
{
    public static class StringExtensions
    {
        /// <summary>
        /// base64解码(用于JWT)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Base64UrlEncode(this byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        /// <summary>
        /// base64编码 用于JWT
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] Base64UrlDecode(this string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }
    }
}
