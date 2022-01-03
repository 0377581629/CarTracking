using Zero.Customize.Dto.CurrencyRate;

namespace Zero.Web.Areas.App.Models.CurrencyRate
{
    public class CreateOrEditCurrencyRateModalViewModel
    {
       public CreateOrEditCurrencyRateDto CurrencyRate { get; set; }

       public bool IsEditMode => CurrencyRate.Id.HasValue;
    }
}