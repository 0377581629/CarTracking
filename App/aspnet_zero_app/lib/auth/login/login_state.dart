import 'package:aspnet_zero_app/auth/form_submission_status.dart';

class LoginState {
  final String usernameOrEmail;

  bool get isValidUsernameOrEmail => usernameOrEmail.length > 3;

  final String password;

  bool get isValidPassword => password.length > 3;

  final FormSubmissionStatus formStatus;

  LoginState(
      {this.usernameOrEmail = '',
      this.password = '',
      this.formStatus = const InitialFormStatus()});

  LoginState copyWith(
      {String? usernameOrEmail,
      String? password,
      FormSubmissionStatus? formStatus}) {
    return LoginState(
        usernameOrEmail: usernameOrEmail ?? this.usernameOrEmail,
        password: password ?? this.password,
        formStatus: formStatus ?? this.formStatus);
  }
}
