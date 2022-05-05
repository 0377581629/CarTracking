using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Basic.Rfid.RfidType
{
    public class GetAllRfidTypeInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}