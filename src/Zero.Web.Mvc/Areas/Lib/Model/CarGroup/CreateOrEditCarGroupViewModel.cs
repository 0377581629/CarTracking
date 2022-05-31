using DPS.Lib.Application.Shared.Dto.Transport.CarGroup;

namespace Zero.Web.Areas.Lib.Model.CarGroup
{
    public class CreateOrEditCarGroupViewModel
    {
        public CreateOrEditCarGroupDto CarGroup { get; set; }

        public bool IsEditMode => CarGroup.Id.HasValue;
    }
}