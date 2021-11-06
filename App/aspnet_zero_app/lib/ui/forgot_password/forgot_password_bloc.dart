import 'package:aspnet_zero_app/abp/interfaces/account_service.dart';
import 'package:aspnet_zero_app/abp/models/auth/forgot_password_model.dart';
import 'package:aspnet_zero_app/abp/models/common/ajax_response.dart';
import 'package:aspnet_zero_app/ui/form_submission_status.dart';
import 'package:dio/dio.dart';
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
        accountService.forgotPasswordModel = ForgotPasswordModel(emailAddress: state.email);
        await accountService.forgotPassword();
        yield state.copyWith(formStatus: SubmissionSuccess());

      } on DioError catch (e) {
        var exceptionMessage = '';
        if (e.response != null && e.response!.data is Map<String, dynamic>) {
          var simpleResponse = SimpleAjaxResponse.fromJson(e.response!.data);
          if (simpleResponse.errorInfo != null) {
            exceptionMessage = simpleResponse.errorInfo!.message!;
          }
        } else {
          exceptionMessage = e.toString();
        }
        yield state.copyWith(formStatus: SubmissionFailed(Exception(exceptionMessage)));
      } catch (e) {
        yield state.copyWith(formStatus: SubmissionFailed(e as Exception));
      }
      yield state.copyWith(formStatus: const InitialFormStatus());
    }
  }
}
