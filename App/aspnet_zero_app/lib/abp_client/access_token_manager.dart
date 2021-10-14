import 'dart:io';

import 'package:aspnet_zero_app/abp_client/interfaces/application_context.dart';
import 'package:aspnet_zero_app/configuration/abp_config.dart';
import 'package:aspnet_zero_app/abp_client/interfaces/access_token_manager.dart';
import 'package:aspnet_zero_app/abp_client/models/auth/authenticate_model.dart';
import 'package:aspnet_zero_app/abp_client/models/auth/authenticate_result_model.dart';
import 'package:aspnet_zero_app/abp_client/models/auth/refresh_token_result.dart';
import 'package:dio/dio.dart';
import 'package:get_it/get_it.dart';
import 'models/common/ajax_response.dart';

class AccessTokenManager implements IAccessTokenManager {
  IApplicationContext? applicationContext;
  AccessTokenManager() {
    applicationContext = GetIt.I.get<IApplicationContext>();
  }

  Dio createApiClient() {
    var _dio = Dio();
    _dio.options.baseUrl = AbpConfig.hostUrl;
    _dio.options.headers["User-Agent"] = AbpConfig.userAgent;
    _dio.options.headers["X-Requested-With"] = "XMLHttpRequest";
    if (applicationContext?.currentTenant != null) {
      _dio.options.headers[AbpConfig.tenantResolveKey] =
          applicationContext!.currentTenant!.tenantId;
    }
    return _dio;
  }

  @override
  AuthenticateModel? authenticateModel;

  @override
  AuthenticateResultModel? authenticateResult;

  @override
  String getAccessToken() {
    if (authenticateResult == null) {
      return '';
    }
    return authenticateResult!.accessToken!;
  }

  @override
  bool isTokenExpired() {
    if (authenticateResult == null) {
      return false;
    } else {
      return DateTime.now().isAfter(authenticateResult!.tokenExpireDate!);
    }
  }

  @override
  bool isRefreshTokenExpired() {
    if (authenticateResult == null) {
      return false;
    } else {
      return DateTime.now()
          .isAfter(authenticateResult!.refreshTokenExpireDate!);
    }
  }

  @override
  bool isUserLoggedIn() {
    return authenticateResult != null &&
        authenticateResult!.accessToken != null;
  }

  @override
  Future<AuthenticateResultModel> loginAsync() async {
    if (authenticateModel!.userNameOrEmailAddress!.isEmpty ||
        authenticateModel!.password!.isEmpty) {
      throw UnimplementedError(
          "userNameOrEmailAddress and password cannot be empty");
    }

    var client = createApiClient();

    var clientResponse =
        await client.post(AbpConfig.loginUrlSegment, data: authenticateModel);

    if (clientResponse.statusCode != 200) {
      authenticateResult = null;
      throw UnimplementedError('Login failed');
    }

    var ajaxReponse = AjaxResponse<AuthenticateResultModel>.fromJson(
        clientResponse.data,
        (data) =>
            AuthenticateResultModel.fromJson(data as Map<String, dynamic>));

    if (!ajaxReponse.success) {
      throw UnimplementedError(
          'Login failed' + ajaxReponse.errorInfo!.message!);
    }

    authenticateResult = ajaxReponse.result!;

    if (authenticateResult!.expireInSeconds != null) {
      authenticateResult!.tokenExpireDate = DateTime.now()
          .add(Duration(seconds: authenticateResult!.expireInSeconds!));
    }
    if (authenticateResult!.refreshTokenExpireInSeconds != null) {
      authenticateResult!.refreshTokenExpireDate = DateTime.now().add(
          Duration(seconds: authenticateResult!.refreshTokenExpireInSeconds!));
    }

    return authenticateResult!;
  }

  @override
  void logout() {
    authenticateResult = null;
  }

  @override
  Future refreshTokenAsync() async {
    if (authenticateResult!.refreshToken!.isEmpty) {
      throw Exception("No refresh token!");
    }

    if (isRefreshTokenExpired()) {
      throw Exception('Refresh token expired');
    }

    var client = createApiClient();

    var clientResponse = await client.post(AbpConfig.refreshTokenUrlSegment,
        data: {'refreshToken': authenticateResult!.refreshToken!},
        options: Options(contentType: Headers.formUrlEncodedContentType));

    if (clientResponse.statusCode != 200) {
      authenticateResult = null;
      throw Exception('Refresh token failed');
    }

    var ajaxReponse = AjaxResponse<RefreshTokenResult>.fromJson(
        clientResponse.data,
        (data) => RefreshTokenResult.fromJson(data as Map<String, dynamic>));

    if (!ajaxReponse.success) {
      throw Exception('Refresh token failed' + ajaxReponse.errorInfo!.message!);
    }

    authenticateResult!.accessToken = ajaxReponse.result!.accessToken;
    return ajaxReponse.result!.accessToken;
  }
}
