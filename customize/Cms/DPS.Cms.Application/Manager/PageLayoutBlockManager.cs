using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Abp.Zero;
using DPS.Cms.Core.Page;
using Microsoft.EntityFrameworkCore;
using Zero.Customize.NestedItem;

namespace DPS.Cms.Application.Manager
{
    public class PageLayoutBlockManager : DomainService
    {
        private readonly IRepository<PageLayoutBlock> _pageLayoutBlockRepository;

        public PageLayoutBlockManager(IRepository<PageLayoutBlock> pageLayoutBlockRepository)
        {
            _pageLayoutBlockRepository = pageLayoutBlockRepository;
            LocalizationSourceName = AbpZeroConsts.LocalizationSourceName;
        }

        [UnitOfWork]
        public virtual async Task CreateAsync(PageLayoutBlock obj)
        {
            obj.Code = await GetNextChildCodeAsync(obj.PageLayoutId, obj.ParentLayoutBlockId);
            await Validate(obj);
            await _pageLayoutBlockRepository.InsertAndGetIdAsync(obj);
        }

        protected virtual async Task<string> GetNextChildCodeAsync(int pageLayoutId, int? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(pageLayoutId, parentId);
            if (lastChild != null) return PageLayoutBlock.CalculateNextCode(lastChild.Code);
            var parentCode = parentId != null ? await GetCodeAsync(parentId.Value) : null;
            return PageLayoutBlock.AppendCode(parentCode, PageLayoutBlock.CreateCode(1));
        }

        protected virtual async Task<PageLayoutBlock> GetLastChildOrNullAsync(int pageLayoutId, int? parentId)
        {
            var children = await _pageLayoutBlockRepository.GetAllListAsync(o => o.PageLayoutId == pageLayoutId && o.ParentLayoutBlockId == parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        protected virtual async Task<string> GetCodeAsync(int id)
        {
            return (await _pageLayoutBlockRepository.GetAsync(id)).Code;
        }

        protected virtual async Task Validate(PageLayoutBlock obj)
        {
            var res = await _pageLayoutBlockRepository.GetAll()
                .Where(o => o.Code.Equals(obj.Code) && o.PageLayoutId == obj.PageLayoutId)
                .WhereIf(obj.Id > 0, o => o.Id != obj.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }
    }
}