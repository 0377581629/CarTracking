// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'is_tenancy_available.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

IsTenantAvailableInput _$IsTenantAvailableInputFromJson(
        Map<String, dynamic> json) =>
    IsTenantAvailableInput(
      json['tenancyName'] as String,
    );

Map<String, dynamic> _$IsTenantAvailableInputToJson(
        IsTenantAvailableInput instance) =>
    <String, dynamic>{
      'tenancyName': instance.tenancyName,
    };

IsTenantAvailableOutput _$IsTenantAvailableOutputFromJson(
        Map<String, dynamic> json) =>
    IsTenantAvailableOutput(
      state: json['state'] as int?,
      tenantId: json['tenantId'] as int?,
      serverRootAddress: json['serverRootAddress'] as String?,
    );

Map<String, dynamic> _$IsTenantAvailableOutputToJson(
        IsTenantAvailableOutput instance) =>
    <String, dynamic>{
      'state': instance.state,
      'tenantId': instance.tenantId,
      'serverRootAddress': instance.serverRootAddress,
    };
