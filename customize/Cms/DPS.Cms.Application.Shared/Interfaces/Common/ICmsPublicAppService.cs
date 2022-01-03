using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using DPS.Cms.Application.Shared.Dto.Common;
using DPS.Cms.Application.Shared.Dto.ImageBlock;
using DPS.Cms.Application.Shared.Dto.Menu;
using DPS.Cms.Application.Shared.Dto.Page;
using DPS.Cms.Application.Shared.Dto.PageLayout;

namespace DPS.Cms.Application.Shared.Interfaces.Common
{
    public interface ICmsPublicAppService : IApplicationService
    {
        #region Page
        Task<PageDto> GetHomePage();
        
        Task<PageDto> GetPageById(GetPageInput input);
        
        Task<PageDto> GetPageBySlug(GetPageInput input);
        
        Task<List<PageWidgetDto>> GetPageWidgets(CmsInput input);
        
        Task<PageWidgetDto> GetPageWidget(CmsInput input);

        Task<List<PageLayoutBlockDto>> GetPageLayoutBlocks(CmsInput input);
        
        #endregion
        
        #region Image Block
        Task<List<ImageBlockDto>> GetImageBlocksByGroupIds(CmsPublicInput input);
        
        #endregion
        
        #region Menu
        Task<List<MenuDto>> GetMenusByGroupIds(CmsPublicInput input);
        
        #endregion
    }
}