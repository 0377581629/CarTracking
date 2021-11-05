import 'package:aspnet_zero_app/abp/models/auth/login_result.dart';
import 'package:aspnet_zero_app/ui/form_submission_status.dart';

class LoginState {
  final String usernameOrEmail;

  bool get isValidUsernameOrEmail => usernameOrEmail.length > 3;

  final String password;

  bool get isValidPassword => password.length > 3;

  final FormSubmissionStatus formStatus;

  LoginResultOutput? loginResult;

  LoginState({this.usernameOrEmail = '', this.password = '', this.formStatus = const InitialFormStatus(), this.loginResult});

  LoginState copyWith({String? usernameOrEmail, String? password, FormSubmissionStatus? formStatus, LoginResultOutput? loginResult}) {
    return LoginState(
        usernameOrEmail: usernameOrEmail ?? this.usernameOrEmail,
        password: password ?? this.password,
        formStatus: formStatus ?? this.formStatus,
        loginResult: loginResult ?? this.loginResult);
  }
}
