import 'package:json_annotation/json_annotation.dart';
part 'authenticate_result_model.g.dart';

@JsonSerializable(explicitToJson: true)
class AuthenticateResultModel {
  String? accessToken;

  String? encryptedAccessToken;

  String? refreshToken;

  int? expireInSeconds;

  DateTime? tokenExpireDate;

  bool? shouldResetPassword;

  String? passwordResetCode;

  int? userId;

  bool? requiresTwoFactorVerification;

  List<String>? twoFactorAuthProviders;

  String? twoFactorRememberClientToken;

  String? returnUrl;

  int? refreshTokenExpireInSeconds;

  DateTime? refreshTokenExpireDate;

  AuthenticateResultModel(
      {this.accessToken,
      this.tokenExpireDate,
      this.encryptedAccessToken,
      this.expireInSeconds,
      this.shouldResetPassword,
      this.passwordResetCode,
      this.userId,
      this.requiresTwoFactorVerification,
      this.twoFactorAuthProviders,
      this.twoFactorRememberClientToken,
      this.returnUrl,
      this.refreshTokenExpireInSeconds,
      this.refreshTokenExpireDate});

  factory AuthenticateResultModel.fromJson(Map<String, dynamic> json) =>
      _$AuthenticateResultModelFromJson(json);

  Map<String, dynamic> toJson() => _$AuthenticateResultModelToJson(this);

  static AuthenticateResultModel fromJsonModel(Map<String, dynamic> json) =>
      AuthenticateResultModel.fromJson(json);
}
