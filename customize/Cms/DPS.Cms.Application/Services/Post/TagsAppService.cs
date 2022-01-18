using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Cms.Application.Shared.Dto.Tags;
using DPS.Cms.Application.Shared.Interfaces;
using DPS.Cms.Core.Post;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Cms.Application.Services.Post
{
    [AbpAuthorize(CmsPermissions.Tags)]
    public class TagsAppService : ZeroAppServiceBase, ITagsAppService
    {
        private readonly IRepository<Tags> _tagsRepository;
        
        public TagsAppService(IRepository<Tags> tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        private IQueryable<TagsDto> TagsQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from o in _tagsRepository.GetAll()
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new TagsDto
                {
                    Id = o.Id,
                    Numbering = o.Numbering,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note,
                    Order = o.Order,
                    IsDefault = o.IsDefault,
                    IsActive = o.IsActive,
                };
            return query;
        }

        private class QueryInput
        {
            public GetAllTagsInput Input { get; set; }
            public int? Id { get; set; }
        }

        public async Task<PagedResultDto<GetTagsForViewDto>> GetAll(GetAllTagsInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };

            var objQuery = TagsQuery(queryInput);

            var pagedAndFilteredObjs = objQuery
                .OrderBy(input.Sorting ?? "order asc")
                .PageBy(input);

            var objs = from o in pagedAndFilteredObjs
                select new GetTagsForViewDto
                {
                    Tags = o
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetTagsForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(CmsPermissions.Tags_Edit)]
        public async Task<GetTagsForEditOutput> GetTagsForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var objQuery = TagsQuery(queryInput);

            var obj = await objQuery.FirstOrDefaultAsync();

            var output = new GetTagsForEditOutput
            {
                Tags = ObjectMapper.Map<CreateOrEditTagsDto>(obj)
            };

            return output;
        }

        private async Task ValidateDataInput(CreateOrEditTagsDto input)
        {
            var res = await _tagsRepository.GetAll()
                .Where(o => !o.IsDeleted && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditTagsDto input)
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

        [AbpAuthorize(CmsPermissions.Tags_Create)]
        protected virtual async Task Create(CreateOrEditTagsDto input)
        {
            var obj = ObjectMapper.Map<Tags>(input);
            obj.TenantId = AbpSession.TenantId;
            await _tagsRepository.InsertAndGetIdAsync(obj);
            if (obj.IsDefault)
            {
                var otherObjs = await _tagsRepository.GetAllListAsync(o => o.Id != obj.Id);
                if (otherObjs.Any())
                {
                    foreach (var changeDefault in otherObjs)
                    {
                        changeDefault.IsDefault = false;
                    }
                }
            }
        }

        [AbpAuthorize(CmsPermissions.Tags_Edit)]
        protected virtual async Task Update(CreateOrEditTagsDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _tagsRepository.FirstOrDefaultAsync(o => o.Id == (int) input.Id);

                if (obj == null)
                    throw new UserFriendlyException(L("NotFound"));

                ObjectMapper.Map(input, obj);
                if (obj.IsDefault)
                {
                    var otherObjs = await _tagsRepository.GetAllListAsync(o => o.Id != obj.Id);
                    if (otherObjs.Any())
                    {
                        foreach (var changeDefault in otherObjs)
                        {
                            changeDefault.IsDefault = false;
                        }
                    }
                }

                await _tagsRepository.UpdateAsync(obj);
            }
        }

        [AbpAuthorize(CmsPermissions.Tags_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _tagsRepository.FirstOrDefaultAsync(o => o.Id == input.Id);
            if (obj == null)
                throw new UserFriendlyException(L("NotFound"));
            await _tagsRepository.DeleteAsync(obj.Id);
        }
    }
}