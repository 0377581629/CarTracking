using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Lib.Application.Shared.Dto.Transport.Point;
using DPS.Lib.Application.Shared.Interface.Transport.Point;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Lib.Model.Point;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Lib.Controllers
{
    [Area("Lib")]
    [AbpMvcAuthorize(LibPermissions.Point)]
    public class PointController: ZeroControllerBase
    {
        private readonly IPointAppService _pointAppService;

        public PointController(IPointAppService pointAppService)
        {
            _pointAppService = pointAppService;
        }
        
        public ActionResult Index()
        {
            var viewModel = new PointViewModel
            {
                FilterText = ""
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(LibPermissions.Point_Create, LibPermissions.Point_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetPointForEditOutput getPointForEditOutput;

            if (id.HasValue)
            {
                getPointForEditOutput = await _pointAppService.GetPointForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getPointForEditOutput = new GetPointForEditOutput
                {
                    Point = new CreateOrEditPointDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                    }
                };
            }

            var viewModel = new CreateOrEditPointViewModel()
            {
                Point = getPointForEditOutput.Point,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}