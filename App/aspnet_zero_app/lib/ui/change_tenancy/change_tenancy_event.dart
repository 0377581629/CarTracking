abstract class ChangeTenancyEvent {}

class ChangeTenancyTenancyNameChanged extends ChangeTenancyEvent {
  final String? tenancyName;
  ChangeTenancyTenancyNameChanged({this.tenancyName});
}

class ChangeTenancySubmitted extends ChangeTenancyEvent {}
