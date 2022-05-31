using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using DPS.Lib.Application.Shared.Dto.Basic.NetworkProvider;
using DPS.Lib.Application.Shared.Interface.Basic.NetworkProvider;
using DPS.Lib.Core.Shared;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Lib.Model.NetworkProvider;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Lib.Controllers
{
    [Area("Lib")]
    [AbpMvcAuthorize(LibPermissions.NetworkProvider)]
    public class NetworkProviderController: ZeroControllerBase
    {
        private readonly INetworkProviderAppService _networkProviderAppService;

        public NetworkProviderController(INetworkProviderAppService networkProviderAppService)
        {
            _networkProviderAppService = networkProviderAppService;
        }
        
        public ActionResult Index()
        {
            var viewModel = new NetworkProviderViewModel
            {
                FilterText = ""
            };
            return View(viewModel);
        }

        [AbpMvcAuthorize(LibPermissions.NetworkProvider_Create, LibPermissions.NetworkProvider_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetNetworkProviderForEditOutput getNetworkProviderForEditOutput;

            if (id.HasValue)
            {
                getNetworkProviderForEditOutput = await _networkProviderAppService.GetNetworkProviderForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getNetworkProviderForEditOutput = new GetNetworkProviderForEditOutput
                {
                    NetworkProvider = new CreateOrEditNetworkProviderDto()
                    {
                        Code = StringHelper.ShortIdentity(),
                    }
                };
            }

            var viewModel = new CreateOrEditNetworkProviderViewModel()
            {
                NetworkProvider = getNetworkProviderForEditOutput.NetworkProvider,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}