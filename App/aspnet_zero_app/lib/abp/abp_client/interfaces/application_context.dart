import 'package:aspnet_zero_app/abp/models/auth/login_informations.dart';
import 'package:aspnet_zero_app/abp/models/localization/language_info.dart';
import 'package:aspnet_zero_app/abp/models/multi_tenancy/tenant_information.dart';
import 'package:aspnet_zero_app/abp/models/user/user_configuration.dart';

abstract class IApplicationContext {
  TenantInformation? _currentTenant;

  TenantInformation? get currentTenant => _currentTenant;

  LoginInformations? _loginInfo;

  LoginInformations? get loginInfo => _loginInfo;

  UserConfiguration? configuration;

  LanguageInfo? currentLanguage;

  void clearLoginInfo();

  void setLoginInfo(LoginInformations loginInfo);

  void setAsHost();

  void setAsTenant(int tenantId, String tenancyName);

  void load(TenantInformation? currentTenant, LoginInformations? loginInfo);
}
