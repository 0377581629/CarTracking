using System.Collections.Generic;
using DPS.Lib.Application.Shared.Dto.Basic.Rfid.RfidType;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Zero.Web.Areas.Lib.Model.RfidType
{
    public class CreateOrEditRfidTypeViewModel
    {
        public CreateOrEditRfidTypeDto RfidType { get; set; }

        public bool IsEditMode => RfidType.Id.HasValue;
        
        public List<SelectListItem> ListCardType { get; set; }
    }
}