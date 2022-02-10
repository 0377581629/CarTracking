using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Json;
using DPS.Cms.Application.Shared.Dto.Menu;
using DPS.Cms.Core.Menu;
using GHN;
using GHN.Models;
using GHTK;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Cms.Models.Menu;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Cms.Controllers
{
    [Area("Cms")]
    [AbpMvcAuthorize(CmsPermissions.Menu)]
    public class MenuController : ZeroControllerBase
    {
        private readonly IRepository<Menu> _menuRepository;

        public MenuController(IRepository<Menu> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<ActionResult> Index()
        {
            var ghtkApi = new GHTKApiClient();
            var lstAddresses = await ghtkApi.GetAddresses();
            Console.WriteLine(lstAddresses.Count);

            var lstAddressesLevel4 =
                await ghtkApi.GetAddressesLevel4(lstAddresses.First().Name, 
                    lstAddresses.First().Districts.First().Name, 
                    lstAddresses.First().Districts.First().Wards.First().Name);

            Console.WriteLine(lstAddressesLevel4.ToJsonString());
            
            var ghnApi = new GHNApiClient();
            var provinces = await ghnApi.GetProvinces();
            
            Console.WriteLine(provinces.Count);
            
            var districts = await ghnApi.GetDistricts(provinces.First().Id);
            var stations = await ghnApi.GetStations(new GetStationRequestModel
            {
                DistrictId = "1485",
                Limit = 1000,
                Offset = 0
            });
            Console.WriteLine(stations.ToJsonString());
            
            return View();
        }

        [AbpMvcAuthorize(CmsPermissions.Menu_Create, CmsPermissions.Menu_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(CreateOrEditMenuInput input)
        {
            var model = new CreateOrEditMenuViewModel(null)
            {
                Code = StringHelper.ShortIdentity(),
                MenuGroupId = input.MenuGroupId
            };

            if (input.Id.HasValue)
            {
                var obj = await _menuRepository.GetAsync(input.Id.Value);
                model = ObjectMapper.Map<CreateOrEditMenuViewModel>(obj);
            }

            return PartialView("_CreateOrEditModal", model);
        }
    }
}