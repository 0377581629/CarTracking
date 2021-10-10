import 'package:aspnet_zero_app/abp_client/models/common/user_menu.dart';
import 'package:json_annotation/json_annotation.dart';
part 'user_nav_config.g.dart';

@JsonSerializable()
class UserNavConfig {
  Map<String, UserMenu>? menus;
  UserNavConfig({this.menus});

  factory UserNavConfig.fromJson(Map<String, dynamic> json) =>
      _$UserNavConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserNavConfigToJson(this);
}
