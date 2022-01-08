using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using DPS.Cms.Application.Shared.Dto.Common;
using DPS.Cms.Application.Shared.Dto.ImageBlock;
using DPS.Cms.Application.Shared.Dto.Menu;
using DPS.Cms.Application.Shared.Dto.Page;
using DPS.Cms.Application.Shared.Dto.PageLayout;
using DPS.Cms.Application.Shared.Interfaces.Common;
using DPS.Cms.Core.Advertisement;
using DPS.Cms.Core.Menu;
using DPS.Cms.Core.Page;
using DPS.Cms.Core.Shared;
using Microsoft.EntityFrameworkCore;
using Zero;

namespace DPS.Cms.Application.Services.Common
{
    [AbpAllowAnonymous]
    public class CmsPublicAppService : ZeroAppServiceBase, ICmsPublicAppService
    {
        #region Constructor
        
        private readonly IRepository<Core.Page.Page> _pageRepository;
        private readonly IRepository<PageWidget> _pageWidgetRepository;
        private readonly IRepository<PageWidgetDetail> _pageWidgetDetailRepository;
        private readonly IRepository<ImageBlock> _imageBlockRepository;
        private readonly IRepository<Core.Menu.Menu> _menuRepository;
        private readonly IRepository<PageLayoutBlock> _pageLayoutBlockRepository;
        public CmsPublicAppService(IRepository<Core.Page.Page> pageRepository,
            IRepository<PageWidget> pageWidgetRepository,
            IRepository<PageWidgetDetail> pageWidgetDetailRepository,
            IRepository<ImageBlock> imageBlockRepository, 
            IRepository<PageLayoutBlock> pageLayoutBlockRepository,
            IRepository<Core.Menu.Menu> menuRepository)
        {
            _pageRepository = pageRepository;
            _pageWidgetRepository = pageWidgetRepository;
            _pageWidgetDetailRepository = pageWidgetDetailRepository;
            _imageBlockRepository = imageBlockRepository;
            _pageLayoutBlockRepository = pageLayoutBlockRepository;
            _menuRepository = menuRepository;
        }

        #endregion

        #region Page

        private IQueryable<PageDto> PageQuery(GetPageInput input)
        {
            var query = from o in _pageRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.Publish && o.IsActive && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrEmpty(input.PageSlug), e => e.Slug.ToLower() == input.PageSlug.ToLower())
                    .WhereIf(input is { PageId: { } }, o=>o.Id == input.PageId)
                    .WhereIf(input is { HomePage: { } }, e => e.IsHomePage == input.HomePage)
                select new PageDto
                {
                    Id = o.Id,
                    
                    #region Theme + Layout
                    
                    PageThemeId = o.PageLayout.PageThemeId,
                    PageThemeCode = o.PageLayout.PageTheme != null ? o.PageLayout.PageTheme.Code : "",
                    PageThemeName = o.PageLayout.PageTheme != null ? o.PageLayout.PageTheme.Name : "",
                    
                    PageLayoutId = o.PageLayoutId,
                    PageLayoutName = o.PageLayout.Name,
                    
                    #endregion
                    
                    Numbering = o.Numbering,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note,
                    Order = o.Order,
                    IsDefault = o.IsDefault,
                    IsActive = o.IsActive,

                    Summary = o.Summary,
                    Content = o.Content,
                    About = o.About,
                    Slug = o.Slug,
                    Url = o.Url,

                    IsHomePage = o.IsHomePage,
                    Publish = o.Publish,

                    #region SEO

                    TitleDefault = o.TitleDefault,
                    Title = o.Title,
                    DescriptionDefault = o.DescriptionDefault,
                    Description = o.Description,
                    AuthorDefault = o.AuthorDefault,
                    Author = o.Author,
                    KeywordDefault = o.KeywordDefault,
                    Keyword = o.Keyword

                    #endregion
                };

            return query;
        }
        
        public async Task<PageDto> GetHomePage()
        {
            return await PageQuery(new GetPageInput{HomePage = true}).FirstOrDefaultAsync();
        }

        public async Task<PageDto> GetPageById(GetPageInput input)
        {
            if (!input.PageId.HasValue)
                return null;
            return await PageQuery(new GetPageInput{PageId = input.PageId}).FirstOrDefaultAsync();
        }

        public async Task<PageDto> GetPageBySlug(GetPageInput input)
        {
            if (string.IsNullOrEmpty(input.PageSlug))
                return null;
            return await PageQuery(new GetPageInput{PageSlug = input.PageSlug}).FirstOrDefaultAsync();
        }

        private IQueryable<PageWidgetDto> PageWidgetByPageQuery(int pageId)
        {
            var query = from o in _pageWidgetRepository.GetAll().Where(o => o.PageId == pageId)
                select new PageWidgetDto
                {
                    Id = o.Id,
                    PageId = o.PageId,
                    PageBlockColumnId = o.PageBlockColumnId,

                    WidgetId = o.WidgetId,
                    WidgetName = o.Widget.Name,
                    WidgetControllerName = o.Widget.ControllerName,
                    WidgetActionName = o.Widget.ActionName,
                    WidgetContentType = o.Widget.ContentType,
                    WidgetContentCount = o.Widget.ContentCount,
                    WidgetJsBundleUrl = o.Widget.JsBundleUrl,
                    WidgetJsScript = o.Widget.JsScript,
                    WidgetJsPlain = o.Widget.JsPlain,

                    WidgetCssBundleUrl = o.Widget.CssBundleUrl,
                    WidgetCssScript = o.Widget.CssScript,
                    WidgetCssPlain = o.Widget.CssPlain,

                    Order = o.Order
                };

            return query;
        }

        private IQueryable<PageWidgetDto> PageWidgetByIdQuery(int pageWidgetId)
        {
            var query = from o in _pageWidgetRepository.GetAll().Where(o => o.Id == pageWidgetId)
                select new PageWidgetDto
                {
                    Id = o.Id,
                    PageId = o.PageId,
                    
                    PageThemeId = o.Page.PageLayout.PageThemeId,
                    PageThemeCode = o.Page.PageLayout.PageTheme != null ? o.Page.PageLayout.PageTheme.Code : "",
                    PageThemeName = o.Page.PageLayout.PageTheme != null ? o.Page.PageLayout.PageTheme.Name : "",
                    
                    PageBlockColumnId = o.PageBlockColumnId,

                    WidgetId = o.WidgetId,
                    WidgetName = o.Widget.Name,
                    WidgetControllerName = o.Widget.ControllerName,
                    WidgetActionName = o.Widget.ActionName,
                    WidgetContentType = o.Widget.ContentType,
                    WidgetContentCount = o.Widget.ContentCount,
                    WidgetJsBundleUrl = o.Widget.JsBundleUrl,
                    WidgetJsScript = o.Widget.JsScript,
                    WidgetJsPlain = o.Widget.JsPlain,

                    WidgetCssBundleUrl = o.Widget.CssBundleUrl,
                    WidgetCssScript = o.Widget.CssScript,
                    WidgetCssPlain = o.Widget.CssPlain,

                    Order = o.Order
                };

            return query;
        }

        private IQueryable<PageWidgetDetailDto> PageWidgetDetailByPageWidgetIdsQuery(ICollection<int> pageWidgetIds)
        {
            var query = from o in _pageWidgetDetailRepository.GetAll()
                    .Where(o => pageWidgetIds.Contains(o.PageWidgetId))
                select new PageWidgetDetailDto
                {
                    Id = o.Id,
                    PageWidgetId = o.PageWidgetId,


                    #region Image Block Group

                    ImageBlockGroupId = o.ImageBlockGroupId,
                    ImageBlockGroupCode = o.ImageBlockGroup != null ? o.ImageBlockGroup.Code : "",
                    ImageBlockGroupName = o.ImageBlockGroup != null ? o.ImageBlockGroup.Name : "",

                    #endregion

                    #region Menu Group

                    MenuGroupId = o.MenuGroupId,
                    MenuGroupCode = o.MenuGroup != null ? o.MenuGroup.Code : "",
                    MenuGroupName = o.MenuGroup != null ? o.MenuGroup.Name : "",

                    #endregion

                    CustomContent = o.CustomContent
                };

            return query;
        }

        public async Task<List<PageWidgetDto>> GetPageWidgets(CmsInput input)
        {
            if (!input.PageId.HasValue) return null;

            var query = PageWidgetByPageQuery(input.PageId.Value);
            var res = await query.ToListAsync();
            if (res == null || !res.Any()) return res;
            var pageWidgetIds = res.Where(o => o.Id.HasValue).Select(o => o.Id.Value).ToList();
            var pageWidgetDetailQuery = PageWidgetDetailByPageWidgetIdsQuery(pageWidgetIds);
            var pageWidgetDetails = await pageWidgetDetailQuery.ToListAsync();
            if (pageWidgetDetails == null || !pageWidgetDetails.Any()) return res;
            foreach (var pageWidget in res)
            {
                pageWidget.Details = pageWidgetDetails.Where(o => o.PageWidgetId == pageWidget.Id).ToList();
            }

            return res;
        }

        public async Task<PageWidgetDto> GetPageWidget(CmsInput input)
        {
            if (!input.PageWidgetId.HasValue) return null;
            var query = PageWidgetByIdQuery(input.PageWidgetId.Value);
            var res = await query.FirstOrDefaultAsync();
            if (res is { Id: { } })
            {
                var pageWidgetDetailQuery = PageWidgetDetailByPageWidgetIdsQuery(new List<int> { res.Id.Value });
                var pageWidgetDetails = await pageWidgetDetailQuery.ToListAsync();
                if (pageWidgetDetails == null || !pageWidgetDetails.Any()) return res;
                res.Details = pageWidgetDetails.Where(o => o.PageWidgetId == res.Id).ToList();
                if (res.Details.Any())
                {
                    switch (res.WidgetContentType)
                    {
                        case (int)CmsEnums.WidgetContentType.Service:
                            
                            break;
                        case (int)CmsEnums.WidgetContentType.ServiceType:
                            
                            break;
                        case (int)CmsEnums.WidgetContentType.ServiceCategory:
                            
                            break;
                        case (int)CmsEnums.WidgetContentType.ServiceArticle:
                            
                            break;
                        case (int)CmsEnums.WidgetContentType.ServiceVendor:
                            
                            break;
                        case (int)CmsEnums.WidgetContentType.ServiceBrand:
                            
                            break;
                        case (int)CmsEnums.WidgetContentType.ServicePropertyGroup:
                           
                            break;
                        case (int)CmsEnums.WidgetContentType.ReviewPost:
                            
                            break;
                        case (int)CmsEnums.WidgetContentType.ImageBlock:
                           
                            break;
                        case (int)CmsEnums.WidgetContentType.MenuGroup:
                            
                            break;
                    }
                }
            }

            return res;
        }

        private IQueryable<PageLayoutBlockDto> PageLayoutBlockByPageLayoutQuery(int pageLayoutId)
        {
            var query = from o in _pageLayoutBlockRepository.GetAll()
                    .Where(o => o.PageLayoutId == pageLayoutId)
                select new PageLayoutBlockDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    Name = o.Name,
                    PageLayoutId = o.PageLayoutId,
                    UniqueId = o.UniqueId,
                    ColumnCount = o.ColumnCount,
                    WrapInRow = o.WrapInRow,
                    Order = o.Order,
                    ParentBlockId = o.ParentLayoutBlockId,
                    ParentBlockUniqueId = o.ParentLayoutBlock.UniqueId,
                    ParentColumnUniqueId = o.ParentColumnUniqueId,

                    Col1Id = o.Col1Id,
                    Col1UniqueId = o.Col1UniqueId,
                    Col1Class = o.Col1Class,

                    Col2Id = o.Col2Id,
                    Col2UniqueId = o.Col2UniqueId,
                    Col2Class = o.Col2Class,

                    Col3Id = o.Col3Id,
                    Col3UniqueId = o.Col3UniqueId,
                    Col3Class = o.Col3Class,

                    Col4Id = o.Col4Id,
                    Col4UniqueId = o.Col4UniqueId,
                    Col4Class = o.Col4Class
                };

            return query.OrderBy(o => o.Code);
        }

        public async Task<List<PageLayoutBlockDto>> GetPageLayoutBlocks(CmsInput input)
        {
            if (!input.PageLayoutId.HasValue) return null;

            var query = PageLayoutBlockByPageLayoutQuery(input.PageLayoutId.Value);
            var pageBlocks = await query.ToListAsync();
            if (pageBlocks == null || !pageBlocks.Any()) return null;
            var res = pageBlocks
                .Where(o => o.ParentBlockId == null)
                .Select(o => new PageLayoutBlockDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    Name = o.Name,

                    UniqueId = o.UniqueId,
                    ColumnCount = o.ColumnCount,
                    WrapInRow = o.WrapInRow,
                    Order = o.Order,
                    ParentBlockId = o.ParentBlockId,
                    ParentBlockUniqueId = o.ParentBlockUniqueId,
                    ParentColumnUniqueId = o.ParentColumnUniqueId,

                    Col1Id = o.Col1Id,
                    Col1UniqueId = o.Col1UniqueId,
                    Col1Class = o.Col1Class,

                    Col2Id = o.Col2Id,
                    Col2UniqueId = o.Col2UniqueId,
                    Col2Class = o.Col2Class,

                    Col3Id = o.Col3Id,
                    Col3UniqueId = o.Col3UniqueId,
                    Col3Class = o.Col3Class,

                    Col4Id = o.Col4Id,
                    Col4UniqueId = o.Col4UniqueId,
                    Col4Class = o.Col4Class,
                    SubBlocks = GetChildren(pageBlocks, o.Id)
                })
                .ToList();

            return res;
        }

        #endregion

        #region Image Block

        private IQueryable<ImageBlockDto> ImageBlockQuery(CmsPublicInput input = null)
        {
            var query = from o in _imageBlockRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.IsActive)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter),
                        e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                    .WhereIf(input is { ImageBlockGroupIds: { } } && input.ImageBlockGroupIds.Any(),
                        o => input.ImageBlockGroupIds.Contains(o.ImageBlockGroupId))
                select new ImageBlockDto
                {
                    Id = o.Id,
                    Numbering = o.Numbering,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note,
                    Order = o.Order,
                    IsDefault = o.IsDefault,
                    IsActive = o.IsActive,

                    ImageBlockGroupId = o.ImageBlockGroupId,
                    ImageBlockGroupCode = o.ImageBlockGroup.Code,
                    ImageBlockGroupName = o.ImageBlockGroup.Name,
                    Image = o.Image,
                    TargetUrl = o.TargetUrl
                };
            return query;
        }

        public async Task<List<ImageBlockDto>> GetImageBlocksByGroupIds(CmsPublicInput input)
        {
            return await ImageBlockQuery(input).ToListAsync();
        }

        #endregion

        #region Menu

        private IQueryable<MenuDto> MenuQuery(CmsPublicInput input = null)
        {
            var query = from o in _menuRepository.GetAll()
                    .Where(o => o.IsActive)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter),
                        e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                    .WhereIf(input is { MenuGroupIds: { } } && input.MenuGroupIds.Any(),
                        o => input.MenuGroupIds.Contains(o.MenuGroupId))
                select new MenuDto
                {
                    Id = o.Id,
                    Numbering = o.Numbering,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note,
                    Order = o.Order,
                    IsDefault = o.IsDefault,
                    IsActive = o.IsActive,

                    MenuGroupId = o.MenuGroupId,
                    MenuGroupCode = o.MenuGroup.Code,
                    MenuGroupName = o.MenuGroup.Name,
                    Url = o.Url
                };
            return query;
        }

        public async Task<List<MenuDto>> GetMenusByGroupIds(CmsPublicInput input)
        {
            return await MenuQuery(input).ToListAsync();
        }

        #endregion

        #region Support Methods

        private List<PageLayoutBlockDto> GetChildren(List<PageLayoutBlockDto> blocks, int? parentId)
        {
            return blocks
                .Where(o => o.ParentBlockId == parentId)
                .Select(o => new PageLayoutBlockDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    PageLayoutId = o.PageLayoutId,
                    UniqueId = o.UniqueId,
                    ColumnCount = o.ColumnCount,
                    Order = o.Order,
                    ParentBlockId = o.ParentBlockId,
                    ParentBlockUniqueId = o.ParentBlockUniqueId,
                    ParentColumnUniqueId = o.ParentColumnUniqueId,

                    Col1Id = o.Col1Id,
                    Col1UniqueId = o.Col1UniqueId,
                    Col1Class = o.Col1Class,

                    Col2Id = o.Col2Id,
                    Col2UniqueId = o.Col2UniqueId,
                    Col2Class = o.Col2Class,

                    Col3Id = o.Col3Id,
                    Col3UniqueId = o.Col3UniqueId,
                    Col3Class = o.Col3Class,

                    Col4Id = o.Col4Id,
                    Col4UniqueId = o.Col4UniqueId,
                    Col4Class = o.Col4Class,
                    SubBlocks = GetChildren(blocks, o.Id)
                })
                .ToList();
        }

        #endregion
    }
}