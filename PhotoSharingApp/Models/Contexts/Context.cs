using PhotoSharingApp.Properties;

namespace PhotoSharingApp.Models.Contexts
{
    internal class Context
    {
        internal readonly string ConString;

        public Context()
        {
            ConString = Settings.Default.DefaultConfig;
        }
    }
}
