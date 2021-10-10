// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_nav_config.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserNavConfig _$UserNavConfigFromJson(Map<String, dynamic> json) =>
    UserNavConfig(
      menus: (json['menus'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(k, UserMenu.fromJson(e as Map<String, dynamic>)),
      ),
    );

Map<String, dynamic> _$UserNavConfigToJson(UserNavConfig instance) =>
    <String, dynamic>{
      'menus': instance.menus,
    };
