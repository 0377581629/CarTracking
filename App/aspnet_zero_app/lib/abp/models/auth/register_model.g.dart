// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'register_model.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

RegisterModel _$RegisterModelFromJson(Map<String, dynamic> json) =>
    RegisterModel(
      name: json['name'] as String?,
      surname: json['surname'] as String?,
      userName: json['userName'] as String?,
      emailAddress: json['emailAddress'] as String?,
      password: json['password'] as String?,
      isExternalLogin: json['isExternalLogin'] as bool?,
      externalLoginAuthSchema: json['externalLoginAuthSchema'] as String?,
      returnUrl: json['returnUrl'] as String?,
      singleSignIn: json['singleSignIn'] as String?,
    );

Map<String, dynamic> _$RegisterModelToJson(RegisterModel instance) =>
    <String, dynamic>{
      'name': instance.name,
      'surname': instance.surname,
      'userName': instance.userName,
      'emailAddress': instance.emailAddress,
      'password': instance.password,
      'isExternalLogin': instance.isExternalLogin,
      'externalLoginAuthSchema': instance.externalLoginAuthSchema,
      'returnUrl': instance.returnUrl,
      'singleSignIn': instance.singleSignIn,
    };
