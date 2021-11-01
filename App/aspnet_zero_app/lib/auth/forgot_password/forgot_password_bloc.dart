import 'package:aspnet_zero_app/abp/abp_base/interfaces/account_service.dart';
import 'package:aspnet_zero_app/abp/models/auth/forgot_password_model.dart';
import 'package:aspnet_zero_app/auth/form_submission_status.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'forgot_password_event.dart';
import 'forgot_password_state.dart';

class ForgotPasswordBloc extends Bloc<ForgotPasswordEvent, ForgotPasswordState> {
  IAccountService accountService;

  ForgotPasswordBloc({required this.accountService})
      : super(ForgotPasswordState());

  @override
  Stream<ForgotPasswordState> mapEventToState(ForgotPasswordEvent event) async* {
    if (event is ForgotPasswordEmailChanged) {
      yield state.copyWith(email: event.email);
    } else if (event is ForgotPasswordSubmitted) {
      yield state.copyWith(formStatus: FormSubmitting());
      try {
        accountService.forgotPasswordModel = ForgotPasswordModel(email: state.email);
        await accountService.forgotPassword();
        yield state.copyWith(formStatus: SubmissionSuccess());
      } catch (e) {
        yield state.copyWith(
            formStatus: SubmissionFailed(Exception(e.toString())));
      }
    }
  }
}
