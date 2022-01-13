using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Abp.Zero;
using DPS.Cms.Core.Menu;
using Microsoft.EntityFrameworkCore;
using Zero.Customize.NestedItem;

namespace DPS.Cms.Application.Manager
{
    public class MenuManager: DomainService
    {
        private readonly IRepository<Menu> _menuRepository;

        public MenuManager(IRepository<Menu> menuRepository)
        {
            _menuRepository = menuRepository;
            LocalizationSourceName = AbpZeroConsts.LocalizationSourceName;
        }

        [UnitOfWork]
        public virtual async Task CreateAsync(Menu obj)
        {
            obj.Code = await GetNextChildCodeAsync(obj.ParentId);
            await Validate(obj);
            await _menuRepository.InsertAndGetIdAsync(obj);
        }

        public virtual async Task UpdateAsync(Menu obj)
        {
            await _menuRepository.UpdateAsync(obj);
        }

        protected virtual async Task<string> GetNextChildCodeAsync(int? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild != null) return Menu.CalculateNextCode(lastChild.Code);
            var parentCode = parentId != null ? await GetCodeAsync(parentId.Value) : null;
            return Menu.AppendCode(parentCode, Menu.CreateCode(1));
        }

        protected virtual async Task<Menu> GetLastChildOrNullAsync(int? parentId)
        {
            var children = await _menuRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        protected virtual async Task<string> GetCodeAsync(int id)
        {
            return (await _menuRepository.GetAsync(id)).Code;
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(int id)
        {
            var children = await FindChildrenAsync(id, true);

            foreach (var child in children)
            {
                await _menuRepository.DeleteAsync(child);
            }

            await _menuRepository.DeleteAsync(id);
        }

        [UnitOfWork]
        public virtual async Task MoveAsync(int id, int? parentId)
        {
            var menu = await _menuRepository.GetAsync(id);
            if (menu.ParentId == parentId)
                return;
            
            //Should find children before Code change
            var children = await FindChildrenAsync(id, true);

            //Store old code
            var oldCode = menu.Code;

            //Move
            menu.Code = await GetNextChildCodeAsync(parentId);
            menu.ParentId = parentId;

            await Validate(menu);

            //Update Children Codes
            foreach (var child in children)
            {
                child.Code = Menu.AppendCode(menu.Code, Menu.GetRelativeCode(child.Code, oldCode));
            }
        }

        [UnitOfWork]
        public virtual async Task MoveUpAsync(int id)
        {
            var menu = await _menuRepository.GetAsync(id);

            // All same parent nodes
            var lstNodes = _menuRepository.GetAll()
                .Where(o => o.ParentId == menu.ParentId)
                .OrderBy(o => o.Code).ToList();
            if (lstNodes.Any())
            {
                if (lstNodes.Count == 1)
                    return;
                var beforeNode = new Menu();
                for (var i = 0; i < lstNodes.Count; i++)
                {
                    if (lstNodes[i].Id == menu.Id && i > 0)
                    {
                        beforeNode = lstNodes[i - 1];
                    }
                }
                if (beforeNode.Id > 0)
                {
                    //Swap children code
                    var children = await FindChildrenAsync(id, true);
                    var beforeChildren = await FindChildrenAsync(beforeNode.Id, true);
                    if (children != null && children.Any())
                    {
                        foreach (var h in children)
                        {
                            var oldCode = h.Code.Split(".");
                            oldCode[0] = beforeNode.Code;
                            h.Code = string.Join(".", oldCode);
                        }
                    }

                    if (beforeChildren != null && beforeChildren.Any())
                    {
                        foreach (var h in beforeChildren)
                        {
                            var oldCode = h.Code.Split(".");
                            oldCode[0] = menu.Code;
                            h.Code = string.Join(".", oldCode);
                        }
                    }

                    // Swap code.
                    var tempCode = beforeNode.Code;
                    beforeNode.Code = menu.Code;
                    menu.Code = tempCode;
                }
            }
        }
        
        [UnitOfWork]
        public virtual async Task MoveDownAsync(int id)
        {
            var menu = await _menuRepository.GetAsync(id);

            // All same parent nodes
            var lstNodes = _menuRepository.GetAll()
                .Where(o => o.ParentId == menu.ParentId)
                .OrderBy(o => o.Code).ToList();
            if (lstNodes.Any())
            {
                if (lstNodes.Count == 1)
                    return;
                var afterNode = new Menu();
                for (var i = 0; i < lstNodes.Count; i++)
                {
                    if (lstNodes[i].Id == menu.Id && i < lstNodes.Count-1)
                    {
                        afterNode = lstNodes[i + 1];
                    }
                }
                if (afterNode.Id > 0)
                {
                    //Swap children code
                    var children = await FindChildrenAsync(id, true);
                    var beforeChildren = await FindChildrenAsync(afterNode.Id, true);
                    if (children != null && children.Any())
                    {
                        foreach (var h in children)
                        {
                            var oldCode = h.Code.Split(".");
                            oldCode[0] = afterNode.Code;
                            h.Code = string.Join(".", oldCode);
                        }
                    }

                    if (beforeChildren != null && beforeChildren.Any())
                    {
                        foreach (var h in beforeChildren)
                        {
                            var oldCode = h.Code.Split(".");
                            oldCode[0] = menu.Code;
                            h.Code = string.Join(".", oldCode);
                        }
                    }

                    // Swap code.
                    var tempCode = afterNode.Code;
                    afterNode.Code = menu.Code;
                    menu.Code = tempCode;
                }
            }
        }

        [UnitOfWork]
        public virtual async Task RebuildCode(int? tenantId, List<NestedItem> nestedItems)
        {
            var menus = await _menuRepository.GetAllListAsync(o => o.TenantId == tenantId);
            
            // Recreate Code by Nested Items
            var tempMenus = (from nest in nestedItems let menu = menus.FirstOrDefault(o => o.Id == nest.Id) where menu != null select new Menu() {Id = menu.Id, ParentId = nest.ParentId, MenuGroupId = menu.MenuGroupId}).ToList();
            foreach (var dep in tempMenus)
            {
                dep.Code = GetNextChildCodeAsync(dep.ParentId, tempMenus);
                var menu = menus.FirstOrDefault(o => o.Id == dep.Id);
                if (menu == null) continue;
                menu.Code = dep.Code;
                menu.ParentId = dep.ParentId;
            }
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        protected virtual string GetNextChildCodeAsync(int? parentId, List<Menu> lstMenu)
        {
            var lastChild = GetLastChildOrNullAsync(parentId,lstMenu);
            if (lastChild != null) return Menu.CalculateNextCode(lastChild.Code);
            var parentCode = parentId != null ? GetCodeAsync(parentId.Value,lstMenu) : null;
            return Menu.AppendCode(parentCode, Menu.CreateCode(1));
        }

        protected virtual Menu GetLastChildOrNullAsync(int? parentId, IEnumerable<Menu> lstMenu)
        {
            var children = lstMenu.Where(o => !string.IsNullOrEmpty(o.Code) && o.ParentId == parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        protected virtual string GetCodeAsync(int id, IEnumerable<Menu> lstMenu)
        {
            return lstMenu.FirstOrDefault(o=>o.Id == id)?.Code;
        }
        
        private async Task<List<Menu>> FindChildrenAsync(int? parentId, bool recursive = false)
        {
            if (!recursive)
            {
                return await _menuRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            }

            if (!parentId.HasValue)
            {
                return await _menuRepository.GetAllListAsync();
            }

            var code = await GetCodeAsync(parentId.Value);

            return await _menuRepository.GetAllListAsync(
                ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value
            );
        }

        protected virtual async Task Validate(Menu obj)
        {
            var res = await _menuRepository.GetAll()
                .Where(o => o.TenantId == obj.TenantId && o.Code.Equals(obj.Code))
                .WhereIf(obj.Id > 0, o => o.Id != obj.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }
    }
}