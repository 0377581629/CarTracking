import 'package:aspnet_zero_app/abp/interfaces/account_service.dart';
import 'package:aspnet_zero_app/abp/models/auth/reset_password_model.dart';
import 'package:aspnet_zero_app/ui/form_submission_status.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'reset_password_event.dart';
import 'reset_password_state.dart';

class ResetPasswordBloc extends Bloc<ResetPasswordEvent, ResetPasswordState> {
  IAccountService accountService;

  ResetPasswordBloc({required this.accountService})
      : super(ResetPasswordState());

  @override
  Stream<ResetPasswordState> mapEventToState(ResetPasswordEvent event) async* {
    if (event is ResetPasswordPasswordChanged) {
      yield state.copyWith(password: event.password);
    } else if (event is ResetPasswordReInputPasswordChanged) {
      yield state.copyWith(reInputPassword: event.reInputPassword);
    } else if (event is ResetPasswordSubmitted) {
      yield state.copyWith(formStatus: FormSubmitting());
      try {
        accountService.resetPasswordModel = ResetPasswordModel(
            userId: accountService.authenticateResultModel!.userId!,
            resetCode: accountService.authenticateResultModel!
                .passwordResetCode!,
            password: state.password);
        await accountService.resetPassword();
        yield state.copyWith(formStatus: SubmissionSuccess());
      } catch (e) {
        yield state.copyWith(
            formStatus: SubmissionFailed(Exception(e.toString())));
      }
    }
  }
}
