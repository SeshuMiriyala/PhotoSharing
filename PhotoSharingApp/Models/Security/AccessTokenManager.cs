namespace PhotoSharingApp.Models.Security
{
    internal static class AccessTokenManager
    {
        public static void GenerateAccessToken(string userName)
        {
            var token = AccessTokenFactory.GenerateAccessToken();
            AccessTokenRepository.AddToCache(userName, token);
        }
    }
}   