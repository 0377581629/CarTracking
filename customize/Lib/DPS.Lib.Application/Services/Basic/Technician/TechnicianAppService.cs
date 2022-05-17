using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using DPS.Lib.Application.Shared.Dto.Basic.Technician;
using DPS.Lib.Application.Shared.Interface.Basic.Technician;
using Microsoft.EntityFrameworkCore;
using Zero;
using Zero.Authorization;

namespace DPS.Lib.Application.Services.Basic.Technician
{
    [AbpAuthorize(LibPermissions.Technician)]
    public class TechnicianAppService: ZeroAppServiceBase,ITechnicianAppService
    {
        private readonly IRepository<Core.Basic.Technician.Technician> _technicianRepository;

        public TechnicianAppService(IRepository<Core.Basic.Technician.Technician> technicianRepository)
        {
            _technicianRepository = technicianRepository;
        }
        
        private IQueryable<TechnicianDto> TechnicianQuery(QueryInput queryInput)
        {
            var input = queryInput.Input;
            var id = queryInput.Id;

            var query = from obj in _technicianRepository.GetAll()
                    .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId)
                    .WhereIf(input != null && !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                    .WhereIf(id.HasValue, e => e.Id == id.Value)
                select new TechnicianDto
                {
                    TenantId = obj.TenantId,
                    Id = obj.Id,
                    Code = obj.Code,
                    Name = obj.Name,
                    Address = obj.Address,
                    Avatar = obj.Avatar,
                    Email = obj.Email,
                    Gender = obj.Gender,
                    PhoneNumber = obj.PhoneNumber,
                    IsStopWorking = obj.IsStopWorking,
                    
                    RfidTypeId = obj.RfidTypeId,
                    RfidTypeCardNumber = obj.RfidType.CardNumber
                };
            return query;
        }

        private class QueryInput
        {
            public GetAllTechnicianInput Input { get; init; }
            public int? Id { get; init; }
        }

        public async Task<PagedResultDto<GetTechnicianForViewDto>> GetAll(GetAllTechnicianInput input)
        {
            var queryInput = new QueryInput
            {
                Input = input
            };
            var objQuery = TechnicianQuery(queryInput);

            var pagedAndFilteredTechnician = objQuery.OrderBy(input.Sorting ?? "id asc").PageBy(input);

            var objs = from o in pagedAndFilteredTechnician
                select new GetTechnicianForViewDto
                {
                    Technician = ObjectMapper.Map<TechnicianDto>(o)
                };

            var totalCount = await objQuery.CountAsync();
            var res = await objs.ToListAsync();

            return new PagedResultDto<GetTechnicianForViewDto>(
                totalCount,
                res
            );
        }

        [AbpAuthorize(LibPermissions.Technician_Edit)]
        public async Task<GetTechnicianForEditOutput> GetTechnicianForEdit(EntityDto input)
        {
            var queryInput = new QueryInput
            {
                Id = input.Id
            };

            var technician = await TechnicianQuery(queryInput).FirstOrDefaultAsync();

            var output = new GetTechnicianForEditOutput
            {
                Technician = ObjectMapper.Map<CreateOrEditTechnicianDto>(technician)
            };
            return output;
        }

        private async Task ValidateDataInput(CreateOrEditTechnicianDto input)
        {
            var res = await _technicianRepository.GetAll()
                .Where(o => !o.IsDeleted && o.TenantId == AbpSession.TenantId && o.Code.Equals(input.Code))
                .WhereIf(input.Id.HasValue, o => o.Id != input.Id)
                .FirstOrDefaultAsync();
            if (res != null)
                throw new UserFriendlyException(L("Error"), L("CodeAlreadyExists"));
        }

        public async Task CreateOrEdit(CreateOrEditTechnicianDto input)
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

        [AbpAuthorize(LibPermissions.Technician_Create)]
        protected virtual async Task Create(CreateOrEditTechnicianDto input)
        {
            var obj = ObjectMapper.Map<Core.Basic.Technician.Technician>(input);
            obj.TenantId = AbpSession.TenantId;
            await _technicianRepository.InsertAndGetIdAsync(obj);
        }

        [AbpAuthorize(LibPermissions.Technician_Edit)]
        protected virtual async Task Update(CreateOrEditTechnicianDto input)
        {
            if (input.Id.HasValue)
            {
                var obj = await _technicianRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
                if (obj == null) throw new UserFriendlyException(L("NotFound"));
                ObjectMapper.Map(input, obj);
            }
        }

        [AbpAuthorize(LibPermissions.Technician_Delete)]
        public async Task Delete(EntityDto input)
        {
            var obj = await _technicianRepository.FirstOrDefaultAsync(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            if (obj == null) throw new UserFriendlyException(L("NotFound"));
            await _technicianRepository.DeleteAsync(input.Id);
        }
    }
}