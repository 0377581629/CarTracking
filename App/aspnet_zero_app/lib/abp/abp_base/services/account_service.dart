import 'package:aspnet_zero_app/abp/abp_base/interfaces/account_service.dart';
import 'package:aspnet_zero_app/abp/abp_base/interfaces/data_storage_service.dart';
import 'package:aspnet_zero_app/abp/abp_base/interfaces/session_service.dart';
import 'package:aspnet_zero_app/abp/abp_client/interfaces/access_token_manager.dart';
import 'package:aspnet_zero_app/abp/abp_client/interfaces/application_context.dart';
import 'package:aspnet_zero_app/abp/models/auth/authenticate_result_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/authenticate_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/login_result.dart';
import 'package:aspnet_zero_app/abp/models/auth/reset_password_model.dart';
import 'package:get_it/get_it.dart';

class AccountService implements IAccountService {
  IApplicationContext? applicationContext;
  ISessionAppService? sessionAppService;
  IAccessTokenManager? accessTokenManager;
  IDataStorageService? dataStorageService;

  AccountService({this.authenticateModel}) {
    var getIt = GetIt.I;
    applicationContext = getIt.get<IApplicationContext>();
    sessionAppService = getIt.get<ISessionAppService>();
    accessTokenManager = getIt.get<IAccessTokenManager>();
    dataStorageService = getIt.get<IDataStorageService>();
  }

  @override
  AuthenticateModel? authenticateModel;

  @override
  AuthenticateResultModel? authenticateResultModel;

  @override
  ResetPasswordModel? resetPasswordModel;

  @override
  Future<LoginResult> loginUser() async {
    try {
      accessTokenManager!.authenticateModel = authenticateModel;
      authenticateResultModel = await accessTokenManager!.loginAsync();

      if (authenticateResultModel!.shouldResetPassword! == true) {
        return LoginResult.needToChangePassword;
      }

      if (authenticateResultModel!.requiresTwoFactorVerification! == true) {
        return LoginResult.requireTwoFactorVerification;
      }

      if (!authenticateModel!.isTwoFactorVerification == false) {
        await dataStorageService!
            .storeAuthenticateResult(authenticateResultModel!);
      }

      authenticateModel!.password = null;
      var loginInfo = await sessionAppService!.getCurrentLoginInformations();
      await dataStorageService!.storeLoginInfomation(loginInfo);
      return LoginResult.success;
    } catch (e) {
      return LoginResult.fail;
    }
  }

  @override
  Future logout() async {
    accessTokenManager!.logout();
    applicationContext!.clearLoginInfo();
    dataStorageService!.clearSessionPeristance();
  }

  @override
  Future resetPassword() async {

  }
}
