abstract class LoginEvent {}

class LoginUsernameOrEmailChanged extends LoginEvent {
  final String? usernameOrEmail;
  LoginUsernameOrEmailChanged({this.usernameOrEmail});
}

class LoginPasswordChanged extends LoginEvent {
  final String? password;
  LoginPasswordChanged({this.password});
}

class LoginSubmitted extends LoginEvent {}
