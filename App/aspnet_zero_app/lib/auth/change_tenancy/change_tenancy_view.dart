import 'package:aspnet_zero_app/abp/abp_base/interfaces/account_service.dart';
import 'package:aspnet_zero_app/auth/forgot_password/forgot_password_view.dart';
import 'package:aspnet_zero_app/auth/form_submission_status.dart';
import 'package:aspnet_zero_app/auth/change_tenancy/change_tenancy_event.dart';
import 'package:aspnet_zero_app/auth/register/register_view.dart';
import 'package:aspnet_zero_app/auth/reset_password/reset_password_view.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_theme.dart';
import 'package:aspnet_zero_app/flutter_flow/flutter_flow_widgets.dart';
import 'package:aspnet_zero_app/helpers/form_helper.dart';
import 'package:aspnet_zero_app/helpers/localization_helper.dart';
import 'package:aspnet_zero_app/helpers/ui_element_helper.dart';
import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:get_it/get_it.dart';

import 'change_tenancy_bloc.dart';
import 'change_tenancy_state.dart';

final lang = LocalizationHelper();

class ChangeTenancyPage extends StatelessWidget {
  final _formKey = FormHelper.getKey('ChangeTenancy');

  ChangeTenancyPage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        backgroundColor: FlutterFlowTheme.primaryColor,
        body: BlocProvider(
            create: (context) => ChangeTenancyBloc(accountService: GetIt.I.get<IAccountService>()), child: _changeTenancyForm()));
  }

  Widget _changeTenancyForm() {
    return BlocListener<ChangeTenancyBloc, ChangeTenancyState>(
        listener: (context, state) {
          final formStatus = state.formStatus;
          if (formStatus is SubmissionFailed) {
            if (state.changeTenancyResult!.result == ChangeTenancyResult.needToChangePassword) {
              Navigator.of(context).push(MaterialPageRoute(builder: (BuildContext context) {
                return ResetPasswordPage();
              }));
            }

            if (state.changeTenancyResult!.result == ChangeTenancyResult.needToChangePassword) {}

            if (state.changeTenancyResult!.exceptionMessage!.isNotEmpty) {
              _showSnackbar(context, state.changeTenancyResult!.exceptionMessage!);
            } else {
              _showSnackbar(context, formStatus.exception.toString());
            }
          }
          if (formStatus is SubmissionSuccess) {
            _showSnackbar(context, "ChangeTenancySuccess");
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
                        children: [
                          Padding(padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20), child: UIHelper.appLogo()),
                          _signInChangeTenancyHeader()
                        ],
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
                                    Padding(
                                        padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20),
                                        child: _userOrEmailField()),
                                    Padding(
                                        padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20),
                                        child: _passwordField()),
                                    Padding(
                                        padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 20),
                                        child: _changeTenancyButton())
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
                  ])
                ]))));
  }

  Widget _signInChangeTenancyHeader() {
    return BlocBuilder<ChangeTenancyBloc, ChangeTenancyState>(builder: (context, state) {
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
                    lang.get('ChangeTenancy'),
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
                      );
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
    });
  }

  Widget _userOrEmailField() {
    return BlocBuilder<ChangeTenancyBloc, ChangeTenancyState>(builder: (context, state) {
      return TextFormField(
          validator: (value) => state.isValidUsernameOrEmail ? null : lang.get('InvalidUsernameOrEmail'),
          onChanged: (value) => context.read<ChangeTenancyBloc>().add(ChangeTenancyUsernameOrEmailChanged(usernameOrEmail: value)),
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
    return BlocBuilder<ChangeTenancyBloc, ChangeTenancyState>(builder: (context, state) {
      return TextFormField(
          obscureText: true,
          validator: (value) => state.isValidPassword ? null : lang.get('InvalidPassword'),
          onChanged: (value) => context.read<ChangeTenancyBloc>().add(ChangeTenancyPasswordChanged(password: value)),
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

  Widget _changeTenancyButton() {
    return BlocBuilder<ChangeTenancyBloc, ChangeTenancyState>(builder: (context, state) {
      if (state.formStatus is FormSubmitting) {
        return const CircularProgressIndicator();
      }
      return FFButtonWidget(
          onPressed: () {
            if (_formKey.currentState?.validate() ?? false) {
              BlocProvider.of<ChangeTenancyBloc>(context).add(ChangeTenancySubmitted());
            }
          },
          text: lang.get('ChangeTenancy'),
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
    return BlocBuilder<ChangeTenancyBloc, ChangeTenancyState>(builder: (context, state) {
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

  void _showSnackbar(BuildContext context, String message) {
    final snackBar = SnackBar(
      content: Text(message),
      duration: const Duration(seconds: 3),
    );
    ScaffoldMessenger.of(context).showSnackBar(snackBar);
  }
}
