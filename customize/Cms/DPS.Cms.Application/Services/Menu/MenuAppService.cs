using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Cms.Application.Shared.Dto.Menu;
using DPS.Cms.Application.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Cms.Application.Services.Menu
{
    [AbpAuthorize(CmsPermissions.Menu)]
    public class MenuAppService : ZeroAppServiceBase, IMenuAppService
    {
        private readonly IRepository<Core.Menu.Menu> _menuRepository;

        public MenuAppService(IRepository<Core.Menu.Menu> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        private IQueryable<MenuDto> MenuQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from o in _menuRepository.GetAll()
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e =>
                        EF.Functions.Like(e.Name, $"%{input.Filter}%") ||
                        EF.Functions.Like(e.MenuGroup.Name, $"%{input.Filter}%"))
                    .WhereIf(input is {MenuGroupId: { }}, o => o.MenuGroupId == input.MenuGroupId)
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
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

                    Url = o.Url,

                    MenuGroupId = o.MenuGroupId,
                    MenuGroupCode = o.MenuGroup.Code,
                    MenuGroupName = o.MenuGroup.Name
                };

            return query;
        }

        private class QueryInput
        {
            public GetAllMenuInput Input { get; set; }
            public int? Id { get; set; }
        }

        public async Task<PagedResultDto<GetMenuForViewDto>> GetAll(GetAllMenuInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };

            var objQuery = MenuQuery(queryInput);

            var pagedAndFilteredObjs = objQuery
                .OrderBy(input.Sorting ?? "order asc")
                .PageBy(input);

            var objs = from o in pagedAndFilteredObjs
                select new GetMenuForViewDto
                {
                    Menu = o
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetMenuForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(CmsPermissions.Menu_Edit)]
        public async Task<GetMenuForEditOutput> GetMenuForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var objQuery = MenuQuery(queryInput);

            var obj = await objQuery.FirstOrDefaultAsync();

            var output = new GetMenuForEditOutput
            {
                Menu = ObjectMapper.Map<CreateOrEditMenuDto>(obj)
            };

            return output;
        }

        private async Task ValidateDataInput(CreateOrEditMenuDto input)
        {
            var res = await _menuRepository.GetAll()
                .Where(o => o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditMenuDto input)
        {
            input.Code = input.Code.Replace(" ", "");
            await ValidateDataInput(input);
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(CmsPermissions.Menu_Create)]
        protected virtual async Task Create(CreateOrEditMenuDto input)
        {
            var obj = ObjectMapper.Map<Core.Menu.Menu>(input);
            obj.TenantId = AbpSession.TenantId;
            await _menuRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(CmsPermissions.Menu_Edit)]
        protected virtual async Task Update(CreateOrEditMenuDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _menuRepository.FirstOrDefaultAsync(o => o.Id == (int) input.Id);

                if (obj == null)
                    throw new UserFriendlyException(L("NotFound"));

                ObjectMapper.Map(input, obj);

                await _menuRepository.UpdateAsync(obj);
            }
        }

        [AbpAuthorize(CmsPermissions.Menu_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _menuRepository.FirstOrDefaultAsync(o => o.Id == input.Id);
            if (obj == null)
                throw new UserFriendlyException(L("NotFound"));
            await _menuRepository.DeleteAsync(obj.Id);
        }
    }
}