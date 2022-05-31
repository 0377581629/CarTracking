using DPS.Lib.Application.Shared.Dto.Basic.NetworkProvider;

namespace Zero.Web.Areas.Lib.Model.NetworkProvider
{
    public class CreateOrEditNetworkProviderViewModel
    {
        public CreateOrEditNetworkProviderDto NetworkProvider { get; set; }

        public bool IsEditMode => NetworkProvider.Id.HasValue;
    }
}