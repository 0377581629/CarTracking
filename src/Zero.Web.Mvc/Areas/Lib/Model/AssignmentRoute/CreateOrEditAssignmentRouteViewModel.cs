using DPS.Lib.Application.Shared.Dto.Transport.AssignmentRoute;

namespace Zero.Web.Areas.Lib.Model.AssignmentRoute
{
    public class CreateOrEditAssignmentRouteViewModel
    {
        public CreateOrEditAssignmentRouteDto AssignmentRoute { get; set; }

        public bool IsEditMode => AssignmentRoute.Id.HasValue;
    }
}