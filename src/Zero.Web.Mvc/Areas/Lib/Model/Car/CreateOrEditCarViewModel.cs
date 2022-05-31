using System.Collections.Generic;
using DPS.Lib.Application.Shared.Dto.Transport.Car;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Zero.Web.Areas.Lib.Model.Car
{
    public class CreateOrEditCarViewModel
    {
        public CreateOrEditCarDto Car { get; set; }

        public bool IsEditMode => Car.Id.HasValue;
        
        public List<SelectListItem> ListFuelType { get; set; }
    }
}