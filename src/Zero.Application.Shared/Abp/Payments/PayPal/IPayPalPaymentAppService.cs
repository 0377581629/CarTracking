using System.Threading.Tasks;
using Abp.Application.Services;
using Zero.Abp.Payments.PayPal.Dto;

namespace Zero.Abp.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        Task ConfirmUserPayment(long paymentId, string paypalOrderId);
        
        PayPalConfigurationDto GetConfiguration();
    }
}
