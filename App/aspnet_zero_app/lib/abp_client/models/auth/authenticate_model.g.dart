// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'authenticate_model.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

AuthenticateModel _$AuthenticateModelFromJson(Map<String, dynamic> json) =>
    AuthenticateModel(
      userNameOrEmailAddress: json['userNameOrEmailAddress'] as String?,
      password: json['password'] as String?,
      rememberClient: json['rememberClient'] as bool?,
    )
      ..twoFactorVerificationCode = json['twoFactorVerificationCode'] as String?
      ..twoFactorRememberClientToken =
          json['twoFactorRememberClientToken'] as String?
      ..singleSignIn = json['singleSignIn'] as bool?
      ..returnUrl = json['returnUrl'] as String?;

Map<String, dynamic> _$AuthenticateModelToJson(AuthenticateModel instance) =>
    <String, dynamic>{
      'userNameOrEmailAddress': instance.userNameOrEmailAddress,
      'password': instance.password,
      'rememberClient': instance.rememberClient,
      'twoFactorVerificationCode': instance.twoFactorVerificationCode,
      'twoFactorRememberClientToken': instance.twoFactorRememberClientToken,
      'singleSignIn': instance.singleSignIn,
      'returnUrl': instance.returnUrl,
    };
