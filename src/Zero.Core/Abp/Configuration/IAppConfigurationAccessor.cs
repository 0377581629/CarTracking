using Microsoft.Extensions.Configuration;

namespace Zero.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
