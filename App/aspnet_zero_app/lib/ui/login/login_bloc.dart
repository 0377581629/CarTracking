import 'package:aspnet_zero_app/abp/interfaces/account_service.dart';
import 'package:aspnet_zero_app/abp/models/auth/authenticate_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/login_result.dart';
import 'package:aspnet_zero_app/ui/form_submission_status.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'login_event.dart';
import 'login_state.dart';

class LoginBloc extends Bloc<LoginEvent, LoginState> {
  IAccountService accountService;

  LoginBloc({required this.accountService}) : super(LoginState());

  @override
  Stream<LoginState> mapEventToState(LoginEvent event) async* {
    if (event is LoginUsernameOrEmailChanged) {
      yield state.copyWith(usernameOrEmail: event.usernameOrEmail);
    } else if (event is LoginPasswordChanged) {
      yield state.copyWith(password: event.password);
    } else if (event is LoginSubmitted) {
      yield state.copyWith(formStatus: FormSubmitting());
      try {
        accountService.authenticateModel = AuthenticateModel(
            userNameOrEmailAddress: state.usernameOrEmail,
            password: state.password,
            rememberClient: false);
        var loginResult = await accountService.loginUser();

        if (loginResult.result == LoginResult.success) {
          yield state.copyWith(formStatus: SubmissionSuccess());
        }
        else{
          yield state.copyWith(
              formStatus: SubmissionFailed(Exception('LoginFailed')),
              loginResult: loginResult);
        }
      } catch (e) {
        yield state.copyWith(
            formStatus: SubmissionFailed(Exception(e.toString())));
      }
      yield state.copyWith(formStatus: const InitialFormStatus());
    }
  }
}
