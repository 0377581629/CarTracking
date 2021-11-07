import 'dart:io';

import 'package:after_layout/after_layout.dart';
import 'package:aspnet_zero_app/abp/interfaces/account_service.dart';
import 'package:aspnet_zero_app/abp/manager/app_settings_manager.dart';
import 'package:aspnet_zero_app/abp/manager/interfaces/app_settings_manager.dart';
import 'package:aspnet_zero_app/abp/services/account_service.dart';
import 'package:aspnet_zero_app/configuration/abp_config.dart';
import 'package:aspnet_zero_app/error_page/error_page_widget.dart';
import 'package:aspnet_zero_app/helpers/localization_helper.dart';
import 'package:aspnet_zero_app/ui/login/login_view.dart';
import 'package:aspnet_zero_app/ui/onboarding_page/onboarding_page_widget.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:get_it/get_it.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'abp/interfaces/data_storage_service.dart';
import 'abp/interfaces/session_service.dart';
import 'abp/services/data_storage_service.dart';
import 'abp/services/session_service.dart';
import 'abp/services/user_configuration_service.dart';
import 'abp/manager/access_token_manager.dart';
import 'abp/manager/application_context.dart';
import 'abp/manager/interfaces/access_token_manager.dart';
import 'abp/manager/interfaces/application_context.dart';
import 'flutter_flow/flutter_flow_theme.dart';

final lang = LocalizationHelper();
final getIt = GetIt.I;

void main() async {
  HttpOverrides.global = MyHttpOverrides();
  getIt.registerLazySingleton<IDataStorageService>(() => DataStorageService());
  getIt.registerLazySingleton<IApplicationContext>(() => ApplicationContext());
  getIt.registerLazySingleton<IAccessTokenManager>(() => AccessTokenManager());
  getIt.registerLazySingleton<ISessionAppService>(() => SessionAppService());
  getIt.registerLazySingleton<IAppSettingsManager>(() => AppSettingsManager());

  getIt.registerFactory<IAccountService>(() => AccountService());

  runApp(const MyApp());
}

class MyHttpOverrides extends HttpOverrides {
  @override
  HttpClient createHttpClient(SecurityContext? context) {
    return super.createHttpClient(context)
      ..badCertificateCallback =
          (X509Certificate cert, String host, int port) => true;
  }
}

class MyApp extends StatelessWidget {
  const MyApp({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: AbpConfig.appName,
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: const InitializeApp(title: AbpConfig.appName),
    );
  }
}

class InitializeApp extends StatefulWidget {
  const InitializeApp({Key? key, required this.title}) : super(key: key);

  final String title;

  @override
  State<StatefulWidget> createState() => _InitializeApp();
}

class _InitializeApp extends State<InitializeApp>
    with AfterLayoutMixin<InitializeApp> {
  initInfo() async {
    var appContext = getIt.get<IApplicationContext>();
    var dataStorageService = getIt.get<IDataStorageService>();
    var accessTokenManager = getIt.get<IAccessTokenManager>();

    var _userConfigService = UserConfigurationService();

    accessTokenManager.authenticateResult = await dataStorageService.retrieveAuthenticateResult();
    appContext.load(await dataStorageService.retrieveTenantInfo(), await dataStorageService.retrieveLoginInfo());

    if (appContext.configuration == null) {
      try {
        var userConfig = await _userConfigService.getUserConfiguration();
        appContext.configuration = userConfig;

        // Redirect to Intro pages or homePage
        SharedPreferences prefs = await SharedPreferences.getInstance();
        bool _seen = (prefs.getBool('introPageSeen') ?? false);
        if (_seen) {
          Navigator.of(context).pushReplacement(MaterialPageRoute(
              builder: (_) => RepositoryProvider(
                  create: (context) => AccountService(), child: LoginPage())));
        } else {
          await prefs.setBool('introPageSeen', true);
          Navigator.of(context).pushReplacement(
            MaterialPageRoute(builder: (context) => const OnboardingPageWidget()),
          );
        }
      } catch (e) {
        Navigator.of(context).pushReplacement(
          MaterialPageRoute(builder: (context) => ErrorPageWidget
            (errorMessage: 'This app is currently undergoing maintenance. '
              'Please try again later!',)),
        );
      }
    }
  }

  @override
  void afterFirstLayout(BuildContext context) => initInfo();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: FlutterFlowTheme.primaryColor,
      body: SafeArea(
        child: Column(
          mainAxisSize: MainAxisSize.max,
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Row(
              mainAxisSize: MainAxisSize.max,
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Padding(
                  padding: const EdgeInsetsDirectional.fromSTEB(0, 0, 0, 50),
                  child: Image.asset(
                    'assets/images/trueinvest-logo.png',
                    width: 270,
                    height: 100,
                    fit: BoxFit.fitWidth,
                  ),
                )
              ],
            )
          ],
        ),
      ),
    );
  }
}
