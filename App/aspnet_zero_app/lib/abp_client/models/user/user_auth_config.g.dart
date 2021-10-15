// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_auth_config.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserAuthConfig _$UserAuthConfigFromJson(Map<String, dynamic> json) =>
    UserAuthConfig(
      allPermissions: (json['allPermissions'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(k, e as String),
      ),
      grantedPermissions:
          (json['grantedPermissions'] as Map<String, dynamic>?)?.map(
        (k, e) => MapEntry(k, e as String),
      ),
    );

Map<String, dynamic> _$UserAuthConfigToJson(UserAuthConfig instance) =>
    <String, dynamic>{
      'allPermissions': instance.allPermissions,
      'grantedPermissions': instance.grantedPermissions,
    };
