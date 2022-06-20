using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Lib.Application.Shared.Dto.Basic.ManagementUnit;
using DPS.Lib.Application.Shared.Interface.Basic.ManagementUnit;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Lib.Model.ManagementUnit;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Lib.Controllers
{
    [Area("Lib")]
    [AbpMvcAuthorize(LibPermissions.ManagementUnit)]
    public class ManagementUnitController: ZeroControllerBase
    {
        private readonly IManagementUnitAppService _managementUnitAppService;

        public ManagementUnitController(IManagementUnitAppService managementUnitAppService)
        {
            _managementUnitAppService = managementUnitAppService;
        }
        
        public ActionResult Index()
        {
            var viewModel = new ManagementUnitViewModel
            {
                FilterText = ""
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(LibPermissions.ManagementUnit_Create, LibPermissions.ManagementUnit_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetManagementUnitForEditOutput getManagementUnitForEditOutput;

            if (id.HasValue)
            {
                getManagementUnitForEditOutput = await _managementUnitAppService.GetManagementUnitForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getManagementUnitForEditOutput = new GetManagementUnitForEditOutput
                {
                    ManagementUnit = new CreateOrEditManagementUnitDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                    }
                };
            }

            var viewModel = new CreateOrEditManagementUnitViewModel()
            {
                ManagementUnit = getManagementUnitForEditOutput.ManagementUnit,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}