import 'package:json_annotation/json_annotation.dart';
part 'user_feature_config.g.dart';

@JsonSerializable()
class UserFeatureConfig {
  Map<String, dynamic>? allFeatures;
  UserFeatureConfig({this.allFeatures});
  factory UserFeatureConfig.fromJson(Map<String, dynamic> json) =>
      _$UserFeatureConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserFeatureConfigToJson(this);
}
