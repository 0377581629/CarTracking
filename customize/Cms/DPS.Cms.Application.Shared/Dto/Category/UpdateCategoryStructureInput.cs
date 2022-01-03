using System.Collections.Generic;
using Zero.Customize.NestedItem;

namespace DPS.Cms.Application.Shared.Dto.Category
{
    public class UpdateCategoryStructureInput
    {
        public List<NestedItem> NestedItems { get; set; }
    }
}