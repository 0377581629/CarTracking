using Abp.Extensions;

namespace Zero.Authentication
{
    public class GoogleExternalLoginProviderSettings
    {
        public bool IsEnabled { get; set; }
        
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string UserInfoEndpoint { get; set; }
        
        public bool IsValid()
        {
            return !ClientId.IsNullOrWhiteSpace() && !ClientSecret.IsNullOrWhiteSpace();
        }
    }
}
