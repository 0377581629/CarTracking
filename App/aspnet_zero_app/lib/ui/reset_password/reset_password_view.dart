import 'package:aspnet_zero_app/abp/interfaces/account_service.dart';
import 'package:aspnet_zero_app/ui/reset_password/reset_password_bloc.dart';
import 'package:aspnet_zero_app/ui/reset_password/reset_password_event.dart';
import 'package:aspnet_zero_app/ui/reset_password/reset_password_state.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_theme.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_widgets.dart';
import 'package:aspnet_zero_app/helpers/localization_helper.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:get_it/get_it.dart';

import '../form_submission_status.dart';

final lang = LocalizationHelper();

class ResetPasswordPage extends StatelessWidget {
  final _formKey = GlobalKey<FormState>();

  ResetPasswordPage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        key: _formKey,
        backgroundColor: FlutterFlowTheme.primaryColor,
        body: BlocProvider(
            create: (context) => ResetPasswordBloc(
                accountService: GetIt.I.get<IAccountService>()),
            child: _resetPasswordForm()));
  }

  Widget _resetPasswordForm() {
    return BlocListener<ResetPasswordBloc, ResetPasswordState>(
        listener: (context, state) {
          final formStatus = state.formStatus;
          if (formStatus is SubmissionFailed) {
            _showSnackbar(context, formStatus.exception.toString());
          }
          if (formStatus is SubmissionSuccess) {
            _showSnackbar(context, "LoginSuccess");
          }
        },
        child: Form(
            key: _formKey,
            child: Center(
                child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                _appLogo(),
                _passwordField(),
                _reInputPasswordField(),
                _submitButton()
              ],
            ))));
  }

  Widget _appLogo() {
    return Image.asset(
      'assets/images/trueinvest-logo.png',
      width: 240,
      height: 70,
      fit: BoxFit.cover,
    );
  }

  Widget _passwordField() {
    return BlocBuilder<ResetPasswordBloc, ResetPasswordState>(
        builder: (context, state) {
      return TextFormField(
          obscureText: true,
          validator: (value) =>
              state.isValidPassword ? null : 'Password is too short',
          onChanged: (value) => context
              .read<ResetPasswordBloc>()
              .add(ResetPasswordPasswordChanged(password: value)));
    });
  }

  Widget _reInputPasswordField() {
    return BlocBuilder<ResetPasswordBloc, ResetPasswordState>(
        builder: (context, state) {
      return TextFormField(
          obscureText: true,
          validator: (value) =>
              state.isValidPassword ? null : 'Password is too short',
          onChanged: (value) => context.read<ResetPasswordBloc>().add(
              ResetPasswordReInputPasswordChanged(reInputPassword: value)));
    });
  }

  Widget _submitButton() {
    return BlocBuilder<ResetPasswordBloc, ResetPasswordState>(
        builder: (context, state) {
      if (state.formStatus is FormSubmitting) {
        return const CircularProgressIndicator();
      }
      return FFButtonWidget(
          onPressed: () {
            if (_formKey.currentState?.validate() ?? false) {
              BlocProvider.of<ResetPasswordBloc>(context)
                  .add(ResetPasswordSubmitted());
            }
          },
          text: lang.get('Submit'),
          options: FFButtonOptions(
            width: 230,
            height: 60,
            color: FlutterFlowTheme.secondaryColor,
            textStyle: FlutterFlowTheme.subtitle2.override(
              fontFamily: 'Lexend Deca',
              color: FlutterFlowTheme.primaryColor,
              fontSize: 16,
              fontWeight: FontWeight.w500,
            ),
            elevation: 3,
            borderSide: const BorderSide(
              color: Colors.transparent,
              width: 1,
            ),
            borderRadius: 8,
          ));
    });
  }

  void _showSnackbar(BuildContext context, String message) {
    final snackBar = SnackBar(
      content: Text(message),
      duration: const Duration(seconds: 3),
    );
    ScaffoldMessenger.of(context).showSnackBar(snackBar);
  }
}
