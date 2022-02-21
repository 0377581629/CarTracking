using System;
using System.IO;
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
        private readonly IFileAppService _fileAppService;

        public MenuController(IRepository<Menu> menuRepository, IFileAppService fileAppService)
        {
            _menuRepository = menuRepository;
            _fileAppService = fileAppService;
        }

        public async Task<ActionResult> Index()
        {
            var ghtkApi = new GHTKApiClient();
            var res = await ghtkApi.PrintLabel("S19806501.BO.SG01-F06.1965415192");
            
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