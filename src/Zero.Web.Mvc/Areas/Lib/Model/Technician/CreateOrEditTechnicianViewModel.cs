using DPS.Lib.Application.Shared.Dto.Basic.Technician;

namespace Zero.Web.Areas.Lib.Model.Technician
{
    public class CreateOrEditTechnicianViewModel
    {
        public CreateOrEditTechnicianDto Technician { get; set; }

        public bool IsEditMode => Technician.Id.HasValue;
    }
}