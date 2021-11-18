import 'package:aspnet_zero_app/abp/manager/interfaces/application_context.dart';
import 'package:aspnet_zero_app/abp/models/user/user_configuration.dart';
import 'package:aspnet_zero_app/abp/models/multi_tenancy/tenant_information.dart';
import 'package:aspnet_zero_app/abp/models/localization/language_info.dart';
import 'package:aspnet_zero_app/abp/models/auth/login_informations.dart';

class ApplicationContext implements IApplicationContext {
  TenantInformation? _currentTenant;

  @override
  TenantInformation? get currentTenant => _currentTenant;

  LoginInformations? _loginInfo;

  @override
  LoginInformations? get loginInfo => _loginInfo;

  @override
  UserConfiguration? configuration;

  @override
  LanguageInfo? currentLanguage;

  @override
  void setAsTenant(int tenantId, String tenancyName) {
    if (tenancyName.isNotEmpty) {
      _currentTenant = TenantInformation(tenantId, tenancyName);
    }
  }

  @override
  void clearLoginInfo() {
    _loginInfo = null;
  }

  @override
  void setLoginInfo(LoginInformations loginInfo) {
    _loginInfo = loginInfo;
  }

  @override
  void setAsHost() {
    _currentTenant = null;
  }

  @override
  void load(TenantInformation? currentTenant, LoginInformations? loginInfo) {
    _currentTenant = currentTenant;
    _loginInfo = loginInfo;
  }
}
