using System.Collections.Generic;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments
{
    public interface IPaymentGatewayStore
    {
        List<PaymentGatewayModel> GetActiveGateways();
    }
}
