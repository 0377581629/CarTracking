using Abp.Extensions;

namespace Zero.Authentication
{
    public class TwitterExternalLoginProviderSettings
    {
        public bool IsEnabled { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        
        public bool IsValid()
        {
            return !ConsumerKey.IsNullOrWhiteSpace() && !ConsumerSecret.IsNullOrWhiteSpace();
        }
    }
}
 
