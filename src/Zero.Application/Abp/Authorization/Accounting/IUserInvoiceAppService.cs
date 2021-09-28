using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Zero.Abp.Authorization.Accounting.Dto;

namespace Zero.Abp.Authorization.Accounting
{
    public interface IUserInvoiceAppService
    {
        Task<UserInvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateUserInvoiceDto input);
    }
}
