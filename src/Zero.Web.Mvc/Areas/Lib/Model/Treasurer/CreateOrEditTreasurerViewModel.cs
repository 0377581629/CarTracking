using DPS.Lib.Application.Shared.Dto.Basic.Treasurer;

namespace Zero.Web.Areas.Lib.Model.Treasurer
{
    public class CreateOrEditTreasurerViewModel
    {
        public CreateOrEditTreasurerDto Treasurer { get; set; }

        public bool IsEditMode => Treasurer.Id.HasValue;
    }
}