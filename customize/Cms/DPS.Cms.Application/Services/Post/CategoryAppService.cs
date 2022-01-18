using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using DPS.Cms.Application.Manager;
using DPS.Cms.Application.Shared.Dto.Category;
using DPS.Cms.Application.Shared.Interfaces;
using DPS.Cms.Core.Post;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;
using Zero.Customize;
using Zero.Customize.NestedItem;

namespace DPS.Cms.Application.Services.Post
{
    [AbpAuthorize(CmsPermissions.Category)]
    public class CategoryAppService : ZeroAppServiceBase, ICategoryAppService
    {
        private readonly CategoryManager _categoryManager;
        private readonly IRepository<Category> _categoryRepository;

        public CategoryAppService(
            CategoryManager categoryManager,
            IRepository<Category> categoryRepository)
        {
            _categoryManager = categoryManager;
            _categoryRepository = categoryRepository;
        }

        private IQueryable<NestedItem> CategoryNestedQuery()
        {
            var query = from obj in _categoryRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .OrderBy(o => o.Code)
                select new NestedItem
                {
                    Id = obj.Id,
                    ParentId = obj.ParentId,
                    DisplayName = $"{obj.CategoryCode} - {obj.Name}"
                };
            return query;
        }

        public async Task<List<NestedItem>> GetAllNested()
        {
            var objQuery = CategoryNestedQuery();
            var res = await objQuery.ToListAsync();
            NestedItemHelper.BuildRecursiveItem(ref res);
            return res;
        }

        public async Task<ListResultDto<CategoryDto>> GetCategories()
        {
            var objs = await _categoryRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                .Select(o => new CategoryDto
                {
                    Id = o.Id,
                    ParentId = o.ParentId,
                    ParentCode = o.Parent != null ? o.Parent.Code : "",
                    ParentCategoryCode = o.Parent != null ? o.Parent.CategoryCode : "",
                    ParentName = o.Parent != null ? o.Parent.Name : "",
                    Name = o.Name,
                    Code = o.Code,
                    CategoryCode = o.CategoryCode,
                    About = o.About,
                    Url = o.Url,
                    Slug = o.Slug,
                    Image = o.Image,
                    PostCount = o.PostCount,
                    CommentCount = o.CommentCount,
                    ViewCount = o.ViewCount,
                    
                    TitleDefault = o.TitleDefault,
                    Title = o.Title,
                    
                    DescriptionDefault = o.DescriptionDefault,
                    Description = o.Description,
                    
                    KeywordDefault = o.KeywordDefault,
                    Keyword = o.Keyword,
                    
                    AuthorDefault = o.AuthorDefault,
                    Author = o.Author

                }).OrderBy(o => o.Code).ToListAsync();

            return new ListResultDto<CategoryDto>(objs);
        }

        public async Task<NestedItem> CreateOrEditCategory(CreateOrEditCategoryDto input)
        {
            if (input.Id.HasValue)
            {
                return await UpdateCategory(input);
            }

            return await CreateCategory(input);
        }

        [AbpAuthorize(CmsPermissions.Category_Create)]
        private async Task<NestedItem> CreateCategory(CreateOrEditCategoryDto input)
        {
            var category = ObjectMapper.Map<Category>(input);
            category.TenantId = AbpSession.TenantId;
            await _categoryManager.CreateAsync(category);
            await CurrentUnitOfWork.SaveChangesAsync();

            return new NestedItem
            {
                Id = category.Id,
                DisplayName = $"{category.CategoryCode} - {category.Name}"
            };
        }

        [AbpAuthorize(CmsPermissions.Category_Edit)]
        private async Task<NestedItem> UpdateCategory(CreateOrEditCategoryDto input)
        {
            var obj = await _categoryRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            
            if (obj == null)
                throw new UserFriendlyException(L("NotFound"));

            if (obj.TenantId != AbpSession.TenantId)
                throw new UserFriendlyException(L("NotHavePermission"));
            
            obj.CategoryCode = input.CategoryCode;
            obj.Name = input.Name;
            obj.About = input.About;
            obj.Url = input.Url;
            obj.Slug = input.Slug;
            
            obj.TitleDefault = input.TitleDefault;
            obj.Title = input.Title;
            
            obj.DescriptionDefault = input.DescriptionDefault;
            obj.Description = input.Description;
            
            obj.KeywordDefault = input.KeywordDefault;
            obj.Keyword = input.Keyword;
            
            obj.AuthorDefault = input.AuthorDefault;
            obj.Author = input.Author;
            
            await _categoryManager.UpdateAsync(obj);
            
            return new NestedItem
            {
                Id = obj.Id,
                DisplayName = $"{obj.CategoryCode} - {obj.Name}"
            };
        }

        [AbpAuthorize(CmsPermissions.Category_Create, CmsPermissions.Category_Edit)]
        public async Task UpdateStructure(UpdateCategoryStructureInput input)
        {
            await _categoryManager.RebuildCode(AbpSession.TenantId, input.NestedItems);
        }

        [AbpAuthorize(CmsPermissions.Category_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _categoryRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            
            if (obj == null)
                throw new UserFriendlyException(L("NotFound"));
            
            if (obj.TenantId != AbpSession.TenantId)
                throw new UserFriendlyException(L("NotHavePermission"));
            
            await _categoryManager.DeleteAsync(input.Id);
        }
    }
}