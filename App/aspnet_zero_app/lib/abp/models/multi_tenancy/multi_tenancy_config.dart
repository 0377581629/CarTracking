import 'package:json_annotation/json_annotation.dart';
part 'multi_tenancy_config.g.dart';

@JsonSerializable(explicitToJson: true)
class MultiTenancyConfig {

  bool? isEnabled;

  bool? ignoreFeatureCheckForHostUsers;

  MultiTenancyConfig({this.isEnabled, this.ignoreFeatureCheckForHostUsers});

  factory MultiTenancyConfig.fromJson(Map<String, dynamic> json) =>
      _$MultiTenancyConfigFromJson(json);

  Map<String, dynamic> toJson() => _$MultiTenancyConfigToJson(this);
}
