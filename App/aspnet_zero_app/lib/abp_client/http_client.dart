import 'package:aspnet_zero_app/abp_client/configuration/abp_config.dart';
import 'package:aspnet_zero_app/abp_client/interfaces/application_context.dart';
import "package:dio/dio.dart";
import 'dart:async';

import 'package:get_it/get_it.dart';

class HttpClient {
  IApplicationContext? applicationContext;

  HttpClient() {
    GetIt getIt = GetIt.I;
    applicationContext = getIt.get<IApplicationContext>();
  }

  Dio init() {
    Dio _dio = Dio();
    _dio.options.baseUrl = AbpConfig.hostUrl;
    _dio.options.headers["User-Agent"] = AbpConfig.userAgent;
    _dio.options.headers["X-Requested-With"] = "XMLHttpRequest";
    if (applicationContext!.currentTenant != null) {
      _dio.options.headers[AbpConfig.tenantResolveKey] =
          applicationContext!.currentTenant!.tenantId;
    }
    return _dio;
  }
}
