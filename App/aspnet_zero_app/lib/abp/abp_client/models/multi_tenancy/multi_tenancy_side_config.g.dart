// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'multi_tenancy_side_config.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

MultiTenancySideConfig _$MultiTenancySideConfigFromJson(
        Map<String, dynamic> json) =>
    MultiTenancySideConfig(
      host: _$enumDecodeNullable(_$MultiTenancySidesEnumMap, json['host']),
      tenant: _$enumDecodeNullable(_$MultiTenancySidesEnumMap, json['tenant']),
    );

Map<String, dynamic> _$MultiTenancySideConfigToJson(
        MultiTenancySideConfig instance) =>
    <String, dynamic>{
      'host': _$MultiTenancySidesEnumMap[instance.host],
      'tenant': _$MultiTenancySidesEnumMap[instance.tenant],
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

K? _$enumDecodeNullable<K, V>(
  Map<K, V> enumValues,
  dynamic source, {
  K? unknownValue,
}) {
  if (source == null) {
    return null;
  }
  return _$enumDecode<K, V>(enumValues, source, unknownValue: unknownValue);
}

const _$MultiTenancySidesEnumMap = {
  MultiTenancySides.none: 'none',
  MultiTenancySides.tenant: 'tenant',
  MultiTenancySides.host: 'host',
};
