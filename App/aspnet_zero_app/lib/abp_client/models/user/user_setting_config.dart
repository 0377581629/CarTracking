import 'package:tuple/tuple.dart';
import 'package:json_annotation/json_annotation.dart';
part 'user_setting_config.g.dart';

@JsonSerializable()
class UserSettingConfig {
  Map<String, dynamic>? values;
  UserSettingConfig({this.values});

  factory UserSettingConfig.fromJson(Map<String, dynamic> json) =>
      _$UserSettingConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserSettingConfigToJson(this);
}
