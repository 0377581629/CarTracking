import 'package:json_annotation/json_annotation.dart';

part 'reset_password_model.g.dart';

@JsonSerializable(explicitToJson: true)
class ResetPasswordModel {
  int userId;

  String resetCode;

  String password;

  String? returnUrl;

  String? singleSignIn;

  @JsonKey(name: 'c')
  String? cipher;

  ResetPasswordModel(
      {required this.userId, required this.resetCode,required this.password, this
          .returnUrl, this.singleSignIn});

  factory ResetPasswordModel.fromJson(Map<String, dynamic> json) =>
      _$ResetPasswordModelFromJson(json);

  Map<String, dynamic> toJson() => _$ResetPasswordModelToJson(this);
}
