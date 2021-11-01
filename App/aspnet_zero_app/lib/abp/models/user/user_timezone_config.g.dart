// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_timezone_config.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserIanaTimeZoneConfig _$UserIanaTimeZoneConfigFromJson(
        Map<String, dynamic> json) =>
    UserIanaTimeZoneConfig(
      timeZoneId: json['timeZoneId'] as String?,
    );

Map<String, dynamic> _$UserIanaTimeZoneConfigToJson(
        UserIanaTimeZoneConfig instance) =>
    <String, dynamic>{
      'timeZoneId': instance.timeZoneId,
    };

UserWindowsTimeZoneConfig _$UserWindowsTimeZoneConfigFromJson(
        Map<String, dynamic> json) =>
    UserWindowsTimeZoneConfig(
      timeZoneId: json['timeZoneId'] as String?,
      baseUtcOffsetInMilliseconds:
          (json['baseUtcOffsetInMilliseconds'] as num?)?.toDouble(),
      currentUtcOffsetInMilliseconds:
          (json['currentUtcOffsetInMilliseconds'] as num?)?.toDouble(),
      isDaylightSavingTimeNow: json['isDaylightSavingTimeNow'] as bool?,
    );

Map<String, dynamic> _$UserWindowsTimeZoneConfigToJson(
        UserWindowsTimeZoneConfig instance) =>
    <String, dynamic>{
      'timeZoneId': instance.timeZoneId,
      'baseUtcOffsetInMilliseconds': instance.baseUtcOffsetInMilliseconds,
      'currentUtcOffsetInMilliseconds': instance.currentUtcOffsetInMilliseconds,
      'isDaylightSavingTimeNow': instance.isDaylightSavingTimeNow,
    };

UserTimeZoneConfig _$UserTimeZoneConfigFromJson(Map<String, dynamic> json) =>
    UserTimeZoneConfig(
      windows: json['windows'] == null
          ? null
          : UserWindowsTimeZoneConfig.fromJson(
              json['windows'] as Map<String, dynamic>),
      iana: json['iana'] == null
          ? null
          : UserIanaTimeZoneConfig.fromJson(
              json['iana'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$UserTimeZoneConfigToJson(UserTimeZoneConfig instance) =>
    <String, dynamic>{
      'windows': instance.windows,
      'iana': instance.iana,
    };

UserTimmingConfig _$UserTimmingConfigFromJson(Map<String, dynamic> json) =>
    UserTimmingConfig(
      timeZoneInfo: json['timeZoneInfo'] == null
          ? null
          : UserTimeZoneConfig.fromJson(
              json['timeZoneInfo'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$UserTimmingConfigToJson(UserTimmingConfig instance) =>
    <String, dynamic>{
      'timeZoneInfo': instance.timeZoneInfo,
    };
