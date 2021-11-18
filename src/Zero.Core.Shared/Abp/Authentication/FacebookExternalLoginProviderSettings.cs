using Abp.Extensions;

namespace Zero.Authentication
{
    public class FacebookExternalLoginProviderSettings
    {
        public bool IsEnabled { get; set; }
        
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        
        public bool IsValid()
        {
            return !AppId.IsNullOrWhiteSpace() && !AppSecret.IsNullOrWhiteSpace();
        }
    }
}