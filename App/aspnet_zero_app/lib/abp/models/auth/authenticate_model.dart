import 'package:json_annotation/json_annotation.dart';
part 'authenticate_model.g.dart';

@JsonSerializable(explicitToJson: true)
class AuthenticateModel {
  String? userNameOrEmailAddress;

  String? password;

  bool? rememberClient;

  String? twoFactorVerificationCode;

  String? twoFactorRememberClientToken;

  bool? singleSignIn;

  String? returnUrl;

  bool get isTwoFactorVerification =>
      twoFactorVerificationCode != null &&
      twoFactorVerificationCode!.isNotEmpty;

  AuthenticateModel(
      {this.userNameOrEmailAddress, this.password, this.rememberClient});

  factory AuthenticateModel.fromJson(Map<String, dynamic> json) =>
      _$AuthenticateModelFromJson(json);

  Map<String, dynamic> toJson() => _$AuthenticateModelToJson(this);
}
