using DPS.Lib.Application.Shared.Dto.Transport.PointType;

namespace Zero.Web.Areas.Lib.Model.PointType
{
    public class CreateOrEditPointTypeViewModel
    {
        public CreateOrEditPointTypeDto PointType { get; set; }

        public bool IsEditMode => PointType.Id.HasValue;
    }
}