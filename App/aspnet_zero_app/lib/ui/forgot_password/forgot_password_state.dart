import 'package:aspnet_zero_app/ui/form_submission_status.dart';

class ForgotPasswordState {
  final String email;

  bool get isValidEmail => email.length > 3;

  final FormSubmissionStatus formStatus;

  ForgotPasswordState({this.email = '', this.formStatus = const InitialFormStatus()});

  ForgotPasswordState copyWith({String? email, FormSubmissionStatus? formStatus}) {
    return ForgotPasswordState(email: email ?? this.email, formStatus: formStatus ?? this.formStatus);
  }
}
