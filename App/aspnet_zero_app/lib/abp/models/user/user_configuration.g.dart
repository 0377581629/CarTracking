// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_configuration.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserConfiguration _$UserConfigurationFromJson(Map<String, dynamic> json) =>
    UserConfiguration(
      multiTenancy: json['multiTenancy'] == null
          ? null
          : MultiTenancyConfig.fromJson(
              json['multiTenancy'] as Map<String, dynamic>),
      session: json['session'] == null
          ? null
          : UserSessionConfig.fromJson(json['session'] as Map<String, dynamic>),
      localization: json['localization'] == null
          ? null
          : UserLocalizationConfig.fromJson(
              json['localization'] as Map<String, dynamic>),
      features: json['features'] == null
          ? null
          : UserFeatureConfig.fromJson(
              json['features'] as Map<String, dynamic>),
      auth: json['auth'] == null
          ? null
          : UserAuthConfig.fromJson(json['auth'] as Map<String, dynamic>),
      nav: json['nav'] == null
          ? null
          : UserNavConfig.fromJson(json['nav'] as Map<String, dynamic>),
      setting: json['setting'] == null
          ? null
          : UserSettingConfig.fromJson(json['setting'] as Map<String, dynamic>),
      clock: json['clock'] == null
          ? null
          : UserClockConfig.fromJson(json['clock'] as Map<String, dynamic>),
      timing: json['timing'] == null
          ? null
          : UserTimmingConfig.fromJson(json['timing'] as Map<String, dynamic>),
      security: json['security'] == null
          ? null
          : UserSecurityConfig.fromJson(
              json['security'] as Map<String, dynamic>),
      custom: json['custom'] as Map<String, dynamic>?,
    );

Map<String, dynamic> _$UserConfigurationToJson(UserConfiguration instance) =>
    <String, dynamic>{
      'multiTenancy': instance.multiTenancy?.toJson(),
      'session': instance.session?.toJson(),
      'localization': instance.localization?.toJson(),
      'features': instance.features?.toJson(),
      'auth': instance.auth?.toJson(),
      'nav': instance.nav?.toJson(),
      'setting': instance.setting?.toJson(),
      'clock': instance.clock?.toJson(),
      'timing': instance.timing?.toJson(),
      'security': instance.security?.toJson(),
      'custom': instance.custom,
    };
