// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_session_config.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserSessionConfig _$UserSessionConfigFromJson(Map<String, dynamic> json) =>
    UserSessionConfig(
      userId: json['userId'] as int?,
      tenantId: json['tenantId'] as int?,
      impersonatorTenantId: json['impersonatorTenantId'] as int?,
      impersonatorUserId: json['impersonatorUserId'] as int?,
      multiTenancySide: json['multiTenancySide'] as int?,
    );

Map<String, dynamic> _$UserSessionConfigToJson(UserSessionConfig instance) =>
    <String, dynamic>{
      'userId': instance.userId,
      'tenantId': instance.tenantId,
      'impersonatorUserId': instance.impersonatorUserId,
      'impersonatorTenantId': instance.impersonatorTenantId,
      'multiTenancySide': instance.multiTenancySide,
    };
