import 'package:json_annotation/json_annotation.dart';

part 'ajax_response.g.dart';

@JsonSerializable(includeIfNull: false)
class ValidationErrorInfo {
  String? message;
  List<String>? members;

  ValidationErrorInfo();

  factory ValidationErrorInfo.fromJson(Map<String, dynamic> json) =>
      _$ValidationErrorInfoFromJson(json);

  Map<String, dynamic> toJson() => _$ValidationErrorInfoToJson(this);
}

@JsonSerializable(explicitToJson: true)
class ErrorInfo {
  int code;
  String? message;
  String? details;
  List<ValidationErrorInfo> validationErrors;

  ErrorInfo(this.code, {List<ValidationErrorInfo>? validationErrors})
      : validationErrors = validationErrors ?? <ValidationErrorInfo>[];

  factory ErrorInfo.fromJson(Map<String, dynamic> json) =>
      _$ErrorInfoFromJson(json);

  Map<String, dynamic> toJson() => _$ErrorInfoToJson(this);
}

@JsonSerializable(explicitToJson: true, genericArgumentFactories: true)
class AjaxResponse<T> {
  String? targetUrl;
  bool success = false;
  bool unAuthorizedRequest = false;

  @JsonKey(name: "__abp")
  bool abp = false;

  @JsonKey(name: "error")
  ErrorInfo? errorInfo;

  T? result;

  AjaxResponse(
      {T? inpResult,
      String inpTargetUrl = '',
      ErrorInfo? inpErrorInfo,
      bool inpUnAuthorizedRequest = false}) {
    targetUrl = inpTargetUrl;
    result = inpResult;
    if (inpErrorInfo != null || inpUnAuthorizedRequest == false) {
      success = false;
    } else {
      success = true;
    }
    errorInfo = inpErrorInfo;
    unAuthorizedRequest = inpUnAuthorizedRequest;
  }

  factory AjaxResponse.fromJson(
          Map<String, dynamic> json, T Function(Object? json) fromJsonT) =>
      _$AjaxResponseFromJson(json, fromJsonT);

  Map<String, dynamic> toJson(Object? Function(T value) toJsonT) =>
      _$AjaxResponseToJson(this, toJsonT);
}

@JsonSerializable(explicitToJson: true)
class SimpleAjaxResponse {
  String? targetUrl;

  bool success = false;

  bool unAuthorizedRequest = false;

  @JsonKey(name: "__abp")
  bool abp = false;

  @JsonKey(name: "error")
  ErrorInfo? errorInfo;

  SimpleAjaxResponse(
      {String inpTargetUrl = '',
      ErrorInfo? inpErrorInfo,
      bool inpUnAuthorizedRequest = false}) {
    targetUrl = inpTargetUrl;
    if (inpErrorInfo != null || inpUnAuthorizedRequest == false) {
      success = false;
    } else {
      success = true;
    }
    errorInfo = inpErrorInfo;
    unAuthorizedRequest = inpUnAuthorizedRequest;
  }

  factory SimpleAjaxResponse.fromJson(Map<String, dynamic> json) =>
      _$SimpleAjaxResponseFromJson(json);

  Map<String, dynamic> toJson() => _$SimpleAjaxResponseToJson(this);
}
