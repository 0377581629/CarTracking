// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_security_config.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserAntiForgeryConfig _$UserAntiForgeryConfigFromJson(
        Map<String, dynamic> json) =>
    UserAntiForgeryConfig(
      tokenCookieName: json['tokenCookieName'] as String?,
      tokenHeaderName: json['tokenHeaderName'] as String?,
    );

Map<String, dynamic> _$UserAntiForgeryConfigToJson(
        UserAntiForgeryConfig instance) =>
    <String, dynamic>{
      'tokenCookieName': instance.tokenCookieName,
      'tokenHeaderName': instance.tokenHeaderName,
    };

UserSecurityConfig _$UserSecurityConfigFromJson(Map<String, dynamic> json) =>
    UserSecurityConfig(
      antiForgery: json['antiForgery'] == null
          ? null
          : UserAntiForgeryConfig.fromJson(
              json['antiForgery'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$UserSecurityConfigToJson(UserSecurityConfig instance) =>
    <String, dynamic>{
      'antiForgery': instance.antiForgery,
    };
