// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'multi_tenancy_config.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

MultiTenancyConfig _$MultiTenancyConfigFromJson(Map<String, dynamic> json) =>
    MultiTenancyConfig(
      isEnabled: json['isEnabled'] as bool?,
      ignoreFeatureCheckForHostUsers:
          json['ignoreFeatureCheckForHostUsers'] as bool?,
    );

Map<String, dynamic> _$MultiTenancyConfigToJson(MultiTenancyConfig instance) =>
    <String, dynamic>{
      'isEnabled': instance.isEnabled,
      'ignoreFeatureCheckForHostUsers': instance.ignoreFeatureCheckForHostUsers,
    };
