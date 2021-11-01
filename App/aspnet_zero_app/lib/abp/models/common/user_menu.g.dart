// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_menu.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserMenu _$UserMenuFromJson(Map<String, dynamic> json) => UserMenu(
      name: json['name'] as String?,
      displayName: json['displayName'] as String?,
      items: (json['items'] as List<dynamic>?)
          ?.map((e) => UserMenuItem.fromJson(e as Map<String, dynamic>))
          .toList(),
    )..customData = json['customData'];

Map<String, dynamic> _$UserMenuToJson(UserMenu instance) => <String, dynamic>{
      'name': instance.name,
      'displayName': instance.displayName,
      'customData': instance.customData,
      'items': instance.items,
    };

UserMenuItem _$UserMenuItemFromJson(Map<String, dynamic> json) => UserMenuItem(
      name: json['name'] as String?,
      icon: json['icon'] as String?,
      displayName: json['displayName'] as String?,
      order: json['order'] as int?,
      url: json['url'] as String?,
      customData: json['customData'],
      target: json['target'] as String?,
      isEnabled: json['isEnabled'] as bool?,
      isVisible: json['isVisible'] as bool?,
      items: (json['items'] as List<dynamic>?)
          ?.map((e) => UserMenuItem.fromJson(e as Map<String, dynamic>))
          .toList(),
    );

Map<String, dynamic> _$UserMenuItemToJson(UserMenuItem instance) =>
    <String, dynamic>{
      'name': instance.name,
      'icon': instance.icon,
      'displayName': instance.displayName,
      'order': instance.order,
      'url': instance.url,
      'customData': instance.customData,
      'target': instance.target,
      'isEnabled': instance.isEnabled,
      'isVisible': instance.isVisible,
      'items': instance.items,
    };
