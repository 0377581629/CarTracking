using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Lib.Application.Shared.Dto.Transport.CarGroup;
using DPS.Lib.Application.Shared.Interface.Transport.CarGroup;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Lib.Model.CarGroup;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Lib.Controllers
{
    [Area("Lib")]
    [AbpMvcAuthorize(LibPermissions.CarGroup)]
    public class CarGroupController : ZeroControllerBase
    {
        private readonly ICarGroupAppService _carGroupAppService;

        public CarGroupController(ICarGroupAppService carGroupAppService)
        {
            _carGroupAppService = carGroupAppService;
        }

        public ActionResult Index()
        {
            var viewModel = new CarGroupViewModel
            {
                FilterText = ""
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(LibPermissions.CarGroup_Create, LibPermissions.CarGroup_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetCarGroupForEditOutput getCarGroupForEditOutput;

            if (id.HasValue)
            {
                getCarGroupForEditOutput = await _carGroupAppService.GetCarGroupForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getCarGroupForEditOutput = new GetCarGroupForEditOutput
                {
                    CarGroup = new CreateOrEditCarGroupDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                        IsActive = true,
                        IsSpecialGroup = false
                    }
                };
            }

            var viewModel = new CreateOrEditCarGroupViewModel()
            {
                CarGroup = getCarGroupForEditOutput.CarGroup,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}