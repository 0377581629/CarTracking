using Abp.Domain.Services;

namespace Zero
{
    public abstract class ZeroDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected ZeroDomainServiceBase()
        {
            LocalizationSourceName = ZeroConst.LocalizationSourceName;
        }
    }
}
