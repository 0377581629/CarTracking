using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Cms.Application.Shared.Dto.PageTheme;
using DPS.Cms.Application.Shared.Interfaces;
using DPS.Cms.Core.Page;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Cms.Application.Services.Page
{
    [AbpAuthorize(CmsPermissions.PageTheme)]
    public class PageThemeAppService: ZeroAppServiceBase, IPageThemeAppService
    {
        private readonly IRepository<PageTheme> _pageThemeRepository;

        public PageThemeAppService(IRepository<PageTheme> pageThemeRepository)
        {
            _pageThemeRepository = pageThemeRepository;
        }


        private IQueryable<PageThemeDto> PageThemeQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from o in _pageThemeRepository.GetAll()
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new PageThemeDto
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
            public GetAllPageThemeInput Input { get; set; }
            public int? Id { get; set; }
        }

        public async Task<PagedResultDto<GetPageThemeForViewDto>> GetAll(GetAllPageThemeInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };

            var objQuery = PageThemeQuery(queryInput);

            var pagedAndFilteredObjs = objQuery
                .OrderBy(input.Sorting ?? "order asc")
                .PageBy(input);

            var objs = from o in pagedAndFilteredObjs
                select new GetPageThemeForViewDto
                {
                    PageTheme = o
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetPageThemeForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(CmsPermissions.PageTheme_Edit)]
        public async Task<GetPageThemeForEditOutput> GetPageThemeForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var objQuery = PageThemeQuery(queryInput);

            var obj = await objQuery.FirstOrDefaultAsync();

            var output = new GetPageThemeForEditOutput
            {
                PageTheme = ObjectMapper.Map<CreateOrEditPageThemeDto>(obj)
            };

            return output;
        }

        private async Task ValidateDataInput(CreateOrEditPageThemeDto input)
        {
            var res = await _pageThemeRepository.GetAll()
                .Where(o => o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditPageThemeDto input)
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

        [AbpAuthorize(CmsPermissions.PageTheme_Create)]
        protected virtual async Task Create(CreateOrEditPageThemeDto input)
        {
            var obj = ObjectMapper.Map<PageTheme>(input);
            await _pageThemeRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(CmsPermissions.PageTheme_Edit)]
        protected virtual async Task Update(CreateOrEditPageThemeDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _pageThemeRepository.FirstOrDefaultAsync(o => o.Id == (int) input.Id);

                if (obj == null)
                    throw new UserFriendlyException(L("NotFound"));

                ObjectMapper.Map(input, obj);
                await _pageThemeRepository.UpdateAsync(obj);
            }
        }

        [AbpAuthorize(CmsPermissions.PageTheme_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _pageThemeRepository.FirstOrDefaultAsync(o => o.Id == input.Id);
            if (obj == null)
                throw new UserFriendlyException(L("NotFound"));
            await _pageThemeRepository.DeleteAsync(obj.Id);
        }
    }
}