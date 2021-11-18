import 'package:json_annotation/json_annotation.dart';

part 'is_tenancy_available.g.dart';

@JsonSerializable(explicitToJson: true)
class IsTenantAvailableInput {
  String tenancyName;
  IsTenantAvailableInput(this.tenancyName);
  factory IsTenantAvailableInput.fromJson(Map<String, dynamic> json) => _$IsTenantAvailableInputFromJson(json);
  Map<String, dynamic> toJson() => _$IsTenantAvailableInputToJson(this);
}

@JsonSerializable(explicitToJson: true)
class IsTenantAvailableOutput {
  int? state;

  int? tenantId;

  String? serverRootAddress;

  IsTenantAvailableOutput({this.state, this.tenantId, this.serverRootAddress});

  factory IsTenantAvailableOutput.fromJson(Map<String, dynamic> json) => _$IsTenantAvailableOutputFromJson(json);

  Map<String, dynamic> toJson() => _$IsTenantAvailableOutputToJson(this);
}