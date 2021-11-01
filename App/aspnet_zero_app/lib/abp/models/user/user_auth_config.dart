import 'package:json_annotation/json_annotation.dart';
part 'user_auth_config.g.dart';

@JsonSerializable()
class UserAuthConfig {
  Map<String, String>? allPermissions;
  Map<String, String>? grantedPermissions;
  UserAuthConfig({this.allPermissions, this.grantedPermissions});
  factory UserAuthConfig.fromJson(Map<String, dynamic> json) =>
      _$UserAuthConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserAuthConfigToJson(this);
}
