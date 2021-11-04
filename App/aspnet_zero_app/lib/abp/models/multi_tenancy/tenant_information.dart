import 'package:json_annotation/json_annotation.dart';
part 'tenant_information.g.dart';

@JsonSerializable(explicitToJson: true)
class TenantInformation {
  int tenantId;
  String tenancyName;
  TenantInformation(this.tenantId, this.tenancyName);
  factory TenantInformation.fromJson(Map<String, dynamic> json) =>
      _$TenantInformationFromJson(json);

  Map<String, dynamic> toJson() => _$TenantInformationToJson(this);
}
