using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Abp.Zero;
using DPS.Cms.Core.Post;
using Microsoft.EntityFrameworkCore;
using Zero.Customize.NestedItem;

namespace DPS.Cms.Application.Manager
{
    public class CategoryManager : DomainService
    {
        private readonly IRepository<Category> _workDepartmentRepository;

        public CategoryManager(IRepository<Category> workDepartmentRepository)
        {
            _workDepartmentRepository = workDepartmentRepository;
            LocalizationSourceName = AbpZeroConsts.LocalizationSourceName;
        }

        [UnitOfWork]
        public virtual async Task CreateAsync(Category obj)
        {
            obj.Code = await GetNextChildCodeAsync(obj.ParentId);
            await Validate(obj);
            await _workDepartmentRepository.InsertAndGetIdAsync(obj);
        }

        public virtual async Task UpdateAsync(Category obj)
        {
            await Validate(obj);
            await _workDepartmentRepository.UpdateAsync(obj);
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
            var children = await _workDepartmentRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        protected virtual async Task<string> GetCodeAsync(int id)
        {
            return (await _workDepartmentRepository.GetAsync(id)).Code;
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(int id)
        {
            var children = await FindChildrenAsync(id, true);

            foreach (var child in children)
            {
                await _workDepartmentRepository.DeleteAsync(child);
            }

            await _workDepartmentRepository.DeleteAsync(id);
        }

        [UnitOfWork]
        public virtual async Task MoveAsync(int id, int? parentId)
        {
            var workDepartment = await _workDepartmentRepository.GetAsync(id);
            if (workDepartment.ParentId == parentId)
                return;
            
            //Should find children before Code change
            var children = await FindChildrenAsync(id, true);

            //Store old code
            var oldCode = workDepartment.Code;

            //Move
            workDepartment.Code = await GetNextChildCodeAsync(parentId);
            workDepartment.ParentId = parentId;

            await Validate(workDepartment);

            //Update Children Codes
            foreach (var child in children)
            {
                child.Code = Category.AppendCode(workDepartment.Code, Category.GetRelativeCode(child.Code, oldCode));
            }
        }

        [UnitOfWork]
        public virtual async Task RebuildCode(int? tenantId, List<NestedItem> nestedItems)
        {
            var departments = await _workDepartmentRepository.GetAllListAsync(o => !o.IsDeleted && o.TenantId == tenantId);
            
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
                return await _workDepartmentRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            }

            if (!parentId.HasValue)
            {
                return await _workDepartmentRepository.GetAllListAsync();
            }

            var code = await GetCodeAsync(parentId.Value);

            return await _workDepartmentRepository.GetAllListAsync(
                ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value
            );
        }

        protected virtual async Task Validate(Category obj)
        {
            var res = await _workDepartmentRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == obj.TenantId && o.CategoryCode.Equals(obj.CategoryCode))
                .WhereIf(obj.Id > 0, o => o.Id != obj.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }
    }
}