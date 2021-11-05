import 'package:aspnet_zero_app/abp/interfaces/account_service.dart';
import 'package:aspnet_zero_app/abp/models/auth/authenticate_model.dart';
import 'package:aspnet_zero_app/ui/form_submission_status.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'register_event.dart';
import 'register_state.dart';

class RegisterBloc extends Bloc<RegisterEvent, RegisterState> {
  IAccountService accountService;

  RegisterBloc({required this.accountService}) : super(RegisterState());

  @override
  Stream<RegisterState> mapEventToState(RegisterEvent event) async* {
    if (event is RegisterUsernameOrEmailChanged) {
      yield state.copyWith(usernameOrEmail: event.usernameOrEmail);
    } else if (event is RegisterPasswordChanged) {
      yield state.copyWith(password: event.password);
    } else if (event is RegisterSubmitted) {
      yield state.copyWith(formStatus: FormSubmitting());

      try {
        accountService.authenticateModel = AuthenticateModel(
            userNameOrEmailAddress: state.usernameOrEmail,
            password: state.password,
            rememberClient: false);

        yield state.copyWith(formStatus: SubmissionSuccess());
      } catch (e) {
        yield state.copyWith(
            formStatus: SubmissionFailed(Exception(e.toString())));
      }
    }
  }
}
