using Abp.Application.Services.Dto;

namespace Zero.Customize.Dto.CurrencyRate
{
    public class GetAllCurrencyRateInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}