import 'package:aspnet_zero_app/abp/abp_client/models/auth/authenticate_result_model.dart';
import 'package:aspnet_zero_app/abp/abp_client/models/auth/login_informations.dart';
import 'package:aspnet_zero_app/abp/abp_client/models/multi_tenancy/tenant_information.dart';

abstract class IDataStorageService {
  Future storeAccessToken(String newAccessToken);
  Future storeAuthenticateResult(AuthenticateResultModel authenResultModel);
  Future<AuthenticateResultModel?> retrieveAuthenticateResult();
  Future<LoginInformations?> retrieveLoginInfo();
  Future<TenantInformation?> retrieveTenantInfo();

  void clearSessionPeristance();
  Future storeLoginInfomation(LoginInformations input);
  Future storeTenantInfo(TenantInformation input);
}
