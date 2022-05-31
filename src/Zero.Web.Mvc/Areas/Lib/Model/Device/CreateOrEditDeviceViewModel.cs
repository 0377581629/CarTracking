using DPS.Lib.Application.Shared.Dto.Basic.Device;

namespace Zero.Web.Areas.Lib.Model.Device
{
    public class CreateOrEditDeviceViewModel
    {
        public CreateOrEditDeviceDto Device { get; set; }

        public bool IsEditMode => Device.Id.HasValue;
    }
}