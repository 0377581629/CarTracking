using System.Linq;
using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace Zero.Localization
{
    public static class ZeroLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            var abpZeroLangIndex = localizationConfiguration.Sources.IndexOf(localizationConfiguration.Sources.FirstOrDefault(o=>o.Name == ZeroConst.LocalizationSourceName));
            localizationConfiguration.Sources.RemoveAt(abpZeroLangIndex);
            
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    ZeroConst.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(ZeroLocalizationConfigurer).GetAssembly(),
                        "Zero.Localization.Zero"
                    )
                )
            );
        }
    }
}