abstract class ResetPasswordEvent {}

class ResetPasswordPasswordChanged extends ResetPasswordEvent {
  final String? password;
  ResetPasswordPasswordChanged({this.password});
}

class ResetPasswordReInputPasswordChanged extends ResetPasswordEvent {
  final String? reInputPassword;
  ResetPasswordReInputPasswordChanged({this.reInputPassword});
}

class ResetPasswordSubmitted extends ResetPasswordEvent {}
