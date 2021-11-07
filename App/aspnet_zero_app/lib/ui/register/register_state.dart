import 'package:aspnet_zero_app/abp/manager/interfaces/app_settings_manager.dart';
import 'package:aspnet_zero_app/abp/models/auth/register_result.dart';
import 'package:aspnet_zero_app/ui/form_submission_status.dart';
import 'package:email_validator/email_validator.dart';
import 'package:get_it/get_it.dart';

class RegisterState {
  final IAppSettingsManager appSettingsManager = GetIt.I.get<IAppSettingsManager>();

  final String name;

  bool get isValidName => name.length > 3;

  final String surname;

  bool get isValidSurname => surname.length > 3;

  final String userName;

  bool get isValidUserName => userName.length > 3;

  final String email;

  bool get isValidEmail => EmailValidator.validate(email);

  final String password;

  bool get isValidPassword => password.length >= appSettingsManager.getNumberSetting('Abp.Zero.UserManagement.PasswordComplexity.RequiredLength', 3);

  final String reInputPassword;

  bool get isValidReInputPassword => reInputPassword == password;

  final RegisterResult? registerResult;

  final FormSubmissionStatus formStatus;

  final String? exceptionMessage;

  RegisterState(
      {this.name = '',
      this.surname = '',
      this.userName = '',
      this.email = '',
      this.password = '',
      this.reInputPassword = '',
      this.registerResult,
      this.formStatus = const InitialFormStatus(),
      this.exceptionMessage = ''});

  RegisterState copyWith(
      {String? name,
      String? surname,
      String? userName,
      String? email,
      String? password,
      String? reInputPassword,
      RegisterResult? registerResult,
      FormSubmissionStatus? formStatus,
      String? exceptionMessage}) {
    return RegisterState(
        name: name ?? this.name,
        surname: surname ?? this.surname,
        userName: userName ?? this.userName,
        email: email ?? this.email,
        password: password ?? this.password,
        reInputPassword: reInputPassword ?? this.reInputPassword,
        registerResult: registerResult ?? this.registerResult,
        formStatus: formStatus ?? this.formStatus,
        exceptionMessage: exceptionMessage ?? this.exceptionMessage);
  }
}
