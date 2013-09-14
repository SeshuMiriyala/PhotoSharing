using System;
using System.Text;

namespace PhotoSharingApp.Models.Security
{
    internal static class AccessTokenFactory
    {
        private const int Size = 10;

        public static AccessToken GenerateAccessToken()
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var builder = new StringBuilder();
            for (var i = 0; i < Size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return new AccessToken {Token = builder.ToString() };
        }
    }
}