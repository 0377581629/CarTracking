using System.Collections.Generic;
using System.Linq;
using Abp.Dependency;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments
{
    public class PaymentGatewayStore : IPaymentGatewayStore, ITransientDependency
    {
        private readonly IIocResolver _iocResolver;

        public PaymentGatewayStore(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public List<PaymentGatewayModel> GetActiveGateways()
        {
            var gateways = _iocResolver.ResolveAll<IPaymentGatewayConfiguration>();

            return gateways.Where(gateway => gateway.IsActiveByConfig).Select(gateway => new PaymentGatewayModel
            {
                GatewayType = gateway.GatewayType,
                SupportsRecurringPayments = gateway.SupportsRecurringPayments
            }).ToList();
        }
    }
}