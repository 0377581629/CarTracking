import 'package:aspnet_zero_app/abp_client/models/common/string_value.dart';
import 'package:json_annotation/json_annotation.dart';
import 'package:tuple/tuple.dart';
part 'user_feature_config.g.dart';

@JsonSerializable()
class UserFeatureConfig {
  Map<String, dynamic>? allFeatures;
  UserFeatureConfig({this.allFeatures});
  factory UserFeatureConfig.fromJson(Map<String, dynamic> json) =>
      _$UserFeatureConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserFeatureConfigToJson(this);
}
