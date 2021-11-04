import 'package:aspnet_zero_app/abp/abp_base/interfaces/account_service.dart';
import 'package:aspnet_zero_app/abp/models/auth/authenticate_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/changeTenancy_result.dart';
import 'package:aspnet_zero_app/auth/form_submission_status.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'change_tenancy_event.dart';
import 'change_tenancy_state.dart';

class ChangeTenancyBloc extends Bloc<ChangeTenancyEvent, ChangeTenancyState> {
  IAccountService accountService;

  ChangeTenancyBloc({required this.accountService}) : super(ChangeTenancyState());

  @override
  Stream<ChangeTenancyState> mapEventToState(ChangeTenancyEvent event) async* {
    if (event is ChangeTenancyUsernameOrEmailChanged) {
      yield state.copyWith(usernameOrEmail: event.usernameOrEmail);
    } else if (event is ChangeTenancyPasswordChanged) {
      yield state.copyWith(password: event.password);
    } else if (event is ChangeTenancySubmitted) {
      yield state.copyWith(formStatus: FormSubmitting());
      try {
        accountService.authenticateModel = AuthenticateModel(
            userNameOrEmailAddress: state.usernameOrEmail,
            password: state.password,
            rememberClient: false);
        var changeTenancyResult = await accountService.changeTenancyUser();

        if (changeTenancyResult.result == ChangeTenancyResult.success) {
          yield state.copyWith(formStatus: SubmissionSuccess());
        }
        else{
          yield state.copyWith(
              formStatus: SubmissionFailed(Exception('ChangeTenancyFailed')),
              changeTenancyResult: changeTenancyResult);
        }

      } catch (e) {
        yield state.copyWith(
            formStatus: SubmissionFailed(Exception(e.toString())));
      }
    }
  }
}
