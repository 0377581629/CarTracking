using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Cms.Application.Shared.Dto.MenuGroup;
using DPS.Cms.Application.Shared.Interfaces;
using DPS.Cms.Application.Shared.Interfaces.Menu;
using DPS.Cms.Core.Menu;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Cms.Application.Services.Menu
{
    [AbpAuthorize(CmsPermissions.MenuGroup)]
    public class MenuGroupAppService: ZeroAppServiceBase, IMenuGroupAppService
    {
        private readonly IRepository<MenuGroup> _menuGroupRepository;

        public MenuGroupAppService(IRepository<MenuGroup> menuGroupRepository)
        {
            _menuGroupRepository = menuGroupRepository;
        }


        private IQueryable<MenuGroupDto> MenuGroupQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from o in _menuGroupRepository.GetAll()
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
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

        private class QueryInput
        {
            public GetAllMenuGroupInput Input { get; set; }
            public int? Id { get; set; }
        }

        public async Task<PagedResultDto<GetMenuGroupForViewDto>> GetAll(GetAllMenuGroupInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };

            var objQuery = MenuGroupQuery(queryInput);

            var pagedAndFilteredObjs = objQuery
                .OrderBy(input.Sorting ?? "order asc")
                .PageBy(input);

            var objs = from o in pagedAndFilteredObjs
                select new GetMenuGroupForViewDto
                {
                    MenuGroup = o
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetMenuGroupForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(CmsPermissions.MenuGroup_Edit)]
        public async Task<GetMenuGroupForEditOutput> GetMenuGroupForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var objQuery = MenuGroupQuery(queryInput);

            var obj = await objQuery.FirstOrDefaultAsync();

            var output = new GetMenuGroupForEditOutput
            {
                MenuGroup = ObjectMapper.Map<CreateOrEditMenuGroupDto>(obj)
            };

            return output;
        }

        private async Task ValidateDataInput(CreateOrEditMenuGroupDto input)
        {
            var res = await _menuGroupRepository.GetAll()
                .Where(o => o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditMenuGroupDto input)
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

        [AbpAuthorize(CmsPermissions.MenuGroup_Create)]
        protected virtual async Task Create(CreateOrEditMenuGroupDto input)
        {
            var obj = ObjectMapper.Map<MenuGroup>(input);
            obj.TenantId = AbpSession.TenantId;
            await _menuGroupRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(CmsPermissions.MenuGroup_Edit)]
        protected virtual async Task Update(CreateOrEditMenuGroupDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _menuGroupRepository.FirstOrDefaultAsync(o => o.Id == (int) input.Id);
                if (obj == null)
                    throw new UserFriendlyException(L("NotFound"));

                ObjectMapper.Map(input, obj);
                await _menuGroupRepository.UpdateAsync(obj);
            }
        }

        [AbpAuthorize(CmsPermissions.MenuGroup_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _menuGroupRepository.FirstOrDefaultAsync(o => o.Id == input.Id);
            if (obj == null)
                throw new UserFriendlyException(L("NotFound"));
            await _menuGroupRepository.DeleteAsync(obj.Id);
        }
    }
}