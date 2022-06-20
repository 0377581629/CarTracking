using System.Collections.Generic;
using DPS.Lib.Application.Shared.Dto.Transport.Route;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Zero.Web.Areas.Lib.Model.Route
{
    public class CreateOrEditRouteViewModel
    {
        public CreateOrEditRouteDto Route { get; set; }

        public bool IsEditMode => Route.Id.HasValue;
        
        public List<SelectListItem> ListRouteType { get; set; }
    }
}