// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'register_result.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

RegisterResult _$RegisterResultFromJson(Map<String, dynamic> json) =>
    RegisterResult(
      isSuccess: json['isSuccess'] as bool?,
      isEmailConfirmationRequiredForLogin:
          json['isEmailConfirmationRequiredForLogin'] as bool?,
      canLogin: json['canLogin'] as bool?,
      isUserActivated: json['isUserActivated'] as bool?,
      exceptionMessage: json['exceptionMessage'] as String?,
    );

Map<String, dynamic> _$RegisterResultToJson(RegisterResult instance) =>
    <String, dynamic>{
      'isSuccess': instance.isSuccess,
      'isEmailConfirmationRequiredForLogin':
          instance.isEmailConfirmationRequiredForLogin,
      'canLogin': instance.canLogin,
      'isUserActivated': instance.isUserActivated,
      'exceptionMessage': instance.exceptionMessage,
    };
