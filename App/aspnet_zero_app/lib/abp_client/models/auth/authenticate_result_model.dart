import 'package:json_annotation/json_annotation.dart';
part 'authenticate_result_model.g.dart';

@JsonSerializable()
class AbpAuthenticateResultModel {
  String? accessToken;

  String? encryptedAccessToken;

  String? refreshToken;

  int? expireInSeconds;

  bool? shouldResetPassword;

  String? passwordResetCode;

  int? userId;

  bool? requiresTwoFactorVerification;

  List<String>? twoFactorAuthProviders;

  String? twoFactorRememberClientToken;

  String? returnUrl;

  DateTime? refreshTokenExpireDate;

  AbpAuthenticateResultModel(
      {this.accessToken,
      this.encryptedAccessToken,
      this.expireInSeconds,
      this.shouldResetPassword,
      this.passwordResetCode,
      this.userId,
      this.requiresTwoFactorVerification,
      this.twoFactorAuthProviders,
      this.twoFactorRememberClientToken,
      this.returnUrl,
      this.refreshTokenExpireDate});

  factory AbpAuthenticateResultModel.fromJson(Map<String, dynamic> json) =>
      _$AbpAuthenticateResultModelFromJson(json);

  Map<String, dynamic> toJson() => _$AbpAuthenticateResultModelToJson(this);

  static AbpAuthenticateResultModel fromJsonModel(Map<String, dynamic> json) =>
      AbpAuthenticateResultModel.fromJson(json);
}
