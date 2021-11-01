// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'language_info.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

LanguageInfo _$LanguageInfoFromJson(Map<String, dynamic> json) => LanguageInfo(
      name: json['name'] as String?,
      displayName: json['displayName'] as String?,
      icon: json['icon'] as String?,
      isDefault: json['isDefault'] as bool?,
      isDisabled: json['isDisabled'] as bool?,
      isRightToLeft: json['isRightToLeft'] as bool?,
    );

Map<String, dynamic> _$LanguageInfoToJson(LanguageInfo instance) =>
    <String, dynamic>{
      'name': instance.name,
      'displayName': instance.displayName,
      'icon': instance.icon,
      'isDefault': instance.isDefault,
      'isDisabled': instance.isDisabled,
      'isRightToLeft': instance.isRightToLeft,
    };
