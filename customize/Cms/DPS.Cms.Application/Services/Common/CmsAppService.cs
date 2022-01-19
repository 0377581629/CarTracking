using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using DPS.Cms.Application.Manager;
using DPS.Cms.Application.Shared.Dto.Category;
using DPS.Cms.Application.Shared.Dto.ImageBlockGroup;
using DPS.Cms.Application.Shared.Dto.Common;
using DPS.Cms.Application.Shared.Dto.MenuGroup;
using DPS.Cms.Application.Shared.Dto.PageLayout;
using DPS.Cms.Application.Shared.Dto.PageTheme;
using DPS.Cms.Application.Shared.Dto.Tags;
using DPS.Cms.Application.Shared.Dto.Widget;
using DPS.Cms.Application.Shared.Interfaces.Common;
using DPS.Cms.Core.Advertisement;
using DPS.Cms.Core.Menu;
using DPS.Cms.Core.Page;
using DPS.Cms.Core.Post;
using DPS.Cms.Core.Widget;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Zero;

namespace DPS.Cms.Application.Services.Common
{
    [AbpAuthorize]
    public class CmsAppService : ZeroAppServiceBase, ICmsAppService
    {
        #region Constructor

        private readonly IRepository<ImageBlockGroup> _imageBlockGroupRepository;
        private readonly IRepository<Widget> _widgetRepository;
        private readonly IRepository<PageTheme> _pageThemeRepository;
        private readonly IRepository<PageLayout> _pageLayoutRepository;
        private readonly IRepository<Tags> _tagsRepository;
        private readonly IRepository<MenuGroup> _menuGroupRepository;
        private readonly CategoryManager _categoryManager;
        private readonly IRepository<Category> _categoryRepository;

        public CmsAppService(IRepository<ImageBlockGroup> imageBlockGroupRepository, 
            IRepository<Widget> widgetRepository, 
            IRepository<PageLayout> pageLayoutRepository,
            IRepository<Tags> tagsRepository,
            IRepository<MenuGroup> menuGroupRepository, 
            IRepository<PageTheme> pageThemeRepository, 
            CategoryManager categoryManager,
            IRepository<Category> categoryRepository)
        {
            _imageBlockGroupRepository = imageBlockGroupRepository;
            _widgetRepository = widgetRepository;
            _pageLayoutRepository = pageLayoutRepository;
            _tagsRepository = tagsRepository;
            _menuGroupRepository = menuGroupRepository;
            _pageThemeRepository = pageThemeRepository;
            _categoryManager = categoryManager;
            _categoryRepository = categoryRepository;
        }

        #endregion

        #region Image Block Group

        private IQueryable<ImageBlockGroupDto> ImageBlockGroupQuery(CmsInput input = null)
        {
            var query = from o in _imageBlockGroupRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.IsActive)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                select new ImageBlockGroupDto
                {
                    Id = o.Id,
                    Numbering = o.Numbering,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note,
                    Order = o.Order,
                    IsDefault = o.IsDefault,
                    IsActive = o.IsActive
                };
            return query;
        }

        public async Task<List<ImageBlockGroupDto>> GetAllImageBlockGroup()
        {
            var query = ImageBlockGroupQuery();
            return await query.OrderBy(o => o.Order).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetAllImageBlockGroupDropDown(int? current = default)
        {
            var res = await GetAllImageBlockGroup();
            return res.Select(o => new SelectListItem(o.Name, o.Id.ToString(), o.Id == current)).ToList();
        }

        public async Task<PagedResultDto<ImageBlockGroupDto>> GetPagedImageBlockGroups(CmsInput input)
        {
            var objQuery = ImageBlockGroupQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<ImageBlockGroupDto>(
                totalCount,
                res
            );
        }

        #endregion

        #region Menu Group 

        private IQueryable<MenuGroupDto> MenuGroupQuery(CmsInput input = null)
        {
            var query = from o in _menuGroupRepository.GetAll()
                    .Where(o => o.IsActive)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                select new MenuGroupDto
                {
                    Id = o.Id,
                    Numbering = o.Numbering,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note,
                    Order = o.Order,
                    IsDefault = o.IsDefault,
                    IsActive = o.IsActive
                };
            return query;
        }

        public async Task<List<MenuGroupDto>> GetAllMenuGroup()
        {
            var query = MenuGroupQuery();
            return await query.OrderBy(o => o.Order).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetAllMenuGroupDropDown(int? current = default)
        {
            var res = await GetAllMenuGroup();
            return res.Select(o => new SelectListItem(o.Name, o.Id.ToString(), o.Id == current)).ToList();
        }

        public async Task<PagedResultDto<MenuGroupDto>> GetPagedMenuGroups(CmsInput input)
        {
            var objQuery = MenuGroupQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<MenuGroupDto>(
                totalCount,
                res
            );
        }

        #endregion
        
        #region Widget
        private IQueryable<WidgetDto> WidgetQuery(CmsInput input = null)
        {
            var query = from o in _widgetRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.IsActive)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                    .WhereIf(input != null && input.WidgetId > 0 , o=>o.Id == input.WidgetId)
                select new WidgetDto
                {
                    Id = o.Id,
                    Numbering = o.Numbering,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note,
                    Order = o.Order,
                    IsDefault = o.IsDefault,
                    IsActive = o.IsActive,
                    
                    About = o.About,
                    ActionName = o.ActionName,
                    ControllerName = o.ControllerName,
                    JsBundleUrl = o.JsBundleUrl,
                    JsScript = o.JsScript,
                    JsPlain = o.JsPlain,
                    CssBundleUrl = o.CssBundleUrl,
                    CssScript = o.CssScript,
                    CssPlain = o.CssPlain,
                    ContentType = o.ContentType,
                    ContentCount = o.ContentCount,
                    AsyncLoad = o.AsyncLoad
                };

            return query;
        }
        
        public async Task<WidgetDto> GetWidget(EntityDto input)
        {
            var query = WidgetQuery(new CmsInput {WidgetId = input.Id});
            return await query.FirstOrDefaultAsync();
        }
        
        public async Task<List<WidgetDto>> GetAllWidget()
        {
            var query = WidgetQuery();
            return await query.OrderBy(o => o.Order).ToListAsync();
        }
        
        #endregion
        
        #region Page Theme
        
        private IQueryable<PageThemeDto> PageThemeQuery(CmsInput input = null)
        {
            var query = from o in _pageThemeRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.IsActive)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                select new PageThemeDto
                {
                    Id = o.Id,
                    Numbering = o.Numbering,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note,
                    Order = o.Order,
                    IsDefault = o.IsDefault,
                    IsActive = o.IsActive
                };
            return query;
        }

        public async Task<List<PageThemeDto>> GetAllPageTheme()
        {
            var query = PageThemeQuery();
            return await query.OrderBy(o => o.Order).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetAllPageThemeDropDown(int? current = default)
        {
            var res = await GetAllPageTheme();
            return res.Select(o => new SelectListItem(o.Name, o.Id.ToString(), o.Id == current)).ToList();
        }

        public async Task<PagedResultDto<PageThemeDto>> GetPagedPageThemes(CmsInput input)
        {
            var objQuery = PageThemeQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<PageThemeDto>(
                totalCount,
                res
            );
        }
        
        #endregion
        
        #region Page Layout
        
        private IQueryable<PageLayoutDto> PageLayoutQuery(CmsInput input = null)
        {
            var query = from o in _pageLayoutRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.IsActive)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                select new PageLayoutDto
                {
                    Id = o.Id,
                    Numbering = o.Numbering,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note,
                    Order = o.Order,
                    IsDefault = o.IsDefault,
                    IsActive = o.IsActive,
                    
                    PageThemeId = o.PageThemeId,
                    PageThemeCode = o.PageTheme != null ? o.PageTheme.Code : "",
                    PageThemeName = o.PageTheme != null ? o.PageTheme.Name : ""
                };
            return query;
        }

        public async Task<List<PageLayoutDto>> GetAllPageLayout()
        {
            var query = PageLayoutQuery();
            return await query.OrderBy(o => o.Order).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetAllPageLayoutDropDown(int? current = default)
        {
            var res = await GetAllPageLayout();
            return res.Select(o => new SelectListItem(o.Name, o.Id.ToString(), o.Id == current)).ToList();
        }

        public async Task<PagedResultDto<PageLayoutDto>> GetPagedPageLayouts(CmsInput input)
        {
            var objQuery = PageLayoutQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<PageLayoutDto>(
                totalCount,
                res
            );
        }
        
        #endregion
        
        #region Category
        
        private IQueryable<CategoryDto> CategoryQuery(CmsInput input = null)
        {
            var query = from o in _categoryRepository.GetAll()
                    .Where(o => !o.IsDeleted)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                select new CategoryDto
                {
                    Id = o.Id,
                    Numbering = o.Numbering,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note,
                    Order = o.Order,
                    IsDefault = o.IsDefault,
                    IsActive = o.IsActive
                };
            return query;
        }

        public async Task<List<CategoryDto>> GetAllCategory()
        {
            return await _categoryManager.GetAllCategory();
        }

        public async Task<List<SelectListItem>> GetCategory()
        {
             var res = await _categoryManager.GetAllCategory();
                       return res.Select(o =>
                           new SelectListItem(o.Name, o.Id.ToString())).ToList();
        }
        
        public async Task<PagedResultDto<CategoryDto>> GetPagedCategories(CmsInput input)
        {
            var objQuery = CategoryQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<CategoryDto>(
                totalCount,
                res
            );
        }
        #endregion
        
        #region Tags

        private IQueryable<TagsDto> TagsQuery(CmsInput input = null)
        {
            var query = from o in _tagsRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.IsActive)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                select new TagsDto
                {
                    Id = o.Id,
                    Numbering = o.Numbering,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note,
                    Order = o.Order,
                    IsDefault = o.IsDefault,
                    IsActive = o.IsActive
                };
            return query;
        }

        public async Task<List<TagsDto>> GetAllTags()
        {
            var query = TagsQuery();
            return await query.OrderBy(o => o.Order).ToListAsync();
        }

        public async Task<List<SelectListItem>> GetAllTagsDropDown(int? current = default)
        {
            var res = await GetAllTags();
            return res.Select(o => new SelectListItem(o.Name, o.Id.ToString(), o.Id == current)).ToList();
        }

        public async Task<PagedResultDto<TagsDto>> GetPagedTags(CmsInput input)
        {
            var objQuery = TagsQuery(input);
            var pagedAndFilteredObj = objQuery.OrderBy(input.Sorting ?? "name asc").PageBy(input);
            var totalCount = await objQuery.CountAsync();
            var res = await pagedAndFilteredObj.ToListAsync();

            return new PagedResultDto<TagsDto>(
                totalCount,
                res
            );
        }

        #endregion
    }
}