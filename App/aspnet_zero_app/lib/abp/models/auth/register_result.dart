import 'package:json_annotation/json_annotation.dart';

part 'register_result.g.dart';

@JsonSerializable(explicitToJson: true)
class RegisterResult {
  bool? isSuccess;
  bool? isEmailConfirmationRequiredForLogin;
  bool? canLogin;
  String? exceptionMessage;

  RegisterResult({this.isSuccess, this.isEmailConfirmationRequiredForLogin, this.canLogin, this.exceptionMessage});

  factory RegisterResult.fromJson(Map<String, dynamic> json) => _$RegisterResultFromJson(json);

  Map<String, dynamic> toJson() => _$RegisterResultToJson(this);
}
