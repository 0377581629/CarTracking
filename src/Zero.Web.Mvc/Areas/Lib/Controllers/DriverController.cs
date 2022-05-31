using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Lib.Application.Shared.Dto.Transport.Driver;
using DPS.Lib.Application.Shared.Interface.Transport.Driver;
using DPS.Lib.Core.Shared;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Lib.Model.Driver;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Lib.Controllers
{
    [Area("Lib")]
    [AbpMvcAuthorize(LibPermissions.Driver)]
    public class DriverController: ZeroControllerBase
    {
        private readonly IDriverAppService _driverAppService;

        public DriverController(IDriverAppService driverAppService)
        {
            _driverAppService = driverAppService;
        }
        
        public ActionResult Index()
        {
            var viewModel = new DriverViewModel
            {
                FilterText = ""
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(LibPermissions.Driver_Create, LibPermissions.Driver_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetDriverForEditOutput getDriverForEditOutput;

            if (id.HasValue)
            {
                getDriverForEditOutput = await _driverAppService.GetDriverForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getDriverForEditOutput = new GetDriverForEditOutput
                {
                    Driver = new CreateOrEditDriverDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                        IsStopWorking = false,
                        Gender = true,
                        BloodType = (int)LibEnums.BloodType.OPlus,
                        DoB = DateTime.Today,
                        DrivingLicenseStartDate = DateTime.Today.AddYears(-1),
                        DrivingLicenseEndDate = DateTime.Today.AddYears(1)
                    }
                };
            }

            var viewModel = new CreateOrEditDriverViewModel()
            {
                Driver = getDriverForEditOutput.Driver,
                ListBloodType = LibHelper.ListBloodType(0, LocalizationSource)
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}