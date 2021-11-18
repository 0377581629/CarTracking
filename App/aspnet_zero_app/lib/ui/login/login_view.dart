import 'dart:async';

import 'package:aspnet_zero_app/abp/consts/app_settings.dart';
import 'package:aspnet_zero_app/abp/interfaces/account_service.dart';
import 'package:aspnet_zero_app/abp/manager/interfaces/app_settings_manager.dart';
import 'package:aspnet_zero_app/abp/models/auth/login_result.dart';
import 'package:aspnet_zero_app/ui/change_tenancy/change_tenancy_view.dart';
import 'package:aspnet_zero_app/ui/forgot_password/forgot_password_view.dart';
import 'package:aspnet_zero_app/ui/form_submission_status.dart';
import 'package:aspnet_zero_app/ui/login/login_event.dart';
import 'package:aspnet_zero_app/ui/register/register_view.dart';
import 'package:aspnet_zero_app/ui/reset_password/reset_password_view.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_theme.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_widgets.dart';
import 'package:aspnet_zero_app/helpers/form_helper.dart';
import 'package:aspnet_zero_app/helpers/localization_helper.dart';
import 'package:aspnet_zero_app/helpers/ui_helper.dart';
import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:get_it/get_it.dart';

import 'login_bloc.dart';
import 'login_state.dart';

final lang = LocalizationHelper();
final getIt = GetIt.I;

class LoginPage extends StatefulWidget {
  const LoginPage({Key? key}) : super(key: key);

  @override
  _LoginPageState createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  final _formKey = FormHelper.getKey('Login');
  final _appSettingsManager = getIt.get<IAppSettingsManager>();

  FutureOr onGoBack(dynamic value) {
    setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        backgroundColor: FlutterFlowTheme.primaryColor,
        body: BlocProvider(create: (context) => LoginBloc(accountService: GetIt.I.get<IAccountService>()), child: _loginForm()));
  }

  Widget _loginForm() {
    return BlocListener<LoginBloc, LoginState>(
        listener: (context, state) {
          final formStatus = state.formStatus;
          if (formStatus is SubmissionFailed) {
            if (state.loginResult!.result == LoginResult.needToChangePassword) {
              Navigator.of(context).push(MaterialPageRoute(builder: (BuildContext context) {
                return ResetPasswordPage();
              }));
            }

            if (state.loginResult!.result == LoginResult.needToChangePassword) {}

            if (state.loginResult!.exceptionMessage!.isNotEmpty) {
              UIHelper.showSnackbar(context, state.loginResult!.exceptionMessage!, messType: 'error');
            } else {
              UIHelper.showSnackbar(context, formStatus.exception.toString(), messType: 'error');
            }
          }
          if (formStatus is SubmissionSuccess) {
            UIHelper.showSnackbar(context, "LoginSuccess", messType: 'success');
          }
        },
        child: SingleChildScrollView(
            child: ConstrainedBox(
                constraints: const BoxConstraints(
                  minHeight: 700,
                ),
                child: Column(mainAxisSize: MainAxisSize.max, mainAxisAlignment: MainAxisAlignment.center, children: [
                  Row(
                    mainAxisSize: MainAxisSize.max,
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Column(
                        mainAxisSize: MainAxisSize.max,
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: UIHelper.appLogo()), _signInLoginHeader()],
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
                                    Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: _userOrEmailField()),
                                    Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: _passwordField()),
                                    Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: _loginButton())
                                  ])))))
                    ],
                  ),
                  Row(mainAxisSize: MainAxisSize.max, mainAxisAlignment: MainAxisAlignment.center, children: [
                    Container(
                        constraints: const BoxConstraints(
                          maxWidth: 350,
                        ),
                        decoration: const BoxDecoration(
                          color: FlutterFlowTheme.primaryColor,
                        ),
                        child: Center(
                            child: Column(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: [_forgotPasswordButton()],
                        )))
                  ]),
                  Padding(
                    padding: const EdgeInsetsDirectional.fromSTEB(0, 10, 0, 10),
                    child: CurrentTenancy((context) => {
                          Navigator.of(context).push(MaterialPageRoute(builder: (BuildContext context) {
                            return ChangeTenancyPage();
                          })).then(onGoBack)
                        }),
                  )
                ]))));
  }

  Widget _signInLoginHeader() {
    var allowSelfRegistration = _appSettingsManager.confirmSetting(UserManagement.AllowSelfRegistration, true);

    return BlocBuilder<LoginBloc, LoginState>(builder: (context, state) {
      if (allowSelfRegistration) {
        return Row(
          mainAxisSize: MainAxisSize.max,
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Padding(
                padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 10, 0),
                child: Column(
                  mainAxisSize: MainAxisSize.max,
                  children: [
                    Text(
                      lang.get('Login'),
                      style: FlutterFlowTheme.subtitle1.override(
                        fontFamily: FlutterFlowTheme.defaultFontFamily,
                        color: Colors.white,
                        fontSize: 18,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsetsDirectional.fromSTEB(0, 12, 0, 0),
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
                )),
            Padding(
                padding: const EdgeInsetsDirectional.fromSTEB(10, 0, 0, 0),
                child: Column(
                  mainAxisSize: MainAxisSize.max,
                  children: [
                    InkWell(
                      onTap: () async {
                        await Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (context) => RegisterPage(),
                          ),
                        ).then(onGoBack);
                      },
                      child: Text(
                        lang.get('SignUp'),
                        style: FlutterFlowTheme.subtitle1.override(
                          fontFamily: FlutterFlowTheme.defaultFontFamily,
                          color: Colors.white,
                          fontSize: 18,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsetsDirectional.fromSTEB(0, 12, 0, 0),
                      child: Container(
                        width: 90,
                        height: 3,
                        decoration: BoxDecoration(
                          color: const Color(0xFF4B39EF),
                          borderRadius: BorderRadius.circular(2),
                        ),
                      ),
                    )
                  ],
                ))
          ],
        );
      } else {
        return Row(
          mainAxisSize: MainAxisSize.max,
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Padding(
                padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 0),
                child: Column(
                  mainAxisSize: MainAxisSize.max,
                  children: [
                    Text(
                      lang.get('Login'),
                      style: FlutterFlowTheme.subtitle1.override(
                        fontFamily: FlutterFlowTheme.defaultFontFamily,
                        color: Colors.white,
                        fontSize: 18,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsetsDirectional.fromSTEB(0, 12, 0, 0),
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
                ))
          ],
        );
      }
    });
  }

  Widget _userOrEmailField() {
    return BlocBuilder<LoginBloc, LoginState>(builder: (context, state) {
      return TextFormField(
          validator: (value) => state.isValidUsernameOrEmail ? null : lang.get('InvalidUsernameOrEmail'),
          onChanged: (value) => context.read<LoginBloc>().add(LoginUsernameOrEmailChanged(usernameOrEmail: value)),
          decoration: InputDecoration(
            hintText: lang.get('MB_EnterYourUserNameOrEmail'),
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

  Widget _passwordField() {
    return BlocBuilder<LoginBloc, LoginState>(builder: (context, state) {
      return TextFormField(
          obscureText: true,
          validator: (value) => state.isValidPassword ? null : lang.get('InvalidPassword'),
          onChanged: (value) => context.read<LoginBloc>().add(LoginPasswordChanged(password: value)),
          decoration: InputDecoration(
            hintText: lang.get('MB_EnterYourPassword'),
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
            width: 220,
            height: 50,
            color: FlutterFlowTheme.secondaryColor,
            textStyle: FlutterFlowTheme.subtitle2.override(
              fontFamily: FlutterFlowTheme.defaultFontFamily,
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

  Widget _forgotPasswordButton() {
    return BlocBuilder<LoginBloc, LoginState>(builder: (context, state) {
      if (state.formStatus is FormSubmitting) {
        return Row();
      }
      return FFButtonWidget(
          onPressed: () {
            Navigator.of(context).push(MaterialPageRoute(builder: (BuildContext context) {
              return ForgotPasswordPage();
            }));
          },
          text: lang.get('ForgotPassword'),
          options: FFButtonOptions(
            width: 180,
            height: 40,
            color: FlutterFlowTheme.secondaryColor,
            textStyle: FlutterFlowTheme.subtitle2.override(
              fontFamily: FlutterFlowTheme.defaultFontFamily,
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
