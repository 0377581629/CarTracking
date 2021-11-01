// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'reset_password_model.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ResetPasswordModel _$ResetPasswordModelFromJson(Map<String, dynamic> json) =>
    ResetPasswordModel(
      userId: json['userId'] as int,
      resetCode: json['resetCode'] as String,
      password: json['password'] as String,
      returnUrl: json['returnUrl'] as String?,
      singleSignIn: json['singleSignIn'] as String?,
    )..cipher = json['c'] as String?;

Map<String, dynamic> _$ResetPasswordModelToJson(ResetPasswordModel instance) =>
    <String, dynamic>{
      'userId': instance.userId,
      'resetCode': instance.resetCode,
      'password': instance.password,
      'returnUrl': instance.returnUrl,
      'singleSignIn': instance.singleSignIn,
      'c': instance.cipher,
    };
