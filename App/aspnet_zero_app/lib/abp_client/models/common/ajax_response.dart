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

@JsonSerializable()
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

@JsonSerializable(explicitToJson: true)
class AjaxResponse<T> {
  String targetUrl = '';
  bool success = false;
  bool unAuthorizedRequest = false;

  @JsonKey(name: "__abp")
  bool abp = false;
  ErrorInfo? errorInfo;
  @_Converter()
  T? result;

  AjaxResponse(
      {String inpTargetUrl = '',
      T? inpResult,
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
  factory AjaxResponse.fromJson(Map<String, dynamic> json) =>
      _$AjaxResponseFromJson(json);

  Map<String, dynamic> toJson() => _$AjaxResponseToJson(this);
}

class _Converter<T> implements JsonConverter<T, Object?> {
  const _Converter();

  @override
  T fromJson(Object? json) {
    // This will only work if `json` is a native JSON type:
    //   num, String, bool, null, etc
    // *and* is assignable to `T`.
    return json as T;
  }

  // This will only work if `object` is a native JSON type:
  //   num, String, bool, null, etc
  // Or if it has a `toJson()` function`.
  @override
  Object? toJson(T object) => object;
}
