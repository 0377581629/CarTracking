import 'package:aspnet_zero_app/abp/interfaces/account_service.dart';
import 'package:aspnet_zero_app/helpers/ui_helper.dart';
import 'package:aspnet_zero_app/ui/login/login_view.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_theme.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_widgets.dart';
import 'package:aspnet_zero_app/helpers/form_helper.dart';
import 'package:aspnet_zero_app/helpers/localization_helper.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:get_it/get_it.dart';

import '../form_submission_status.dart';
import 'forgot_password_bloc.dart';
import 'forgot_password_event.dart';
import 'forgot_password_state.dart';

final lang = LocalizationHelper();

class ForgotPasswordPage extends StatelessWidget {
  final _formKey = FormHelper.getKey('ForgotPassword');

  ForgotPasswordPage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        backgroundColor: FlutterFlowTheme.primaryColor,
        body: BlocProvider(create: (context) => ForgotPasswordBloc(accountService: GetIt.I.get<IAccountService>()), child: _resetPasswordForm()));
  }

  Widget _resetPasswordForm() {
    return BlocListener<ForgotPasswordBloc, ForgotPasswordState>(
        listener: (context, state) {
          final formStatus = state.formStatus;
          if (formStatus is SubmissionFailed) {
            UIHelper.showSnackbar(context, formStatus.exception.toString(), messType: 'error');
          }
          if (formStatus is SubmissionSuccess) {
            UIHelper.showSnackbar(context, lang.get("PasswordResetMailSentMessage"), messType: 'success');
            Navigator.pop(context);
          }
        },
        child: Center(
            child: Column(
          mainAxisSize: MainAxisSize.max,
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Row(
              mainAxisSize: MainAxisSize.max,
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Column(
                  mainAxisSize: MainAxisSize.max,
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: UIHelper.appLogo())],
                )
              ],
            ),
            Row(
              mainAxisSize: MainAxisSize.max,
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Padding(
                    padding: const EdgeInsetsDirectional.fromSTEB(0, 20, 0, 0),
                    child: Container(
                        constraints: const BoxConstraints(
                          maxWidth: 350,
                        ),
                        decoration: const BoxDecoration(
                          color: FlutterFlowTheme.primaryColor,
                        ),
                        child: Form(
                            key: _formKey,
                            child: Center(
                                child: Column(mainAxisAlignment: MainAxisAlignment.center, children: [
                              Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: _emailField()),
                              Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: _submitButton())
                            ])))))
              ],
            )
          ],
        )));
  }

  Widget _emailField() {
    return BlocBuilder<ForgotPasswordBloc, ForgotPasswordState>(builder: (context, state) {
      return TextFormField(
          obscureText: false,
          validator: (value) => state.isValidEmail ? null : lang.get('InvalidEmail'),
          onChanged: (value) => context.read<ForgotPasswordBloc>().add(ForgotPasswordEmailChanged(email: value)),
          decoration: InputDecoration(
            hintText: lang.get('EnterYourEmail'),
            hintStyle: FlutterFlowTheme.bodyText1.override(
              fontFamily: FlutterFlowTheme.defaultFontFamily,
              color: FlutterFlowTheme.primaryColor,
              fontSize: 14,
              fontWeight: FontWeight.normal,
            ),
            enabledBorder: OutlineInputBorder(
              borderSide: const BorderSide(
                color: Color(0x00000000),
                width: 1,
              ),
              borderRadius: BorderRadius.circular(8),
            ),
            focusedBorder: OutlineInputBorder(
              borderSide: const BorderSide(
                color: Color(0x00000000),
                width: 1,
              ),
              borderRadius: BorderRadius.circular(8),
            ),
            filled: true,
            fillColor: FlutterFlowTheme.tertiaryColor,
            contentPadding: FlutterFlowTheme.formFieldContentPadding,
          ),
          style: FlutterFlowTheme.bodyText1.override(
            fontFamily: FlutterFlowTheme.defaultFontFamily,
            color: FlutterFlowTheme.primaryColor,
            fontSize: 14,
            fontWeight: FontWeight.normal,
          ));
    });
  }

  Widget _submitButton() {
    return BlocBuilder<ForgotPasswordBloc, ForgotPasswordState>(builder: (context, state) {
      if (state.formStatus is FormSubmitting) {
        return const CircularProgressIndicator();
      }
      return FFButtonWidget(
          onPressed: () {
            if (_formKey.currentState?.validate() ?? false) {
              BlocProvider.of<ForgotPasswordBloc>(context).add(ForgotPasswordSubmitted());
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
}
