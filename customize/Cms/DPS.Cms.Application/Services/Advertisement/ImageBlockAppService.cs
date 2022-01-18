using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Cms.Application.Shared.Dto.ImageBlock;
using DPS.Cms.Application.Shared.Interfaces;
using DPS.Cms.Core.Advertisement;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Cms.Application.Services.Advertisement
{
    [AbpAuthorize(CmsPermissions.ImageBlock)]
    public class ImageBlockAppService : ZeroAppServiceBase, IImageBlockAppService
    {
        private readonly IRepository<ImageBlock> _advertisementRepository;

        public ImageBlockAppService(IRepository<ImageBlock> advertisementRepository)
        {
            _advertisementRepository = advertisementRepository;
        }

        private IQueryable<ImageBlockDto> ImageBlockQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from o in _advertisementRepository.GetAll()
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e =>
                        EF.Functions.Like(e.Name, $"%{input.Filter}%") ||
                        EF.Functions.Like(e.ImageBlockGroup.Name, $"%{input.Filter}%"))
                    .WhereIf(input is {ImageBlockGroupId: { }}, o => o.ImageBlockGroupId == input.ImageBlockGroupId)
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new ImageBlockDto
                {
                    Id = o.Id,
                    Numbering = o.Numbering,
                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note,
                    Order = o.Order,
                    IsDefault = o.IsDefault,
                    IsActive = o.IsActive,

                    Image = o.Image,
                    ImageMobile = o.ImageMobile,

                    TargetUrl = o.TargetUrl,

                    ImageBlockGroupId = o.ImageBlockGroupId,
                    ImageBlockGroupCode = o.ImageBlockGroup.Code,
                    ImageBlockGroupName = o.ImageBlockGroup.Name
                };

            return query;
        }

        private class QueryInput
        {
            public GetAllImageBlockInput Input { get; set; }
            public int? Id { get; set; }
        }

        public async Task<PagedResultDto<GetImageBlockForViewDto>> GetAll(GetAllImageBlockInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };

            var objQuery = ImageBlockQuery(queryInput);

            var pagedAndFilteredObjs = objQuery
                .OrderBy(input.Sorting ?? "order asc")
                .PageBy(input);

            var objs = from o in pagedAndFilteredObjs
                select new GetImageBlockForViewDto
                {
                    ImageBlock = o
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetImageBlockForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(CmsPermissions.ImageBlock_Edit)]
        public async Task<GetImageBlockForEditOutput> GetImageBlockForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var objQuery = ImageBlockQuery(queryInput);

            var obj = await objQuery.FirstOrDefaultAsync();

            var output = new GetImageBlockForEditOutput
            {
                ImageBlock = ObjectMapper.Map<CreateOrEditImageBlockDto>(obj)
            };
            
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditImageBlockDto input)
        {
            var res = await _advertisementRepository.GetAll()
                .Where(o => !o.IsDeleted && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditImageBlockDto input)
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

        [AbpAuthorize(CmsPermissions.ImageBlock_Create)]
        protected virtual async Task Create(CreateOrEditImageBlockDto input)
        {
            var obj = ObjectMapper.Map<ImageBlock>(input);
            obj.TenantId = AbpSession.TenantId;
            await _advertisementRepository.InsertAndGetIdAsync(obj);
            if (obj.IsDefault)
            {
                var otherObjs = await _advertisementRepository.GetAllListAsync(o => o.Id != obj.Id);
                if (otherObjs.Any())
                {
                    foreach (var changeDefault in otherObjs)
                    {
                        changeDefault.IsDefault = false;
                    }
                }
            }
        }

        [AbpAuthorize(CmsPermissions.ImageBlock_Edit)]
        protected virtual async Task Update(CreateOrEditImageBlockDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _advertisementRepository.FirstOrDefaultAsync(o => o.Id == (int) input.Id);

                if (obj == null)
                    throw new UserFriendlyException(L("NotFound"));

                ObjectMapper.Map(input, obj);
                if (obj.IsDefault)
                {
                    var otherObjs = await _advertisementRepository.GetAllListAsync(o => o.Id != obj.Id);
                    if (otherObjs.Any())
                    {
                        foreach (var changeDefault in otherObjs)
                        {
                            changeDefault.IsDefault = false;
                        }
                    }
                }

                await _advertisementRepository.UpdateAsync(obj);
            }
        }

        [AbpAuthorize(CmsPermissions.ImageBlock_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _advertisementRepository.FirstOrDefaultAsync(o => o.Id == input.Id);
            if (obj == null)
                throw new UserFriendlyException(L("NotFound"));
            await _advertisementRepository.DeleteAsync(obj.Id);
        }
    }
}