using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Basic.NetworkProvider
{
    public class GetAllNetworkProviderInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}