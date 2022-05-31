using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Lib.Application.Shared.Dto.Transport.CarType;
using DPS.Lib.Application.Shared.Interface.Transport.CarType;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Lib.Model.CarType;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Lib.Controllers
{
    [Area("Lib")]
    [AbpMvcAuthorize(LibPermissions.CarType)]
    public class CarTypeController: ZeroControllerBase
    {
        private readonly ICarTypeAppService _carTypeAppService;

        public CarTypeController(ICarTypeAppService carTypeAppService)
        {
            _carTypeAppService = carTypeAppService;
        }
        
        public ActionResult Index()
        {
            var viewModel = new CarTypeViewModel
            {
                FilterText = ""
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(LibPermissions.CarType_Create, LibPermissions.CarType_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetCarTypeForEditOutput getCarTypeForEditOutput;

            if (id.HasValue)
            {
                getCarTypeForEditOutput = await _carTypeAppService.GetCarTypeForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getCarTypeForEditOutput = new GetCarTypeForEditOutput
                {
                    CarType = new CreateOrEditCarTypeDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                    }
                };
            }

            var viewModel = new CreateOrEditCarTypeViewModel()
            {
                CarType = getCarTypeForEditOutput.CarType,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}