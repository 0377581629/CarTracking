import 'package:aspnet_zero_app/ui/form_submission_status.dart';

class ResetPasswordState {
  final String password;

  final String reInputPassword;

  bool get isValidPassword =>
      password.length > 3 &&
      reInputPassword.length > 3 &&
      password == reInputPassword;

  final FormSubmissionStatus formStatus;

  ResetPasswordState(
      {this.password = '',
      this.reInputPassword = '',
      this.formStatus = const InitialFormStatus()});

  ResetPasswordState copyWith(
      {String? password,
      String? reInputPassword,
      FormSubmissionStatus? formStatus}) {
    return ResetPasswordState(
        password: password ?? this.password,
        reInputPassword: reInputPassword ?? this.reInputPassword,
        formStatus: formStatus ?? this.formStatus);
  }
}
