using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Services;
using Zero.MultiTenancy.Payments;

namespace Zero.Abp.Payments
{
    public interface IPaymentManager : IDomainService
    {
        bool PaymentEnable();

        Task<List<PaymentGatewayModel>> GetAllPaymentGatewayForSettings();
        
        Task<List<PaymentGatewayModel>> GetAllActivePaymentGateways();
    }
}