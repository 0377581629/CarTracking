import 'dart:convert';

import 'package:aspnet_zero_app/abp_base/interfaces/data_storage_service.dart';
import 'package:aspnet_zero_app/abp_client/models/multi_tenancy/tenant_information.dart';
import 'package:aspnet_zero_app/abp_client/models/auth/login_informations.dart';
import 'package:aspnet_zero_app/abp_client/models/auth/authenticate_result_model.dart';
import 'package:aspnet_zero_app/configuration/abp_config.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class DataStorageService implements IDataStorageService {
  final storage = const FlutterSecureStorage();
  final options =
      const IOSOptions(accessibility: IOSAccessibility.first_unlock);
  @override
  Future storeAccessToken(String newAccessToken) async {
    var authenStr = await storage.read(
        key: DataStorageKey.currentSessionTokenInfo, iOptions: options);
    if (authenStr != null) {
      var authenResult =
          AuthenticateResultModel.fromJson(json.decode(authenStr));
      authenResult.accessToken = newAccessToken;
      authenStr = json.encode(authenResult.toJson());
      await storage.write(
          key: DataStorageKey.currentSessionTokenInfo, value: authenStr);
    }
  }

  @override
  Future storeAuthenticateResult(
      AuthenticateResultModel authenResultModel) async {
    await storage.write(
        key: DataStorageKey.currentSessionTokenInfo,
        value: json.encode(authenResultModel.toJson()),
        iOptions: options);
  }

  @override
  Future<AuthenticateResultModel> retrieveAuthenticateResult() async {
    var authenStr = await storage.read(
        key: DataStorageKey.currentSessionTokenInfo, iOptions: options);
    if (authenStr == null) {
      throw UnimplementedError("Not found authenticate result in storage");
    }
    return AuthenticateResultModel.fromJson(json.decode(authenStr));
  }

  @override
  Future<TenantInformation> retrieveTenantInfo() async {
    var authenStr = await storage.read(
        key: DataStorageKey.currentSessionTenantInfo, iOptions: options);
    if (authenStr == null) {
      throw UnimplementedError("Not found authenticate result in storage");
    }
    return TenantInformation.fromJson(json.decode(authenStr));
  }

  @override
  Future<LoginInformations> retrieveLoginInfo() async {
    var loginInfoStr = await storage.read(
        key: DataStorageKey.currentSessionLoginInfo, iOptions: options);
    if (loginInfoStr == null) {
      throw UnimplementedError("Not found authenticate result in storage");
    }
    return LoginInformations.fromJson(json.decode(loginInfoStr));
  }

  @override
  void clearSessionPeristance() async {
    await storage.delete(key: DataStorageKey.currentSessionTokenInfo);
    await storage.delete(key: DataStorageKey.currentSessionTenantInfo);
    await storage.delete(key: DataStorageKey.currentSessionLoginInfo);
  }

  @override
  Future storeLoginInfomation(LoginInformations input) async {
    await storage.write(
        key: DataStorageKey.currentSessionLoginInfo,
        value: json.encode(input.toJson()),
        iOptions: options);
  }

  @override
  Future storeTenantInfo(TenantInformation input) async {
    await storage.write(
        key: DataStorageKey.currentSessionTenantInfo,
        value: json.encode(input.toJson()),
        iOptions: options);
  }
}
