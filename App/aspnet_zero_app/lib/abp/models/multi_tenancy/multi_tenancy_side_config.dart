import 'package:aspnet_zero_app/abp/enums/multi_tenancy_sides.dart';
import 'package:json_annotation/json_annotation.dart';
part 'multi_tenancy_side_config.g.dart';

@JsonSerializable(explicitToJson: true)
class MultiTenancySideConfig {
  MultiTenancySides? host;
  MultiTenancySides? tenant;
  MultiTenancySideConfig({this.host, this.tenant});

  factory MultiTenancySideConfig.fromJson(Map<String, dynamic> json) =>
      _$MultiTenancySideConfigFromJson(json);

  Map<String, dynamic> toJson() => _$MultiTenancySideConfigToJson(this);
}
