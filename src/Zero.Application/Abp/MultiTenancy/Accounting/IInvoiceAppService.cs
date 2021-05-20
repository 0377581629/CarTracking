using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Zero.MultiTenancy.Accounting.Dto;

namespace Zero.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
