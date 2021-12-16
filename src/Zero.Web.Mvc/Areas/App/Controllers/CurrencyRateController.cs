using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize.Dto.CurrencyRate;
using Zero.Customize.Interfaces;
using Zero.Web.Areas.App.Models.CurrencyRate;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.CurrencyRate)]
    public class CurrencyRateController : ZeroControllerBase
    {
        private readonly ICurrencyRateAppService _currencyRateAppService;
        public CurrencyRateController(ICurrencyRateAppService currencyRateAppService)
        {
	        _currencyRateAppService = currencyRateAppService;
        }

        public ActionResult Index()
        {
            var model = new CurrencyRateViewModel
			{
				FilterText = ""
			};

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.CurrencyRate_Create, AppPermissions.CurrencyRate_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
			GetCurrencyRateForEditOutput getCurrencyRateForEditOutput;

			if (id.HasValue){
				getCurrencyRateForEditOutput = await _currencyRateAppService.GetCurrencyRateForEdit(new EntityDto { Id = (int) id });
			}
			else{
				getCurrencyRateForEditOutput = new GetCurrencyRateForEditOutput{
					CurrencyRate = new CreateOrEditCurrencyRateDto()
				};
			}

			var viewModel = new CreateOrEditCurrencyRateModalViewModel
			{
				CurrencyRate = getCurrencyRateForEditOutput.CurrencyRate
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}