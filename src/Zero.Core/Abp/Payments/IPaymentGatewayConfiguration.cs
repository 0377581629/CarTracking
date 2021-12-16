using Abp.Dependency;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments
{
    public interface IPaymentGatewayConfiguration: ITransientDependency
    {
        bool IsActiveByConfig { get; }
        
        bool IsActive { get; }

        bool SupportsRecurringPayments { get; }

        SubscriptionPaymentGatewayType GatewayType { get; }
    }
}
