using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Lib.Application.Shared.Dto.Basic.Device;
using DPS.Lib.Application.Shared.Interface.Basic.Device;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Lib.Model.Device;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Lib.Controllers
{
    [Area("Lib")]
    [AbpMvcAuthorize(LibPermissions.Device)]
    public class DeviceController: ZeroControllerBase
    {
        private readonly IDeviceAppService _deviceAppService;

        public DeviceController(IDeviceAppService deviceAppService)
        {
            _deviceAppService = deviceAppService;
        }
        
        public ActionResult Index()
        {
            var viewModel = new DeviceViewModel()
            {
                FilterText = ""
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(LibPermissions.Device_Create, LibPermissions.Device_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetDeviceForEditOutput getDeviceForEditOutput;

            if (id.HasValue)
            {
                getDeviceForEditOutput = await _deviceAppService.GetDeviceForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getDeviceForEditOutput = new GetDeviceForEditOutput
                {
                    Device = new CreateOrEditDeviceDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                        StartDate = DateTime.Today,
                        IsActive = true,
                        NeedUpdate = false
                    }
                };
            }

            var viewModel = new CreateOrEditDeviceViewModel()
            {
                Device = getDeviceForEditOutput.Device,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}