import 'package:aspnet_zero_app/abp/abp_base/interfaces/account_service.dart';
import 'package:aspnet_zero_app/abp/abp_base/interfaces/data_storage_service.dart';
import 'package:aspnet_zero_app/abp/abp_base/interfaces/session_service.dart';
import 'package:aspnet_zero_app/abp/abp_client/interfaces/access_token_manager.dart';
import 'package:aspnet_zero_app/abp/abp_client/interfaces/application_context.dart';
import 'package:aspnet_zero_app/abp/abp_client/models/auth/authenticate_result_model.dart';
import 'package:aspnet_zero_app/abp/abp_client/models/auth/authenticate_model.dart';
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
  Future loginUser() async {
    accessTokenManager!.authenticateModel = authenticateModel;
    authenticateResultModel = await accessTokenManager!.loginAsync();
    if (authenticateResultModel!.shouldResetPassword! == true) {
      // TODO: Need to show change password
    }
    if (authenticateResultModel!.requiresTwoFactorVerification! == true) {
      // TODO: Redirect to two factor code view
    }
    if (!authenticateModel!.isTwoFactorVerification == false) {
      await dataStorageService!
          .storeAuthenticateResult(authenticateResultModel!);
    }
    authenticateModel!.password = null;
    var loginInfo = await sessionAppService!.getCurrentLoginInformations();
    await dataStorageService!.storeLoginInfomation(loginInfo);
  }

  @override
  Future logout() async {
    accessTokenManager!.logout();
    applicationContext!.clearLoginInfo();
    dataStorageService!.clearSessionPeristance();
  }
}
