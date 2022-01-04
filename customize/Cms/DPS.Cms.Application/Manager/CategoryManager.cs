using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Configuration.Startup;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Zero;
using DPS.Cms.Application.Shared.Dto.Category;
using DPS.Cms.Core.Post;
using Microsoft.EntityFrameworkCore;
using Zero.Customize.NestedItem;

namespace DPS.Cms.Application.Manager
{
    public class CategoryManager : DomainService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IAbpSession _abpSession;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        
        private const string CacheName = "CmsCategory";
        
        public CategoryManager(IRepository<Category> categoryRepository, ICacheManager cacheManager, IAbpSession abpSession, IMultiTenancyConfig multiTenancyConfig)
        {
            _categoryRepository = categoryRepository;
            _cacheManager = cacheManager;
            _abpSession = abpSession;
            _multiTenancyConfig = multiTenancyConfig;
            LocalizationSourceName = AbpZeroConsts.LocalizationSourceName;
        }

        #region CRUD
        [UnitOfWork]
        public virtual async Task CreateAsync(Category obj)
        {
            obj.Code = await GetNextChildCodeAsync(obj.ParentId);
            await Validate(obj);
            await _categoryRepository.InsertAndGetIdAsync(obj);
            await SetCache();
        }

        public virtual async Task UpdateAsync(Category obj)
        {
            await Validate(obj);
            await _categoryRepository.UpdateAsync(obj);
            await SetCache();
        }

        protected virtual async Task<string> GetNextChildCodeAsync(int? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild != null) return Category.CalculateNextCode(lastChild.Code);
            var parentCode = parentId != null ? await GetCodeAsync(parentId.Value) : null;
            return Category.AppendCode(parentCode, Category.CreateCode(1));
        }

        protected virtual async Task<Category> GetLastChildOrNullAsync(int? parentId)
        {
            var children = await _categoryRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        protected virtual async Task<string> GetCodeAsync(int id)
        {
            return (await _categoryRepository.GetAsync(id)).Code;
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(int id)
        {
            var children = await FindChildrenAsync(id, true);

            foreach (var child in children)
            {
                await _categoryRepository.DeleteAsync(child);
            }

            await _categoryRepository.DeleteAsync(id);
        }

        [UnitOfWork]
        public virtual async Task RebuildCode(int? tenantId, List<NestedItem> nestedItems)
        {
            var departments = await _categoryRepository.GetAllListAsync(o => !o.IsDeleted && o.TenantId == tenantId);
            
            // Recreate Code by Nested Items
            var tempDepartments = (from nest in nestedItems let cat = departments.FirstOrDefault(o => o.Id == nest.Id) where cat != null select new Category {Id = cat.Id, ParentId = nest.ParentId}).ToList();
            foreach (var dep in tempDepartments)
            {
                dep.Code = GetNextChildCodeAsync(dep.ParentId, tempDepartments);
                var department = departments.FirstOrDefault(o => o.Id == dep.Id);
                if (department == null) continue;
                department.Code = dep.Code;
                department.ParentId = dep.ParentId;
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            await SetCache();
        }

        protected virtual string GetNextChildCodeAsync(long? parentId, List<Category> lstDepartment)
        {
            var lastChild = GetLastChildOrNullAsync(parentId,lstDepartment);
            if (lastChild != null) return Category.CalculateNextCode(lastChild.Code);
            var parentCode = parentId != null ? GetCodeAsync(parentId.Value,lstDepartment) : null;
            return Category.AppendCode(parentCode, Category.CreateCode(1));
        }

        protected virtual Category GetLastChildOrNullAsync(long? parentId, IEnumerable<Category> lstDepartment)
        {
            var children = lstDepartment.Where(o => !string.IsNullOrEmpty(o.Code) && o.ParentId == parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        protected virtual string GetCodeAsync(long id, IEnumerable<Category> lstDepartment)
        {
            return lstDepartment.FirstOrDefault(o=>o.Id == id)?.Code;
        }
        
        private async Task<List<Category>> FindChildrenAsync(int? parentId, bool recursive = false)
        {
            if (!recursive)
            {
                return await _categoryRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            }

            if (!parentId.HasValue)
            {
                return await _categoryRepository.GetAllListAsync();
            }

            var code = await GetCodeAsync(parentId.Value);

            return await _categoryRepository.GetAllListAsync(
                ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value
            );
        }

        protected virtual async Task Validate(Category obj)
        {
            var res = await _categoryRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == obj.TenantId && o.CategoryCode.Equals(obj.CategoryCode))
                .WhereIf(obj.Id > 0, o => o.Id != obj.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }
        #endregion
        
        #region Cache

        public async Task<List<CategoryDto>> GetAllCategory()
        {
            var catCache = _cacheManager.GetCache(CacheName);
            List<CategoryDto> res;
            var catList = await catCache.GetOrDefaultAsync( _abpSession.MultiTenancySide == MultiTenancySides.Host ? "Host" : _abpSession.TenantId.ToString());
            if (catList != null)
            {
                res = catList as List<CategoryDto>;
            }
            else
            {
                var catFromDb = await _categoryRepository.GetAll().Where(o => !o.IsDeleted && o.TenantId == _abpSession.TenantId).OrderBy(o=>o.Code).ToListAsync();
                res = ObjectMapper.Map<List<CategoryDto>>(catFromDb);
                await catCache.SetAsync(_abpSession.MultiTenancySide == MultiTenancySides.Host ? "Host" : _abpSession.TenantId.ToString(), res);
            }
            return res;
        }

        private async Task SetCache()
        {
            var catCache = _cacheManager.GetCache(CacheName);
            var catFromDb = await _categoryRepository.GetAll().Where(o => !o.IsDeleted && o.TenantId == _abpSession.TenantId).OrderBy(o=>o.Code).ToListAsync();
            var res = ObjectMapper.Map<List<CategoryDto>>(catFromDb);
            await catCache.SetAsync(_abpSession.MultiTenancySide == MultiTenancySides.Host ? "Host" : _abpSession.TenantId.ToString(), res);
        }
        
        #endregion
    }
}