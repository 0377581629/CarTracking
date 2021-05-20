using Abp.Web.Models.AbpUserConfiguration;

namespace Zero.Configuration
{
    public static class AbpUserConfigurationDtoExtensions
    {
        public static bool HasSessionUserId(this AbpUserConfigurationDto userConfiguration)
        {
            return userConfiguration.Session?.UserId != null;
        }
    }
}