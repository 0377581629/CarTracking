abstract class ChangeTenancyEvent {}

class ChangeTenancyUsernameOrEmailChanged extends ChangeTenancyEvent {
  final String? usernameOrEmail;
  ChangeTenancyUsernameOrEmailChanged({this.usernameOrEmail});
}

class ChangeTenancyPasswordChanged extends ChangeTenancyEvent {
  final String? password;
  ChangeTenancyPasswordChanged({this.password});
}

class ChangeTenancySubmitted extends ChangeTenancyEvent {}
