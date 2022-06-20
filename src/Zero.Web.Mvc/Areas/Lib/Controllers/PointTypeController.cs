using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Lib.Application.Shared.Dto.Transport.PointType;
using DPS.Lib.Application.Shared.Interface.Transport.PointType;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Lib.Model.PointType;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Lib.Controllers
{
    [Area("Lib")]
    [AbpMvcAuthorize(LibPermissions.PointType)]
    public class PointTypeController: ZeroControllerBase
    {
        private readonly IPointTypeAppService _pointTypeAppService;

        public PointTypeController(IPointTypeAppService pointTypeAppService)
        {
            _pointTypeAppService = pointTypeAppService;
        }
        
        public ActionResult Index()
        {
            var viewModel = new PointTypeViewModel
            {
                FilterText = ""
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(LibPermissions.PointType_Create, LibPermissions.PointType_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetPointTypeForEditOutput getPointTypeForEditOutput;

            if (id.HasValue)
            {
                getPointTypeForEditOutput = await _pointTypeAppService.GetPointTypeForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getPointTypeForEditOutput = new GetPointTypeForEditOutput
                {
                    PointType = new CreateOrEditPointTypeDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                    }
                };
            }

            var viewModel = new CreateOrEditPointTypeViewModel()
            {
                PointType = getPointTypeForEditOutput.PointType,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}