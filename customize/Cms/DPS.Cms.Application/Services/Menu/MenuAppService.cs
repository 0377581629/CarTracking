using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using DPS.Cms.Application.Manager;
using DPS.Cms.Application.Shared.Dto.Menu;
using DPS.Cms.Application.Shared.Interfaces.Menu;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;
using Zero.Customize;
using Zero.Customize.NestedItem;

namespace DPS.Cms.Application.Services.Menu
{
    [AbpAuthorize(CmsPermissions.Menu)]
    public class MenuAppService : ZeroAppServiceBase, IMenuAppService
    {
        private readonly MenuManager _menuManager;
        private readonly IRepository<Core.Menu.Menu> _menuRepository;

        public MenuAppService(MenuManager menuManager, IRepository<Core.Menu.Menu> menuRepository)
        {
            _menuManager = menuManager;
            _menuRepository = menuRepository;
        }

        private IQueryable<NestedItem> MenuNestedQuery(GetAllMenuInput input)
        {
            var query = from obj in _menuRepository.GetAll()
                    .Where(o => o.TenantId == AbpSession.TenantId)
                    .OrderBy(o => o.Code)
                    .Where(o => o.MenuGroupId == input.MenuGroupId)
                select new NestedItem
                {
                    Id = obj.Id,
                    ParentId = obj.ParentId,
                    DisplayName = obj.Name
                };
            return query;
        }

        public async Task<List<NestedItem>> GetAllNested(GetAllMenuInput input)
        {
            var objQuery = MenuNestedQuery(input);
            var res = await objQuery.ToListAsync();
            NestedItemHelper.BuildRecursiveItem(ref res);
            return res;
        }

        public async Task<ListResultDto<MenuDto>> GetMenus()
        {
            var objs = await _menuRepository.GetAll().Include(o => o.MenuGroup)
                .Where(o => o.TenantId == AbpSession.TenantId)
                .Select(o => new MenuDto()
                {
                    Id = o.Id,
                    ParentId = o.ParentId,
                    Code = o.Code,
                    Name = o.Name,
                    MenuGroupId = o.MenuGroupId,
                    MenuGroupCode = o.MenuGroup.Code,
                    MenuGroupName = o.MenuGroup.Name,
                    Url = o.Url,
                    IsActive = o.IsActive
                }).OrderBy(o => o.Code).ToListAsync();

            return new ListResultDto<MenuDto>(objs);
        }

        public async Task<NestedItem> CreateOrEditMenu(CreateOrEditMenuDto input)
        {
            if (input.Id.HasValue)
            {
                return await UpdateMenu(input);
            }

            return await CreateMenu(input);
        }

        [AbpAuthorize(CmsPermissions.Menu_Create)]
        private async Task<NestedItem> CreateMenu(CreateOrEditMenuDto input)
        {
            input.TenantId = AbpSession.TenantId;
            var menu = ObjectMapper.Map<Core.Menu.Menu>(input);

            await _menuManager.CreateAsync(menu);
            await CurrentUnitOfWork.SaveChangesAsync();

            return new NestedItem()
            {
                Id = menu.Id,
                DisplayName = menu.Name
            };
        }

        [AbpAuthorize(CmsPermissions.Menu_Edit)]
        private async Task<NestedItem> UpdateMenu(CreateOrEditMenuDto input)
        {
            var obj = await _menuRepository.FirstOrDefaultAsync(o =>
                o.TenantId == AbpSession.TenantId && o.Id == input.Id);

            if (obj == null)
                throw new UserFriendlyException(L("NotFound"));

            obj.MenuGroupId = input.MenuGroupId;
            obj.Code = input.Code;
            obj.Name = input.Name;
            obj.Url = input.Url;

            await _menuManager.UpdateAsync(obj);
            return new NestedItem()
            {
                Id = obj.Id,
                DisplayName = obj.Name
            };
        }

        [AbpAuthorize(CmsPermissions.Menu_Create, CmsPermissions.Menu_Edit)]
        public async Task UpdateStructure(UpdateMenuStructureInput input)
        {
            await _menuManager.RebuildCode(AbpSession.TenantId, input.NestedItems);
        }

        [AbpAuthorize(CmsPermissions.Menu_Delete)]
        public async Task Delete(EntityDto<int> input)
        {
            var obj = await _menuRepository.FirstOrDefaultAsync(o =>
                o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null)
                throw new UserFriendlyException(L("NotFound"));
            await _menuRepository.DeleteAsync(input.Id);
        }
    }
}