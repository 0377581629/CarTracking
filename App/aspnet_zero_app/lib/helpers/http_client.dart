import 'dart:io';
import 'dart:async';
import 'package:aspnet_zero_app/abp_base/interfaces/data_storage_service.dart';
import 'package:aspnet_zero_app/abp_client/interfaces/access_token_manager.dart';
import 'package:aspnet_zero_app/abp_client/interfaces/application_context.dart';
import 'package:aspnet_zero_app/configuration/abp_config.dart';
import 'package:cookie_jar/cookie_jar.dart';
import "package:dio/dio.dart";
import 'package:dio_cookie_manager/dio_cookie_manager.dart';
import 'package:get_it/get_it.dart';
import 'package:path_provider/path_provider.dart';

class HttpClient {
  IApplicationContext? applicationContext;

  final Dio _dio = Dio();
  // PersistCookieJar? persistentCookies;

  HttpClient() {
    GetIt getIt = GetIt.I;
    applicationContext = getIt.get<IApplicationContext>();
  }

  Future<Dio> createClient() async {
    _dio.options.baseUrl = AbpConfig.hostUrl;
    _dio.options.headers["User-Agent"] = AbpConfig.userAgent;
    _dio.options.headers["X-Requested-With"] = "XMLHttpRequest";

    // final Directory dir = await _localCoookieDirectory;
    // final cookiePath = dir.path;
    // persistentCookies = PersistCookieJar(storage: FileStorage(cookiePath));

    // _dio.interceptors.add(CookieManager(persistentCookies!));
    _dio.interceptors.clear();
    _dio.interceptors.add(CustomInterceptor());

    return _dio;
  }

  // Future<String> get _localPath async {
  //   final directory = await getApplicationDocumentsDirectory();
  //   return directory.path;
  // }

  // Future<Directory> get _localCoookieDirectory async {
  //   final path = await _localPath;
  //   final Directory dir = Directory('$path/.cookies/');
  //   await dir.create();
  //   return dir;
  // }

  // Future<String?> getCsrftoken() async {
  //   try {
  //     String csrfTokenValue = '';
  //     final Directory dir = await _localCoookieDirectory;
  //     final cookiePath = dir.path;
  //     persistentCookies = PersistCookieJar(storage: FileStorage(cookiePath));
  //     //clearing any existing cookies for a fresh start
  //     persistentCookies!.deleteAll();
  //     _dio.interceptors.add(
  //         //this sets up _dio to persist cookies throughout subsequent requests
  //         CookieManager(persistentCookies!));
  //     _dio.options = BaseOptions(
  //       baseUrl: AbpConfig.hostUrl,
  //       contentType: ContentType.json.toString(),
  //       responseType: ResponseType.plain,
  //       connectTimeout: 5000,
  //       receiveTimeout: 100000,
  //       headers: {
  //         HttpHeaders.userAgentHeader: "dio",
  //         "Connection": "keep-alive",
  //       },
  //     );
  //     //BaseOptions will be persisted throughout subsequent requests made with _dio
  //     _dio.interceptors.add(InterceptorsWrapper(onResponse:
  //         (Response response, ResponseInterceptorHandler handler) async {
  //       List<Cookie> cookies = await persistentCookies!
  //           .loadForRequest(Uri.parse(AbpConfig.hostUrl));
  //       csrfTokenValue = cookies
  //           .firstWhere((c) => c.name == 'csrftoken',
  //               orElse: () => Cookie('', ''))
  //           .value;

  //       if (csrfTokenValue.isNotEmpty) {
  //         _dio.options.headers['X-CSRF-TOKEN'] =
  //             csrfTokenValue; //setting the csrftoken from the response in the headers
  //       }
  //     }));
  //     await _dio.get("/accounts/login/");
  //     return csrfTokenValue;
  //   } catch (error, stacktrace) {
  //     print("Exception occured: $error stackTrace: $stacktrace");
  //     return null;
  //   }
  // }
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

  @override
  Future onError(DioError err, ErrorInterceptorHandler handler) async {
    return handler.next(err);
  }
}
