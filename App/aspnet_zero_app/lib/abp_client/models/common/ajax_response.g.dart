// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'ajax_response.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ValidationErrorInfo _$ValidationErrorInfoFromJson(Map<String, dynamic> json) =>
    ValidationErrorInfo()
      ..message = json['message'] as String?
      ..members =
          (json['members'] as List<dynamic>?)?.map((e) => e as String).toList();

Map<String, dynamic> _$ValidationErrorInfoToJson(ValidationErrorInfo instance) {
  final val = <String, dynamic>{};

  void writeNotNull(String key, dynamic value) {
    if (value != null) {
      val[key] = value;
    }
  }

  writeNotNull('message', instance.message);
  writeNotNull('members', instance.members);
  return val;
}

ErrorInfo _$ErrorInfoFromJson(Map<String, dynamic> json) => ErrorInfo(
      json['code'] as int,
      validationErrors: (json['validationErrors'] as List<dynamic>?)
          ?.map((e) => ValidationErrorInfo.fromJson(e as Map<String, dynamic>))
          .toList(),
    )
      ..message = json['message'] as String?
      ..details = json['details'] as String?;

Map<String, dynamic> _$ErrorInfoToJson(ErrorInfo instance) => <String, dynamic>{
      'code': instance.code,
      'message': instance.message,
      'details': instance.details,
      'validationErrors': instance.validationErrors,
    };

AjaxResponse<T> _$AjaxResponseFromJson<T>(Map<String, dynamic> json) =>
    AjaxResponse<T>()
      ..targetUrl = json['targetUrl'] as String
      ..success = json['success'] as bool
      ..unAuthorizedRequest = json['unAuthorizedRequest'] as bool
      ..abp = json['__abp'] as bool
      ..errorInfo = json['errorInfo'] == null
          ? null
          : ErrorInfo.fromJson(json['errorInfo'] as Map<String, dynamic>)
      ..result = _Converter<T?>().fromJson(json['result']);

Map<String, dynamic> _$AjaxResponseToJson<T>(AjaxResponse<T> instance) =>
    <String, dynamic>{
      'targetUrl': instance.targetUrl,
      'success': instance.success,
      'unAuthorizedRequest': instance.unAuthorizedRequest,
      '__abp': instance.abp,
      'errorInfo': instance.errorInfo?.toJson(),
      'result': _Converter<T?>().toJson(instance.result),
    };
