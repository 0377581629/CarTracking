import 'package:aspnet_zero_app/abp/interfaces/account_service.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_theme.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_widgets.dart';
import 'package:aspnet_zero_app/helpers/form_helper.dart';
import 'package:aspnet_zero_app/helpers/localization_helper.dart';
import 'package:aspnet_zero_app/helpers/ui_helper.dart';
import 'package:aspnet_zero_app/ui/register/register_bloc.dart';
import 'package:aspnet_zero_app/ui/register/register_event.dart';
import 'package:aspnet_zero_app/ui/register/register_state.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:get_it/get_it.dart';

import '../form_submission_status.dart';

final lang = LocalizationHelper();
final getIt = GetIt.I;

class RegisterPage extends StatelessWidget {
  final _formKey = FormHelper.getKey('Register');

  RegisterPage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        backgroundColor: FlutterFlowTheme.primaryColor,
        body: BlocProvider(create: (context) => RegisterBloc(accountService: GetIt.I.get<IAccountService>()), child: _registerForm()));
  }

  Widget _registerForm() {
    return BlocListener<RegisterBloc, RegisterState>(
        listener: (context, state) {
          final formStatus = state.formStatus;
          if (formStatus is SubmissionFailed) {
            if (state.exceptionMessage!.isNotEmpty) {
              UIHelper.showSnackbar(context, lang.get('RegisterFailed') + ':' + state.exceptionMessage!, messType: 'error');
            } else {
              UIHelper.showSnackbar(context, lang.get('RegisterFailed'), messType: 'error');
            }
          } else if (formStatus is SubmissionSuccess) {
            if (state.registerResult != null) {
              if (state.registerResult!.isSuccess??=false){
                if (state.registerResult!.canLogin??=false) {
                  UIHelper.showSnackbar(context, lang.get("RegisterSuccessful_CanLoginNow"), messType: 'success');
                  Navigator.pop(context);
                } else if (!(state.registerResult!.isUserActivated??=false)) {
                  UIHelper.showSnackbar(context, lang.get('RegisterSuccessful_WaitAdminActive'), messType: 'info');
                  Navigator.pop(context);
                } else if (state.registerResult!.isEmailConfirmationRequiredForLogin??=false) {
                  UIHelper.showSnackbar(context, lang.get("RegisterSuccessful_EmailConfirmationRequiredForLogin"), messType: 'info');
                  Navigator.pop(context);
                }
              } else {
                if ((state.registerResult!.exceptionMessage??='').isNotEmpty) {
                  UIHelper.showSnackbar(context, lang.get("RegisterFailed") + ':' + state.registerResult!.exceptionMessage!, messType: 'error');
                } else if ((state.registerResult!.exceptionMessage??='').isNotEmpty) {
                  UIHelper.showSnackbar(context, lang.get('RegisterFailed') + ':' + state.registerResult!.exceptionMessage!, messType: 'error');
                }
              }
            } else if (state.exceptionMessage!.isNotEmpty) {
              UIHelper.showSnackbar(context, lang.get('RegisterFailed') + ':' + state.exceptionMessage!, messType: 'error');
            }
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
                        children: [Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 20, 0, 20), child: UIHelper.appLogo()), _signInLoginHeader()],
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
                                    Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: _nameField()),
                                    Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: _surnameField()),
                                    Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: _userNameField()),
                                    Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: _emailField()),
                                    Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: _passwordField()),
                                    Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: _reInputPasswordField()),
                                    Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: _registerButton())
                                  ])))))
                    ],
                  )
                ]))));
  }

  Widget _signInLoginHeader() {
    return BlocBuilder<RegisterBloc, RegisterState>(builder: (context, state) {
      return Row(
        mainAxisSize: MainAxisSize.max,
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Padding(
              padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 10, 0),
              child: Column(
                mainAxisSize: MainAxisSize.max,
                children: [
                  InkWell(
                      onTap: () {
                        Navigator.pop(context);
                      },
                      child: Text(
                        lang.get('Login'),
                        style: FlutterFlowTheme.subtitle1.override(
                          fontFamily: FlutterFlowTheme.defaultFontFamily,
                          color: Colors.white,
                          fontSize: 18,
                          fontWeight: FontWeight.bold,
                        ),
                      )),
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
              )),
          Padding(
              padding: const EdgeInsetsDirectional.fromSTEB(10, 0, 0, 0),
              child: Column(
                mainAxisSize: MainAxisSize.max,
                children: [
                  Text(
                    lang.get('SignUp'),
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
    });
  }

  Widget _nameField() {
    return BlocBuilder<RegisterBloc, RegisterState>(builder: (context, state) {
      return TextFormField(
          validator: (value) => state.isValidName ? null : lang.get('InvalidName'),
          onChanged: (value) => context.read<RegisterBloc>().add(RegisterNameChanged(name: value)),
          decoration: InputDecoration(
            hintText: lang.get('MB_EnterYourName'),
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

  Widget _surnameField() {
    return BlocBuilder<RegisterBloc, RegisterState>(builder: (context, state) {
      return TextFormField(
          validator: (value) => state.isValidSurname ? null : lang.get('Invalid'),
          onChanged: (value) => context.read<RegisterBloc>().add(RegisterSurnameChanged(surname: value)),
          decoration: InputDecoration(
            hintText: lang.get('MB_EnterYourSurname'),
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

  Widget _userNameField() {
    return BlocBuilder<RegisterBloc, RegisterState>(builder: (context, state) {
      return TextFormField(
          validator: (value) => state.isValidUserName ? null : lang.get('Invalid'),
          onChanged: (value) => context.read<RegisterBloc>().add(RegisterUserNameChanged(userName: value)),
          decoration: InputDecoration(
            hintText: lang.get('MB_EnterYourUserName'),
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

  Widget _emailField() {
    return BlocBuilder<RegisterBloc, RegisterState>(builder: (context, state) {
      return TextFormField(
          validator: (value) => state.isValidEmail ? null : lang.get('Invalid'),
          onChanged: (value) => context.read<RegisterBloc>().add(RegisterEmailChanged(email: value)),
          decoration: InputDecoration(
            hintText: lang.get('MB_EnterYourEmail'),
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
    return BlocBuilder<RegisterBloc, RegisterState>(builder: (context, state) {
      return TextFormField(
          obscureText: true,
          validator: (value) => state.isValidPassword ? null : lang.get('Invalid'),
          onChanged: (value) => context.read<RegisterBloc>().add(RegisterPasswordChanged(password: value)),
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

  Widget _reInputPasswordField() {
    return BlocBuilder<RegisterBloc, RegisterState>(builder: (context, state) {
      return TextFormField(
          obscureText: true,
          validator: (value) => state.isValidPassword ? null : lang.get('Invalid'),
          onChanged: (value) => context.read<RegisterBloc>().add(RegisterReInputPasswordChanged(reInputPassword: value)),
          decoration: InputDecoration(
            hintText: lang.get('MB_ReEnterYourPassword'),
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

  Widget _registerButton() {
    return BlocBuilder<RegisterBloc, RegisterState>(builder: (context, state) {
      if (state.formStatus is FormSubmitting) {
        return const CircularProgressIndicator();
      }
      return FFButtonWidget(
          onPressed: () {
            if (_formKey.currentState?.validate() ?? false) {
              BlocProvider.of<RegisterBloc>(context).add(RegisterSubmitted());
            }
          },
          text: lang.get('Submit'),
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
}
