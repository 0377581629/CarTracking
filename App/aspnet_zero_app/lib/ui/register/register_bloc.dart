import 'package:aspnet_zero_app/abp/interfaces/account_service.dart';
import 'package:aspnet_zero_app/abp/models/auth/register_model.dart';
import 'package:aspnet_zero_app/abp/models/common/ajax_response.dart';
import 'package:aspnet_zero_app/ui/form_submission_status.dart';
import 'package:dio/dio.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'register_event.dart';
import 'register_state.dart';

class RegisterBloc extends Bloc<RegisterEvent, RegisterState> {
  IAccountService accountService;

  RegisterBloc({required this.accountService}) : super(RegisterState());

  @override
  Stream<RegisterState> mapEventToState(RegisterEvent event) async* {
    if (event is RegisterNameChanged) {
      yield state.copyWith(name: event.name);
    } else if (event is RegisterSurnameChanged) {
      yield state.copyWith(surname: event.surname);
    }else if (event is RegisterUserNameChanged) {
      yield state.copyWith(userName: event.userName);
    }else if (event is RegisterEmailChanged) {
      yield state.copyWith(email: event.email);
    }else if (event is RegisterPasswordChanged) {
      yield state.copyWith(password: event.password);
    }else if (event is RegisterReInputPasswordChanged) {
      yield state.copyWith(reInputPassword: event.reInputPassword);
    } else if (event is RegisterSubmitted) {
      yield state.copyWith(formStatus: FormSubmitting());

      try {
        accountService.registerModel = RegisterModel(
            name: state.name,
            surname: state.surname,
            userName: state.userName,
            emailAddress: state.email,
            password: state.password);
        var registerRes = await accountService.registerUser();
        yield state.copyWith(formStatus: SubmissionSuccess(), registerResult: registerRes);
        yield state.copyWith(formStatus: const InitialFormStatus());
      } on DioError catch(e) {
        if (e.response != null && e.response!.data is Map<String, dynamic>) {
          var simpleResponse = SimpleAjaxResponse.fromJson(e.response!.data);
          if (simpleResponse.errorInfo != null) {
            yield state.copyWith(
                formStatus: SubmissionFailed(e),
                exceptionMessage: simpleResponse.errorInfo!.message);
            return;
          }
        }
        yield state.copyWith(
            formStatus: SubmissionFailed(e));
      } catch (e) {
        yield state.copyWith(
            formStatus: SubmissionFailed(Exception(e.toString())));
      }
    }
  }
}
