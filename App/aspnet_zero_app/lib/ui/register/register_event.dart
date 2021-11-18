abstract class RegisterEvent {}

class RegisterNameChanged extends RegisterEvent {
  final String? name;
  RegisterNameChanged({this.name});
}

class RegisterSurnameChanged extends RegisterEvent {
  final String? surname;
  RegisterSurnameChanged({this.surname});
}

class RegisterUserNameChanged extends RegisterEvent {
  final String? userName;
  RegisterUserNameChanged({this.userName});
}

class RegisterEmailChanged extends RegisterEvent {
  final String? email;
  RegisterEmailChanged({this.email});
}

class RegisterPasswordChanged extends RegisterEvent {
  final String? password;
  RegisterPasswordChanged({this.password});
}

class RegisterReInputPasswordChanged extends RegisterEvent {
  final String? reInputPassword;
  RegisterReInputPasswordChanged({this.reInputPassword});
}

class RegisterSubmitted extends RegisterEvent {}
