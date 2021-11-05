abstract class RegisterEvent {}

class RegisterUsernameOrEmailChanged extends RegisterEvent {
  final String? usernameOrEmail;
  RegisterUsernameOrEmailChanged({this.usernameOrEmail});
}

class RegisterPasswordChanged extends RegisterEvent {
  final String? password;
  RegisterPasswordChanged({this.password});
}

class RegisterSubmitted extends RegisterEvent {}
