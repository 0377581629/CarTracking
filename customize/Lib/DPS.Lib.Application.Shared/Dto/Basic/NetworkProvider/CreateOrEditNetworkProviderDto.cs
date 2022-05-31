using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Basic.NetworkProvider
{
    public class CreateOrEditNetworkProviderDto: FullAuditedEntityDto<int?>
    {
        public int? TenantId { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string AccessPoint { get; set; }

        public string GprsUserName { get; set; }
        
        public string GprsPassword { get; set; }
    }
}