using System.Collections.Generic;
using DPS.Lib.Application.Shared.Dto.Basic.Driver;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Zero.Web.Areas.Lib.Model.Driver
{
    public class CreateOrEditDriverViewModel
    {
        public CreateOrEditDriverDto Driver { get; set; }

        public bool IsEditMode => Driver.Id.HasValue;
        
        public List<SelectListItem> ListBloodType { get; set; }
    }
}