using DPS.Lib.Application.Shared.Dto.Basic.ManagementUnit;

namespace Zero.Web.Areas.Lib.Model.ManagementUnit
{
    public class CreateOrEditManagementUnitViewModel
    {
        public CreateOrEditManagementUnitDto ManagementUnit { get; set; }

        public bool IsEditMode => ManagementUnit.Id.HasValue;
    }
}