import 'package:json_annotation/json_annotation.dart';

part 'register_model.g.dart';

@JsonSerializable(explicitToJson: true)
class RegisterModel {
  String? name;

  String? surname;

  String? userName;

  String? emailAddress;

  String? password;

  bool? isExternalLogin;

  String? externalLoginAuthSchema;

  String? returnUrl;

  String? singleSignIn;

  RegisterModel(
      {this.name,
      this.surname,
      this.userName,
      this.emailAddress,
      this.password,
      this.isExternalLogin,
      this.externalLoginAuthSchema,
      this.returnUrl,
      this.singleSignIn});

  factory RegisterModel.fromJson(Map<String, dynamic> json) => _$RegisterModelFromJson(json);

  Map<String, dynamic> toJson() => _$RegisterModelToJson(this);
}
