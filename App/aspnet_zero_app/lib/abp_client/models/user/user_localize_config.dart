import 'package:aspnet_zero_app/abp_client/models/localization/language_info.dart';
import 'package:aspnet_zero_app/abp_client/models/localization/localize_source.dart';
import 'package:aspnet_zero_app/abp_client/models/user/user_current_culture_config.dart';
import 'package:tuple/tuple.dart';

class UserLocalizationConfig {
  UserCurrentCultureConfig currentCulture;
  List<LanguageInfo> languages;
  LanguageInfo currentLanguage;
  List<LocalizationSource> sources;
  Tuple2<String, Tuple2<String, String>> values;
  UserLocalizationConfig(this.currentCulture, this.currentLanguage,
      this.languages, this.sources, this.values);
}