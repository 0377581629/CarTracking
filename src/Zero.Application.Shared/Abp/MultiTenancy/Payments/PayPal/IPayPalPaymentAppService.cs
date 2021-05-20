using System.Threading.Tasks;
using Abp.Application.Services;
using Zero.MultiTenancy.Payments.PayPal.Dto;

namespace Zero.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
