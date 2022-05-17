using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Lib.Application.Shared.Dto.Basic.Technician;
using DPS.Lib.Application.Shared.Interface.Basic.Technician;
using DPS.Lib.Core.Shared;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Lib.Model.Technician;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Lib.Controllers
{
    [Area("Lib")]
    [AbpMvcAuthorize(LibPermissions.Technician)]
    public class TechnicianController: ZeroControllerBase
    {
        private readonly ITechnicianAppService _technicianAppService;

        public TechnicianController(ITechnicianAppService technicianAppService)
        {
            _technicianAppService = technicianAppService;
        }
        
        public ActionResult Index()
        {
            var viewModel = new TechnicianViewModel
            {
                FilterText = ""
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(LibPermissions.Technician_Create, LibPermissions.Technician_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetTechnicianForEditOutput getTechnicianForEditOutput;

            if (id.HasValue)
            {
                getTechnicianForEditOutput = await _technicianAppService.GetTechnicianForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getTechnicianForEditOutput = new GetTechnicianForEditOutput
                {
                    Technician = new CreateOrEditTechnicianDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                        IsStopWorking = false,
                        Gender = true,
                    }
                };
            }

            var viewModel = new CreateOrEditTechnicianViewModel()
            {
                Technician = getTechnicianForEditOutput.Technician,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}