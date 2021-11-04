import 'package:json_annotation/json_annotation.dart';
part 'user_menu.g.dart';

@JsonSerializable(explicitToJson: true)
class UserMenu {
  String? name;

  String? displayName;

  dynamic customData;

  List<UserMenuItem>? items;

  UserMenu({this.name, this.displayName, this.items});

  factory UserMenu.fromJson(Map<String, dynamic> json) =>
      _$UserMenuFromJson(json);

  Map<String, dynamic> toJson() => _$UserMenuToJson(this);
}

@JsonSerializable(explicitToJson: true)
class UserMenuItem {
  String? name;

  String? icon;

  String? displayName;

  int? order;

  String? url;

  dynamic customData;

  String? target;

  bool? isEnabled;

  bool? isVisible;

  List<UserMenuItem>? items;

  UserMenuItem(
      {this.name,
      this.icon,
      this.displayName,
      this.order,
      this.url,
      this.customData,
      this.target,
      this.isEnabled,
      this.isVisible,
      this.items});

  factory UserMenuItem.fromJson(Map<String, dynamic> json) =>
      _$UserMenuItemFromJson(json);

  Map<String, dynamic> toJson() => _$UserMenuItemToJson(this);
}
