import 'package:aspnet_zero_app/abp/models/localization/language_info.dart';
import 'package:aspnet_zero_app/abp/models/localization/localize_source.dart';
import 'package:aspnet_zero_app/abp/models/user/user_current_culture_config.dart';
import 'package:json_annotation/json_annotation.dart';
part 'user_localize_config.g.dart';

@JsonSerializable(explicitToJson: true)
class UserLocalizationConfig {
  UserCurrentCultureConfig? currentCulture;
  List<LanguageInfo>? languages;
  LanguageInfo? currentLanguage;
  List<LocalizationSource>? sources;
  Map<String, Map<String, String>>? values;
  UserLocalizationConfig(
      {this.currentCulture,
      this.currentLanguage,
      this.languages,
      this.sources,
      this.values});
  factory UserLocalizationConfig.fromJson(Map<String, dynamic> json) =>
      _$UserLocalizationConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserLocalizationConfigToJson(this);
}
