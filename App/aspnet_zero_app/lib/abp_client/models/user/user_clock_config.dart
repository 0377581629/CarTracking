import 'package:json_annotation/json_annotation.dart';
part 'user_clock_config.g.dart';

@JsonSerializable()
class UserClockConfig {
  String? provider;
  UserClockConfig({this.provider});
  factory UserClockConfig.fromJson(Map<String, dynamic> json) =>
      _$UserClockConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserClockConfigToJson(this);
}
