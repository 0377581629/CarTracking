import 'package:aspnet_zero_app/abp/abp_base/interfaces/account_service.dart';
import 'package:aspnet_zero_app/abp/abp_base/interfaces/data_storage_service.dart';
import 'package:aspnet_zero_app/abp/abp_base/interfaces/session_service.dart';
import 'package:aspnet_zero_app/abp/abp_client/interfaces/access_token_manager.dart';
import 'package:aspnet_zero_app/abp/abp_client/interfaces/application_context.dart';
import 'package:aspnet_zero_app/abp/models/auth/authenticate_result_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/authenticate_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/forgot_password_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/login_result.dart';
import 'package:aspnet_zero_app/abp/models/auth/reset_password_model.dart';
import 'package:aspnet_zero_app/abp/models/common/ajax_response.dart';
import 'package:dio/dio.dart';
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
  ForgotPasswordModel? forgotPasswordModel;

  @override
  Future<LoginResultOutput> loginUser() async {
    var res = LoginResultOutput();
    try {
      accessTokenManager!.authenticateModel = authenticateModel;
      authenticateResultModel = await accessTokenManager!.loginAsync();
      if (authenticateResultModel!.shouldResetPassword! == true) {
        res.result = LoginResult.needToChangePassword;
        res;
      }
      if (authenticateResultModel!.requiresTwoFactorVerification! == true) {
        res.result = LoginResult.requireTwoFactorVerification;
        return res;
      }
      if (!authenticateModel!.isTwoFactorVerification == false) {
        await dataStorageService!.storeAuthenticateResult(authenticateResultModel!);
      }
      authenticateModel!.password = null;
      var loginInfo = await sessionAppService!.getCurrentLoginInformations();
      await dataStorageService!.storeLoginInfomation(loginInfo);
      res.result = LoginResult.success;
    } on DioError catch (e) {
      res.result = LoginResult.fail;
      res.exceptionMessage = e.toString();
      if (e.response != null && e.response!.data is Map<String, dynamic>) {
        var simpleResponse = SimpleAjaxResponse.fromJson(e.response!.data);
        if (simpleResponse.errorInfo != null) {
          res.exceptionMessage = simpleResponse.errorInfo!.message;
        }
      }
    }
    return res;
  }

  @override
  Future logout() async {
    accessTokenManager!.logout();
    applicationContext!.clearLoginInfo();
    dataStorageService!.clearSessionPeristance();
  }

  @override
  Future resetPassword() async {}

  @override
  Future forgotPassword() async {}
}
