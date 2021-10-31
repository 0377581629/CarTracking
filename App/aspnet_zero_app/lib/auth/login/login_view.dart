import 'package:aspnet_zero_app/abp/abp_base/interfaces/account_service.dart';
import 'package:aspnet_zero_app/auth/form_submission_status.dart';
import 'package:aspnet_zero_app/auth/login/login_event.dart';
import 'package:aspnet_zero_app/auth/register/register_view.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_theme.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_widgets.dart';
import 'package:aspnet_zero_app/helpers/localization_helper.dart';
import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:get_it/get_it.dart';

import 'login_bloc.dart';
import 'login_state.dart';

final lang = LocalizationHelper();

class LoginPage extends StatelessWidget {
  final _formKey = GlobalKey<FormState>();

  LoginPage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        backgroundColor:  FlutterFlowTheme.primaryColor,
        body: BlocProvider(
            create: (context) =>
                LoginBloc(accountService: GetIt.I.get<IAccountService>()),
            child: _loginForm()));
  }

  Widget _loginForm() {
    return BlocListener<LoginBloc, LoginState>(
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
                _signInLoginHeader(),
                _userOrEmailField(),
                _passwordField(),
                _loginButton()
              ],
            ))));
  }

  Widget _appLogo() {
    return  Image.asset(
      'assets/images/trueinvest-logo.png',
      width: 240,
      height: 70,
      fit: BoxFit.cover,
    );
  }

  Widget _signInLoginHeader() {
    return BlocBuilder<LoginBloc, LoginState>(builder: (context, state)
    {
      return Padding(
        padding: EdgeInsetsDirectional.fromSTEB(20, 0, 20, 0),
        child: Row(
          mainAxisSize: MainAxisSize.max,
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            Column(
              mainAxisSize: MainAxisSize.max,
              children: [
                Text(
                  'Sign In',
                  style: FlutterFlowTheme.subtitle1.override(
                    fontFamily: 'Lexend Deca',
                    color: Colors.white,
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                Padding(
                  padding: EdgeInsetsDirectional.fromSTEB(0, 12, 0, 0),
                  child: Container(
                    width: 90,
                    height: 3,
                    decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.circular(2),
                    ),
                  ),
                )
              ],
            ),
            Column(
              mainAxisSize: MainAxisSize.max,
              children: [
                InkWell(
                  onTap: () async {
                    await Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) => RegisterPage(),
                      ),
                    );
                  },
                  child: Text(
                    'Sign Up',
                    style: FlutterFlowTheme.subtitle1.override(
                      fontFamily: 'Lexend Deca',
                      color: Colors.white,
                      fontSize: 18,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                ),
                Padding(
                  padding: EdgeInsetsDirectional.fromSTEB(0, 12, 0, 0),
                  child: Container(
                    width: 90,
                    height: 3,
                    decoration: BoxDecoration(
                      color: Color(0xFF4B39EF),
                      borderRadius: BorderRadius.circular(2),
                    ),
                  ),
                )
              ],
            )
          ],
        ),
      );
    });
  }

  Widget _userOrEmailField() {
    return BlocBuilder<LoginBloc, LoginState>(builder: (context, state) {
      return TextFormField(
          validator: (value) =>
              state.isValidUsernameOrEmail ? null : 'Username is too short',
          onChanged: (value) => context
              .read<LoginBloc>()
              .add(LoginUsernameOrEmailChanged(usernameOrEmail: value)));
    });
  }

  Widget _passwordField() {
    return BlocBuilder<LoginBloc, LoginState>(builder: (context, state) {
      return TextFormField(
          obscureText: true,
          validator: (value) =>
              state.isValidPassword ? null : 'Password is too short',
          onChanged: (value) => context
              .read<LoginBloc>()
              .add(LoginPasswordChanged(password: value)));
    });
  }

  Widget _loginButton() {
    return BlocBuilder<LoginBloc, LoginState>(builder: (context, state) {
      if (state.formStatus is FormSubmitting) {
        return const CircularProgressIndicator();
      }
      return FFButtonWidget(
          onPressed: () {
            if (_formKey.currentState?.validate() ?? false) {
              BlocProvider.of<LoginBloc>(context).add(LoginSubmitted());
            }
          },
          text: lang.get('Login'),
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
