using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Cms.Application.Shared.Dto.Post;
using DPS.Cms.Application.Shared.Interfaces;
using post = DPS.Cms.Core.Post;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Cms.Application.Services.Post
{
    [AbpAuthorize(CmsPermissions.Post)]
    public class PostAppService : ZeroAppServiceBase, IPostAppService
    {
        private readonly IRepository<post.Post> _postRepository;
        
        public PostAppService(IRepository<post.Post> postRepository)
        {
            _postRepository = postRepository;
        }

        private IQueryable<PostDto> PostQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from o in _postRepository.GetAll()
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => EF.Functions.Like(e.Name, $"%{input.Filter}%"))
                    .WhereIf(input != null && input.CategoryId.HasValue, e => e.CategoryId == input.CategoryId)
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new PostDto
                {
                    Id = o.Id,

                    Code = o.Code,
                    Name = o.Name,
                    Note = o.Note,
                    Order = o.Order,
                    CategoryId = o.CategoryId,
                    About = o.About,
                    Summary = o.Summary,
                    Slug = o.Slug,
                    Url = o.Url,
                    Image = o.Image,
                    ViewCount = o.ViewCount,
                    CommentCount = o.CommentCount,
                    
                    IsDefault = o.IsDefault,
                    IsActive = o.IsActive,
                    
                    TitleDefault = o.TitleDefault,
                    Title = o.Title,
                    
                    DescriptionDefault = o.DescriptionDefault,
                    Description = o.Description,
                    
                    KeywordDefault = o.KeywordDefault,
                    Keyword = o.Keyword,
                    
                    AuthorDefault = o.AuthorDefault,
                    Author = o.Author
                };

            return query;
        }

        private class QueryInput
        {
            public GetAllPostInput Input { get; set; }
            public int? Id { get; set; }
        }

        public async Task<PagedResultDto<GetPostForViewDto>> GetAll(GetAllPostInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };

            var objQuery = PostQuery(queryInput);

            var pagedAndFilteredObjs = objQuery
                .OrderBy(input.Sorting ?? "order asc")
                .PageBy(input);

            var objs = from o in pagedAndFilteredObjs
                select new GetPostForViewDto
                {
                    Post = o
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetPostForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(CmsPermissions.Post_Edit)]
        public async Task<GetPostForEditOutput> GetPostForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var objQuery = PostQuery(queryInput);

            var obj = await objQuery.FirstOrDefaultAsync();

            var output = new GetPostForEditOutput
            {
                Post = ObjectMapper.Map<CreateOrEditPostDto>(obj)
            };

            return output;
        }

        private async Task ValidateDataInput(CreateOrEditPostDto input)
        {
            var res = await _postRepository.GetAll()
                .Where(o => !o.IsDeleted && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditPostDto input)
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

        [AbpAuthorize(CmsPermissions.Post_Create)]
        protected virtual async Task Create(CreateOrEditPostDto input)
        {
            var obj = ObjectMapper.Map<post.Post>(input);
            obj.TenantId = AbpSession.TenantId;
            await _postRepository.InsertAndGetIdAsync(obj);
            if (obj.IsDefault)
            {
                var otherObjs = await _postRepository.GetAllListAsync(o => o.Id != obj.Id);
                if (otherObjs.Any())
                {
                    foreach (var changeDefault in otherObjs)
                    {
                        changeDefault.IsDefault = false;
                    }
                }
            }
        }

        [AbpAuthorize(CmsPermissions.Post_Edit)]
        protected virtual async Task Update(CreateOrEditPostDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _postRepository.FirstOrDefaultAsync(o => o.Id == (int) input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                
                ObjectMapper.Map(input, obj);
                
                if (obj.IsDefault)
                {
                    var otherObjs = await _postRepository.GetAllListAsync(o => o.Id != obj.Id);
                    if (otherObjs.Any())
                    {
                        foreach (var changeDefault in otherObjs)
                        {
                            changeDefault.IsDefault = false;
                        }
                    }
                }

                await _postRepository.UpdateAsync(obj);
            }
        }
        
        [AbpAuthorize(CmsPermissions.Post_Publish)]
        public async Task UpdateStatus(EntityDto input)
        {
            var obj = await _postRepository.FirstOrDefaultAsync(o => o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            obj.IsActive = true;
            await _postRepository.UpdateAsync(obj);
        }

        [AbpAuthorize(CmsPermissions.Post_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _postRepository.FirstOrDefaultAsync(o => o.Id == input.Id);
            if (obj == null)
                throw new UserFriendlyException(L("NotFound"));
            await _postRepository.DeleteAsync(obj.Id);
        }
    }
}