using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Cms.Application.Manager;
using DPS.Cms.Application.Shared.Dto.PageLayout;
using DPS.Cms.Application.Shared.Interfaces;
using DPS.Cms.Core.Page;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Cms.Application.Services.Page
{
    [AbpAuthorize(CmsPermissions.PageLayout)]
    public class PageLayoutAppService : ZeroAppServiceBase, IPageLayoutAppService
    {
        #region Constructor
        private readonly IRepository<PageLayout> _pageLayoutRepository;
        private readonly IRepository<PageLayoutBlock> _pageLayoutBlockRepository;
        private readonly PageLayoutBlockManager _pageLayoutBlockManager;
        
        public PageLayoutAppService(IRepository<PageLayout> pageLayoutRepository, 
            IRepository<PageLayoutBlock> pageLayoutBlockRepository, 
            PageLayoutBlockManager pageLayoutBlockManager)
        {
            _pageLayoutRepository = pageLayoutRepository;
            _pageLayoutBlockRepository = pageLayoutBlockRepository;
            _pageLayoutBlockManager = pageLayoutBlockManager;
        }
        #endregion
        
        private IQueryable<PageLayoutDto> PageLayoutQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from o in _pageLayoutRepository.GetAll()
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                    .WhereIf(input is { PageThemeId: { } }, o=>o.PageThemeId == input.PageThemeId)
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
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

        private class QueryInput
        {
            public GetAllPageLayoutInput Input { get; set; }
            public int? Id { get; set; }
        }

        public async Task<PagedResultDto<GetPageLayoutForViewDto>> GetAll(GetAllPageLayoutInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };

            var objQuery = PageLayoutQuery(queryInput);

            var pageLayoutAndFilteredObjs = objQuery
                .OrderBy(input.Sorting ?? "order asc")
                .PageBy(input);

            var objs = from o in pageLayoutAndFilteredObjs
                select new GetPageLayoutForViewDto
                {
                    PageLayout = o
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetPageLayoutForViewDto>(
                totalCount,
                res
            );
        }
        
        private async Task ValidateDataInput(CreateOrEditPageLayoutDto input)
        {
            var res = await _pageLayoutRepository.GetAll()
                .Where(o => !o.IsDeleted && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        [AbpAuthorize(CmsPermissions.PageLayout_Edit)]
        public async Task<GetPageLayoutForEditOutput> GetPageLayoutForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var objQuery = PageLayoutQuery(queryInput);

            var obj = await objQuery.FirstOrDefaultAsync();

            var output = new GetPageLayoutForEditOutput
            {
                PageLayout = ObjectMapper.Map<CreateOrEditPageLayoutDto>(obj)
            };

            return output;
            
        }
        public async Task CreateOrEdit(CreateOrEditPageLayoutDto input)
        {
            input.Code = input.Code.Replace(" ", "");
            await ValidateDataInput(input);
            if (input.Id == null)
                await Create(input);
            else
                await Update(input);
        }

        [AbpAuthorize(CmsPermissions.PageLayout_Create)]
        protected virtual async Task Create(CreateOrEditPageLayoutDto input)
        {
            var obj = ObjectMapper.Map<PageLayout>(input);
            obj.TenantId = AbpSession.TenantId;
            await _pageLayoutRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(CmsPermissions.PageLayout_Edit)]
        protected virtual async Task Update(CreateOrEditPageLayoutDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _pageLayoutRepository.FirstOrDefaultAsync(o => o.Id == (int) input.Id);
                if (obj == null)
                    throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
                await _pageLayoutRepository.UpdateAsync(obj);
            }
        }

        [AbpAuthorize(CmsPermissions.PageLayout_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _pageLayoutRepository.FirstOrDefaultAsync(o => o.Id == input.Id);
            if (obj == null)
                throw new UserFriendlyException(L("NotFound"));
            await _pageLayoutRepository.DeleteAsync(obj.Id);
        }

        #region Config Layout
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
        
        [AbpAuthorize(CmsPermissions.PageLayout_Edit)]
        public async Task<PageLayoutConfigDto> GetPageLayoutForConfig(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var objQuery = PageLayoutQuery(queryInput);
            var obj = await objQuery.FirstOrDefaultAsync();
            if (obj == null) return null;
            var res = ObjectMapper.Map<PageLayoutConfigDto>(obj);
            var blockQuery = PageLayoutBlockByPageLayoutQuery(obj.Id);
            var pageLayoutBlocks = await blockQuery.ToListAsync();
            res.Blocks = pageLayoutBlocks
                .Where(o => o.ParentBlockId == null)
                .Select(o => new PageLayoutBlockDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    Name = o.Name,
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
                    SubBlocks = GetChildren(pageLayoutBlocks, o.Id)
                })
                .ToList();
            return res;
        }
        
        [AbpAuthorize(CmsPermissions.PageLayout_Edit)]
        public async Task UpdateConfig(PageLayoutConfigDto input)
        {
            var pageLayout = await _pageLayoutRepository.FirstOrDefaultAsync(input.Id);
            if (pageLayout.TenantId != AbpSession.TenantId)
                throw new UserFriendlyException(L("NotHavePermission"));
            var oldBlockIds = await _pageLayoutBlockRepository.GetAll().Where(o => o.PageLayoutId == pageLayout.Id).Select(o => o.Id).ToListAsync();
            input.Blocks ??= new List<PageLayoutBlockDto>();
            if (input.Blocks.Any())
            {
                var addedBlock = new List<PageLayoutBlock>();
                foreach (var block in input.Blocks)
                {
                    block.Id = null;
                    block.PageLayoutId = pageLayout.Id;
                    block.ParentBlockId = null;
                    var blockEntity = ObjectMapper.Map<PageLayoutBlock>(block);
                    if (!string.IsNullOrEmpty(block.ParentBlockUniqueId))
                    {
                        var parent = addedBlock.FirstOrDefault(o => o.UniqueId == block.ParentBlockUniqueId);
                        if (parent != null)
                        {
                            blockEntity.ParentLayoutBlockId = parent.Id;
                        }
                    }
                    await _pageLayoutBlockManager.CreateAsync(blockEntity);
                    addedBlock.Add(blockEntity);
                }
            }
            if (oldBlockIds.Any())
                await _pageLayoutBlockRepository.DeleteAsync(o => oldBlockIds.Contains(o.Id));
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
                    SubBlocks = GetChildren(blocks, o.Id)
                })
                .ToList();
        }
        #endregion
    }
}