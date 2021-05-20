using System.Collections.Generic;
using Abp.Localization;
using Zero.Install.Dto;

namespace Zero.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
