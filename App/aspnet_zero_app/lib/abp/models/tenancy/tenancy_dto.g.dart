// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'tenancy_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

TenancyDto _$TenancyDtoFromJson(Map<String, dynamic> json) => TenancyDto(
      id: json['id'] as int?,
      code: json['code'] as String?,
      parentId: json['parentId'] as int?,
      parentTenancyName: json['parentTenancyName'] as String?,
      parentName: json['parentName'] as String?,
      tenancyName: json['tenancyName'] as String?,
      name: json['name'] as String?,
      editionDisplayName: json['editionDisplayName'] as String?,
      connectionString: json['connectionString'] as String?,
      isActive: json['isActive'] as bool?,
      creationTime: json['creationTime'] == null
          ? null
          : DateTime.parse(json['creationTime'] as String),
      subscriptionEndDateUtc: json['subscriptionEndDateUtc'] == null
          ? null
          : DateTime.parse(json['subscriptionEndDateUtc'] as String),
      editionId: json['editionId'] as int?,
      isInTrialPeriod: json['isInTrialPeriod'] as bool?,
      loginLogoId: json['loginLogoId'] as String?,
      menuLogoId: json['menuLogoId'] as String?,
      loginBackgroundId: json['loginBackgroundId'] as String?,
    );

Map<String, dynamic> _$TenancyDtoToJson(TenancyDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'code': instance.code,
      'parentId': instance.parentId,
      'parentTenancyName': instance.parentTenancyName,
      'parentName': instance.parentName,
      'tenancyName': instance.tenancyName,
      'name': instance.name,
      'editionDisplayName': instance.editionDisplayName,
      'connectionString': instance.connectionString,
      'isActive': instance.isActive,
      'creationTime': instance.creationTime?.toIso8601String(),
      'subscriptionEndDateUtc':
          instance.subscriptionEndDateUtc?.toIso8601String(),
      'editionId': instance.editionId,
      'isInTrialPeriod': instance.isInTrialPeriod,
      'loginLogoId': instance.loginLogoId,
      'menuLogoId': instance.menuLogoId,
      'loginBackgroundId': instance.loginBackgroundId,
    };
