using Abp.AspNetCore.Mvc.Authorization;
using Zero.Authorization;
using Zero.Storage;
using Abp.BackgroundJobs;
using Abp.Authorization;

namespace Zero.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}