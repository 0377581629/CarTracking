using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Zero.Authorization;
using Zero.Customize.Dto.EmailTemplate;
using Zero.Customize.Interfaces;

namespace Zero.Customize
{
    [AbpAuthorize(AppPermissions.Pages_EmailTemplates)]
    public class EmailTemplateAppService : ZeroAppServiceBase, IEmailTemplateAppService
    {
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        public EmailTemplateAppService(IRepository<EmailTemplate> emailTemplateRepository)
        {
            _emailTemplateRepository = emailTemplateRepository;
        }

        private IQueryable<EmailTemplateDto> EmailTemplateQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from o in _emailTemplateRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Title.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new EmailTemplateDto
                {
                    Id = o.Id,
                    EmailTemplateType = o.EmailTemplateType,
                    Title = o.Title,
                    Content = o.Content,
                    Sign = o.Sign,
                    Note = o.Note,
                    AutoCreateForNewTenant = o.AutoCreateForNewTenant,
                    IsActive = o.IsActive
                };

            return query;
        }

        private class QueryInput
        {
            public GetAllEmailTemplateInput Input { get; set; }
            public int? Id { get; set; }
        }

        public async Task<PagedResultDto<GetEmailTemplateForViewDto>> GetAll(GetAllEmailTemplateInput input)
        {
            var queryInput = new QueryInput()
            {
                Input = input
            };

            var objQuery = EmailTemplateQuery(queryInput);

            var pagedAndFilteredObjs = objQuery
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var objs = from o in pagedAndFilteredObjs
                select new GetEmailTemplateForViewDto()
                {
                    EmailTemplate = o
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetEmailTemplateForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(AppPermissions.Pages_EmailTemplates_Edit)]
        public async Task<GetEmailTemplateForEditOutput> GetEmailTemplateForEdit(EntityDto input)
        {
            var queryInput = new QueryInput()
            {
                Id = input.Id
            };

            var objQuery = EmailTemplateQuery(queryInput);

            var obj = await objQuery.FirstOrDefaultAsync();

            var output = new GetEmailTemplateForEditOutput
            {
                EmailTemplate = ObjectMapper.Map<CreateOrEditEmailTemplateDto>(obj)
            };

            return output;
        }

        private async Task ValidateDataInput(CreateOrEditEmailTemplateDto input)
        {
            var res = await _emailTemplateRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Title.Equals(input.Title))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditEmailTemplateDto input)
        {
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

        [AbpAuthorize(AppPermissions.Pages_EmailTemplates_Create)]
        protected virtual async Task Create(CreateOrEditEmailTemplateDto input)
        {
            var obj = ObjectMapper.Map<EmailTemplate>(input);
            obj.TenantId = AbpSession.TenantId;
            await _emailTemplateRepository.InsertAndGetIdAsync(obj);
            if (obj.IsActive)
            {
                var otherEmailTemplates = await _emailTemplateRepository.GetAllListAsync(o => o.EmailTemplateType == obj.EmailTemplateType && o.Id != obj.Id && o.IsActive);
                if (otherEmailTemplates.Any())
                    foreach (var otherTemplate in otherEmailTemplates)
                        otherTemplate.IsActive = false;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_EmailTemplates_Edit)]
        protected virtual async Task Update(CreateOrEditEmailTemplateDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _emailTemplateRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == (int) input.Id);

                if (obj == null)
                    throw new UserFriendlyException(L("NotFound"));

                ObjectMapper.Map(input, obj);
                
                if (obj.IsActive)
                {
                    var otherEmailTemplates = await _emailTemplateRepository.GetAllListAsync(o => o.EmailTemplateType == obj.EmailTemplateType && o.Id != obj.Id && o.IsActive);
                    if (otherEmailTemplates.Any())
                        foreach (var otherTemplate in otherEmailTemplates)
                            otherTemplate.IsActive = false;
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_EmailTemplates_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _emailTemplateRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null)
                throw new UserFriendlyException(L("NotFound"));
            await _emailTemplateRepository.DeleteAsync(obj.Id);
        }
    }
}