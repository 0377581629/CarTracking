using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Cms.Application.Shared.Dto.ImageBlockGroup;
using DPS.Cms.Application.Shared.Interfaces;
using DPS.Cms.Core.Advertisement;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Cms.Application.Services.Advertisement
{
    [AbpAuthorize(CmsPermissions.ImageBlockGroup)]
    public class ImageBlockGroupAppService : ZeroAppServiceBase, IImageBlockGroupAppService
    {
        private readonly IRepository<ImageBlockGroup> _advertisementGroupRepository;
        
        public ImageBlockGroupAppService(IRepository<ImageBlockGroup> advertisementGroupRepository)
        {
            _advertisementGroupRepository = advertisementGroupRepository;
        }

        private IQueryable<ImageBlockGroupDto> ImageBlockGroupQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from o in _advertisementGroupRepository.GetAll()
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new ImageBlockGroupDto
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
            public GetAllImageBlockGroupInput Input { get; set; }
            public int? Id { get; set; }
        }

        public async Task<PagedResultDto<GetImageBlockGroupForViewDto>> GetAll(GetAllImageBlockGroupInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };

            var objQuery = ImageBlockGroupQuery(queryInput);

            var pagedAndFilteredObjs = objQuery
                .OrderBy(input.Sorting ?? "order asc")
                .PageBy(input);

            var objs = from o in pagedAndFilteredObjs
                select new GetImageBlockGroupForViewDto
                {
                    ImageBlockGroup = o
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetImageBlockGroupForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(CmsPermissions.ImageBlockGroup_Edit)]
        public async Task<GetImageBlockGroupForEditOutput> GetImageBlockGroupForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var objQuery = ImageBlockGroupQuery(queryInput);

            var obj = await objQuery.FirstOrDefaultAsync();

            var output = new GetImageBlockGroupForEditOutput
            {
                ImageBlockGroup = ObjectMapper.Map<CreateOrEditImageBlockGroupDto>(obj)
            };

            return output;
        }

        private async Task ValidateDataInput(CreateOrEditImageBlockGroupDto input)
        {
            var res = await _advertisementGroupRepository.GetAll()
                .Where(o => !o.IsDeleted && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditImageBlockGroupDto input)
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

        [AbpAuthorize(CmsPermissions.ImageBlockGroup_Create)]
        protected virtual async Task Create(CreateOrEditImageBlockGroupDto input)
        {
            var obj = ObjectMapper.Map<ImageBlockGroup>(input);
            obj.TenantId = AbpSession.TenantId;
            await _advertisementGroupRepository.InsertAndGetIdAsync(obj);
            if (obj.IsDefault)
            {
                var otherObjs = await _advertisementGroupRepository.GetAllListAsync(o => o.Id != obj.Id);
                if (otherObjs.Any())
                {
                    foreach (var changeDefault in otherObjs)
                    {
                        changeDefault.IsDefault = false;
                    }
                }
            }
        }

        [AbpAuthorize(CmsPermissions.ImageBlockGroup_Edit)]
        protected virtual async Task Update(CreateOrEditImageBlockGroupDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _advertisementGroupRepository.FirstOrDefaultAsync(o => o.Id == (int) input.Id);

                if (obj == null)
                    throw new UserFriendlyException(L("NotFound"));

                ObjectMapper.Map(input, obj);
                if (obj.IsDefault)
                {
                    var otherObjs = await _advertisementGroupRepository.GetAllListAsync(o => o.Id != obj.Id);
                    if (otherObjs.Any())
                    {
                        foreach (var changeDefault in otherObjs)
                        {
                            changeDefault.IsDefault = false;
                        }
                    }
                }

                await _advertisementGroupRepository.UpdateAsync(obj);
            }
        }

        [AbpAuthorize(CmsPermissions.ImageBlockGroup_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _advertisementGroupRepository.FirstOrDefaultAsync(o => o.Id == input.Id);
            if (obj == null)
                throw new UserFriendlyException(L("NotFound"));
            await _advertisementGroupRepository.DeleteAsync(obj.Id);
        }
    }
}