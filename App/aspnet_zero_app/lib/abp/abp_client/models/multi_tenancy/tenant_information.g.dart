// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'tenant_information.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

TenantInformation _$TenantInformationFromJson(Map<String, dynamic> json) =>
    TenantInformation(
      json['tenantId'] as int,
      json['tenancyName'] as String,
    );

Map<String, dynamic> _$TenantInformationToJson(TenantInformation instance) =>
    <String, dynamic>{
      'tenantId': instance.tenantId,
      'tenancyName': instance.tenancyName,
    };
