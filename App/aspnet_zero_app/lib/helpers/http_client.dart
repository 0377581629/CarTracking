import 'dart:io';
import 'dart:async';
import 'package:aspnet_zero_app/abp/interfaces/data_storage_service.dart';
import 'package:aspnet_zero_app/abp/manager/interfaces/access_token_manager.dart';
import 'package:aspnet_zero_app/abp/manager/interfaces/application_context.dart';
import 'package:aspnet_zero_app/configuration/abp_config.dart';
import "package:dio/dio.dart";
import 'package:get_it/get_it.dart';

class HttpClient {
  IApplicationContext? appContext;

  HttpClient() {
    GetIt getIt = GetIt.I;
    appContext = getIt.get<IApplicationContext>();
  }

  Future<Dio> createClient() async {
    var _dio = await createSimpleClient();
    _dio.interceptors.add(CustomInterceptor());
    return _dio;
  }

  Future<Dio> createSimpleClient() async {
    var _dio = Dio();
    _dio.options.baseUrl = AbpConfig.hostUrl;
    _dio.options.headers["User-Agent"] = AbpConfig.userAgent;
    _dio.options.headers["X-Requested-With"] = "XMLHttpRequest";
    _dio.options.headers["FromMobile"] = "true";

    if (appContext?.configuration != null &&
        appContext!.configuration!.multiTenancy != null &&
        appContext!.configuration!.multiTenancy!.isEnabled! &&
        appContext?.currentTenant != null &&
        appContext?.currentTenant!.tenancyName == "Default") {
      _dio.options.headers[AbpConfig.tenantResolveKey] = null;
    } else if (appContext?.currentTenant != null) {
      _dio.options.headers[AbpConfig.tenantResolveKey] =
          appContext!.currentTenant!.tenantId;
    }

    _dio.options.contentType = Headers.jsonContentType;
    _dio.options.connectTimeout = 10000;
    _dio.interceptors.clear();

    return _dio;
  }
}

class CustomInterceptor extends Interceptor {
  IApplicationContext? appContext;
  IAccessTokenManager? accessTokenManager;
  IDataStorageService? dataStorageService;

  CustomInterceptor() {
    GetIt getIt = GetIt.I;
    appContext = getIt.get<IApplicationContext>();
    accessTokenManager = getIt.get<IAccessTokenManager>();
    dataStorageService = getIt.get<IDataStorageService>();
  }

  @override
  Future onRequest(RequestOptions options, RequestInterceptorHandler handler) async {
    if (appContext!.currentTenant != null) {
      options.headers[AbpConfig.tenantResolveKey] = appContext!.currentTenant!.tenantId;
    }

    if (appContext!.currentLanguage != null) {
      options.headers[AbpConfig.languageKey] = "c=" + appContext!.currentLanguage!.name! + '|uic=' + appContext!.currentLanguage!.name!;
    }

    if (accessTokenManager != null && accessTokenManager!.getAccessToken().isNotEmpty) {
      if (accessTokenManager!.isTokenExpired() && !accessTokenManager!.isRefreshTokenExpired()) {
        try {
          await accessTokenManager!.refreshTokenAsync();
        } catch (e) {}
      }
      var accessToken = accessTokenManager!.getAccessToken();
      if (accessToken.isNotEmpty) {
        options.headers[HttpHeaders.authorizationHeader] = "Bearer $accessToken";
      }
    }

    return handler.next(options);
  }
}
