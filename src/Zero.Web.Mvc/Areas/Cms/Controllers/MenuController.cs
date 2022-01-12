using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using DPS.Cms.Application.Shared.Dto.Menu;
using DPS.Cms.Application.Shared.Interfaces;
using DPS.Cms.Application.Shared.Interfaces.Menu;
using DPS.Cms.Core.Menu;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
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
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            var model = new CreateOrEditMenuViewModel(null)
            {
                Code = StringHelper.ShortIdentity()
            };

            if (id.HasValue)
            {
                var obj = await _menuRepository.GetAsync(id.Value);
                model = ObjectMapper.Map<CreateOrEditMenuViewModel>(obj);
            }
            
            return PartialView("_CreateOrEditModal", model);
        }
    }
}