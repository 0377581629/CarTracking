import 'dart:io';

import 'package:after_layout/after_layout.dart';
import 'package:aspnet_zero_app/configuration/abp_config.dart';
import 'package:aspnet_zero_app/helpers/localization_helper.dart';
import 'package:aspnet_zero_app/ui/intro.dart';
import 'package:aspnet_zero_app/ui/login.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'abp_base/interfaces/data_storage_service.dart';
import 'abp_base/interfaces/session_service.dart';
import 'abp_base/services/data_storage_service.dart';
import 'abp_base/services/session_service.dart';
import 'abp_base/services/user_configuration_service.dart';
import 'abp_client/access_token_manager.dart';
import 'abp_client/application_context.dart';
import 'abp_client/interfaces/access_token_manager.dart';
import 'abp_client/interfaces/application_context.dart';
import 'abp_client/interfaces/multi_tenancy_config.dart';
import 'abp_client/models/multi_tenancy/multi_tenancy_config.dart';

final lang = LocalizationHelper();
final getIt = GetIt.I;
void main() async {
  HttpOverrides.global = MyHttpOverrides();
  getIt.registerLazySingleton<IDataStorageService>(() => DataStorageService());
  getIt.registerLazySingleton<IApplicationContext>(() => ApplicationContext());
  getIt.registerLazySingleton<IAccessTokenManager>(() => AccessTokenManager());
  getIt.registerLazySingleton<IMultiTenancyConfig>(() => MultiTenancyConfig());
  getIt.registerLazySingleton<ISessionAppService>(() => SessionAppService());
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
    var dataStorageService = getIt.get<IDataStorageService>();
    var accessTokenManager = getIt.get<IAccessTokenManager>();
    var applicationContext = getIt.get<IApplicationContext>();
    var _userConfigService = UserConfigurationService();
    accessTokenManager.authenticateResult =
        await dataStorageService.retrieveAuthenticateResult();
    applicationContext.load(await dataStorageService.retrieveTenantInfo(),
        await dataStorageService.retrieveLoginInfo());
    if (applicationContext.configuration == null) {
      var userConfiguartion = await _userConfigService.getUserConfiguration();
      applicationContext.configuration = userConfiguartion;
    }
    // Redirect to Intro pages or homePage
    SharedPreferences prefs = await SharedPreferences.getInstance();
    bool _seen = (prefs.getBool('introPageSeen') ?? false);
    if (_seen) {
      Navigator.of(context).pushReplacement(MaterialPageRoute(
          builder: (context) => LoginPage(
                title: lang.get('Login'),
              )));
    } else {
      await prefs.setBool('seen', true);
      Navigator.of(context).pushReplacement(
        MaterialPageRoute(builder: (context) => const IntroPage()),
      );
    }
  }

  @override
  void afterFirstLayout(BuildContext context) => initInfo();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body: Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: const [CircularProgressIndicator()],
      ),
    ));
  }
}
