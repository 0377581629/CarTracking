using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Lib.Application.Shared.Dto.Basic.Treasurer;
using DPS.Lib.Application.Shared.Interface.Basic.Treasurer;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Lib.Model.Treasurer;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Lib.Controllers
{
    [Area("Lib")]
    [AbpMvcAuthorize(LibPermissions.Treasurer)]
    public class TreasurerController: ZeroControllerBase
    {
        private readonly ITreasurerAppService _treasurerAppService;

        public TreasurerController(ITreasurerAppService treasurerAppService)
        {
            _treasurerAppService = treasurerAppService;
        }
        
        public ActionResult Index()
        {
            var viewModel = new TreasurerViewModel
            {
                FilterText = ""
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(LibPermissions.Treasurer_Create, LibPermissions.Treasurer_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetTreasurerForEditOutput getTreasurerForEditOutput;

            if (id.HasValue)
            {
                getTreasurerForEditOutput = await _treasurerAppService.GetTreasurerForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getTreasurerForEditOutput = new GetTreasurerForEditOutput
                {
                    Treasurer = new CreateOrEditTreasurerDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                        IsStopWorking = false,
                        Gender = true,
                    }
                };
            }

            var viewModel = new CreateOrEditTreasurerViewModel()
            {
                Treasurer = getTreasurerForEditOutput.Treasurer,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}