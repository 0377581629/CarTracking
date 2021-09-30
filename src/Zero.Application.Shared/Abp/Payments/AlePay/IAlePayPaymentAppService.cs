using System.Threading.Tasks;
using Abp.Application.Services;
using alepay.Models;
using Zero.Abp.Payments.Dto;

namespace Zero.Abp.Payments.AlePay
{
    public interface IAlePayPaymentAppService : IApplicationService
    {
        /// <summary>
        /// Create AlePay charge
        /// </summary>
        /// <param name="input"></param>
        /// <returns>AlePay Checkout Url</returns>
        Task<string> CreatePayment(AlePayCreatePaymentInput input);

        /// <summary>
        /// Create AlePay charge
        /// </summary>
        /// <param name="input"></param>
        /// <returns>AlePay Checkout Url</returns>
        Task<string> CreateUserPayment(AlePayCreatePaymentInput input);

        Task<GetTransactionInfoResponseModel> GetTransactionInfo(long paymentId);
        
        Task<GetTransactionInfoResponseModel> GetUserTransactionInfo(long paymentId);
    }
}
