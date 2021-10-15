import 'package:json_annotation/json_annotation.dart';
part 'user_security_config.g.dart';

@JsonSerializable()
class UserAntiForgeryConfig {
  String? tokenCookieName;

  String? tokenHeaderName;

  UserAntiForgeryConfig({this.tokenCookieName, this.tokenHeaderName});

  factory UserAntiForgeryConfig.fromJson(Map<String, dynamic> json) =>
      _$UserAntiForgeryConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserAntiForgeryConfigToJson(this);
}

@JsonSerializable()
class UserSecurityConfig {
  UserAntiForgeryConfig? antiForgery;
  UserSecurityConfig({this.antiForgery});

  factory UserSecurityConfig.fromJson(Map<String, dynamic> json) =>
      _$UserSecurityConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserSecurityConfigToJson(this);
}
