using DPS.Lib.Application.Shared.Dto.Transport.Point;

namespace Zero.Web.Areas.Lib.Model.Point
{
    public class CreateOrEditPointViewModel
    {
        public CreateOrEditPointDto Point { get; set; }

        public bool IsEditMode => Point.Id.HasValue;
    }
}