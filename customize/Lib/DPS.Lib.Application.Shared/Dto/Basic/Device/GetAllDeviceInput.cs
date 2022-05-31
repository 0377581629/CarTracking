using Abp.Application.Services.Dto;

namespace DPS.Lib.Application.Shared.Dto.Basic.Device
{
    public class GetAllDeviceInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}