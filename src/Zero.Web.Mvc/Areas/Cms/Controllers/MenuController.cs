using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using DPS.Cms.Application.Shared.Dto.Menu;
using DPS.Cms.Core.Menu;
using GHN.Models;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Cms.Models.Menu;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Cms.Controllers
{
    [Area("Cms")]
    [AbpMvcAuthorize(CmsPermissions.Menu)]
    public class MenuController: ZeroControllerBase
    {
        private readonly IRepository<Menu> _menuRepository;

        public MenuController(IRepository<Menu> menuRepository)
        {
            _menuRepository = menuRepository;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [AbpMvcAuthorize(CmsPermissions.Menu_Create, CmsPermissions.Menu_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(CreateOrEditMenuInput input)
        {
            var ghnApi = new GHN.GHNApiClient();
            var provinces = await ghnApi.GetProvinces();
            Console.WriteLine(provinces.Count);
            var districts = await ghnApi.GetDistricts(provinces.First().Id);
            Console.WriteLine(districts.Count);
            var wards = await ghnApi.GetWards(districts.First().Id);
            Console.WriteLine(wards.Count);
            var stores = await ghnApi.GetStores();
            Console.WriteLine(stores.Count);
            var storeId = await ghnApi.CreateStore(new CreateStoreModel
            {
                DistrictId = wards.First().DistrictId,
                WardId = wards.First().Id.ToString(),
                Address = "663 Truong Dinh Str",
                Name = "Shop thu 3",
                Phone = "0383213993"
            });
            Console.WriteLine(storeId);
            
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