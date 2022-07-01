using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Lib.Application.Shared.Dto.Transport.AssignmentRoute;
using DPS.Lib.Application.Shared.Interface.Transport.AssignmentRoute;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Lib.Model.AssignmentRoute;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Lib.Controllers
{
    [Area("Lib")]
    [AbpMvcAuthorize(LibPermissions.AssignmentRoute)]
    public class AssignmentRouteController: ZeroControllerBase
    {
        private readonly IAssignmentRouteAppService _assignmentRouteAppService;

        public AssignmentRouteController(IAssignmentRouteAppService assignmentRouteAppService)
        {
            _assignmentRouteAppService = assignmentRouteAppService;
        }
        
        public ActionResult Index()
        {
            var viewModel = new AssignmentRouteViewModel
            {
                FilterText = ""
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(LibPermissions.AssignmentRoute_Create, LibPermissions.AssignmentRoute_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetAssignmentRouteForEditOutput getAssignmentRouteForEditOutput;

            if (id.HasValue)
            {
                getAssignmentRouteForEditOutput = await _assignmentRouteAppService.GetAssignmentRouteForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getAssignmentRouteForEditOutput = new GetAssignmentRouteForEditOutput
                {
                    AssignmentRoute = new CreateOrEditAssignmentRouteDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                        IsAssignment = false,
                        StartDate = DateTime.Today,
                        EndDate = DateTime.Today.AddMonths(1)
                    }
                };
            }

            var viewModel = new CreateOrEditAssignmentRouteViewModel()
            {
                AssignmentRoute = getAssignmentRouteForEditOutput.AssignmentRoute,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}