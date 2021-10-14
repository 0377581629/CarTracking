import 'package:aspnet_zero_app/abp_base/interfaces/account_service.dart';
import 'package:aspnet_zero_app/auth/form_submission_status.dart';
import 'package:aspnet_zero_app/auth/login/login_event.dart';
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

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body: BlocProvider(
            create: (context) =>
                LoginBloc(accountService: GetIt.I.get<IAccountService>()),
            child: _loginForm()));
  }

  Widget _loginForm() {
    return Form(
        key: _formKey,
        child: Center(
            child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            _appLogo(),
            _userOrEmailField(),
            _passwordField(),
            _loginButton()
          ],
        )));
  }

  Widget _appLogo() {
    return Image(image: AssetImage("assets/images/img1.jpg"), width: 300);
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
      return state.formStatus is FormSubmitting
          ? const CircularProgressIndicator()
          : ElevatedButton(
              onPressed: () {
                if (_formKey.currentState!.validate()) {
                  context.read<LoginBloc>().add(LoginSubmitted());
                }
              },
              child: Text(lang.get('Login')));
    });
  }
}
