// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'authenticate_result_model.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

AuthenticateResultModel _$AuthenticateResultModelFromJson(
        Map<String, dynamic> json) =>
    AuthenticateResultModel(
      accessToken: json['accessToken'] as String?,
      tokenExpireDate: json['tokenExpireDate'] == null
          ? null
          : DateTime.parse(json['tokenExpireDate'] as String),
      encryptedAccessToken: json['encryptedAccessToken'] as String?,
      expireInSeconds: json['expireInSeconds'] as int?,
      shouldResetPassword: json['shouldResetPassword'] as bool?,
      passwordResetCode: json['passwordResetCode'] as String?,
      userId: json['userId'] as int?,
      requiresTwoFactorVerification:
          json['requiresTwoFactorVerification'] as bool?,
      twoFactorAuthProviders: (json['twoFactorAuthProviders'] as List<dynamic>?)
          ?.map((e) => e as String)
          .toList(),
      twoFactorRememberClientToken:
          json['twoFactorRememberClientToken'] as String?,
      returnUrl: json['returnUrl'] as String?,
      refreshTokenExpireInSeconds: json['refreshTokenExpireInSeconds'] as int?,
      refreshTokenExpireDate: json['refreshTokenExpireDate'] == null
          ? null
          : DateTime.parse(json['refreshTokenExpireDate'] as String),
    )..refreshToken = json['refreshToken'] as String?;

Map<String, dynamic> _$AuthenticateResultModelToJson(
        AuthenticateResultModel instance) =>
    <String, dynamic>{
      'accessToken': instance.accessToken,
      'encryptedAccessToken': instance.encryptedAccessToken,
      'refreshToken': instance.refreshToken,
      'expireInSeconds': instance.expireInSeconds,
      'tokenExpireDate': instance.tokenExpireDate?.toIso8601String(),
      'shouldResetPassword': instance.shouldResetPassword,
      'passwordResetCode': instance.passwordResetCode,
      'userId': instance.userId,
      'requiresTwoFactorVerification': instance.requiresTwoFactorVerification,
      'twoFactorAuthProviders': instance.twoFactorAuthProviders,
      'twoFactorRememberClientToken': instance.twoFactorRememberClientToken,
      'returnUrl': instance.returnUrl,
      'refreshTokenExpireInSeconds': instance.refreshTokenExpireInSeconds,
      'refreshTokenExpireDate':
          instance.refreshTokenExpireDate?.toIso8601String(),
    };
