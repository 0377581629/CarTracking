import 'package:aspnet_zero_app/abp/models/common/user_menu.dart';
import 'package:json_annotation/json_annotation.dart';
part 'user_nav_config.g.dart';

@JsonSerializable(explicitToJson: true)
class UserNavConfig {
  Map<String, UserMenu>? menus;
  UserNavConfig({this.menus});

  factory UserNavConfig.fromJson(Map<String, dynamic> json) =>
      _$UserNavConfigFromJson(json);

  Map<String, dynamic> toJson() => _$UserNavConfigToJson(this);
}
