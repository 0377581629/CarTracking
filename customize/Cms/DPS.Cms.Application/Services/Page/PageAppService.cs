using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Cms.Application.Shared.Dto.Page;
using DPS.Cms.Application.Shared.Dto.PageLayout;
using DPS.Cms.Application.Shared.Interfaces;
using DPS.Cms.Core.Page;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Cms.Application.Services.Page
{
    [AbpAuthorize(CmsPermissions.Page)]
    public class PageAppService : ZeroAppServiceBase, IPageAppService
    {
        #region Constructor
        private readonly IRepository<Core.Page.Page> _pageRepository;
        private readonly IRepository<PageWidget> _pageWidgetRepository;
        private readonly IRepository<PageWidgetDetail> _pageWidgetDetailRepository;
        private readonly IRepository<PageLayoutBlock> _pageLayoutBlockRepository;
        
        public PageAppService(IRepository<Core.Page.Page> pageRepository, 
            IRepository<PageWidgetDetail> pageWidgetDetailRepository, 
            IRepository<PageWidget> pageWidgetRepository, 
            IRepository<PageLayoutBlock> pageLayoutBlockRepository)
        {
            _pageRepository = pageRepository;
            _pageWidgetDetailRepository = pageWidgetDetailRepository;
            _pageWidgetRepository = pageWidgetRepository;
            _pageLayoutBlockRepository = pageLayoutBlockRepository;
        }
        #endregion
        
        private IQueryable<PageDto> PageQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from o in _pageRepository.GetAll()
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                    .WhereIf(input is { PageThemeId: { } }, o=>o.PageLayout.PageThemeId == input.PageThemeId)
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
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

        private class QueryInput
        {
            public GetAllPageInput Input { get; set; }
            public int? Id { get; set; }
        }

        public async Task<PagedResultDto<GetPageForViewDto>> GetAll(GetAllPageInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };

            var objQuery = PageQuery(queryInput);

            var pagedAndFilteredObjs = objQuery
                .OrderBy(input.Sorting ?? "order asc")
                .PageBy(input);

            var objs = from o in pagedAndFilteredObjs
                select new GetPageForViewDto
                {
                    Page = o
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetPageForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(CmsPermissions.Page_Edit)]
        public async Task<GetPageForEditOutput> GetPageForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var objQuery = PageQuery(queryInput);

            var obj = await objQuery.FirstOrDefaultAsync();

            var output = new GetPageForEditOutput
            {
                Page = ObjectMapper.Map<CreateOrEditPageDto>(obj)
            };

            return output;
        }

        private async Task ValidateDataInput(CreateOrEditPageDto input)
        {
            var res = await _pageRepository.GetAll()
                .Where(o => !o.IsDeleted && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditPageDto input)
        {
            input.Code = input.Code.Replace(" ", "");
            await ValidateDataInput(input);
            if (input.Id == null)
                await Create(input);
            else
                await Update(input);
        }

        [AbpAuthorize(CmsPermissions.Page_Create)]
        protected virtual async Task Create(CreateOrEditPageDto input)
        {
            var obj = ObjectMapper.Map<Core.Page.Page>(input);
            obj.TenantId = AbpSession.TenantId;
            await _pageRepository.InsertAndGetIdAsync(obj);

            if (obj.IsHomePage)
            {
                var otherObjs = await _pageRepository.GetAllListAsync(o => o.Id != obj.Id);
                if (otherObjs.Any())
                {
                    foreach (var changeDefault in otherObjs)
                    {
                        changeDefault.IsHomePage = false;
                    }
                }
            }
        }

        [AbpAuthorize(CmsPermissions.Page_Edit)]
        protected virtual async Task Update(CreateOrEditPageDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _pageRepository.FirstOrDefaultAsync(o => o.Id == (int) input.Id);

                if (obj == null)
                    throw new UserFriendlyException(L("NotFound"));

                ObjectMapper.Map(input, obj);

                if (obj.IsHomePage)
                {
                    var otherObjs = await _pageRepository.GetAllListAsync(o => o.Id != obj.Id);
                    if (otherObjs.Any())
                    {
                        foreach (var changeDefault in otherObjs)
                        {
                            changeDefault.IsHomePage = false;
                        }
                    }
                }

                await _pageRepository.UpdateAsync(obj);
            }
        }

        [AbpAuthorize(CmsPermissions.Page_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _pageRepository.FirstOrDefaultAsync(o => o.Id == input.Id);
            if (obj == null)
                throw new UserFriendlyException(L("NotFound"));
            await _pageRepository.DeleteAsync(obj.Id);
        }
        
        #region Config Ui

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
                    WidgetContentType = o.Widget.ContentType,
                    WidgetContentCount = o.Widget.ContentCount,
                    Order = o.Order
                };

            return query;
        }

        private IQueryable<PageWidgetDto> PageWidgetQuery(int id)
        {
            var query = from o in _pageWidgetRepository.GetAll().Where(o => o.Id == id)
                select new PageWidgetDto
                {
                    Id = o.Id,
                    PageId = o.PageId,
                    PageBlockColumnId = o.PageBlockColumnId,
                    WidgetId = o.WidgetId,
                    WidgetName = o.Widget.Name,
                    WidgetContentType = o.Widget.ContentType,
                    WidgetContentCount = o.Widget.ContentCount,
                    Order = o.Order
                };

            return query;
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

            return query.OrderBy(o=>o.Code);
        }
        
        private IQueryable<PageWidgetDetailDto> PageWidgetDetailQuery(int pageWidgetId)
        {
            var query = from o in _pageWidgetDetailRepository.GetAll().Where(o => o.PageWidgetId == pageWidgetId)
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

        private IQueryable<PageWidgetDetailDto> PageWidgetDetailByPageWidgetIdsQuery(List<int> pageWidgetIds)
        {
            var query = from o in _pageWidgetDetailRepository.GetAll().Where(o => pageWidgetIds.Contains(o.PageWidgetId))
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

        [AbpAuthorize(CmsPermissions.Page_Edit)]
        public async Task<PageConfigDto> GetPageConfig(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };
            var objQuery = PageQuery(queryInput);
            var obj = await objQuery.FirstOrDefaultAsync();
            var res = ObjectMapper.Map<PageConfigDto>(obj);
            var pageWidgetQuery = PageWidgetByPageQuery(obj.Id);
            var pageBlocks = await PageLayoutBlockByPageLayoutQuery(obj.PageLayoutId).ToListAsync();
            res.Blocks = pageBlocks
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
            res.Widgets = await pageWidgetQuery.ToListAsync();
            if (res.Widgets == null || !res.Widgets.Any()) return res;
            var pageWidgetIds = res.Widgets.Where(o => o.Id.HasValue).Select(o => o.Id.Value).ToList();
            var pageWidgetDetailQuery = PageWidgetDetailByPageWidgetIdsQuery(pageWidgetIds);
            var pageWidgetDetails = await pageWidgetDetailQuery.ToListAsync();
            if (pageWidgetDetails == null || !pageWidgetDetails.Any()) return res;
            foreach (var pageWidget in res.Widgets)
            {
                pageWidget.Details = pageWidgetDetails.Where(o => o.PageWidgetId == pageWidget.Id).ToList();
            }

            return res;
        }

        [AbpAuthorize(CmsPermissions.Page_Edit)]
        public async Task<PageWidgetDto> GetPageWidgetForEdit(EntityDto input)
        {
            var obj = await PageWidgetQuery(input.Id).FirstOrDefaultAsync();
            if (obj?.Id != null)
                obj.Details = await PageWidgetDetailQuery(obj.Id.Value).ToListAsync();
            return obj;
        }

        [AbpAuthorize(CmsPermissions.Page_Edit)]
        public async Task UpdatePageDetails(PageConfigDto input)
        {
            var page = await _pageRepository.FirstOrDefaultAsync(input.Id);
            if (page.TenantId != AbpSession.TenantId)
                throw new UserFriendlyException(L("NotHavePermission"));

            input.Widgets ??= new List<PageWidgetDto>();
            await _pageWidgetDetailRepository.DeleteAsync(o => o.PageWidget.PageId == page.Id);
            await _pageWidgetRepository.DeleteAsync(o => o.PageId == page.Id);
            if (input.Widgets.Any())
            {
                foreach (var detail in input.Widgets)
                {
                    detail.PageId = page.Id;
                    var detailEntity = ObjectMapper.Map<PageWidget>(detail);
                    await _pageWidgetRepository.InsertAndGetIdAsync(detailEntity);
                    detail.Details ??= new List<PageWidgetDetailDto>();
                    var configDetails = ObjectMapper.Map<List<PageWidgetDetail>>(detail.Details);
                    if (!configDetails.Any()) continue;
                    foreach (var cf in configDetails)
                        cf.PageWidgetId = detailEntity.Id;
                    await _pageWidgetDetailRepository.GetDbContext().BulkInsertAsync(configDetails);
                }
            }
        }
        #endregion
        
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
    }
}