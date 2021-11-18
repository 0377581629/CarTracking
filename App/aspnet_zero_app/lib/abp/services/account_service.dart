import 'package:aspnet_zero_app/abp/interfaces/account_service.dart';
import 'package:aspnet_zero_app/abp/interfaces/data_storage_service.dart';
import 'package:aspnet_zero_app/abp/interfaces/session_service.dart';
import 'package:aspnet_zero_app/abp/manager/interfaces/access_token_manager.dart';
import 'package:aspnet_zero_app/abp/manager/interfaces/application_context.dart';
import 'package:aspnet_zero_app/abp/models/auth/authenticate_result_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/authenticate_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/forgot_password_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/login_result.dart';
import 'package:aspnet_zero_app/abp/models/auth/register_model.dart';
import 'package:aspnet_zero_app/abp/models/auth/register_result.dart';
import 'package:aspnet_zero_app/abp/models/auth/reset_password_model.dart';
import 'package:aspnet_zero_app/abp/models/common/ajax_response.dart';
import 'package:aspnet_zero_app/abp/models/tenancy/is_tenancy_available.dart';
import 'package:aspnet_zero_app/helpers/http_client.dart';
import 'package:aspnet_zero_app/helpers/localization_helper.dart';
import 'package:dio/dio.dart';
import 'package:get_it/get_it.dart';

final lang = LocalizationHelper();

class AccountService implements IAccountService {
  IApplicationContext? applicationContext;
  ISessionAppService? sessionAppService;
  IAccessTokenManager? accessTokenManager;
  IDataStorageService? dataStorageService;

  static const registerEndpoint = "api/services/app/Account/Register";
  static const forgotPasswordEndpoint = "Account/SendPasswordResetLink";
  static const isTenantAvailableEndpoint = "api/services/app/Account/IsTenantAvailable";

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
  RegisterModel? registerModel;

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
  Future forgotPassword() async {
    if (forgotPasswordModel != null) {
      var client = await HttpClient().createSimpleClient();
      await client.post(forgotPasswordEndpoint, data: FormData.fromMap(forgotPasswordModel!.toJson()));
    } else {
      throw Exception(lang.get('InvalidData'));
    }
  }

  @override
  Future<RegisterResult> registerUser() async {
    var res = RegisterResult();
    try {
      var client = await HttpClient().createSimpleClient();
      var clientRes = await client.post(registerEndpoint, data: registerModel);
      if (clientRes.statusCode != 200) {
        throw UnimplementedError('Register failed');
      }
      var ajaxRes = AjaxResponse<RegisterResult>.fromJson(clientRes.data, (data) => RegisterResult.fromJson(data as Map<String, dynamic>));
      if (!ajaxRes.success) {
        throw UnimplementedError('Register failed' + ajaxRes.errorInfo!.message!);
      }
      return ajaxRes.result!;
    } on DioError catch (e) {
      res.isSuccess = false;
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
  Future<IsTenantAvailableOutput> isTenantAvailable(IsTenantAvailableInput input) async {
    var client = await HttpClient().createSimpleClient();
    var clientRes = await client.post(isTenantAvailableEndpoint, data: input);
    var ajaxRes = AjaxResponse<IsTenantAvailableOutput>.fromJson(clientRes.data, (data) => IsTenantAvailableOutput.fromJson(data as Map<String, dynamic>));
    return ajaxRes.result!;
  }
}
