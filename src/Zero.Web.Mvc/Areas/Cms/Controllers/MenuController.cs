using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Json;
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
    public class MenuController : ZeroControllerBase
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
            try
            {
                var provinces = await ghnApi.GetProvinces();
                var districts = await ghnApi.GetDistricts(provinces.First().Id);
                var pickShifts = await ghnApi.GetPickShifts();
                var wards = await ghnApi.GetWards(districts.First().Id);
                
                var services = await ghnApi.GetServices(1786717, districts.First().Id, districts.Last().Id);

                Console.WriteLine(pickShifts.ToJsonString());
                Console.WriteLine(services.ToJsonString());

                ghnApi = new GHN.GHNApiClient(shopId: "1786717");
                var feeCalculation = await ghnApi.FeeCalculation(new FeeCalculationRequestModel
                {
                    FromDistrictId = districts.First().Id,
                    ToDistrictId = wards.Last().DistrictId,
                    ToWardCode = wards.Last().Code,
                    Weight = 10000,
                    Length = 10,
                    Width = 10,
                    Height = 10,
                    ServiceId = services.First().Id,
                    InsuranceValue = 5000000
                });
                Console.WriteLine(feeCalculation.ToJsonString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

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