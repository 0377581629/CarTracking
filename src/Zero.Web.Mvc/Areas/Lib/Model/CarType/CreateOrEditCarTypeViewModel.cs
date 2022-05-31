using DPS.Lib.Application.Shared.Dto.Transport.CarType;

namespace Zero.Web.Areas.Lib.Model.CarType
{
    public class CreateOrEditCarTypeViewModel
    {
        public CreateOrEditCarTypeDto CarType { get; set; }

        public bool IsEditMode => CarType.Id.HasValue;
    }
}