using System;
using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Basic.Rfid.RfidType
{
    public class RfidTypeDto: FullAuditedEntityDto
    {
        public int? TenantId { get; set; }
        
        public string Code { get; set; }
        
        public string CardNumber { get; set; }
        
        public string CardDer { get; set; }
        
        public DateTime RegisterDate { get; set; }
        
        public long? UserId { get; set; }
        
        public string UserName { get; set; }
        
        public bool IsBlackList { get; set; }
        
        public string SerialNumber { get; set; }
        
        public int CardType { get; set; }
    }
}