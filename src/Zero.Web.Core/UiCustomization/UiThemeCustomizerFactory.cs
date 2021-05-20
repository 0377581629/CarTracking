using System;
using System.Threading.Tasks;
using Abp.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zero.Configuration;
using Zero.UiCustomization;
using Zero.Web.UiCustomization.Metronic;

namespace Zero.Web.UiCustomization
{
    public class UiThemeCustomizerFactory : IUiThemeCustomizerFactory
    {
        private readonly ISettingManager _settingManager;
        private readonly IServiceProvider _serviceProvider;

        public UiThemeCustomizerFactory(
            ISettingManager settingManager,
            IServiceProvider serviceProvider)
        {
            _settingManager = settingManager;
            _serviceProvider = serviceProvider;
        }

        public async Task<IUiCustomizer> GetCurrentUiCustomizer()
        {
            var theme = await _settingManager.GetSettingValueAsync(AppSettings.UiManagement.Theme);
            return GetUiCustomizerInternal(theme);
        }

        public IUiCustomizer GetUiCustomizer(string theme)
        {
            return GetUiCustomizerInternal(theme);
        }

        private IUiCustomizer GetUiCustomizerInternal(string theme)
        {
            if (theme.Equals(AppConsts.Theme10, StringComparison.InvariantCultureIgnoreCase))
            {
                return _serviceProvider.GetService<Theme10UiCustomizer>();
            }

            return _serviceProvider.GetService<ThemeDefaultUiCustomizer>();
        }
    }
}
