using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Lib.Application.Shared.Dto.Transport.Route;
using DPS.Lib.Application.Shared.Dto.Transport.Route.Details;
using DPS.Lib.Application.Shared.Interface.Transport.Route;
using DPS.Lib.Core.Shared;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Lib.Model.Route;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Lib.Controllers
{
    [Area("Lib")]
    [AbpMvcAuthorize(LibPermissions.Route)]
    public class RouteController: ZeroControllerBase
    {
        private readonly IRouteAppService _routeAppService;

        public RouteController(IRouteAppService routeAppService)
        {
            _routeAppService = routeAppService;
        }
        public ActionResult Index()
        {
            var viewModel = new RouteViewModel
            {
                FilterText = ""
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(LibPermissions.Route_Create, LibPermissions.Route_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetRouteForEditOutput getRouteForEditOutput;

            if (id.HasValue)
            {
                getRouteForEditOutput = await _routeAppService.GetRouteForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getRouteForEditOutput = new GetRouteForEditOutput
                {
                    Route = new CreateOrEditRouteDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                        IsPermanentRoute = true,
                        HasConstraintTime = true,
                        RouteType = (int)LibEnums.RouteType.PGD,
                    }
                };
            }

            var viewModel = new CreateOrEditRouteViewModel()
            {
                Route = getRouteForEditOutput.Route,
                ListRouteType = LibHelper.ListRouteType(0, LocalizationSource)
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
        
        [AbpMvcAuthorize(LibPermissions.Route_Create, LibPermissions.Route_Edit)]
        public PartialViewResult NewRouteDetail()
        {
            var res = new RouteDetailDto();
            return PartialView("Components/Details/_RouteDetail", res);
        }
        
    }
}