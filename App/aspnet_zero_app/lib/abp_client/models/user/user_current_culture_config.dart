import 'package:json_annotation/json_annotation.dart';

part 'user_current_culture_config.g.dart';

@JsonSerializable()
class UserCurrentCultureConfig {
  String? name;
  String? displayName;
  UserCurrentCultureConfig({this.name, this.displayName});
  factory UserCurrentCultureConfig.fromJson(Map<String, dynamic> json) =>
      _$UserCurrentCultureConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserCurrentCultureConfigToJson(this);
}
