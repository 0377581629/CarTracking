import 'package:json_annotation/json_annotation.dart';

part 'tenancy_dto.g.dart';

@JsonSerializable(explicitToJson: true)
class TenancyDto {
  int? id;
  String? code;

  int? parentId;
  String? parentTenancyName;
  String? parentName;

  String? tenancyName;
  String? name;
  String? editionDisplayName;
  String? connectionString;

  bool? isActive;
  DateTime? creationTime;
  DateTime? subscriptionEndDateUtc;
  int? editionId;
  bool? isInTrialPeriod;

  String? loginLogoId;
  String? menuLogoId;
  String? loginBackgroundId;

  TenancyDto(
      {this.id,
      this.code,
      this.parentId,
      this.parentTenancyName,
      this.parentName,
      this.tenancyName,
      this.name,
      this.editionDisplayName,
      this.connectionString,
      this.isActive,
      this.creationTime,
      this.subscriptionEndDateUtc,
      this.editionId,
      this.isInTrialPeriod,
      this.loginLogoId,
      this.menuLogoId,
      this.loginBackgroundId});

  factory TenancyDto.fromJson(Map<String, dynamic> json) => _$TenancyDtoFromJson(json);

  Map<String, dynamic> toJson() => _$TenancyDtoToJson(this);
}
