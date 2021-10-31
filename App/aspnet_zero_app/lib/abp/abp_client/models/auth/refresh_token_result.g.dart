// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'refresh_token_result.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

RefreshTokenResult _$RefreshTokenResultFromJson(Map<String, dynamic> json) =>
    RefreshTokenResult(
      json['accessToken'] as String,
      json['encryptedAccessToken'] as String,
      json['expireInSeconds'] as int,
    );

Map<String, dynamic> _$RefreshTokenResultToJson(RefreshTokenResult instance) =>
    <String, dynamic>{
      'accessToken': instance.accessToken,
      'encryptedAccessToken': instance.encryptedAccessToken,
      'expireInSeconds': instance.expireInSeconds,
    };
