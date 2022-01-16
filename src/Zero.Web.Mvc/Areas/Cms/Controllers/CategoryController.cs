using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using DPS.Cms.Application.Shared.Dto.Category;
using DPS.Cms.Core.Post;
using Microsoft.AspNetCore.Mvc;
using Zero.Authorization;
using Zero.Customize;
using Zero.Web.Areas.Cms.Models.Category;
using Zero.Web.Controllers;

namespace Zero.Web.Areas.Cms.Controllers
{
    [Area("Cms")]
    [AbpMvcAuthorize(CmsPermissions.Category)]
    public class CategoryController : ZeroControllerBase
    {
        private readonly IRepository<Category> _categoryRepository;
        
        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AbpMvcAuthorize(CmsPermissions.Category_Create,CmsPermissions.Category_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            var model = new CreateOrEditCategoryModalViewModel
            {
                Category = new CreateOrEditCategoryDto
                {
                    Code = StringHelper.ShortIdentity(),
                    CategoryCode = StringHelper.ShortIdentity() 
                }
            };

            if (id.HasValue)
            {
                model.Category = ObjectMapper.Map<CreateOrEditCategoryDto>(await _categoryRepository.GetAsync(id.Value));
            }
            
            return PartialView("_CreateOrEditModal", model);
        }
    }
}