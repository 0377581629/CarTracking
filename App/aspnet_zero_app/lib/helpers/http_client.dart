import 'dart:io';
import 'dart:async';
import 'package:aspnet_zero_app/abp/abp_base/interfaces/data_storage_service.dart';
import 'package:aspnet_zero_app/abp/abp_client/interfaces/access_token_manager.dart';
import 'package:aspnet_zero_app/abp/abp_client/interfaces/application_context.dart';
import 'package:aspnet_zero_app/abp/models/common/ajax_response.dart';
import 'package:aspnet_zero_app/configuration/abp_config.dart';
import "package:dio/dio.dart";
import 'package:get_it/get_it.dart';

class HttpClient {
  IApplicationContext? applicationContext;

  final Dio _dio = Dio();

  HttpClient() {
    GetIt getIt = GetIt.I;
    applicationContext = getIt.get<IApplicationContext>();
  }

  Future<Dio> createClient() async {
    _dio.options.baseUrl = AbpConfig.hostUrl;
    _dio.options.headers["User-Agent"] = AbpConfig.userAgent;
    _dio.options.headers["X-Requested-With"] = "XMLHttpRequest";
    _dio.options.connectTimeout = 3000;
    _dio.interceptors.clear();
    _dio.interceptors.add(CustomInterceptor());

    return _dio;
  }
}

class CustomInterceptor extends Interceptor {
  IApplicationContext? applicationContext;
  IAccessTokenManager? accessTokenManager;
  IDataStorageService? dataStorageService;

  CustomInterceptor() {
    GetIt getIt = GetIt.I;
    applicationContext = getIt.get<IApplicationContext>();
    accessTokenManager = getIt.get<IAccessTokenManager>();
    dataStorageService = getIt.get<IDataStorageService>();
  }

  @override
  Future onRequest(
      RequestOptions options, RequestInterceptorHandler handler) async {
    if (applicationContext!.currentTenant != null) {
      options.headers[AbpConfig.tenantResolveKey] =
          applicationContext!.currentTenant!.tenantId;
    }

    if (applicationContext!.currentLanguage != null) {
      options.headers[AbpConfig.languageKey] = "c=" +
          applicationContext!.currentLanguage!.name! +
          '|uic=' +
          applicationContext!.currentLanguage!.name!;
    }

    if (accessTokenManager != null &&
        accessTokenManager!.getAccessToken().isNotEmpty) {
      if (accessTokenManager!.isTokenExpired() &&
          !accessTokenManager!.isRefreshTokenExpired()) {
        await accessTokenManager!.refreshTokenAsync();
      }
      var accessToken = accessTokenManager!.getAccessToken();
      options.headers[HttpHeaders.authorizationHeader] = "Bearer $accessToken";
    }

    return handler.next(options);
  }
}
