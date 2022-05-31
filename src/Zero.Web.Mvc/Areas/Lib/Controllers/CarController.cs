using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Lib.Application.Shared.Dto.Transport.Car;
using DPS.Lib.Application.Shared.Dto.Transport.Car.Details;
using DPS.Lib.Application.Shared.Interface.Transport.Car;
using DPS.Lib.Core.Shared;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Lib.Model.Car;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Lib.Controllers
{
    [Area("Lib")]
    [AbpMvcAuthorize(LibPermissions.Car)]
    public class CarController: ZeroControllerBase
    {
        private readonly ICarAppService _carAppService;

        public CarController(ICarAppService carAppService)
        {
            _carAppService = carAppService;
        }
        
        public ActionResult Index()
        {
            var viewModel = new CarViewModel
            {
                FilterText = ""
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(LibPermissions.Car_Create, LibPermissions.Car_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetCarForEditOutput getCarForEditOutput;

            if (id.HasValue)
            {
                getCarForEditOutput = await _carAppService.GetCarForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getCarForEditOutput = new GetCarForEditOutput
                {
                    Car = new CreateOrEditCarDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                        FuelType = (int)LibEnums.FuelType.Gasoline,
                    }
                };
            }

            var viewModel = new CreateOrEditCarViewModel()
            {
                Car = getCarForEditOutput.Car,
                ListFuelType = LibHelper.ListFuelType(0, LocalizationSource)
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
        
        [AbpMvcAuthorize(LibPermissions.Car_Create, LibPermissions.Car_Edit)]
        public PartialViewResult NewCamera()
        {
            var res = new CarCameraDto();
            return PartialView("Components/Details/_CameraDetail", res);
        }
    }
}