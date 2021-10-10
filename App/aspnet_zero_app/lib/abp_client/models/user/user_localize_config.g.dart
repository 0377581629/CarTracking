// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_localize_config.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserLocalizationConfig _$UserLocalizationConfigFromJson(
        Map<String, dynamic> json) =>
    UserLocalizationConfig(
      currentCulture: json['currentCulture'] == null
          ? null
          : UserCurrentCultureConfig.fromJson(
              json['currentCulture'] as Map<String, dynamic>),
      currentLanguage: json['currentLanguage'] == null
          ? null
          : LanguageInfo.fromJson(
              json['currentLanguage'] as Map<String, dynamic>),
      languages: (json['languages'] as List<dynamic>?)
          ?.map((e) => LanguageInfo.fromJson(e as Map<String, dynamic>))
          .toList(),
      sources: (json['sources'] as List<dynamic>?)
          ?.map((e) => LocalizationSource.fromJson(e as Map<String, dynamic>))
          .toList(),
      values: (json['values'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(k, Map<String, String>.from(e as Map)),
      ),
    );

Map<String, dynamic> _$UserLocalizationConfigToJson(
        UserLocalizationConfig instance) =>
    <String, dynamic>{
      'currentCulture': instance.currentCulture,
      'languages': instance.languages,
      'currentLanguage': instance.currentLanguage,
      'sources': instance.sources,
      'values': instance.values,
    };
