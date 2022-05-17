using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Lib.Application.Shared.Dto.Basic.Rfid.RfidType;
using DPS.Lib.Application.Shared.Interface.Basic.Rfid;
using DPS.Lib.Core.Shared;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Lib.Model.RfidType;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Lib.Controllers
{
    [Area("Lib")]
    [AbpMvcAuthorize(LibPermissions.RfidType)]
    public class RfidTypeController : ZeroControllerBase
    {
        private readonly IRfidTypeAppService _rfidTypeAppService;

        public RfidTypeController(IRfidTypeAppService rfidTypeAppService)
        {
            _rfidTypeAppService = rfidTypeAppService;
        }

        public ActionResult Index()
        {
            var viewModel = new RfidTypeViewModel
            {
                FilterText = ""
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(LibPermissions.RfidType_Create, LibPermissions.RfidType_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetRfidTypeForEditOutput getRfidTypeForEditOutput;

            if (id.HasValue)
            {
                getRfidTypeForEditOutput = await _rfidTypeAppService.GetRfidTypeForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getRfidTypeForEditOutput = new GetRfidTypeForEditOutput
                {
                    RfidType = new CreateOrEditRfidTypeDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                        IsBlackList = false,
                        CardType = (int)LibEnums.CardType.Treasurer,
                        RegisterDate = DateTime.Today
                    }
                };
            }

            var viewModel = new CreateOrEditRfidTypeViewModel()
            {
                RfidType = getRfidTypeForEditOutput.RfidType,
                ListCardType = LibHelper.ListCardType(0, LocalizationSource)
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}