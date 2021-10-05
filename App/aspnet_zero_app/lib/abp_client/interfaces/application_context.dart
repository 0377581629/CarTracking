import 'package:aspnet_zero_app/abp_client/models/auth/login_informations.dart';
import 'package:aspnet_zero_app/abp_client/models/localization/language_info.dart';
import 'package:aspnet_zero_app/abp_client/models/multi_tenancy/tenant_information.dart';
import 'package:aspnet_zero_app/abp_client/models/user/user_configuration.dart';

abstract class IApplicationContext {
  final TenantInformation? _currentTenant;

  TenantInformation? get currentTenant => _currentTenant;

  UserConfiguration configuration;

  final LoginInformations _loginInfo;

  LoginInformations get loginInfo => _loginInfo;

  void clearLoginInfo();

  void setLoginInfo(LoginInformations loginInfo);

  void setAsHost();

  void setAsTenant(String tenancyName, int tenantId);

  LanguageInfo currentLanguage;

  void load(TenantInformation currentTenant, LoginInformations loginInfo);

  IApplicationContext(this.configuration, this.currentLanguage,
      this._currentTenant, this._loginInfo);
}
