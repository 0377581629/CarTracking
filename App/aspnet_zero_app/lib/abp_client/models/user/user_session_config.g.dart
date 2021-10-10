// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_session_config.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserSessionConfig _$UserSessionConfigFromJson(Map<String, dynamic> json) =>
    UserSessionConfig(
      _$enumDecode(_$MultiTenancySidesEnumMap, json['multiTenancySide']),
    )
      ..userId = json['userId'] as int?
      ..tenantId = json['tenantId'] as int?
      ..impersonatorUserId = json['impersonatorUserId'] as int?
      ..impersonatorTenantId = json['impersonatorTenantId'] as int?;

Map<String, dynamic> _$UserSessionConfigToJson(UserSessionConfig instance) =>
    <String, dynamic>{
      'userId': instance.userId,
      'tenantId': instance.tenantId,
      'impersonatorUserId': instance.impersonatorUserId,
      'impersonatorTenantId': instance.impersonatorTenantId,
      'multiTenancySide': _$MultiTenancySidesEnumMap[instance.multiTenancySide],
    };

K _$enumDecode<K, V>(
  Map<K, V> enumValues,
  Object? source, {
  K? unknownValue,
}) {
  if (source == null) {
    throw ArgumentError(
      'A value must be provided. Supported values: '
      '${enumValues.values.join(', ')}',
    );
  }

  return enumValues.entries.singleWhere(
    (e) => e.value == source,
    orElse: () {
      if (unknownValue == null) {
        throw ArgumentError(
          '`$source` is not one of the supported values: '
          '${enumValues.values.join(', ')}',
        );
      }
      return MapEntry(unknownValue, enumValues.values.first);
    },
  ).key;
}

const _$MultiTenancySidesEnumMap = {
  MultiTenancySides.none: 'none',
  MultiTenancySides.tenant: 'tenant',
  MultiTenancySides.host: 'host',
};
