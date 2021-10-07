import 'package:aspnet_zero_app/abp_client/configuration/abp_config.dart';
import 'package:aspnet_zero_app/abp_client/http_client.dart';
import 'package:aspnet_zero_app/abp_client/interfaces/access_token_manager.dart';
import 'package:aspnet_zero_app/abp_client/models/auth/authenticate_model.dart';
import 'package:aspnet_zero_app/abp_client/models/auth/authenticate_result_model.dart';
import 'package:aspnet_zero_app/abp_client/models/auth/refresh_token_result.dart';
import 'package:get_it/get_it.dart';
import 'interfaces/application_context.dart';
import 'models/common/ajax_response.dart';

class AccessTokenManager implements IAccessTokenManager {
  AuthenticateModel? authenticateModel;
  IApplicationContext _applicationContext;

  AccessTokenManager(this._applicationContext) {
    GetIt getIt = GetIt.I;
    _applicationContext = getIt.get<IApplicationContext>();
  }

  @override
  AbpAuthenticateResultModel? authenticateResult;

  @override
  String getAccessToken() {
    if (authenticateResult == null) {
      throw UnimplementedError();
    }
    return authenticateResult!.accessToken!;
  }

  @override
  bool get isRefreshTokenExpired =>
      authenticateResult == null ||
      DateTime.now().isAfter(authenticateResult!.refreshTokenExpireDate!);

  @override
  bool get isUserLoggedIn => authenticateResult!.accessToken != null;

  @override
  Future<AbpAuthenticateResultModel> loginAsync() async {
    if (authenticateModel!.userNameOrEmailAddress.isEmpty ||
        authenticateModel!.password.isEmpty) {
      throw UnimplementedError(
          "userNameOrEmailAddress and password cannot be empty");
    }

    _applicationContext.setAsTenant(100, "ABC");

    var client = HttpClient().init();

    var clientResponse =
        await client.post(AbpConfig.loginUrlSegment, data: authenticateModel);

    if (clientResponse.statusCode != 200) {
      authenticateResult = null;
      throw UnimplementedError('Login failed');
    }

    var ajaxReponse = AjaxResponse<AbpAuthenticateResultModel>.fromJson(
        clientResponse.data,
        (data) =>
            AbpAuthenticateResultModel.fromJson(data as Map<String, dynamic>));

    if (!ajaxReponse.success) {
      throw UnimplementedError(
          'Login failed' + ajaxReponse.errorInfo!.message!);
    }

    authenticateResult = ajaxReponse.result!;
    authenticateResult!.refreshTokenExpireDate = DateTime.now()
        .add(const Duration(days: AbpConfig.refreshTokenExpirationDays));

    return authenticateResult!;
  }

  @override
  void logout() {
    authenticateResult = null;
  }

  @override
  Future refreshTokenAsync() async {
    if (authenticateResult!.refreshToken!.isNotEmpty) {
      throw UnimplementedError("No refresh token!");
    }

    var client = HttpClient().init();

    if (_applicationContext.currentTenant != null) {
      client.options.headers[AbpConfig.tenantResolveKey] =
          _applicationContext.currentTenant!.tenantId;
    }

    var clientResponse = await client.post(AbpConfig.refreshTokenUrlSegment,
        data: {'refreshToken': authenticateResult!.refreshToken});

    if (clientResponse.statusCode != 200) {
      authenticateResult = null;
      throw UnimplementedError('Refresh token failed');
    }

    var ajaxReponse = AjaxResponse<RefreshTokenResult>.fromJson(
        clientResponse.data,
        (data) => RefreshTokenResult.fromJson(data as Map<String, dynamic>));

    if (!ajaxReponse.success) {
      throw UnimplementedError(
          'Refresh token failed' + ajaxReponse.errorInfo!.message!);
    }

    authenticateResult!.accessToken = ajaxReponse.result!.accessToken;
    return ajaxReponse.result!.accessToken;
  }
}
