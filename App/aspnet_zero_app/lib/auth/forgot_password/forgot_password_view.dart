import 'package:aspnet_zero_app/abp/abp_base/interfaces/account_service.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_theme.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_widgets.dart';
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

class ForgotPasswordPage extends StatefulWidget {
  const ForgotPasswordPage({Key? key}) : super(key: key);

  @override
  State<StatefulWidget> createState() => _ForgotPasswordPageState();
}

class _ForgotPasswordPageState extends State<ForgotPasswordPage> {
  final _formKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        key: _formKey,
        backgroundColor: FlutterFlowTheme.primaryColor,
        body: BlocProvider(create: (context) => ForgotPasswordBloc(accountService: GetIt.I.get<IAccountService>()), child: _resetPasswordForm()));
  }

  Widget _resetPasswordForm() {
    return BlocListener<ForgotPasswordBloc, ForgotPasswordState>(
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
              children: [_appLogo(), _emailField(), _submitButton()],
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

  Widget _emailField() {
    return BlocBuilder<ForgotPasswordBloc, ForgotPasswordState>(builder: (context, state) {
      return TextFormField(
          obscureText: true,
          validator: (value) => state.isValidEmail ? null : lang.get('InvalidEmail'),
          onChanged: (value) => context.read<ForgotPasswordBloc>().add(ForgotPasswordEmailChanged(email: value)));
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

  void _showSnackbar(BuildContext context, String message) {
    final snackBar = SnackBar(
      content: Text(message),
      duration: const Duration(seconds: 3),
    );
    ScaffoldMessenger.of(context).showSnackBar(snackBar);
  }
}
