import 'package:aspnet_zero_app/abp_client/interfaces/multi_tenancy_config.dart';
import 'package:aspnet_zero_app/abp_client/models/multi_tenancy/multi_tenancy_side_config.dart';
import 'package:json_annotation/json_annotation.dart';
part 'multi_tenancy_config.g.dart';

@JsonSerializable()
class MultiTenancyConfig implements IMultiTenancyConfig {
  @override
  bool? isEnabled;
  @override
  bool? ignoreFeatureCheckForHostUsers;
  @override
  MultiTenancySideConfig? sides;
  MultiTenancyConfig(
      {this.isEnabled, this.ignoreFeatureCheckForHostUsers, this.sides});
  factory MultiTenancyConfig.fromJson(Map<String, dynamic> json) =>
      _$MultiTenancyConfigFromJson(json);

  Map<String, dynamic> toJson() => _$MultiTenancyConfigToJson(this);
}
